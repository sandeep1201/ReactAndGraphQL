using System;

namespace DCF.Core.Auditing
{
    public interface IAuditInfo
    {
        String BrowserInfo { get; set; }
        String ClientIpAddress { get; set; }
        String ClientName { get; set; }
        String CustomData { get; set; }
        Exception Exception { get; set; }
        Int32 ExecutionDuration { get; set; }
        DateTime ExecutionTime { get; set; }
        Int64? ImpersonatorUserId { get; set; }
        String MethodName { get; set; }
        String ModuleName { get; set; }
        String Parameters { get; set; }
        Int64? UserId { get; set; }

        String ToString();
    }
}