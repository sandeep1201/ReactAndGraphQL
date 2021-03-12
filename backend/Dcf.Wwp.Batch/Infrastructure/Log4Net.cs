using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public static class Log4Net
    {
        #region Properties

        public static DateTime Now     { get; private set; }
        public static string   LogPath { get; private set; }
        public static string   SubGuid { get; private set; }

        public static string LogPattern  => "%date{HH:mm:ss:fff} - %-5level - %message %newline";
        public static string DataPattern => "%message %newline";

        #endregion

        #region Methods

        public static void Configure(string logPath, string logLevel, string jobName)
        {
            var appName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName); // sneaky use of Path class ~ lol
            LogPath = logPath;
            Now     = DateTime.Now;
            SubGuid = Guid.NewGuid().ToString().Substring(24);

            var fileName = $"{logPath}\\{jobName}\\{jobName}_{Now:MM-dd-yyyy-hh-mm-sstt}-{SubGuid}.log";
            var layout   = new PatternLayout(LogPattern);
            layout.ActivateOptions();

            BasicConfigurator.Configure();

            var hierarchy = (Hierarchy) LogManager.GetRepository();
            var appLogger = (Logger) LogManager.GetLogger(appName).Logger;
            //var jobLogger     = (Logger) LogManager.GetLogger($"{appName}.Job").Logger;

            appLogger.Parent     = hierarchy.Root;
            appLogger.Level      = Level.All;
            appLogger.Additivity = false;

            //jobLogger.Parent = hierarchy.Root;
            // jobLogger.Parent     = consoleLogger;
            // jobLogger.Level      = Level.Debug;
            // jobLogger.Additivity = true;

            // the appenders
            var appAppender = new ConsoleAppender
                              {
                                  Layout = layout
                              };

            appAppender.ActivateOptions();
            appLogger.AddAppender(appAppender);

            var appLogAppender = new FileAppender
                                 {
                                     File = fileName,
                                     Layout = layout
                                 };

            appLogAppender.ActivateOptions();
            appLogger.AddAppender(appLogAppender);
        }

        public static void ConfigureJobLog(string jobName)
        {
        }

        public static Level SetLogLevel(ILog log, string logLevel)
        {
            //var logger   = (Logger) LogManager.GetLogger("Dcf.Wwp.Batch").Logger;
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

            ((Logger) log.Logger).Level = newLevel;

            //BasicConfigurator.Configure();

            return (newLevel);
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

        public static void FlushBuffers2()
        {
            var repo = LogManager.GetRepository("log4net-default-repository");

            var appenders = repo.GetAppenders().ToList();

            appenders.ForEach(a =>
                              {
                                  if (a is BufferingAppenderSkeleton buffered)
                                  {
                                      buffered.Flush();
                                  }
                              });
        }

        // public static long ToUnixTime(this DateTime _value)
        // {
        //     var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //
        //     return (long)(_value - sTime).TotalSeconds;
        // }
        //
        // public static DateTime FromUnixTime(this long _value)
        // {
        //     var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //     return sTime.AddSeconds(_value);
        // }
        //
        // public static long ConvertToUnixTime(DateTime datetime)
        // {
        //     DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //
        //     return (long)(datetime - sTime).TotalSeconds;
        // }
        //
        // public static DateTime UnixTimeToDateTime(long unixtime)
        // {
        //     DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //     return sTime.AddSeconds(unixtime);
        // }
        //
        // //public static DateTime FromUnixTime(this DateTime _value, long unixTime) => (UnixEpoch + TimeSpan.FromSeconds(unixTime));
        //
        // private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion
    }
}
