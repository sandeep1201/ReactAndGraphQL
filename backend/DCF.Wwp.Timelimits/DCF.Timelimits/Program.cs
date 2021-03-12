using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Variance;
using CommandLine;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.Model.Services;
using DCF.Common;
using DCF.Common.Configuration;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using MediatR;
using Nito.AsyncEx;
using Polly;
using Serilog;

namespace DCF.Timelimits
{
    class Program
    {
        static Int32 Main(String[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = AsyncContext.Run(() => { return Program.AsyncMain(args); });

            //if (Debugger.IsAttached)
            //{
            //    Debugger.Break();
            //}

            Console.WriteLine($"Processing Time: {sw.Elapsed.TotalSeconds:N1}");

            return (result);
        }

        static async Task<Int32> AsyncMain(string[] args)
        {
            var exitCode = 0;
            try
            {
                var appContext = ApplicationContext.Current; //So other parts can use it :)

                //Get encrypted Configuration for database
                //var connection2 = ConfigurationManager.GetSection("sqlConnection") as ConnectionStringSettingsCollection;
                //var connectionStringSettings = connection2["SQLDatabaseConnection"];
                //var connectionStringBuilder = new SqlConnectionStringBuilder(connectionStringSettings.ConnectionString);
                //var connectionStringBuilder = new SqlConnectionStringBuilder(connectionStringSettings.ConnectionString);
                //var connectionStringBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["TimelimitsDataContext"].ConnectionString);

                //appContext.DatabaseConfig.Catalog = connectionStringBuilder.InitialCatalog;
                //appContext.DatabaseConfig.Server = connectionStringBuilder.DataSource;
                //appContext.DatabaseConfig.Password = connectionStringBuilder.Password;
                //appContext.DatabaseConfig.UserId = connectionStringBuilder.UserID;


                Action<Exception, TimeSpan, Context> logAction = (exception, duration, context) => { Log.Logger.Error(exception, "Database exception caught on attempt {duration} - retry trying operation.", duration); };

                //// Setup / configure polly retry policy
                //Policy policy = Policy.Handle<SqlException>().Or<DataException>()
                //    .WaitAndRetry(5, (rettryAttempt, context) => TimeSpan.FromSeconds(Math.Pow(2, rettryAttempt)), logAction);
                //Policy asyncPolicy = Policy.Handle<SqlException>().Or<DataException>()
                //    .WaitAndRetryAsync(5, (rettryAttempt, context) => TimeSpan.FromSeconds(Math.Pow(2, rettryAttempt)), logAction);

                //appContext.DatabaseConfig.ExecutionStrategyFactory = () => new DcfPollyDbExecutionStrategy(policy, asyncPolicy);

                ContainerBuilder container = Program.BuildContainer(appContext);
                // register DbContext configuration to use the retrysettings in the WwpEntitiesConfiguration
                DbConfiguration.SetConfiguration(new WwpEnttitesTransientFaultDbConfiguration(appContext.DatabaseConfig));

                var parsedResult = new Parser(with =>
                {
                    with.EnableDashDash = true;
                    with.IgnoreUnknownArguments = true;
                    with.HelpWriter = Console.Error;
                }).ParseArguments<BatchPrepProgramOptions, RunBatchProgramOptions, ProcessBatchResultsProgramOptions, AuxillaryImportOptions, RunBatchSimulationProgramOptions, BatchCleanupProgramOptions>(args);

                //// If we parsed the arguments correctly, then we will apply the parsed options
                //if (parsedResult.Tag == ParserResultType.Parsed)
                //{
                //    var parsed = parsedResult as Parsed<Object>;
                //    var options = parsed?.Value as BatchProgramOptionsBase;

                //    //    await app.Initialize(container).ConfigureAwait(false);
                //    //    exitCode = await app.RunAsync().ConfigureAwait(false);
                //}

                ApplicationContext.SetIsInTransitionPeriod();

                BatchApplication app = null;

                exitCode = await parsedResult.MapResult(
                    async (BatchPrepProgramOptions o) =>
                    {
                        app = new BatchPrepApplication(appContext);
                        app.Context.ApplyOptions(o);
                        await app.InitializeAsync(container).ConfigureAwait(false);
                        return await app.RunAsync().ConfigureAwait(false);
                    },
                    async (RunBatchProgramOptions o) =>
                    {
                        app = new EvaluateTimelimitsBatchApplication(appContext);
                        app.Context.ApplyOptions(o);
                        await app.InitializeAsync(container).ConfigureAwait(false);
                        return await app.RunAsync().ConfigureAwait(false);
                    },
                    async (ProcessBatchResultsProgramOptions o) =>
                    {
                        app = new ProcessBatchResultsApplication(appContext);
                        app.Context.ApplyOptions(o);
                        await app.InitializeAsync(container).ConfigureAwait(false);
                        return await app.RunAsync().ConfigureAwait(false);

                    },
                    async (AuxillaryImportOptions o) =>
                    {
                        app = new ProcessAuxillaryImportsApplication(appContext);
                        app.Context.ApplyOptions(o);
                        await app.InitializeAsync(container).ConfigureAwait(false);
                        return await app.RunAsync().ConfigureAwait(false);

                    },
                    async (RunBatchSimulationProgramOptions o) =>
                    {
                        app = new TimelimitsBatchApplicationSimulator(appContext);
                        app.Context.ApplyOptions(o);
                        await app.InitializeAsync(container).ConfigureAwait(false);
                        return await app.RunAsync().ConfigureAwait(false);

                    },
                    async (BatchCleanupProgramOptions o) =>
                    {
                        app = new BatchCleanupApplication(appContext);
                        app.Context.ApplyOptions(o);
                        await app.InitializeAsync(container).ConfigureAwait(false);
                        return await app.RunAsync().ConfigureAwait(false);

                    },
                    errors =>
                    {
                        if (errors != null)
                        {
                            foreach (var error in errors)
                            {
                                Console.WriteLine($"Parsing Error: {error.Tag}");
                            }
                        }
                        Debugger.Break();
                        return Task.FromResult(99);
                    });

            }
            catch (Exception e)
            {
                exitCode = 120;
                Console.WriteLine($"Unhandled processing exception error: {e.Message}. Inner: {e.InnerException?.Message}");
            }

            //Debugger.Break();
            return exitCode;
        }

        public static ContainerBuilder BuildContainer(ApplicationContext appContext)
        {
            var builder = new ContainerBuilder();

            // Mediatr Registration
            // enables contravariant Resolve() for interfaces with single contravariant ("in") arg
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterInstance(appContext).AsSelf();

            //Regiser Applications
            builder.RegisterAssemblyTypes(typeof(BatchApplication<,,>).Assembly).Where(t => t.BaseType.IsClosedTypeOf(typeof(BatchApplication<,,>))).AsSelf();

            // Register the DbContext Instance
            builder.RegisterType<WwpEntities>().AsSelf().As<DbContext>().InstancePerDependency();
            builder.RegisterType<TimelimitService>().AsImplementedInterfaces().InstancePerDependency();

            //builder.RegisterType<DatabaseConfiguration>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterInstance(appContext.DatabaseConfig).As<IDatabaseConfiguration>().AsSelf().SingleInstance();
            //builder.RegisterType<DatabaseConfiguration>().As<IDatabaseConfiguration>().InstancePerDependency();


            builder.RegisterType<Repository>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<Db2TimelimitService>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<JobQueueService>().AsImplementedInterfaces().InstancePerDependency();
            builder.Register<Func<WwpEntities>>((c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return () => ctx.Resolve<WwpEntities>();
            }));


            // mediator itself
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterType<BatchTaskRunner>().AsSelf().As<ITaskMediator>().InstancePerLifetimeScope();

            // request handlers
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t =>
                {
                    object o;
                    return c.TryResolve(t, out o) ? o : null;
                };
            }).InstancePerLifetimeScope();

