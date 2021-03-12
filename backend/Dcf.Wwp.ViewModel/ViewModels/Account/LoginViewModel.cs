using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Core.EntSec;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Core;

namespace Dcf.Wwp.Api.Library.ViewModels.Account
{
    public class LoginViewModel : BaseInformalAssessmentViewModel
    {
        public LoginViewModel(IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
        }

        public AuthContract GetUserAuthContract(IUserProfile user)
        {
            var ac = new AuthContract { User = new UserProfileContract() };

            if (user != null)
            {
                ac.Message = "auth OK";

                ac.User.FirstName  = user.FirstName;
                ac.User.MiddleName = user.MiddleInitial;
                ac.User.LastName   = user.LastName;
                ac.User.Username   = user.WamsUserId;
                ac.User.Wiuid      = user.Wiuid;
                ac.User.AgencyCode = user.OrganizationCode;
                ac.User.OfficeName = Repo.GetOrganizationByCode(user.OrganizationCode)?.AgencyName?.Trim();

                ac.User.Authorizations = Repo.AuthorizationsForRoles(user.Roles);
                //ac.LastRefreshedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            else
            {
                ac.Message = "User not authorized for WWP";
                ac.Token   = string.Empty;
            }

            return ac;
        }

        public void RecordLogin(IUserProfile user)
        {
            var wl     = Repo.GetOrCreateWorkerLogin(user.WamsUserId);
            var fnMfId = Repo.GetFnMFId();
            wl.FirstName              = user.FirstName;
            wl.MiddleInitial          = user.MiddleInitial;
            wl.LastName               = user.LastName;
            wl.WorkerActiveStatusCode = "ACTIVE";
            wl.MFUserId               = user.MainFrameId == fnMfId ? "" : user.MainFrameId;
            var org = Repo.GetOrganizationByCode(user.OrganizationCode);
            wl.OrganizationId = org.Id;
            wl.WIUID          = user.Wiuid;

            //if (!string.IsNullOrWhiteSpace(user.OrganizationCode))
            //{
            //    var org           = Repo.GetOrganizationByCode(user.OrganizationCode);
            //    wl.OrganizationId = org.Id;
            //}
            //else
            //{
            //    // TODO: Remove this hack once the new EntSec code is in place.
            //    // HACK: This is temporary!!!
            //    user.OrganizationCode = wl.Organization?.EntsecAgencyCode;
            //    if (!string.IsNullOrWhiteSpace(wl.Roles))
            //    {
            //        user.Roles = wl.Roles.Split(',');
            //        user.RoleNames = Repo.AuthorizationRoles(user.Roles).Select(x => x.Name).ToArray();
            //    }
            //}

            wl.Roles     = string.Join(",", user.Roles);
            wl.LastLogin = DateTime.Now;
            wl.IsDeleted = false;

            Repo.Save();
        }

        public void ElevatedAccess(ElevatedAccessContract contract)
        {
            if (contract == null)
                throw new InvalidOperationException("Elevated Access Request data is missing");

            var worker = Repo.WorkerByWamsId(AuthUser.Username);

            Repo.NewElevatedAccess(AuthUser.Username, worker.Id, Participant.Id, contract.ElevatedAccessReasonId, contract.Details);

            Repo.Save();
        }
    }
}
