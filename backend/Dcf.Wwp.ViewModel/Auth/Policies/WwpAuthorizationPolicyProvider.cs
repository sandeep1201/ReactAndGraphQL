using System;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Auth.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Dcf.Wwp.Api.Library.Auth.Policies
{
    public class WwpAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public WwpAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        #region Implementation of IAuthorizationPolicyProvider

        /// <inheritdoc />
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policyBuilder = new AuthorizationPolicyBuilder();

            if (policyName?.EndsWith("ExternalAPI") == true && Enum.TryParse<KeyIssuer>(policyName.Replace("ExternalAPI", string.Empty), out var issuer))
            {
                policyBuilder.AuthenticationSchemes.Add("ApiKey");
                policyBuilder.Requirements.Add(new ApiKeyRequirement(issuer));
            }
            else
            {
                policyBuilder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policyBuilder.RequireAuthenticatedUser();
            }

            policyBuilder.AuthenticationSchemes.Add("Automatic");
            return Task.FromResult(policyBuilder.Build());
        }

        /// <inheritdoc />
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return GetPolicyAsync(string.Empty);
        }

        #endregion
    }
}
