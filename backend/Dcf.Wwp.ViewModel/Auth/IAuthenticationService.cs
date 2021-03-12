using System;
using System.Collections.Generic;
using System.Security.Claims;
using Dcf.Wwp.Api.Core.EntSec;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Api.Library.Auth
{
    public interface IAuthenticationService
    {
        string               GenerateToken(string    username, params Claim[] privateClaims);
        IUserProfile         AuthenticateUser(string username, string         password, string environment, DateTime? cdoDate = null);
        bool                 ValidateToken(string    token,    string         environment);
        IEnumerable<IRole>   GetRoles();
        IEnumerable<string>  GetAllUserames();
        IEnumerable<IOffice> GetAllOffices();
    }
}
