using System;
using System.Collections.Generic;
using System.Data;
using Dcf.Wwp.Api.Library.Utils;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Dcf.Wwp.Api.Library.Logging
{
    public static class WebPerfLogger
    {
        #region Properties

        private static string _cs;

        #endregion

        #region Methods

        public static void Init(string cs)
        {
            _cs = cs;
            Configure();
        }

        public static void Configure()
        {
            var adoParms = new List<AdoNetAppenderParameter>();
            var rlc      = new RawLayoutConverter();

            var methodName = new MethodNameParameter
                             {
                                 ParameterName = "@methodName",
                                 DbType        = DbType.String,
                                 Size          = 50
                             };

            adoParms.Add(methodName);

            var start = new StartTimeParameter
                        {
                            ParameterName = "@startTime",
                            DbType        = DbType.DateTime2,
                        };

            adoParms.Add(start);

            var end = new StopTimeParameter
                      {
                          ParameterName = "@stopTime",
                          DbType        = DbType.DateTime2,
                      };

            adoParms.Add(end);

            var elapsed = new ElapsedParameter
                          {
                              ParameterName = "@elapsed",
                              DbType        = DbType.Time,
                          };

            adoParms.Add(elapsed);

            var cached = new CachedParameter
                         {
                             ParameterName = "@cached",
                             DbType        = DbType.Int32,
                             Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{cached}"))
                         };

            adoParms.Add(cached);

            var web = new WebParameter
                      {
                          ParameterName = "@web",
                          DbType        = DbType.Int32,
                          Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{web}"))
                      };

            adoParms.Add(web);

            var retries = new RetriesParameter
                      {
                          ParameterName = "@retries",
                          DbType        = DbType.Int32,
                          Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{retries}"))
                      };

            adoParms.Add(retries);

            var total = new TotalParameter
                        {
                            ParameterName = "@total",
                            DbType        = DbType.Int32,
                            Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{total}"))
                        };

            adoParms.Add(total);

            var userId = new UserIdParameter
                         {
                             ParameterName = "@userId",
                             DbType        = DbType.String,
                             Size          = 20,
                             Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%userId"))
                         };

            adoParms.Add(userId);

            var adoAppender = new AdoNetAppender
                              {
                                  BufferSize       = 1, //00,
                                  CommandType      = CommandType.StoredProcedure,
                                  CommandText      = "wwp.USP_InsertWebPerformance",
                                  ConnectionType   = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                  ConnectionString = _cs,
                                  ReconnectOnError = true,
                                  Threshold        = Level.Debug
                              };

            adoParms.ForEach(p => adoAppender.AddParameter(p));

            adoAppender.ActivateOptions();

            var hierarchy = (Hierarchy) LogManager.GetRepository("log4net-default-repository");

            var thatLog    = LogManager.GetLogger("log4net-default-repository", "Dcf.Wwp.Api.Library.ViewModels.ParticipantsViewModel");
            var thatLogger = (Logger) thatLog.Logger;
            thatLogger.Parent     = hierarchy.Root;
            thatLogger.Level      = Level.Debug;
            thatLogger.Additivity = false;
            thatLogger.AddAppender(adoAppender);
        }

        public static string GetLevel()
        {
            var hierarchy = (Hierarchy) LogManager.GetRepository(); 
            var levelName = hierarchy.Root.Level.DisplayName; //TODO: This is wrong. It's not the correct Logger, it's the Root (affects all descendants)

            return (levelName);
        }

        public static void SetLevel(string level)
        {
            var hierarchy = (Hierarchy) LogManager.GetRepository();

            switch (level.ToLower())
            {
                case "all":
                    hierarchy.Root.Level = Level.All;
                    break;

                case "debug":
                    hierarchy.Root.Level = Level.Debug;
                    break;

                case "info":
                    hierarchy.Root.Level = Level.Info;
                    break;

                case "warn":
                    hierarchy.Root.Level = Level.Warn;
                    break;

                case "error":
                    hierarchy.Root.Level = Level.Error;
                    break;

                case "off":
                    hierarchy.Root.Level = Level.Off;
                    break;

                default:
                    hierarchy.Root.Level = Level.Off;
                    break;
            }

            hierarchy.Configured = true;

            BasicConfigurator.Configure(hierarchy);
        }

        #endregion
    }

    public class MethodNameParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.MethodName;

            param.Value = formattedValue;
        }
    }

    public class StartTimeParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.StartTime;

            param.Value = formattedValue;
        }
    }

    public class StopTimeParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.StopTime;

            param.Value = formattedValue;
        }
    }

    public class ElapsedParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var dateTime       = DateTime.Today;
            var timeSpan       = new TimeSpan(0, 0, dp.Elapsed.Minutes, dp.Elapsed.Seconds, dp.Elapsed.Milliseconds); //timing.Elapsed.Duration;
            var elapsed        = dateTime + timeSpan;
            var formattedValue = elapsed;

            param.Value = formattedValue;
        }
    }

    public class CachedParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.Cached;

            param.Value = formattedValue;
        }
    }

    public class WebParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.Web;

            param.Value = formattedValue;
        }
    }

    public class RetriesParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.Retries;

            param.Value = formattedValue;
        }
    }

    public class TotalParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.Total;

            param.Value = formattedValue;
        }
    }

    public class UserIdParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var dp             = (PerfDataPoint) loggingEvent.MessageObject;
            var formattedValue = dp.UserId;

            param.Value = formattedValue;
        }
    }
}
