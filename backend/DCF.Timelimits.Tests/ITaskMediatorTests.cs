using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Variance;
using Dcf.Wwp.Data.Sql.Model;
using DCF.Common.Configuration;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Shouldly;

namespace DCF.Timelimits.Tests
{
    [TestClass]
    public class ITaskMediatorTests
    {
        public static ILogger _logger;
        public static ContainerBuilder container;
        public StringBuilder log = new StringBuilder();

        [TestInitialize]
        public void Initialize()
        {
            
            var builder = new ContainerBuilder();

            // Mediatr Registration
            // enables contravariant Resolve() for interfaces with single contravariant ("in") arg
            builder.RegisterSource(new ContravariantRegistrationSource());

            // Register logger
            var stringWriter = new StringWriter(log);
            ITaskMediatorTests._logger = new LoggerConfiguration().WriteTo.LiterateConsole().WriteTo.TextWriter(stringWriter)
                .CreateLogger();

            builder.RegisterInstance(ITaskMediatorTests._logger).As<ILogger>().SingleInstance();


            ITaskMediatorTests.AppContext = new ApplicationContext(DateTime.Now);

            builder.RegisterInstance(ITaskMediatorTests.AppContext).AsSelf();

            //Regiser Applications
            builder.RegisterAssemblyTypes(typeof(BatchApplication<,,>).Assembly).Where(t => t.BaseType.IsClosedTypeOf(typeof(BatchApplication<,,>))).AsSelf();

            builder.RegisterInstance(new DatabaseConfiguration());

            // Register the DbContext Instance
            builder.Register<WwpEntities>((componentContext, parameters) =>
            {
                if (ApplicationContext.AppEnvironment == ApplicationEnvironment.Local)
                {
                    return new WwpEntities();
                }
                else
                {
                    return new WwpEntities(ITaskMediatorTests.AppContext.DatabaseConfig);
                }
            }).AsSelf().As<DbContext>().InstancePerDependency().ExternallyOwned();

            builder.RegisterType<TimelimitService>().AsImplementedInterfaces().InstancePerDependency();

            builder.Register<Func<WwpEntities>>((c, p) => c.Resolve<WwpEntities>);

            //builder.Register((c, p) =>
            //{
            //    var singleInstanceFactory = c.Resolve<SingleInstanceFactory>();
            //    return new TimelimitService(singleInstanceFactory);
            //}).AsImplementedInterfaces();


            // mediator itself
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterType<BatchTaskRunner>().AsSelf().As<ITaskMediator>().InstancePerLifetimeScope();

            // request handlers
            builder.Register<SingleInstanceFactory>(ctx => {
                var c = ctx.Resolve<IComponentContext>();
                return t => { object o; return c.TryResolve(t, out o) ? o : null; };
            }).InstancePerLifetimeScope();

            // notification handlers
            builder.Register<MultiInstanceFactory>(ctx => {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            }).InstancePerLifetimeScope();

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(BatchApplication<,,>).Assembly).Where(t=>t.Namespace.Equals("DCF.Timelimits.Processors")).Where(t => t.GetInterfaces().Any(i =>
                {
                    var shouldRegister = this.IsClosedTypeOfMediatr(i) || this.ImplementsMediatrGenericType(i);
                    return shouldRegister;
                }
                                                 )).AsImplementedInterfaces();
            // via assembly scan

            ITaskMediatorTests.container = builder;

        }

        private Boolean ImplementsMediatrGenericType(Type i)
        {
            return i.IsGenericTypeDefinition && ( i == typeof(IRequestHandler<,>)
                                     || i == typeof (IRequestHandler<>)
                                     || i == typeof (IAsyncRequestHandler<>)
                                     || i == typeof (IAsyncRequestHandler<,>)
                                     || i == typeof (ICancellableAsyncRequestHandler<>)
                                     || i == typeof (ICancellableAsyncRequestHandler<,>)
                                     || i == typeof (INotificationHandler<>)
                                     || i == typeof (IAsyncNotificationHandler<>)
                                     || i == typeof (ICancellableAsyncNotificationHandler<>));
        }

        private Boolean IsClosedTypeOfMediatr(Type i)
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

        public static ApplicationContext AppContext { get; set; }

        [TestMethod]
        public async Task TimelimitsBatchApplication_Should_Initialize()
        {

            var app = new TimelimitsBatchApplication(ITaskMediatorTests.AppContext);
            await app.Initialize(ITaskMediatorTests.container).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Mediator_should_resolve_all_task_type_handlers()
        {
            var app = new TimelimitsBatchApplication(ITaskMediatorTests.AppContext);
            app.OnContainerInitialized.Invoke(ITaskMediatorTests.container);

            var container = ITaskMediatorTests.container.Build();
            var mediator = container.Resolve<ITaskMediator>();

            var task1 = new EvaluateTimelimitsTaskContext();

            await mediator.SendToMany(task1);
            var handlers = BatchTaskRunner.CurrentRequestHandlers;
            handlers.Count.ShouldBe(1);

            var task2 = new EvaluateTimelimitsTaskContext();
        }

        public async Task Mediator_should_resolve_publish_to_handlers()
        {
            var container = ITaskMediatorTests.container.Build();
            var mediator = container.Resolve<ITaskMediator>();

            var task1 = new EvaluateTimelimitsTaskContext();

            await mediator.SendToMany(task1);
        }

        public async Task Mediator_should_resolve_publish_to_handlers_by_priority()
        {
            var container = ITaskMediatorTests.container.Build();
            var mediator = container.Resolve<ITaskMediator>();

            var task1 = new EvaluateTimelimitsTaskContext();

            await mediator.SendToMany(task1);
        }


    }
}
