using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Dcf.Wwp.ConnectedServices.Mci
{
    public class MCIMessageFormatter : IClientMessageFormatter
    {
        #region Properties

        private readonly IClientMessageFormatter _formatter;

        #endregion

        #region Methods

        public MCIMessageFormatter(IClientMessageFormatter formatter)
        {
            _formatter = formatter;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            var message = _formatter.SerializeRequest(messageVersion, parameters);

            return new MciMessage(message);
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            return _formatter.DeserializeReply(message, parameters);
        }

        #endregion
    }
}
