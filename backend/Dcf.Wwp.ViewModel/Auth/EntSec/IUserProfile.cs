using System;

namespace Dcf.Wwp.Api.Core.EntSec
{
    public interface IUserProfile
    {
        string    Wiuid            { get; set; } // WIUID
        string    WamsUserId       { get; set; } // WAMS Directory ID
        string    EntSecToken      { get; set; } // Entsec security token, stored for possible reuse (SSO?)
        DateTime? LastLogon        { get; set; } // Last Logon
        string    FirstName        { get; set; } // User First Name
        string    LastName         { get; set; } // User Last Name
        string    MiddleInitial    { get; set; } // User Middle Initial
        string    Email            { get; set; } // Email
        string    MainFrameId      { get; set; } // User Main Frame ID
        string    OrganizationCode { get; set; } // User Organization Code
        bool      IsAuthorized     { get; set; } // Check for User Authorization
        string[]  Roles            { get; set; } // Application Roles assigned to User
        string[]  RoleNames        { get; set; }
        string    Status           { get; set; }
        string    ErrorMessage     { get; set; }
        DateTime? CDODate          { get; set; }
    }
}