            // notification handlers
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            }).InstancePerLifetimeScope();

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(BatchApplication<,,>).Assembly).Where(t => t.Namespace.Equals("DCF.Timelimits.Processors")).Where(t => t.GetInterfaces().Any(i =>
                {
                    var shouldRegister = Program.IsClosedTypeOfMediatr(i) || Program.ImplementsMediatrGenericType(i);
                    return shouldRegister;
                }
            )).AsImplementedInterfaces();
            // via assembly scan


            return builder;
        }

        public static Boolean ImplementsMediatrGenericType(Type i)
        {
            return i.IsGenericTypeDefinition && (i == typeof(IRequestHandler<,>)
                                                 || i == typeof(IRequestHandler<>)
                                                 || i == typeof(IAsyncRequestHandler<>)
                                                 || i == typeof(IAsyncRequestHandler<,>)
                                                 || i == typeof(ICancellableAsyncRequestHandler<>)
                                                 || i == typeof(ICancellableAsyncRequestHandler<,>)
                                                 || i == typeof(INotificationHandler<>)
                                                 || i == typeof(IAsyncNotificationHandler<>)
                                                 || i == typeof(ICancellableAsyncNotificationHandler<>));
        }

        public static Boolean IsClosedTypeOfMediatr(Type i)
        {
            return i.IsClosedTypeOf(typeof(IRequestHandler<,>))
                   || i.IsClosedTypeOf(typeof(IRequestHandler<>))
                   || i.IsClosedTypeOf(typeof(IAsyncRequestHandler<>))
                   || i.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>))
                   || i.IsClosedTypeOf(typeof(ICancellableAsyncRequestHandler<>))
                   || i.IsClosedTypeOf(typeof(ICancellableAsyncRequestHandler<,>))
                   || i.IsClosedTypeOf(typeof(INotificationHandler<>))
                   || i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))
                   || i.IsClosedTypeOf(typeof(ICancellableAsyncNotificationHandler<>));
        }
    }

    public class ApplicationBootstrapper
    {
        private readonly BatchProgramOptionsBase _options;
        private readonly BatchApplication _app;

        public ApplicationBootstrapper(ApplicationContext context, BatchProgramOptionsBase options, BatchApplication app)
        {
            this._options = options;
            this._app = app;
        }

        public void CreateProcessingQueue()
        {
            this._app.CreateProcessingQueue();
        }

        internal Task StartProducerQueue()
        {
            return this._app.StartProducerQueue();
        }

        public Task StartAsync()
        {
            return this._app.StartProducerQueue();
        }
    }
}