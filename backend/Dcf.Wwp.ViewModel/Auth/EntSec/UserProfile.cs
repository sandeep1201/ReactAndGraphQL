using System;

namespace Dcf.Wwp.Api.Core.EntSec
{
    public class UserProfile : IUserProfile
    {
        public string    Wiuid            { get; set; }
        public string    WamsUserId       { get; set; }
        public string    EntSecToken      { get; set; } = string.Empty;
        public DateTime? LastLogon        { get; set; }
        public string    FirstName        { get; set; }
        public string    LastName         { get; set; }
        public string    MiddleInitial    { get; set; }
        public string    Email            { get; set; }
        public string    MainFrameId      { get; set; }
        public string    OrganizationCode { get; set; }
        public bool      IsAuthorized     { get; set; }
        public string[]  Roles            { get; set; }
        public string[]  RoleNames        { get; set; }
        public string    Status           { get; set; }
        public string    ErrorMessage     { get; set; }
        public DateTime? CDODate          { get; set; }

        public bool IsInRole(string roleName)
        {
            bool isInRole = false;
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    isInRole = (role.ToLower().Equals(roleName.ToLower()));
                    if (isInRole)
                        break;
                }
            }

            return isInRole;
        }
    }
}
