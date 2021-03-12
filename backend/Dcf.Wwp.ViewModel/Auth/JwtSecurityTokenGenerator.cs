using Dcf.Wwp.Api.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DCF.Core.Threading;

namespace Dcf.Wwp.Api.Library.Auth
{
    public class JwtSecurityTokenProvider : ITokenProvider
    {
        private TokenProviderOptions _options { get; set; }
        public JwtSecurityTokenProvider(TokenProviderOptions options)
        {
            this._options = options;
        }

        /// <summary>
        /// Will generate a JWT for the given username/password and private claims.
        /// This method will not authenticate the username and password, so make sure they 
        /// are valid before generating a client token (with an <see cref="IAuthenticationService"/> )
        /// </summary>
        /// <param name="username"></param>
        /// <param name="privateClaims"></param>
        /// <returns></returns>
        public String GenerateToken(String username, params Claim[] privateClaims)
        {
            this.ThrowIfInvalidOptions(this._options);

            //var identity = await this._options.IdentityResolver(username, password);
            //if (identity == null)
            //{
            //    throw new UnauthorizedAccessException();
            //}

            var now = DateTimeOffset.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                //new Claim(ClaimTypes.Role, "someRoleThisUserShouldHave"),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, AsyncHelper.RunSync(()=> this._options.NonceGenerator())),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };


            claims.AddRange(privateClaims);

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: this._options.Issuer,
                audience: this._options.Audience,
                claims: claims,
                notBefore: now.UtcDateTime,
                expires: now.Add(this._options.Expiration).UtcDateTime,
                signingCredentials: this._options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }


        private void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (String.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            }

            if (String.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (String.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            //if (options.IdentityResolver == null)
            //{
            //    throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            //}

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }
    }
}
