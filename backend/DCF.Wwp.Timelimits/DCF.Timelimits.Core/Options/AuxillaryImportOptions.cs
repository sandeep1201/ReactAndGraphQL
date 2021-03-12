using System;
using CommandLine;

namespace DCF.Timelimits
{
    [Verb("aux", HelpText = "Run Auxillary Import logic")]
    public class AuxillaryImportOptions : BatchProgramOptionsBase
    {
        [Option('q', "queue", HelpText = "The JobQueue Name to process")]
        public override String JobQueueName { get; set; } = "AuxillaryImport";
    }
}