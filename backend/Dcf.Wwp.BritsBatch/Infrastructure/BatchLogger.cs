using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Dcf.Wwp.BritsBatch.Models;

namespace Dcf.Wwp.BritsBatch.Infrastructure
{
    public static class BatchLogger
    {
        #region Properties

        #endregion

        #region Methods

        //public static void Init(string cs)
        //{
        //    _cs = cs;
        //    Configure();
        //}

        public static void Configure()
        {
            var adoParms = new List<AdoNetAppenderParameter>();

            var messageParm = new MessageParameter
                              {
                                  ParameterName = "@message",
                                  DbType        = DbType.String,
                                  Size          = 50
                              };

            adoParms.Add(messageParm);

            //var messageTemplateParm = new MessageTemplateParameter
            //            {
            //                ParameterName = "@messageTemplate",
            //                DbType        = DbType.String,
            //            };

            //adoParms.Add(messageTemplateParm);

            var levelParm = new LevelParameter
                            {
                                ParameterName = "@level",
                                DbType        = DbType.Byte
                            };

            adoParms.Add(levelParm);

            var timestampParm = new TimestampParameter
                                {
                                    ParameterName = "@timestamp",
                                    DbType        = DbType.DateTimeOffset
                                };

            adoParms.Add(timestampParm);

            var exceptionParm = new ExceptionParameter
                                {
                                    ParameterName = "@exception",
                                    DbType        = DbType.String
                                };

            adoParms.Add(exceptionParm);

            var propertiesParm = new PropertiesParameter
            {
                ParameterName = "@properties",
                DbType = DbType.Xml,
            };

            adoParms.Add(propertiesParm);

            var logEventParm = new LogEventParameter
                               {
                                   ParameterName = "@logEvent",
                                   DbType        = DbType.String
                               };

            adoParms.Add(logEventParm);

            var adoAppender = new AdoNetAppender
                              {
                                  BufferSize       = 1, //00,
                                  CommandType      = CommandType.StoredProcedure,
                                  CommandText      = "wwp.USP_InsertIntoLogEvent",
                                  ConnectionType   = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                  ConnectionString = _cs,
                                  ReconnectOnError = true,
                                  Threshold        = Level.Debug
                              };

            adoParms.ForEach(p => adoAppender.AddParameter(p));

            adoAppender.ActivateOptions();

            var logRepo = LogManager.CreateRepository("log4net-default-repository");
            var hierarchy = (Hierarchy)LogManager.GetRepository("log4net-default-repository");

            //var allRepos = LogManager.GetAllRepositories();
            //var glr = LogManager.GetLoggerRepository("log4net-default-repository");

            ////var log = LogManager.GetLogger("log4net-default-repository", "Dcf.Wwp.BritsBatch.WWP00");
            //var log = LogManager.GetLogger(log4net-default-repository", "Dcf.Wwp.BritsBatch.Models");
            var log = LogManager.GetLogger(typeof(Program));
            //var log    = LogManager.GetLogger("log4net-default-repository");
            var logger = (Logger)log.Logger;
            logger.Parent = hierarchy.Root;
            logger.Level = Level.Debug;
            logger.Additivity = false;
            logger.AddAppender(adoAppender);

            LogLog.InternalDebugging = true;

            BasicConfigurator.Configure(hierarchy);

        }

        #endregion
    }

    public class MessageParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            
            var a0 = AppDomain.CurrentDomain.FriendlyName;
            
            var appName        = AppDomain.CurrentDomain.FriendlyName;

            //var appName        = loggingEvent.Domain.Split('.')[0].ToUpper();

            var formattedValue = appName;

            param.Value = formattedValue;
        }
    }

    public class MessageTemplateParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var formattedValue = "messageTemplateParm";

            param.Value = formattedValue;
        }
    }

    public class LevelParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var formattedValue = 0;

            // translate Log4Net's enums to Serilog's (didn't have time to use Serilog)
            switch (loggingEvent.Level.Value)
            {
                case 30000:
                    formattedValue = 1; // Debug
                    //formattedValue = loggingEvent.Level.Value / 10000;
                    break;

                case 40000:
                    formattedValue = 2; // Info
                    //formattedValue = loggingEvent.Level.Value / 10000;
                    break;

                case 60000:
                    formattedValue = 3; // Warn
                    //formattedValue = loggingEvent.Level.Value / 10000;
                    break;

                case 70000:
                    formattedValue = 4; // Error
                    //formattedValue = loggingEvent.Level.Value / 10000;
                    break;

                case 110000:
                    formattedValue = 5; // Fatal
                    //formattedValue = loggingEvent.Level.Value / 10000;
                    break;

                default:
                    formattedValue = 0;
                    break;
            }

            param.Value = formattedValue;
        }
    }

    public class TimestampParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var timestamp      = DateTimeOffset.Now;
            var formattedValue = timestamp;

            param.Value = formattedValue;
        }
    }

    public class ExceptionParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var formattedValue = "n/a"; //string.Empty;

            if (loggingEvent.MessageObject is Exception exception)
            {
                formattedValue = exception.ToString();
            }
            else
            {
                if (loggingEvent.MessageObject is Test)
                {
                    formattedValue = "props";
                }
            }

            param.Value = formattedValue;
        }
    }

    public class PropertiesParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param = (IDbDataParameter) command.Parameters[ParameterName];
            var mo             = loggingEvent.MessageObject;

            var xml = string.Empty;

            var xsSubmit = new XmlSerializer(mo.GetType());

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, mo);
                    xml = sww.ToString();
                }
            }

            var formattedValue = xml;

            param.Value = formattedValue;
        }
    }

    public class LogEventParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var formattedValue = string.Empty;

            if (loggingEvent.MessageObject is Exception)
            {
                formattedValue = ((Exception) loggingEvent.MessageObject).StackTrace; //TODO: could be an inner exception, check for that
            }
            else
            {
                if (loggingEvent.MessageObject is Test)
                {
                    formattedValue = "props";
                }
                else
                {
                    formattedValue = (string) loggingEvent.MessageObject;
                }
                
            }

            param.Value = formattedValue;
        }
    }
}
