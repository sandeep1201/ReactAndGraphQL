using System;
using System.Security.Claims;

namespace Dcf.Wwp.Api.Library.Auth
{
    public interface ITokenProvider
    {
        String GenerateToken(string username, params Claim[] privateClaims);
    }

    public class RefreshTokenProvider 
    {
        public String GenerateToken(String username, params Claim[] privateClaims)
        {
            throw new NotImplementedException();
        }
    }
}