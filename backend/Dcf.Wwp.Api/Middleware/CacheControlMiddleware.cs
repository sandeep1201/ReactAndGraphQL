using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class CacheControlMiddlewareExtensions
    {
        public static IApplicationBuilder UseNoCacheCacheControl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheControlMiddleware>();
        }
    }
    public class CacheControlMiddleware
    {
        private readonly RequestDelegate _next;

        public CacheControlMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public  Task Invoke(HttpContext context)
        {
            var isFileRequest = context.Request.Headers["Accept"].Any(x => x.ToLower().Contains("text/html")) || System.IO.Path.HasExtension(context.Request.Path.Value);
            if (!isFileRequest)
            {
                context.Response.Headers.Remove("Cache-Control");
                context.Response.Headers.Add("Cache-Control","no-cache, no-store, must-revalidate");

            }
            return this._next.Invoke(context);
        }
    }
}
