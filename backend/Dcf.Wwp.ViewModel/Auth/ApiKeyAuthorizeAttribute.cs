using System;
using Dcf.Wwp.Api.Library.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;


namespace Dcf.Wwp.Api.Library.Auth
{
    public class ApiKeyAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_SUFFIX = "ExternalAPI";

        public ApiKeyAuthorizeAttribute(KeyIssuer issuer)
        {
            ActiveAuthenticationSchemes = "ApiKey";
            Issuer                      = issuer;
        }

        public KeyIssuer Issuer
        {
            get => Enum.TryParse<KeyIssuer>(Policy.Replace(POLICY_SUFFIX, ""), out var issuer) ? issuer : KeyIssuer.UnAuthorized;
            set => Policy = $"{value}{POLICY_SUFFIX}";
        }
    }
}
