using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DCF.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Serilog;
using Serilog.Events;

namespace Microsoft.AspNetCore.Builder
{
    public static class RequestResponseLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next)
        {
            _next = next;
            _logger = Log.Logger.ForContext<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            // Avoid logging file system requests
            var isFileRequest = context.Request.Headers["Accept"].Any(x => x.ToLower().Contains("text/html")) || System.IO.Path.HasExtension(context.Request.Path.Value);

            if (isFileRequest || !this._logger.IsEnabled(LogEventLevel.Debug) || (context.Request.Method != HttpMethods.Get && context.Request.Method != HttpMethods.Post))
            {
                await this._next(context);
                return;
            }

            
            
            // Log the request at the start of the 
            var requestObj = await this.GetRequestInfo(context.Request).ConfigureAwait(false);
            this._logger.Information("Request Recieved: {@RequestObj}", requestObj);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context).ConfigureAwait(false);


                var responsObj = await this.GetResponseInfo(context.Response).ConfigureAwait(false);
                if (!responsObj.IsNullOrWhiteSpace())
                {
                    this._logger.Information("Sending Response: {@data} ", responsObj); 
                    await responseBody.CopyToAsync(originalBodyStream).ConfigureAwait(false);
                }
            }
        }

        private Task<Object> GetRequestInfo(HttpRequest request)
        {
            var injectedRequestStream = new MemoryStream();
            if (request == null)
            {
                return null;
            }

            String bodyAsText = null;
            using (var bodyReader = new StreamReader(request.Body))
            {
                bodyAsText = bodyReader.ReadToEnd();

                var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                request.Body = injectedRequestStream;
            }

            return Task.FromResult<Object>(new RequestInfo { Scheme = request.Scheme, Host = request.Host.Host, Path = request.Path, Method = request.Method, Body = bodyAsText });

        }

        private async Task<String> GetResponseInfo(HttpResponse response)
        {
            String text = null;
            if (response.ContentType.Contains("application/json"))
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                text = await new StreamReader(response.Body).ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
            }
            return text;
        }

        public class RequestInfo
        {
            public String Scheme { get; set; }
            public String Host { get; set; }
            public String Path { get; set; }
            public String Method { get; set; }
            public String Body { get; set; }
        }
    }
}