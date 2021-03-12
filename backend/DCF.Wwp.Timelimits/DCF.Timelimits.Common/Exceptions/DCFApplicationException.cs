using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DCF.Core;
using EnsureThat;

namespace DCF.Common.Exceptions
{
    [Serializable]
    public class DCFApplicationException : Exception
    {
        // ReSharper disable once StyleCop.SA1310
        private const String APPLICATION_DETAILS_VALUE = "AppExDetails";
        public IDictionary<String, Object> Details { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DCFApplicationException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ApplicationException"/> object.
        /// </summary>
        public DCFApplicationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            this.Details = (IDictionary<String, Object>) serializationInfo.GetValue(DCFApplicationException.APPLICATION_DETAILS_VALUE, typeof(IDictionary<String, Object>));
        }

        /// <summary>
        /// Creates a new <see cref="ApplicationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DCFApplicationException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ApplicationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DCFApplicationException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DCFApplicationException(String message, Object details)
            : base(message)
        {
            Ensure.That(details, nameof(details)).IsNotNull();
            this.Details = details.ToDictionary();
        }

        public DCFApplicationException(String message, Object details, Exception innerException)
            : base(message, innerException)
        {
            Ensure.That(details, nameof(details)).IsNotNull();
            this.Details = details.ToDictionary();
        }


        public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext context)
        {
            serializationInfo.AddValue(DCFApplicationException.APPLICATION_DETAILS_VALUE, this.Details);
            base.GetObjectData(serializationInfo, context);
        }
    }
}