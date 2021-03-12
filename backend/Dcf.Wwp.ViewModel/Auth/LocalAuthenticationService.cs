using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Core.EntSec;
using Dcf.Wwp.Api.Library.Helpers;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.Auth
{
    public class LocalAuthenticationService : IAuthenticationService
    {
        private readonly IRepository    _repo;
        private readonly ITokenProvider _tokenProvider;

        public LocalAuthenticationService(IRepository repo, ITokenProvider tokenProvider)
        {
            _repo          = repo;
            _tokenProvider = tokenProvider;
        }

        public IUserProfile AuthenticateUser(string username, string password, string environment, DateTime? cdoDate = null)
        {
            var loginDetail = _repo.WorkerByWamsId(username);
            var profile = new UserProfile
                          {
                              WamsUserId       = username,
                              OrganizationCode = loginDetail.Organization?.EntsecAgencyCode,
                              MainFrameId      = loginDetail.MFUserId.IsNullOrEmpty() ? _repo.GetFnMFId() : loginDetail.MFUserId,
                              Wiuid            = loginDetail.WIUID
                          };

            var status = AuthRequestStatus.AuthenticationFail;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (loginDetail != null)
            {
                profile.Roles            = (loginDetail.Roles ?? "" ).Split(',').ToArray();
                profile.RoleNames        = _repo.AuthorizationRoles(profile.Roles).Select(x => x.Name).ToArray();
                status                   = AuthRequestStatus.AuthenticationSuccess;
                profile.FirstName        = loginDetail.FirstName;
                profile.LastName         = loginDetail.LastName;
                profile.EntSecToken      = Guid.NewGuid().ToString("N");
                profile.OrganizationCode = loginDetail.Organization?.EntsecAgencyCode;
            }

            profile.Status       = status.ToString();
            profile.ErrorMessage = EnumHelpers.GetEnumDescription(status);
            profile.IsAuthorized = status == AuthRequestStatus.AuthenticationSuccess;
            profile.CDODate      = cdoDate;

            return profile;
        }

        public bool ValidateToken(string token, string environment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRole> GetRoles()
        {
            return _repo.AuthorizationRoles();
        }

        public IEnumerable<string> GetAllUserames()
        {
            return _repo.GetWorkerUsernames();
        }

        public IEnumerable<IOffice> GetAllOffices()
        {
            return this._repo.GetOffices();
        }

        public string GenerateToken(string username, params Claim[] privateClaims)
        {
            return _tokenProvider.GenerateToken(username, privateClaims);
        }
    }
}
