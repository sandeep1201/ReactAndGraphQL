using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;


namespace Dcf.Wwp.Api.Library.Auth
{
        /// <summary>
        /// Provides options for <see cref="TokenProviderMiddleware"/>.
        /// </summary>
        public class TokenProviderOptions
        {
            /// <summary>
            /// The relative request path to listen on.
            /// </summary>
            /// <remarks>The default path is <c>/token</c>.</remarks>
            public String Path { get; set; } = "/token";

            /// <summary>
            ///  The Issuer (iss) claim for generated tokens.
            /// </summary>
            public String Issuer { get; set; }

            /// <summary>
            /// The Audience (aud) claim for the generated tokens.
            /// </summary>
            public String Audience { get; set; }

            /// <summary>
            /// The expiration time for the generated tokens.
            /// </summary>
            /// <remarks>The default is five minutes (600 seconds).</remarks>
            public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(15);

            /// <summary>
            /// The signing key to use when generating tokens.
            /// </summary>
            public SigningCredentials SigningCredentials { get; set; }

            ///// <summary>
            ///// Resolves a user identity given a username and password.
            ///// </summary>
            //public Func<String, String, Task<ClaimsIdentity>> IdentityResolver { get; set; }

            /// <summary>
            /// Generates a random value (nonce) for each generated token.
            /// </summary>
            /// <remarks>The default nonce is a random GUID.</remarks>
            public Func<Task<String>> NonceGenerator { get; set; }
                = new Func<Task<String>>(() => Task.FromResult(Guid.NewGuid().ToString()));

            public TokenValidationParameters TokenValidationParameters { get; set; }
                = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
            };
        }
}