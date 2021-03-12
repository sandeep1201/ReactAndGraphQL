using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Auth;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace Dcf.Wwp.Api.Middleware
{
    public interface IApiUser
    {
        bool IsAuthenticated { get; set; }
    }

    public class ApiUser : IApiUser
    {
        public bool IsAuthenticated { get; set; }
    }


    // ReSharper disable once ClassNeverInstantiated.Global
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger         _logger;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next   = next;
            _logger = Log.Logger;
        }

        public Task Invoke(HttpContext context, IApiUser apiUser, IRepository repository, IAuthenticationService authService)
        {
            if (context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                var apikey = authorization.FirstOrDefault(x => x.ToLower().StartsWith("apikey"));

                if (!string.IsNullOrWhiteSpace(apikey))
                {
                    var parts = apikey.Split(' ');
                    if (parts.Length == 2)
                    {
                        var token = parts[1];

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            // Validate the key with EntSec. 
                            apiUser.IsAuthenticated = authService.ValidateToken(token, repository.Database);
                        }
                    }
                }
            }

            return _next.Invoke(context);
        }
    }
}
