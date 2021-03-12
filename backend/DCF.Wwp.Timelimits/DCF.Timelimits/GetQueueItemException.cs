using System;
using System.Runtime.Serialization;
using DCF.Common.Exceptions;

namespace DCF.Timelimits
{
    internal class GetQueueItemException : DCFApplicationException
    {
        public GetQueueItemException()
        {
        }

        public GetQueueItemException(String message) : base(message)
        {
        }

        public GetQueueItemException(string message, object details) : base(message, details)
        {
        }

        public GetQueueItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public GetQueueItemException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }

        public GetQueueItemException(string message, object details, Exception innerException) : base(message, details, innerException)
        {
        }
    }
}