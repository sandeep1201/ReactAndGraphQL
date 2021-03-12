using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    [DataContract(Name = "WisconsinInterop", Namespace = "http://cares.wisconsin.gov/WisconsinInterop")]
    public class WisconsinInteropHeader : MessageHeader
    {
        #region Properties

        public string NamespaceAlias  { get; set; }
        public string UserAcctID      { get; set; } // req'd by Deloitte, but must remain blank (no idea why it exists then...lol)
        public string ConsumerAppName { get; set; }
        public string Timeout         { get; set; }
        public override string Name   { get; }
        public override string Namespace { get; }

        //private string Namespace { get; set; }

        #endregion

        #region Methods

        public WisconsinInteropHeader()
        {
            Name = "WisconsinInterop";
            Namespace = "http://cares.wisconsin.gov/WisconsinInterop";
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteElementString(HeaderValues.NsAlias, "UserAcctID",      HeaderValues.HeaderNamespace, UserAcctID);
            writer.WriteElementString(HeaderValues.NsAlias, "ConsumerAppName", HeaderValues.HeaderNamespace, "WWP");
            writer.WriteElementString(HeaderValues.NsAlias, "TimeOut",         HeaderValues.HeaderNamespace, "60");
        }

        public static WisconsinInteropHeader ReadHeader(XmlDictionaryReader reader)
        {
            if (reader.ReadToDescendant("UserAcctID", HeaderValues.HeaderNamespace))
            {
                return (new WisconsinInteropHeader());
            }

            return (null);
        }

        #endregion
    }

    public static class HeaderValues
    {
        public const string NsAlias          = "wis";
        public const string HeaderNamespace  = "http://cares.wisconsin.gov/WisconsinInterop";
    }
}

// produces (for soap header):
//<wis:WisconsinInterop>
//  <wis:UserAcctID></wis:UserAcctID>
//  <wis:ConsumerAppName>WWP</wis:ConsumerAppName>
//  <wis:Timeout>60</wis:Timeout>
//</wis:WisconsinInterop>
