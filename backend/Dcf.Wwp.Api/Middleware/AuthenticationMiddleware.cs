using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Dcf.Wwp.Model.Interface.Core;


namespace Dcf.Wwp.Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthUser authUser)
        {
            if (context.User?.Identity != null)
            {
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    authUser.Username       = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    authUser.MainFrameId    = context.User.Claims.Where(x => x.Type == "mainframe_id").Select(y => y.Value).FirstOrDefault();
                    authUser.AgencyCode     = context.User.Claims.Where(x => x.Type == "agency").Select(y => y.Value).FirstOrDefault();
                    authUser.Authorizations = context.User.Claims.Where(x => x.Type == "authorizations").Select(y => y.Value).ToList();
                    authUser.WIUID          = context.User.Claims.Where(c => c.Type == "wiuid").Select(y => y.Value).FirstOrDefault();

                    var cdoDate = context.User.Claims.Where(c => c.Type == "cdo_date").Select(y => y.Value).FirstOrDefault();
                    if (cdoDate != null)
                        authUser.CDODate = DateTime.Parse(cdoDate);
                }
                else
                {
                    // TODO: we have a identity but it isn't valid, maybe do something special here
                }
            }

            using (LogContext.PushProperty("Username", authUser.Username))
            {
                await _next.Invoke(context);
            }
        }
    }

    #region ExtensionMethod

    public static class AuthenticationMiddlewareExtension
    {
        //public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app)
        //{
        //    app.UseMiddleware<AuthenticationMiddleware>();
        //    return app;
        //}
    }

    #endregion
}
