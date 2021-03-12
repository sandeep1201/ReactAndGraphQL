using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public class EndpointBehavior : IEndpointBehavior
    {
        #region Properties

        private readonly IClientMessageInspector _messageInspector;

        #endregion

        #region Methods

        public EndpointBehavior(IClientMessageInspector messageInspector)
        {
            _messageInspector = messageInspector;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(_messageInspector);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
