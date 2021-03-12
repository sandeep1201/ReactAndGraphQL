using System;

namespace Dcf.Wwp.Api.Common
{
    public class ErrorInfo
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public Int32 Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        public String Details { get; set; }

        /// <summary>
        /// Validation errors if exists.
        /// </summary>
        public ValidationErrorInfo[] ValidationErrors { get; set; }

        /// <summary>
        /// Additional Content to include in the error response
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        public ErrorInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        public ErrorInfo(String message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        public ErrorInfo(Int32 code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public ErrorInfo(Int32 code, String message)
            : this(message)
        {
            this.Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public ErrorInfo(String message, String details)
            : this(message)
        {
            this.Details = details;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public ErrorInfo(Int32 code, String message, String details)
            : this(message, details)
        {
            this.Code = code;
        }
    }
}