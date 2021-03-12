using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Contexts;
using Dcf.Wwp.DataAccess.Infrastructure;
using Dcf.Wwp.DataAccess.Interfaces;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
//using AutoMapper;

namespace Dcf.Wwp.DataAccess.ConsoleApp
{
    public class AutofacModule : Autofac.Module
    {
        #region Properties

        private static readonly ILog _log = LogManager.GetLogger("Dcf.Wwp.Batch");

        public string Message  { get; set; }
        public string LogLevel { get; set; }
        public string LogPath  { get; set; }
        public string OutPath  { get; set; }

        #endregion

        #region Methods

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Mapper.Reset();
            //Mapper.Initialize(cfg => cfg.CreateMap<ProgramOptions, BatchJobOptions>());

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsImplementedInterfaces()
                   .InstancePerRequest();

            builder.RegisterType<EPContext>()
                   .As<IDbContext>().WithParameter(new NamedParameter("connString", @"Data Source=DBWMAD0D2613, 2025;Database=WWPDEV;Integrated Security=True;MultipleActiveResultSets=True"))
                   .InstancePerLifetimeScope();

            builder.RegisterType<DatabaseFactory>()
                   .As<IDatabaseFactory>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(RepositoryBase<>).Assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            //var domains = Assembly.Load("ContosoU.Domains");

            //builder.RegisterAssemblyTypes(domains)
            //       .AsImplementedInterfaces()
            //       .InstancePerRequest();
            // done...
        }

        private void ConfigureLog(string appName, string fileName)
        {
            var logPattern = "%date{HH:mm:ss:fff} - %-5level - %message %newline";

            // configure logger 
            BasicConfigurator.Configure();

            // the layout for the appenders
            var logLayout = new PatternLayout(logPattern);
            logLayout.ActivateOptions();

            // swap-out root appender's default layout for ours
            var hierarchy = (Hierarchy) LogManager.GetRepository();
            var ra        = (ConsoleAppender) hierarchy.Root.Appenders[0];
            ra.Layout = logLayout;

            // configure batch log
            var log    = LogManager.GetLogger(appName);
            var logger = (Logger) log.Logger;

            logger.Parent     = hierarchy.Root;
            logger.Level      = ParseLogLevel(LogLevel);
            logger.Additivity = false;

            // the appenders (one for Ctrl-M's console, teh other for the logfile)
            var cntlMAppender = new ConsoleAppender { Layout = logLayout };
            cntlMAppender.ActivateOptions();

            var fileAppender = new FileAppender { File = fileName, Layout = logLayout };
            fileAppender.ActivateOptions();

            logger.AddAppender(cntlMAppender);
            logger.AddAppender(fileAppender);

            //return (log);
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            var properties = instanceType
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0)
                             .ToList();

            properties.ForEach(p => p.SetValue(instance, LogManager.GetLogger(instanceType), null));
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs eventArgs)
        {
            eventArgs.Parameters = eventArgs.Parameters.Union(
                                                              new[]
                                                              {
                                                                  new ResolvedParameter(
                                                                                        (p, i) => p.ParameterType == typeof(ILog),
                                                                                        (p, i) => LogManager.GetLogger("Dcf.Wwp.Batch") // LogManager.GetLogger(p.Member.DeclaringType)
                                                                                       )
                                                              });
        }

        private static Level ParseLogLevel(string logLevel)
        {
            var logRepo = LoggerManager.GetAllRepositories().FirstOrDefault();

            // if (logRepo == null)
            // {
            //     throw new Exception("No logging repositories defined");
            // }

            var level = logRepo?.LevelMap[logLevel];

            if (level == null)
            {
                throw new Exception("Invalid logging level specified");
            }

            return (level);
        }

        #endregion
    }
}
