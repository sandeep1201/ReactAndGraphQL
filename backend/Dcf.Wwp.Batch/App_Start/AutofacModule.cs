using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Dcf.Wwp.Batch.Infrastructure;
using Dcf.Wwp.Batch.Interfaces;
using Dcf.Wwp.Batch.Models;
using Dcf.Wwp.Batch.Models.Parsers;
using Microsoft.Extensions.Configuration;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Dcf.Wwp.Batch
{
    public class AutofacModule : Autofac.Module
    {
        #region Properties

        private static readonly ILog      _log      = LogManager.GetLogger("Dcf.Wwp.Batch");
        private static readonly IDbConfig _dbConfig = new DbConfig(_log);

        public string Message          { get; set; }
        public string LogLevel         { get; set; }
        public string LogPath          { get; set; }
        public string OutPath          { get; set; }
        public string PreferredSection { get; set; }
        public string EmailSection     { get; set; }
        public string WWPPath          { get; set; }

        #endregion

        #region Methods

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // defaults
            LogPath  = LogPath  ?? "Logs";
            OutPath  = OutPath  ?? "Output";
            LogLevel = LogLevel ?? "INFO";

            // stuff we'll need
            var appName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            var runTime = DateTime.Now;
            var subGuid = Guid.NewGuid().ToString().Substring(24);
            var jobName = Program.JobName;
            var fileName = string.IsNullOrWhiteSpace(Program.JobNumber)
                               ? $"{LogPath}\\{jobName}\\{jobName}_{runTime:MM-dd-yyyy-hh-mm-sstt}-{subGuid}.log"
                               : $"{LogPath}\\{Program.JobNumber}\\{Program.JobNumber}_{runTime:MM-dd-yyyy-hh-mm-sstt}-{subGuid}.log";

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

            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddEnvironmentVariables("WWP_")
                     .AddJsonFile($"Dcf.Wwp.Batch.json")
                     .Build();

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
                                              OutPath      = OutPath,
                                              Sproc        = bo.Sproc,
                                              Desc         = bo.Desc,
                                              JobNumber    = bo.JobNumber,
                                              IsRQJ        = bo.IsRQJ
                                          };

                                 return (jo);
                             }
                            ).As<IBatchJobOptions>();

            builder.RegisterType<DbConfig>()
                   .As<IDbConfig>()
                   .SingleInstance();

            var csvSection  = cb.GetSection(PreferredSection);
            var csvConfigs  = csvSection.Get<List<CsvPathConfig>>();
            var csvEnvConfig = csvConfigs.FirstOrDefault(i => i.Env == _dbConfig.Catalog); // keys off of database name
            builder.RegisterInstance(csvEnvConfig).As<ICsvPathConfig>().ExternallyOwned();

            var emailSection = cb.GetSection(EmailSection);
            var emailConfig  = emailSection.Get<EmailConfig>();
            builder.RegisterInstance(emailConfig).As<IEmailConfig>().ExternallyOwned();

            var wwpPath          = cb.GetSection(WWPPath);
            var wwpPathConfig    = wwpPath.Get<List<WwpPathConfig>>();
            var wwpPathEnvConfig = wwpPathConfig.FirstOrDefault(i => i.Env == _dbConfig.Catalog); // keys off of database name
            builder.RegisterInstance(wwpPathEnvConfig).As<IWwpPathConfig>().ExternallyOwned();

            builder.RegisterType<BaseJob>().As<IBaseJob>().SingleInstance();
            builder.RegisterType<HttpWebRequestWrapper>().As<IHttpWebRequestWrapper>().SingleInstance();

            builder.RegisterType<JWWWP00>().Named<IBatchJob>("JWWWP00"); // wwp.SP_DB2_Auto_Disenrollment_Update
            builder.RegisterType<JWWWP01>().Named<IBatchJob>("JWWWP01"); // wwp.SP_RFA_System_Denial
            builder.RegisterType<JWWWP02>().Named<IBatchJob>("JWWWP02"); // wwp.SP_DB2_NE_Disenrollment_Update
            builder.RegisterType<JWWWP03>().Named<IBatchJob>("JWWWP03"); // wwp.USP_TJTMJ_NA_Disenrollment_Update
            builder.RegisterType<JWWWP04>().Named<IBatchJob>("JWWWP04"); // Repo.UpsertParticantEnrollment - Participant Repository
            builder.RegisterType<JWWWP05>().Named<IBatchJob>("JWWWP05"); // Update WKR Table
            builder.RegisterType<JWWWP06>().Named<IBatchJob>("JWWWP06"); // Deactivate Assigned Participants with no Activity in Past 1 Month 
            builder.RegisterType<JWWWP07>().Named<IBatchJob>("JWWWP07"); // wwp.USP_Create_Participation_Entries 
            builder.RegisterType<JWWWP08>().Named<IBatchJob>("JWWWP08"); // wwp.USP_PullDown_Batch
            builder.RegisterType<JWWWP09>().Named<IBatchJob>("JWWWP09"); // wwp.USP_Copy_T0485_Data
            builder.RegisterType<JWWWP10>().Named<IBatchJob>("JWWWP10"); // wwp.USP_Get_T3018_Data
            builder.RegisterType<JWWWP11>().Named<IBatchJob>("JWWWP11"); // wwp.USP_After_Pull_Down_Batch
            builder.RegisterType<JWWWP21>().Named<IBatchJob>("JWWWP21"); // wwp.USP_After_Pull_Down_Batch
            builder.RegisterType<JWWWP>().Named<IBatchJob>("JWWWP");     // wwp.USP_CreateWorkerTasks_CF

            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP00");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP01");                   // generic export as csv
            builder.RegisterType<ExportByEnrollmentGroupsAsCsv>().Named<IExportable>("JWWWP02"); // exports by grouping by CoEnrollment status for wwp.SP_DB2_NE_Disenrollment_Update
            builder.RegisterType<ExportByEnrollmentGroupsAsCsv>().Named<IExportable>("JWWWP03"); // exports by grouping by CoEnrollment status for wwp.USP_TJTMJ_NA_Disenrollment_Update
            builder.RegisterType<ExportByEnrollmentGroupsAsCsv>().Named<IExportable>("JWWWP04"); // xports by grouping by CoEnrollment status
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP05");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP06");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP07");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP08");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP09");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP10");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP11");                   // generic export as csv
            builder.RegisterType<ExportAsCsv>().Named<IExportable>("JWWWP21");                   // generic export as csv
            builder.RegisterType<ExportBySpAsCsv>().Named<IExportable>("JWWWP");                 // generic export as csv

            // parsers
            builder.Register(c => new CsvParserOptions(true, ',')).SingleInstance();
            builder.RegisterType<TinyCsvPEPMapping>().As<ICsvMapping<PEPLine>>().SingleInstance();
            builder.RegisterType<CsvParser<PEPLine>>().As<ICsvParser<PEPLine>>().AsSelf();
            builder.RegisterType<SmtpEmail>().As<ISmtpEmail>().SingleInstance();
            builder.RegisterType<OverUnderPaymentEmail>().As<IOverUnderPaymentEmail>().SingleInstance();

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
                                                                                        (p, i) => LogManager.GetLogger("Dcf.Wwp.Batch")
                                                                                       )
                                                              });
        }

        private static Level ParseLogLevel(string logLevel)
        {
            var logRepo = LoggerManager.GetAllRepositories().FirstOrDefault();
            var level   = logRepo?.LevelMap[logLevel];

            if (level == null)
            {
                throw new Exception("Invalid logging level specified");
            }

            return (level);
        }

        #endregion
    }
}
