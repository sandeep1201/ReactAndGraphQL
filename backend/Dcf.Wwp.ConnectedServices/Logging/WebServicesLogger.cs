using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Dcf.Wwp.ConnectedServices.Logging
{
    public static class WebServicesLogger
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

            var msgId = new MsgIdParameter
                        {
                            ParameterName = "@msgId",
                            DbType        = DbType.Guid,
                            Size          = 50,
                            Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%msgId"))
                        };

            adoParms.Add(msgId);

            var msgDateTime = new MsgDateTimeParameter
                              {
                                  ParameterName = "@msgDateTime",
                                  DbType        = DbType.DateTime2,
                                  Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{msgDateTime}"))
                              };

            adoParms.Add(msgDateTime);

            var msgEndpoint = new MsgEndpointParameter
                              {
                                  ParameterName = "@msgEndpoint",
                                  DbType        = DbType.String,
                                  Size          = 75
                              };

            adoParms.Add(msgEndpoint);

            var msgOperation = new MsgOperationParameter
                               {
                                   ParameterName = "@msgOperation",
                                   DbType        = DbType.String,
                                   Size          = 75,
                                   Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{msgOperation}"))
                               };

            adoParms.Add(msgOperation);

            var msgDirection = new MsgDirectionParameter
                               {
                                   ParameterName = "@msgDirection",
                                   DbType        = DbType.String,
                                   Size          = 1,
                                   Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%property{msgDirection}"))
                               };

            adoParms.Add(msgDirection);

            var msgXml = new MsgXmlParameter
                         {
                             ParameterName = "@msgXml",
                             DbType        = DbType.String,
                             Size          = 4000,
                             Layout        = (IRawLayout) rlc.ConvertFrom(new PatternLayout("%msgXml"))
                         };

            adoParms.Add(msgXml);

            var adoAppender = new AdoNetAppender
                              {
                                  BufferSize       = 1, //00,
                                  CommandType      = CommandType.StoredProcedure,
                                  CommandText      = "wwp.USP_InsertWebServiceMessage",
                                  ConnectionType   = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                  ConnectionString = _cs,
                                  ReconnectOnError = true,
                                  Threshold        = Level.Debug
                              };

            // Below is to configure Log4Net to use dynamic-SQL, but
            // we're using stored proc for speed and better flexibility

            //var adoAppender = new AdoNetAppender
            //                  {
            //                      BufferSize       = 1,
            //                      CommandType      = CommandType.Text,
            //                      CommandText      = "INSERT INTO wwp.SoapMessage ([MsgId], [MsgDateTime], [MsgEndpoint], [MsgOperation], [MsgDirection], [MsgXml]) VALUES (@msgId, @msgDateTime, @msgEndpoint, @msgOperation, @msgDirection, @msgXml)",
            //                      ConnectionType   = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
            //                      ConnectionString = cs,
            //                      ReconnectOnError = true,
            //                      Threshold        = Level.Debug
            //                  };

            adoParms.ForEach(p => adoAppender.AddParameter(p));

            adoAppender.ActivateOptions();

            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var thisLog    = LogManager.GetLogger("log4net-default-repository", "Dcf.Wwp.ConnectedServices.Shared.MessageInspector");
            var thisLogger = (Logger) thisLog.Logger;
            thisLogger.Parent     = hierarchy.Root;
            thisLogger.Level      = Level.Debug;
            thisLogger.Additivity = false;
            thisLogger.AddAppender(adoAppender);

            //hierarchy.Root.AddAppender(adoAppender);
            //hierarchy.Root.Level = Level.Debug;
            //hierarchy.Configured = true;

            //TODO: add another appender to log real errors to wwp.LogEvent separately 

            //BasicConfigurator.Configure(hierarchy);
        }

        public static string GetLevel()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var levelName = hierarchy.Root.Level.DisplayName;   //TODO: this is not the correct logger to throttle - fix it! ~ lol

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

    public class MsgIdParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param       = (IDbDataParameter) command.Parameters[ParameterName];
            var message     = loggingEvent.RenderedMessage;
            var xeMessage   = XElement.Parse(message);
            var xeMessageId = xeMessage.Descendants(NsWsa + "MessageID").FirstOrDefault();
            var guid        = (xeMessageId != null ? (Guid) xeMessageId : Guid.Empty)  ;

            var formattedValue = guid;

            param.Value = formattedValue;
        }
    }

    public class MsgDateTimeParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var message        = loggingEvent.RenderedMessage;
            var xeMessage      = XElement.Parse(message);
            var xeDateTime     = xeMessage.Descendants(NsWsu + "Created").FirstOrDefault();
            var formattedValue = (xeDateTime != null ? (DateTime) xeDateTime : DateTime.Now.ToUniversalTime());

            param.Value = formattedValue;
        }
    }

    public class MsgEndpointParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var message        = loggingEvent.RenderedMessage;
            var xeMessage      = XElement.Parse(message);
            var formattedValue = string.Empty;

            var xnsEnv = xeMessage.Name.NamespaceName == NsSoap11Env.NamespaceName ? NsSoap11Env : NsSoap12Env;

            if (xeMessage.Name.NamespaceName == NsSoap11Env.NamespaceName) // quick fix
            {
                var xeBody = xeMessage.Descendants(xnsEnv + "Body").Elements().FirstOrDefault();
                var xe     = xeBody?.Elements().FirstOrDefault();
                formattedValue = string.IsNullOrEmpty(xe?.Name.NamespaceName) ? "Undetermined" : xe.Name.NamespaceName;
            }
            else
            {
                var xeBody = xeMessage.Descendants(xnsEnv + "Body").Elements().FirstOrDefault();
                formattedValue = string.IsNullOrEmpty(xeBody?.Name.NamespaceName) ? "Undetermined" : xeBody.Name.NamespaceName;
            }

            if (formattedValue == "Undetermined")
            {
                var z = 0;
            }

            param.Value = formattedValue;
        }
    }

    public class MsgOperationParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var formattedValue = string.Empty;
            var message        = loggingEvent.RenderedMessage;
            var xeMessage      = XElement.Parse(message.ToString());

            var xnsEnv      = xeMessage.Name.NamespaceName == NsSoap11Env.NamespaceName ? NsSoap11Env : NsSoap12Env;
            var xeOperation = xeMessage.Descendants(xnsEnv + "Body").Elements().FirstOrDefault();
            formattedValue = string.IsNullOrEmpty(xeOperation?.Name.LocalName) ? "Undetermined" : xeOperation.Name.LocalName;

            if (formattedValue == "Undetermined")
            {
                var z = 0;
            }

            param.Value = formattedValue;
        }
    }

    public class MsgDirectionParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param     = (IDbDataParameter) command.Parameters[ParameterName];
            var message   = loggingEvent.RenderedMessage;
            var xeMessage = XElement.Parse(message);

            // determine if it's SOAP 1.1 or 1.2 envelope so we know what namespace to use, and what to look for, where...
            var ns = xeMessage.Name.NamespaceName == NsSoap11Env.NamespaceName ? NsSoap11Env : NsSoap12Env;

            // Replies from Deloitte don't have headers, so if there's a header, it's a request
            var hasHeader      = xeMessage.Element(ns + "Header") != null;
            var formattedValue = (hasHeader ? "O" : "I");
            param.Value = formattedValue;
        }
    }

    public class MsgXmlParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var formattedValue = loggingEvent.RenderedMessage;

            param.Value = formattedValue;
        }
    }
}
