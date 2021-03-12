using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Dcf.Wwp.Api
{
    public class StartupLogger
    {
        #region Properties

        private static readonly ILog _log = LogManager.GetLogger("Dcf.Wwp.Api");

        #endregion

        #region Methods

        public static void Configure()
        {
            // just re-read the config file, trust no one ;)
            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddEnvironmentVariables()
                     .AddJsonFile("appsettings.json")
                     .Build();

            var config    = cb.GetSection("log");
            var logConfig = config.Get<StartupLogConfig>();

            // defaults don't belong here, but we're in a hurry ~ lol
            logConfig          = logConfig ?? new StartupLogConfig();
            logConfig.FileName = string.IsNullOrEmpty(logConfig.FileName) ? "Logs\\Dcf.Wwp.Api.Startup.log" : logConfig.FileName;
            logConfig.LogLevel = string.IsNullOrEmpty(logConfig.LogLevel) ? "DEBUG" : logConfig.LogLevel;
            logConfig.Layout   = string.IsNullOrEmpty(logConfig.Layout) ? "%date{MM-dd-yyyy HH:mm:ss tt} - %logger -  %-5level - %message %newline" : logConfig.Layout;

            var ourLayout = new PatternLayout(logConfig.Layout);

            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var log    = LogManager.GetLogger("Dcf.Wwp.Api");
            var logger = (Logger) log.Logger;

            logger.Parent     = hierarchy.Root;
            logger.Level      = ParseLogLevel(logConfig.LogLevel);
            logger.Additivity = false;

            var fileAppender = new FileAppender
                               {
                                   File           = logConfig.FileName,
                                   AppendToFile   = false,
                                   Layout         = ourLayout,
                                   ImmediateFlush = logConfig.ImmediateFlush,
                                   Threshold      = ParseLogLevel(logConfig.LogLevel)
                               };

            fileAppender.ActivateOptions();

            logger.AddAppender(fileAppender);

            // leaving this in in case you want a rollig file instead...

            // var rollingAppender = new RollingFileAppender
            //                       {
            //                           File               = logConfig.FileName,
            //                           AppendToFile       = false,
            //                           RollingStyle       = RollingFileAppender.RollingMode.Size,
            //                           MaximumFileSize    = logConfig.MaximumFileSize,
            //                           MaxSizeRollBackups = logConfig.MaxSizeRollBackups,
            //                           StaticLogFileName  = true, // true if always should be logged to the same file, otherwise false.
            //                           Layout             = new PatternLayout("%date{MM-dd-yyyy HH:mm:ss tt} - %logger -  %-5level - %message %newline")
            //                       };
            //
            // rollingAppender.ActivateOptions();
            //
            // logger.AddAppender(rollingAppender);

            // this is the syntax for troubleshooting 
            // log4net logger setup issues.
            // log4net.Util.LogLog.InternalDebugging = true;

            BasicConfigurator.Configure();

            // swap-out root logger's crappy default layout for ours (same as one above) ;)
            var ra = (ConsoleAppender) hierarchy.Root.Appenders[0];
            ra.Layout = ourLayout;
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
                throw new Exception("Unknown Log level specified");
            }

            return (level);
        }

        #endregion
    }

    public class StartupLogConfig
    {
        #region Properties

        public string FileName       { get; set; }
        public string LogLevel       { get; set; }
        public string Layout         { get; set; }
        public bool   ImmediateFlush { get; set; }

        #endregion

        #region Methods

        public StartupLogConfig()
        {
          //FileName = AppDomain.CurrentDomain.BaseDirectory
            FileName = @"Logs\Dcf.Wwp.Api.Startup.log";
        }

        #endregion
    }
}



