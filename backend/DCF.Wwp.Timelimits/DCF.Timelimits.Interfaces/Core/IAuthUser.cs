using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Core
{
    public interface IAuthUser
    {
        string       Username       { get; set; }
        string       AgencyCode     { get; set; }
        string       MainFrameId    { get; set; }
        List<string> Authorizations { get; set; }

        bool      IsAuthenticated { get; }
        string    WIUID           { get; set; }
        DateTime? CDODate         { get; set; }
    }
}
