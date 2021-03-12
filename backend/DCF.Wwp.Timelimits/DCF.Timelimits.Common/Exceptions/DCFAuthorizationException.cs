using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DCF.Core.Exceptions;
//using DCF.Core.Logging;
using DCF.Common.Logging;

namespace DCF.Common.Exceptions
{
    public class DCFAuthorizationException : DCFApplicationException, IHasLogSeverity, IHasHttpStatusCode
    {
        /// <summary>
        /// Severity of the exception.
        /// Default: Warn.
        /// </summary>
        public LogLevel Severity { get; set; }

        /// <summary>
        /// Creates a new <see cref="DCFAuthorizationException"/> object.
        /// </summary>
        public DCFAuthorizationException()
        {
            this.Severity = LogLevel.Warn;;
        }


        /// <summary>
        /// Creates a new <see cref="DCFAuthorizationException"/> object.
        /// </summary>
        public DCFAuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }


        /// <summary>
        /// Creates a new <see cref="DCFAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DCFAuthorizationException(string message)
            : base(message)
        {
            Severity = LogLevel.Warn;
        }

        /// <summary>
        /// Creates a new <see cref="DCFAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DCFAuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = LogLevel.Warn;
        }

        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.Unauthorized;
    }
}
