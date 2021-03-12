using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Dcf.Wwp.BritsBatch.Infrastructure
{
    public static class Log4NetOld
    {
        #region Properties

        public static string        AppName     { get; set; }
        public static DateTime      Now         { get; set; }
        public static long          UnixTimeNow { get; set; }
        public static PatternLayout Layout      { get; set; }

        #endregion

        #region Methods

        public static void Configure()
        {
            var now     = DateTime.Now;
            var nut     = UnixTimeNow = now.ToUnixTime();
            var appName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName); // sneaky use of Path class ~ lol
            var layout  = new PatternLayout("%logger - %date{dd-MM-yyyy HH:mm:ss tt} - %-5level - %message %newline");
            layout.ActivateOptions();

            var h = (Hierarchy) LogManager.GetRepository("log4net-default-repository");
            

            BasicConfigurator.Configure();

            var ra = (ConsoleAppender) h.Root.Appenders[0];
            ra.Layout = layout;

            var cfa = new FileAppender
                      {
                          File         = $"Logs\\{appName}-{DateTime.Now:MM-dd-yyyy-hh-mm-sstt}-{nut}.log",
                          AppendToFile = true,
                          Layout       = layout,
                          Name         = "CntlM-Log"
                      };

            cfa.ActivateOptions();
            h.Root.AddAppender(cfa);


            //ILog _log = LogManager.GetLogger("Dcf.Wwp.BritsBatch");
            //var appLog = (Logger) LogManager.GetLogger("Dcf.Wwp.BritsBatch.Console").Logger;

            //var afa = new FileAppender
            //          {
            //              File         = $"Logs\\{appName}-{DateTime.Now:MM-dd-yyyy-hh-mm-sstt}-{nut}.log",
            //              AppendToFile = true,
            //              Layout       = layout,
            //              Name         = "App-Log"
            //          };

            //appLog = appLog;

        }

        public static void Configure(string jobName, string logPath, string logLevel)
        {
            var now     = DateTime.Now;
            var nut     = UnixTimeNow = now.ToUnixTime();                                         // quick hack
            var appName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName); // sneaky use of Path class ~ lol

            // need 1 logger, and 2 appenders (ConsoleAppender (for Control-M output) && FileAppender (for actual log file)

            var hierarchy = (Hierarchy) LogManager.GetRepository("log4net-default-repository");
            var logger    = (Logger) LogManager.GetLogger("Dcf.Wwp.BritsBatch").Logger;

            var layout = new PatternLayout("%logger - %date{dd-MM-yyyy HH:mm:ss tt} - %-5level - %message %newline");
            layout.ActivateOptions();

            var consoleAppender = new ConsoleAppender
                                  {
                                      Target = "Console.Out",
                                      Layout = layout
                                  };

            consoleAppender.ActivateOptions();

            //var appAppender = new FileAppender
            //                  {
            //                      File         = $"Logs\\{appName}-{DateTime.Now:MM-dd-yyyy-hh-mm-sstt}-{nut}.log",
            //                      AppendToFile = true,
            //                      Layout       = layout
            //};

            logger.Parent     = hierarchy.Root;
            logger.Level      = SetLogLevel(string.IsNullOrEmpty(logLevel) ? "INFO" : logLevel);
            logger.Additivity = true;
            //appAppender.ActivateOptions();
            logger.AddAppender(consoleAppender);

            //logger.AddAppender(appAppender);

#if DEBUG
            //LogLog.InternalDebugging = false;
#endif
            BasicConfigurator.Configure();
        }

        public static void FlushBuffers()
        {
            var rep = LogManager.GetRepository("log4net-default-repository");

            foreach (var appender in rep.GetAppenders())
            {
                if (appender is BufferingAppenderSkeleton buffered)
                {
                    buffered.Flush();
                }
            }
        }

        public static Level SetLogLevel(string logLevel)
        {
            var logger   = (Logger) LogManager.GetLogger("Dcf.Wwp.BritsBatch").Logger;
            var newLevel = Level.Info;

            switch (logLevel.ToLower())
            {
                case "all":
                    newLevel = Level.All;
                    break;

                case "debug":
                    newLevel = Level.Debug;
                    break;

                case "info":
                    newLevel = Level.Info;
                    break;

                case "warn":
                    newLevel = Level.Warn;
                    break;

                case "error":
                    newLevel = Level.Error;
                    break;

                case "off":
                    newLevel = Level.Off;
                    break;

                default:
                    newLevel = Level.Info;
                    break;
            }

            logger.Level = newLevel;

            BasicConfigurator.Configure();

            return (newLevel);
        }

        #endregion
    }
}
