using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DCF.Core.Exceptions;
//using DCF.Core.Logging;
using DCF.Common.Logging;
using FluentValidation.Results;

namespace DCF.Common.Exceptions
{
    [Serializable]

    public class DCFValidationException : DCFApplicationException, IHasLogSeverity
    {
        /// <summary>
        /// Detailed list of validation errors for this exception.
        /// </summary>
        public List<ValidationResult> ValidationErrors { get; set; }
        /// <summary>
        /// Severity of the exception.
        /// Default: Warn.
        /// </summary>
        public LogLevel Severity { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        public DCFValidationException()
        {
            this.ValidationErrors = new List<ValidationResult>();
            this.Severity = LogLevel.Warn;
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public DCFValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            this.ValidationErrors = new List<ValidationResult>();
            this.Severity = LogLevel.Warn;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DCFValidationException(string message)
            : base(message)
        {
            this.ValidationErrors = new List<ValidationResult>();
            this.Severity = LogLevel.Warn;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="validationErrors">Validation errors</param>
        public DCFValidationException(string message, IEnumerable<ValidationResult> validationErrors)
            : base(message)
        {
            if (validationErrors != null)
            {
                this.ValidationErrors = validationErrors.ToList();
            }
            this.Severity = LogLevel.Warn;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DCFValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ValidationErrors = new List<ValidationResult>();
            this.Severity = LogLevel.Warn;;
        }
    }
}