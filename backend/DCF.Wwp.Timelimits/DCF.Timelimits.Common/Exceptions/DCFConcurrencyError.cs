using System;
using System.Net;
using System.Runtime.Serialization;
using DCF.Core.Exceptions;
// using DCF.Core.Logging;
using DCF.Common.Logging;

namespace DCF.Common.Exceptions
{
    public class DCFConcurrencyError : DCFApplicationException, IHasLogSeverity,IHasErrorCode, IHasHttpStatusCode
    {
        public LogLevel Severity { get; set; } = LogLevel.Warn;
        public Int32 Code { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.Conflict;

        public DCFConcurrencyError()
        {
        }

        public DCFConcurrencyError(string message) : base(message)
        {
        }

        public DCFConcurrencyError(string message, object details) : base(message, details)
        {
        }

        public DCFConcurrencyError(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DCFConcurrencyError(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }

        public DCFConcurrencyError(string message, object details, Exception innerException) : base(message, details, innerException)
        {
        }
    }
}