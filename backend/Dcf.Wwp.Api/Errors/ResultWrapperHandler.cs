using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Common;

namespace Dcf.Wwp.Api.Errors
{
    /// <summary>
    /// Wraps Web API Return values by <see cref="JsonApiErrorResponse"/>.
    /// </summary>
    public class ResultWrapperHandler : DelegatingHandler
    {


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);
            this.WrapResultIfNeeded(request, result);
            return result;
        }

        protected virtual void WrapResultIfNeeded(HttpRequestMessage request, HttpResponseMessage response)
        {
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        return;
            //    }

            //    object resultObject;
            //    if (!response.TryGetContentValue(out resultObject) || resultObject == null)
            //    {
            //        response.StatusCode = HttpStatusCode.OK;
            //        response.Content = new ObjectContent<JsonApiErrorResponse>(
            //        new JsonApiErrorResponse()
            //        );
            //        return;
            //    }

            //    if (resultObject is JsonApiErrorResponseBase)
            //    {
            //        return;
            //    }

            //    request.CreateErrorResponse()
            //    response.Content = new ObjectContent<JsonApiErrorResponse>(
            //    new JsonApiErrorResponse(resultObject),
            //    _configuration.HttpConfiguration.Formatters.JsonFormatter
            //    );
            //}
        }
    }
}
