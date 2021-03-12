using System;

namespace DCF.Core.Runtime.Session
{
    public interface IApplicationSession
    {
        Int64? ImpersonatorUserId { get; set; }
        Int64? UserId { get; set; }
        String Username { get; set; }
    }
}