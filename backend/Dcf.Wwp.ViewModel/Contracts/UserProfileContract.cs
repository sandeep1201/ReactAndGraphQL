using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class UserProfileContract
    {
        public string Wiuid;
        public string Username;
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public string OfficeName { get; set; }
        public string AgencyCode;

        public List<string> Authorizations { get; set; } = new List<string>();
    }
}
