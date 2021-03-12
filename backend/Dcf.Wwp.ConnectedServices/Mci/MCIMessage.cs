using System.ServiceModel.Channels;
using System.Xml;
//TODO: package it
namespace Dcf.Wwp.ConnectedServices.Mci
{
    public class MciMessage : Message
    {
        #region Properties

        private readonly Message _message;

        public override MessageHeaders Headers
        {
            get => _message.Headers;
        }

        public override MessageProperties Properties
        {
            get => _message.Properties;
        }

        public override MessageVersion Version
        {
            get => _message.Version;
        }

        #endregion

        #region Methods

        public MciMessage(Message message)
        {
            _message = message;
        }

        protected override void OnWriteStartBody(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("s", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            _message.WriteBodyContents(writer);
        }

        protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
        {
            //writer.WriteStartElement("s", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            writer.WriteStartElement("s", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            writer.WriteAttributeString("xmlns", "wsa", null, "http://schemas.xmlsoap.org/ws/2004/08/addressing");
            writer.WriteAttributeString("xmlns", "wis", null, "http://cares.wisconsin.gov/WisconsinInterop");
            //writer.WriteAttributeString("xmlns", "key", null, "http://keysecurity.services.business.cares.wisconsin.gov/");
        }

        #endregion
    }
}
