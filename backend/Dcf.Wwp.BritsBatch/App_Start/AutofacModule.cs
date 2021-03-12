using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Dcf.Wwp.BritsBatch.Infrastructure;
using Dcf.Wwp.BritsBatch.Interfaces;
using Dcf.Wwp.BritsBatch.Models;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Configuration;

//using AutoMapper;

namespace Dcf.Wwp.BritsBatch
{
    public class AutofacModule : Autofac.Module
    {
        #region Properties

        private static readonly ILog      _log      = LogManager.GetLogger("Dcf.Wwp.BritsBatch");
        private static readonly IDbConfig _dbConfig = new DbConfig(_log);

        public string Message          { get; set; }
        public string LogLevel         { get; set; }
        public string LogPath          { get; set; }
        public string OutPath          { get; set; }
        public string PreferredSection { get; set; }

        #endregion

        #region Methods

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Mapper.Reset();
            //Mapper.Initialize(cfg => cfg.CreateMap<ProgramOptions, BatchJobOptions>());

            // defaults
            LogPath  = LogPath  ?? "Logs";
            OutPath  = OutPath  ?? "Output";
            LogLevel = LogLevel ?? "INFO";

            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddEnvironmentVariables("WWP_")
                     .AddJsonFile($"Dcf.Wwp.BritsBatch.json")
                     .Build();

            var esSection   = cb.GetSection(PreferredSection);
            var esConfigs   = esSection.Get<List<EntSecConfig>>();
            var esEnvConfig = esConfigs.FirstOrDefault(i => i.Env == _dbConfig.Catalog); // keys off of database name
            builder.RegisterInstance(esEnvConfig).As<IEntSecConfig>().ExternallyOwned();

            var getRecoupSection   = cb.GetSection("getRecoup");
            var getRecoupConfigs   = getRecoupSection.Get<List<GetRecoupConfig>>();
            var getRecoupEnvConfig = getRecoupConfigs.FirstOrDefault(i => i.Env == _dbConfig.Catalog);
            builder.RegisterInstance(getRecoupEnvConfig).As<IGetRecoupConfig>().ExternallyOwned();

            // stuff we'll need
            var appName  = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            var runTime  = DateTime.Now;
            var subGuid  = Guid.NewGuid().ToString().Substring(24);
            var jobName  = Program.JobName; // hokey, but expedient for now ~ lol
            var fileName = $"{LogPath}\\{jobName}\\{jobName}_{runTime:MM-dd-yyyy-hh-mm-sstt}-{subGuid}.log";

            // configure the logs and get that out of the way...
            ConfigureLog(appName, fileName);
            _log.Info("BEGIN LOG ".PadRight(50, '*'));
            _log.Info($"{appName} starting");
            _log.Debug("\tinitializing");
            _log.Info($"Logging to {fileName} ");
            _log.Debug("\tconfiguring container");

            // now register our legos (logs, options, runtime options, db configs, batch jobs, file exporters, etc...)
            _log.Debug("\tregistering components");
            builder.RegisterInstance(_log).As<ILog>().ExternallyOwned();

            builder.Register(c =>
                             {
                                 var bo = c.Resolve<IProgramOptions>();

                                 var jo = new BatchJobOptions
                                          {
                                              JobName      = bo.JobName,
                                              ExportFormat = bo.ExportFormat,
                                              IsSimulation = bo.IsSimulation,
                                              RunTime      = runTime,
                                              SubGuid      = subGuid,
                                              OutPath      = OutPath
                                          };

                                 return (jo);
                             }
                            ).As<IBatchJobOptions>();

            builder.RegisterType<DbConfig>()
                   .As<IDbConfig>()
                   .SingleInstance();

            builder.RegisterType<JWBWP00>().Named<IBatchJob>("JWBWP00");
            builder.RegisterType<JWBWP01>().Named<IBatchJob>("JWBWP01");

            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWBWP00"); // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWBWP01"); // generic export as csv
            builder.RegisterType<EntSecService>().As<IEntSecService>();
            builder.RegisterType<RecoupService>().As<IRecoupService>();
            builder.RegisterType<RecoupAmtSproc>().As<IRecoupAmtSproc>();

            _log.Info("Initialized");
            _log.Debug(Message);

            // done...
        }

        private void ConfigureLog(string appName, string fileName)
        {
            var logPattern = "%date{MM-dd-yyyy HH:mm:ss:fff} - %-5level - %message %newline";

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
                                                                                        (p, i) => LogManager.GetLogger("Dcf.Wwp.BritsBatch")
                                                                                       )
                                                              });
        }

        private static Level ParseLogLevel(string logLevel)
        {
            var logRepo = LoggerManager.GetAllRepositories().FirstOrDefault();

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
