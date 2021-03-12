using System;
using System.Linq;
using System.Threading.Tasks;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Api.Common
{
    public class JsonApiErrorResponse : JsonApiErrorResponse<object>
    {
        /// <summary>
        /// Creates an <see cref="JsonApiErrorResponse"/> object.
        /// <see cref="JsonApiErrorResponseBase.Success"/> is set as true.
        /// </summary>
        public JsonApiErrorResponse()
        {

        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="JsonApiErrorResponseBase.Success"/> specified.
        /// </summary>
        /// <param name="success">Indicates success status of the result</param>
        public JsonApiErrorResponse(bool success)
            : base(success)
        {

        }

        /// <summary>
        /// Creates an <see cref="JsonApiErrorResponse"/> object with <see cref="JsonApiErrorResponse{TResult}.Result"/> specified.
        /// <see cref="AjaxResponseBase.Success"/> is set as true.
        /// </summary>
        /// <param name="result">The actual result object</param>
        public JsonApiErrorResponse(object result)
            : base(result)
        {

        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="AjaxResponseBase.Error"/> specified.
        /// <see cref="AjaxResponseBase.Success"/> is set as false.
        /// </summary>
        /// <param name="error">Error details</param>
        /// <param name="unAuthorizedRequest">Used to indicate that the current user has no privilege to perform this request</param>
        public JsonApiErrorResponse(ErrorInfo error, bool unAuthorizedRequest = false)
            : base(error, unAuthorizedRequest)
        {

        }
    }

    public class JsonApiErrorResponse<TResult> : JsonApiErrorResponseBase
    {
        /// <summary>
        /// The actual result object of AJAX request.
        /// It is set if <see cref="AjaxResponseBase.Success"/> is true.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="Result"/> specified.
        /// <see cref="AjaxResponseBase.Success"/> is set as true.
        /// </summary>
        /// <param name="result">The actual result object of AJAX request</param>
        public JsonApiErrorResponse(TResult result)
        {
            Result = result;
            Success = true;
        }
        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object.
        /// <see cref="AjaxResponseBase.Success"/> is set as true.
        /// </summary>
        public JsonApiErrorResponse()
        {
            this.Success = true;
        }

        public JsonApiErrorResponse(ErrorInfo error, Boolean unAuthorizedRequest = false)
        {
            this.Error = error;
            this.UnAuthorizedRequest = unAuthorizedRequest;
            this.Success = false;
        }
    }
}
