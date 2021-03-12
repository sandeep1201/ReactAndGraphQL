using System;
using System.Collections.Generic;

namespace DCF.Core.Auditing
{
    public interface IClientInfoProvider
    {
        Version OsVersion { get;  }
        Version ClrVersion { get; }
        IEnumerable<Version> InstalledDotNetVersions { get; }
        UInt64 TotalSystemMemory { get; }
        Int32 ProcessorCount { get; }
        Int64 UtcOffsetInSeconds { get; }
        String DomainName { get; }
        String BrowserInfo { get; }

        String ClientIpAddress { get; }

        String ComputerName { get; }
    }
}