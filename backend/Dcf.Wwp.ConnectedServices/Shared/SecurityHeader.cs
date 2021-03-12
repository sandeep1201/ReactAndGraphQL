using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    [DataContract(Name = "Security", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class SecurityHeader : MessageHeader
    {
        // Full specification can be find here http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0.pdf

        #region Properties

        private const string WssePrefix       = "wsse";
        private const string WsseName         = "Security";
        private const string WsseNamespace    = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string WsuPrefix        = "wsu";
        private const string WsuNamespace     = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string DeloitteRequired = "DeloitteRequired";

        private readonly string _uid;
        private readonly string _pwd;

        public override string Name      { get; }
        public override string Namespace { get; }

        #endregion

        #region Methods

        public SecurityHeader(string uid , string pwd)
        {
            Name      = WsseName;
            Namespace = WsseNamespace;

            _uid = uid;
            _pwd = pwd;
        }

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement(WssePrefix, WsseName, WsseNamespace);
            writer.WriteAttributeString(WssePrefix, DeloitteRequired, WsseNamespace, "this-has-to-be-here-too"); // leave this line in or Deloitte's auth won't work
            writer.WriteAttributeString(WsuPrefix , DeloitteRequired, WsuNamespace,  "this-has-to-be-here-too"); // leave this line in or Deloitte's auth won't work

            //TODO: revisit this later
            //writer.WriteAttributeString(WssePrefix, WsseNamespace); // leave this line in or Deloitte's auth won't work
            //writer.WriteAttributeString(WsuPrefix, WsuNamespace);   // leave this line in or Deloitte's auth won't work
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            var usernameToken = new UsernameToken(_uid, _pwd);
            var xeUserToken   = usernameToken.GetXml();

            xeUserToken.WriteTo(writer);
        }

        #endregion
    }
}

// produces (Deloitte required formatting for soap header):
//    <wsse:Security xmlns:wsse="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd" xmlns:wsu="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
//      <wsse:UsernameToken wsu:Id="UsernameToken-3300475d-5f0f-432a-91dd-a4475dca6b19">
//          <wsse:Username>cwwwwpkeysrvcuser</wsse:Username>
//          <wsse:Password Type = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest">zJf/0n+Uth8jQPflhmFLTV1LOk=</wsse:Password>
//          <wsse:Nonce EncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">2U6jkBLMJKD7dmwzB43COg==</wsse:Nonce>
//          <wsu:Created>2018-09-20T00:48:47Z</wsu:Created>
//      </wsse:UsernameToken>
//    </wsse:Security>

// and see: http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0.pdf
// and see: https://stackoverflow.com/questions/25776403/wcf-create-usernametoken-with-timestamp-and-password-digest-for-oasis-200401-w?rq=1
// and see: https://stackoverflow.com/questions/896901/wcf-adding-nonce-to-usernametoken
// and see: https://stackoverflow.com/questions/16028014/how-can-i-pass-a-username-password-in-the-header-to-a-soap-wcf-service <------ 
// and see: https://stackoverflow.com/questions/3102693/error-in-wcf-client-consuming-axis-2-web-service-with-ws-security-usernametoken-p
