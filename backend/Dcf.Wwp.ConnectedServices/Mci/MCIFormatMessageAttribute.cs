using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Dcf.Wwp.ConnectedServices.Mci
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MCIFormatMessageAttribute : Attribute, IOperationBehavior
    {
        #region Properties

        #endregion

        #region Methods

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            var serializerBehavior = operationDescription.Behaviors.Find<XmlSerializerOperationBehavior>();

            if (clientOperation.Formatter == null)
            {
                ((IOperationBehavior)serializerBehavior).ApplyClientBehavior(operationDescription, clientOperation);
            }

            var innerClientFormatter = clientOperation.Formatter;
            clientOperation.Formatter = new MCIMessageFormatter(innerClientFormatter);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }
}
