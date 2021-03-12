using Microsoft.AspNetCore.Authorization;

namespace Dcf.Wwp.Api.Library.Auth.Requirements
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public KeyIssuer Issuer { get; }

        public ApiKeyRequirement(KeyIssuer issuer)
        {
            this.Issuer = issuer;
        }
    }
}
