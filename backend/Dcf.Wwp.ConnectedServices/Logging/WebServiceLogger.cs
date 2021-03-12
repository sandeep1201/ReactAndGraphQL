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

// http://ryanwilliams.io/articles/2013/02/17/logging-with-log4net
namespace Dcf.Wwp.ConnectedServices
{
    /// <summary>
    ///     DCF custom Log4Net-SqlServer Appender
    /// </summary>
    /// <remarks>
    ///     for further info http://logging.apache.org/log4net/release/manual/introduction.html
    ///     05/01/2017 - Scott V.
    /// </remarks>
    public class WebServiceLogger
    {
        #region Properties

        #endregion

        #region Methods

        public static void Init()
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
                                  //Layout = (IRawLayout)rlc.ConvertFrom(new PatternLayout("%property{logger}"))
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

            var cs = @"Data Source=DBWMAD0D2613, 2025;initial catalog=WWPDEV;User ID=WWPDEV_APP;Password=Br0cc0l1;Integrated Security=True;MultipleActiveResultSets=True;";
            //var cs = @"Data Source=DBWMAD0D2613, 2025;initial catalog=WWPDEV;Integrated Security=True;MultipleActiveResultSets=True;;Connection Timeout=5;";
            //var cs = @"Data Source=DBWMAD0D2613, 2025;Database=WWPDEV;Integrated Security=True;MultipleActiveResultSets=True;;Connection Timeout=5;";
            //var cs = @"Data source=PNS-DSK2\SQLSVR2012;initial catalog=Logging;integrated security=true;persist security info=True;MultipleActiveResultSets=True;Connection Timeout=5;";

            var adoAppender = new AdoNetAppender
                              {
                                  BufferSize       = 1,
                                  CommandType      = CommandType.StoredProcedure,
                                  CommandText      = "wwp.USP_InsertWebServiceMessage",
                                  ConnectionType   = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                  ConnectionString = cs,
                                  ReconnectOnError = true,
                                  Threshold        = Level.Debug //TODO: make this dynamically adjustable
                              };

            //var adoAppender = new AdoNetAppender
            //                  {
            //                      BufferSize       = 1,
            //                      CommandType      = CommandType.Text,
            //                      CommandText      = "INSERT INTO wwp.SoapMessage ([MsgId], [MsgDateTime], [MsgEndpoint], [MsgOperation], [MsgDirection], [MsgCopy]) VALUES (@msgId, @msgDateTime, @msgEndpoint, @msgOperation, @msgDirection, @msgCopy)",
            //                      ConnectionType   = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
            //                      ConnectionString = cs,
            //                      ReconnectOnError = true,
            //                      Threshold        = Level.Debug //TODO: make this dynamically adjustable
            //                  };

            adoParms.ForEach(p => adoAppender.AddParameter(p));

            adoAppender.ActivateOptions();

            var hierarchy = (Hierarchy) LogManager.GetRepository();
            hierarchy.Root.AddAppender(adoAppender);
            hierarchy.Root.Level = Level.Debug; //TODO: make this dynamically adjustable
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
            var message     = loggingEvent.MessageObject;
            var xeMessage   = XElement.Parse(message.ToString());
            var xeMessageId = xeMessage.Descendants(NsWsa + "MessageID").FirstOrDefault();
            var guid        = (xeMessageId != null ? (Guid) xeMessageId : Guid.Empty)  ;

            var formattedValue = guid; //.ToString("D");

            param.Value = formattedValue;
        }
    }

    public class MsgDateTimeParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var message        = loggingEvent.MessageObject;
            var xeMessage      = XElement.Parse(message.ToString());
            var xeDateTime     = xeMessage.Descendants(NsWsu + "Created").FirstOrDefault();
            var formattedValue = (xeDateTime != null ? (DateTime) xeDateTime : DateTime.Now);

            param.Value = formattedValue;
        }
    }

    public class MsgEndpointParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param          = (IDbDataParameter) command.Parameters[ParameterName];
            var message        = loggingEvent.MessageObject;
            var xeMessage      = XElement.Parse(message.ToString());
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
                //var xe     = xeBody?.Elements().FirstOrDefault();
                formattedValue = string.IsNullOrEmpty(xeBody?.Name.NamespaceName) ? "Undetermined" : xeBody.Name.NamespaceName;
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
            var message        = loggingEvent.MessageObject;
            var xeMessage      = XElement.Parse(message.ToString());

            var xnsEnv      = xeMessage.Name.NamespaceName == NsSoap11Env.NamespaceName ? NsSoap11Env : NsSoap12Env;
            var xeOperation = xeMessage.Descendants(xnsEnv + "Body").Elements().FirstOrDefault();
            formattedValue = string.IsNullOrEmpty(xeOperation?.Name.LocalName) ? "Undetermined" : xeOperation.Name.LocalName;

            param.Value = formattedValue;
        }
    }

    public class MsgDirectionParameter : WcfAdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            var param     = (IDbDataParameter) command.Parameters[ParameterName];
            var message   = loggingEvent.MessageObject;
            var xeMessage = XElement.Parse(message.ToString());

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
            var formattedValue = loggingEvent.MessageObject.ToString();

            param.Value = formattedValue;
        }
    }

    //public class MsgXmlParameter : WcfAdoNetAppenderParameter
    //{
    //    public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
    //    {
    //        var param          = (IDbDataParameter) command.Parameters[ParameterName];
    //        var message        = loggingEvent.MessageObject;
    //        var xeMessage      = XElement.Parse(message.ToString());
    //        var formattedValue = xeMessage.ToString();

    //        param.Value = formattedValue;
    //    }
    //}
}
