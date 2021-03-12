using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Dcf.Wwp.Api.Middleware
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            return app;
        }
    }

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;
        private readonly IApiExceptionHandler _apiExceptionHandler;
        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IApiExceptionHandler apiExceptionHandler)
        {
            _next = next;
            _logger = Log.ForContext<ExceptionHandlerMiddleware>();
            _apiExceptionHandler = apiExceptionHandler;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                var dispatchInfo = ExceptionDispatchInfo.Capture(ex);
                await _apiExceptionHandler.Handle(context, dispatchInfo);

                //var dispatchInfo = ExceptionDispatchInfo.Capture(ex);

                //try
                //{
                //}
                //catch (Exception ex2)
                //{
                //    // Suppress secondary exceptions, re-throw the original.
                //    _logger.Fatal(ex2, "Global Exception Handler Error occured. {message}, {@errorInfo}");
                //}
                //throw; // Re-throw the original if we couldn't handle it
            }
        }
    }
}
