using DCF.Common.Logging;

namespace DCF.Core.Exceptions
{
    public interface IHasLogSeverity
    {
        LogLevel Severity { get; set; }
    }
}