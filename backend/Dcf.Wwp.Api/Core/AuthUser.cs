using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Core
{
    public class AuthUser : IAuthUser
    {
        public string       Username       { get; set; }
        public string       AgencyCode     { get; set; }
        public string       MainFrameId    { get; set; }
        public List<string> Authorizations { get; set; }

        public bool      IsAuthenticated => !string.IsNullOrWhiteSpace(Username);
        public string    WIUID           { get; set; }
        public DateTime? CDODate         { get; set; }
    }
}
