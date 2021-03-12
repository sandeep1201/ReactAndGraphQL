using CommandLine;

namespace DCF.Timelimits
{
    [Verb("cleanup", HelpText = "Close placements with extensions ending and update out of sync DB2 data")]
    public class BatchCleanupProgramOptions : BatchProgramOptionsBase
    {

    }
}