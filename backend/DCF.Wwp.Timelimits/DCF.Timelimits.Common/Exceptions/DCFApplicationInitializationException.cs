using System;
using System.Runtime.Serialization;

namespace DCF.Common.Exceptions
{
    [Serializable]
    public class DCFApplicationInitializationException : DCFApplicationException
    {
        #region Properties
    
        #endregion

        #region Methods

        public DCFApplicationInitializationException()
        {
        }

        public DCFApplicationInitializationException(string message) : base(message)
        {
        }

        public DCFApplicationInitializationException(string message, object details) : base(message, details)
        {
        }

        public DCFApplicationInitializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DCFApplicationInitializationException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }

        public DCFApplicationInitializationException(string message, object details, Exception innerException) : base(message, details, innerException)
        {
        }

        #endregion
    }
}
