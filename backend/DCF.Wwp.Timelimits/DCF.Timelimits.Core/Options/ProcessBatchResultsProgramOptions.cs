using System;
using CommandLine;
using DCF.Timelimits.Rules.Domain;

namespace DCF.Timelimits
{
    [Verb("results", HelpText = "Process the results from a regular batch run partition. ")]
    public class ProcessBatchResultsProgramOptions : BatchProgramOptionsBase
    {
        [Option('q', "queue", HelpText = "The JobQueue Name to process", Default = "Process Batch Results")]
        public override String JobQueueName { get; set; }

        [Option('e', "environment", HelpText = "The environment to we are process against", Default = ApplicationEnvironment.Development)]
        public override ApplicationEnvironment Environment { get; set; }
    }
}