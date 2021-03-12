using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Auth;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Timelimits.Rules.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Microsoft.AspNetCore.Builder
{
    public class ApplicationContextMiddleware
    {
        private ILogger _logger;
        private RequestDelegate _next;

        public ApplicationContextMiddleware(RequestDelegate next)
        {
            _next = next;
            this._logger = Log.Logger;
        }

        public async Task Invoke(HttpContext context, ApplicationContext appContext)
        {
            if (context.Request.Method != HttpMethods.Options && this.IsAuthorizedToModifyTime(context))
            {
                DateTime simulatedDateTime;
                if (context.Request.Headers.ContainsKey("simulated_datetime") && DateTime.TryParseExact(context.Request.Headers["simulated_datetime"], "u", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out simulatedDateTime))
                {
                    appContext.Date = simulatedDateTime;
                }
            }

            await this._next(context).ConfigureAwait(false);
        }

        private Boolean IsAuthorizedToModifyTime(HttpContext context)
        {
            var authorizationClaims = context.User.FindAll(x => x.Type == "authorizations");
            var hasSimulateDateTimePermision = authorizationClaims?.Any(x=>x.Type == "canSimulateDateTime");

            return hasSimulateDateTimePermision.GetValueOrDefault();
        }
    }

    public static class ApplicationContextMiddlewareExtension
    {
        public static IApplicationBuilder UseApplicationContext(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApplicationContextMiddleware>();
            return app;
        }
    }
}
