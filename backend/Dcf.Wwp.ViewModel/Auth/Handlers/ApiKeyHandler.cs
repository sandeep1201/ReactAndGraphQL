using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Auth.Requirements;
using DCF.Common.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Dcf.Wwp.Api.Library.Auth.Handlers
{
    public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private readonly EntSecAuthenticationService _entSecAuthenticationService;
        private          ILog                        _logger { get; }

        public ApiKeyHandler(EntSecAuthenticationService entSecAuthenticationService)
        {
            _entSecAuthenticationService = entSecAuthenticationService;
            _logger                      = LogProvider.GetLogger(GetType());
        }

        #region Overrides of AuthorizationHandler<ApiKeyRequirement>

        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            var issuer = KeyIssuer.EntSec;

            bool ClaimPredicate(Claim c) => c.Type == "ApiKey";
            //_logger.WarnFormat("Evaluating authorization requirement for Api Key issuer == {issuer}", requirement.Issuer);

            // If we don't have an ApiKey for the "ExternalAPI" Scheme move on and let any other handlers try (they should fail unless another handler is registered)
            // TODO: Move magic strings to contants
            if (!context.User.HasClaim(ClaimPredicate) || issuer == KeyIssuer.UnAuthorized)
            {
                _logger.WarnFormat("No ApiKey claim found with isser: {issuer}", requirement.Issuer);
                return Task.CompletedTask;
            }

            // validate the issuer is valide for the requirement
            if (issuer != requirement.Issuer)
            {
                _logger.WarnFormat("ApiKey claim found for wrong issuer. Found:{FoundIssuer}. Requested Issuer {RequestedIssuer}", issuer, requirement.Issuer);
                // Don't return anything, another handler might validate requirement(s)
            }

            // We have an API Key and valid issuer
            var apiKey = context.User.FindFirst(ClaimPredicate).Value;

            switch (issuer)
            {
                case KeyIssuer.Wwp:
                    // TODO: remove, this is for testing before EntSec is hooked up
                    // TODO: in the future, let WWP authorize clients with a key somehow
                    if (apiKey.StartsWith("12345"))
                    {
                        //_logger.WarnFormat("WWP Api key requirement satisfied: {apiKey}. {issuer}", apiKey, issuer);
                        context.Succeed(requirement);
                    }

                    break;

                case KeyIssuer.EntSec:
                    var response = _entSecAuthenticationService.AuthenticateAPIKey(apiKey);

                    if (response == AuthRequestStatus.AuthenticationSuccess)
                    {
                        context.Succeed(requirement);
                    }
                    else
                        if (response == AuthRequestStatus.TokenIsExpired)
                        {
                            //var refreshResponse =this._entSecAuthenticationService.RefreshToken();
                            throw new NotImplementedException();
                        }
                        else
                        {
                            context.Fail();
                        }

                    break;
            }


            return Task.CompletedTask;
        }

        #endregion
    }
}
