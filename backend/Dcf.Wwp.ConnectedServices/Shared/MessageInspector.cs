using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using log4net;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public class MessageInspector : IClientMessageInspector
    {
        #region Properties

        private static readonly ILog _log = LogManager.GetLogger(typeof(MessageInspector));

        private readonly XNamespace _nsWas = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
        private readonly string     _toHeader;
        private readonly string     _uid ;
        private readonly string     _pwd ;
        private          Guid       _msgIdGuid;

        private readonly string[] _ssnElementsToMask = { "SSNNumber", "AliasSSN", "CorrectedSSN", "SSN" };

        //private readonly string   _ssnMask           = "*".PadRight(9, '*');  // unnecessarily eats up too much SQLSVR space
        private readonly string _ssnMask = "*"; // so just use 1 char.

        #endregion

        #region Methods

        public MessageInspector(string uid, string pwd, string toHeader = null)
        {
            _uid      = uid;
            _pwd      = pwd;
            _toHeader = toHeader; // only used for MCI Soap 1.1 calls
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // A WCF message can only be consumed (read) once.
            // So create a copy for logging, and then another
            // copy to reassign BACK to the original (bcs the
            // the first copy operation 'read' the message,
            // see how that works? lol)

            // then insert the MessageId that Deloitte doesn't 
            // always return, and mask any SSNs on the msg copy
            // that gets logged to SqlSvr.

            _msgIdGuid = Guid.NewGuid();

            request.Headers.Clear();
            request.Headers.Add(MessageHeader.CreateHeader("MessageID", _nsWas.NamespaceName, _msgIdGuid));
            request.Headers.Add(new WisconsinInteropHeader());
            request.Headers.Add(new SecurityHeader(_uid, _pwd));

            var v = request.Version; // check if...

            if (Equals(v, MessageVersion.Soap11))
            {
                // it's an MCI Soap 1.1 call, insert the '<wsa:TO>' header manually
                request.Headers.Add(MessageHeader.CreateHeader("To", _nsWas.NamespaceName, _toHeader));
            }

            var buffer = request.CreateBufferedCopy(int.MaxValue);
            var copy   = buffer.CreateMessage(); // create a copy for logging 
            request = buffer.CreateMessage();    // assign a copy back to original

            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb))
            {
                copy.WriteMessage(xw);
                xw.Close();
            }

            var xe = XElement.Parse(sb.ToString());

            var xeSSNs = xe.Descendants()
                           .Where(i => _ssnElementsToMask.Contains(i.Name.LocalName))
                           .Select(i => i)
                           .ToList();

            xeSSNs.ForEach(i => i.Value = _ssnMask);

            _log.DebugFormat("{0}", xe);

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // A WCF message can only be consumed (read) once.
            // So create a copy for logging, and then another
            // copy to reassign BACK to the original (bcs the
            // the first copy operation 'read' the message,
            // see how that works? lol)

            // then insert the MessageId that Deloitte doesn't 
            // always return, and mask any SSNs on the msg copy
            // that gets logged to SqlSvr.

            var buffer = reply.CreateBufferedCopy(int.MaxValue);
            var copy   = buffer.CreateMessage(); // create a copy for logging 
            reply = buffer.CreateMessage();      // assign a copy back to original

            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb))
            {
                copy.WriteMessage(xw);
                xw.Close();
            }

            var xe = XElement.Parse(sb.ToString());
            xe.AddFirst(new XElement(_nsWas + "MessageID", _msgIdGuid)); // _msgIdGuid is set in BeforeSendRequest() 

            var xeSSNs = xe.Descendants()
                           .Where(i => _ssnElementsToMask.Contains(i.Name.LocalName))
                           .Select(i => i)
                           .ToList();

            if (xeSSNs != null && xeSSNs.Any())
            {
                xeSSNs.ForEach(i => i.Value = _ssnMask);
            }

            _log.Debug(xe);
        }

        #endregion
    }
}
