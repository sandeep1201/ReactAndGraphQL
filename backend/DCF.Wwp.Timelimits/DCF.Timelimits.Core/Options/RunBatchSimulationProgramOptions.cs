using System;
using CommandLine;
using DCF.Timelimits.Rules.Domain;

namespace DCF.Timelimits
{
    [Verb("simulate", HelpText = "Regular batch run by partition")]
    public class RunBatchSimulationProgramOptions : BatchProgramOptionsBase
    {
        [Option('q', "queue", HelpText = "The JobQueue Name to process", Default = "Simulator")]
        public override String JobQueueName { get; set; }

        [Option('n', "name", HelpText = "The application instance name.", Default = "Simulator 1")]
        public override String ApplicationInstanceName { get; set; }

        [Option('e', "environment", HelpText = "The environment to we are process against", Default = ApplicationEnvironment.Development)]
        public override ApplicationEnvironment Environment { get; set; }

    }
}