using System;
using System.Data;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Common;
using DCF.Common.Exceptions;
using DCF.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Serilog;

namespace Dcf.Wwp.Api.Errors
{
    /// <summary>
    /// Exception Handler class will format and return exceptions that we can handle from the API in a standard format.
    /// ***ExceptionHandler only works in APS.NET, NOT CORE!
    /// Some errors cannont be handled:
    /// - Exceptions thrown from controller constructors.
    /// - Exceptions thrown from message handlers.
    /// - Exceptions thrown during routing.
    /// - Exceptions thrown during response content serialization.
    /// </summary>
    public class ApiExceptionHandler : IApiExceptionHandler
    {
        private readonly IErrorInfoConverter _errorInfoConverter;
        private readonly ILogger _logger;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;
        private JsonSerializerSettings _jsonSerializerSettings;

        public ApiExceptionHandler(IErrorInfoConverter errorInfoConverter, JsonSerializerSettings serializerSettings)
        {
            this._errorInfoConverter = errorInfoConverter;
            _logger = Log.ForContext<ApiExceptionHandler>();
            _clearCacheHeadersDelegate = ClearCacheHeaders;

            // Clone so we don't mess with global settings
            this._jsonSerializerSettings = new JsonSerializerSettings
                                           {
                                               ContractResolver      = serializerSettings.ContractResolver,
                                               NullValueHandling     = serializerSettings.NullValueHandling,
                                               DateFormatHandling    = serializerSettings.DateFormatHandling,
                                               DateTimeZoneHandling  = serializerSettings.DateTimeZoneHandling,
                                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                           };
        }
        public Task Handle(HttpContext context, ExceptionDispatchInfo dispatchInfo)
        {
            // 1.  Capture thrown context and Log Error
            var ex = dispatchInfo.SourceException;
            var errorInfoObj = this._errorInfoConverter.Convert(ex);
            _logger.Error(ex, "Unhandled Global error occured. {message}, {@errorInfo}", errorInfoObj.Message, errorInfoObj);

            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
            {
                _logger.Fatal(ex, "Global Exception Handler \"Response Already Started!\" Error occured.");
                dispatchInfo.Throw();
            }
            try
            {
                context.Response.Clear();
                context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

                var statusCode            = ApiExceptionHandler.GetStatusCode(ex);
                var isUnauthorizedRequest = statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.Forbidden;
                var responseObj           = new JsonApiErrorResponse(errorInfoObj, isUnauthorizedRequest);

                var responseBody = JsonConvert.SerializeObject(responseObj);
                context.Response.StatusCode = (Int32)statusCode;
                return context.Response.WriteAsync(responseBody);
                
            }
            catch (Exception ex2)
            {
                // Suppress secondary exceptions, re-throw the original.
                _logger.Fatal(ex2, "Global Exception Handler Error occured. {message}, {@errorInfo}", ex2.Message, errorInfoObj);
            }
            dispatchInfo.Throw(); // Re-throw the original if we couldn't handle it
            return Task.CompletedTask;
        }

        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }

        public static HttpStatusCode GetStatusCode(Exception ex)
        {
            if (ex is IHasHttpStatusCode)
            {
                return ((IHasHttpStatusCode)ex).HttpStatusCode;
            }

            if (ex is DCFValidationException)
            {
                return HttpStatusCode.BadRequest;
            }

            if (ex is EntityNotFoundException)
            {
                return HttpStatusCode.NotFound;
            }

            if (ex is DBConcurrencyException || ex is DCFConcurrencyError)
            {
                return HttpStatusCode.Conflict;
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}