using System;
using CommandLine;

namespace DCF.Timelimits
{
    [Verb("batchPrep", HelpText = "Run the batch prep steps. Cleans/archives batch runs. Flips Timelimit status")]
    public class BatchPrepProgramOptions : BatchProgramOptionsBase
    {
        [Option('q', "queue", HelpText = "The JobQueue Name to process")]
        public override String JobQueueName { get; set; } = "BatchPrep";

        
    }
}