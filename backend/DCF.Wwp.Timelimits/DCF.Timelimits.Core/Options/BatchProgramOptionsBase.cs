using System;
using CommandLine;
using DCF.Common.Tasks;
using DCF.Timelimits.Rules.Domain;

namespace DCF.Timelimits
{   //TODO: Remove magic strings from JobQueueName's
    public abstract class BatchProgramOptionsBase
    {
        [Option('c', "concurrency", HelpText = "Item to process concurrently.", Default = 40)]
        public virtual Int32 ItemsToProcessesConcurrently { get; set; }

        [Option('n', "name", HelpText = "The application instance name.", Required = true)]
        public virtual String ApplicationInstanceName { get; set; }

        [Option('q', "queue", HelpText = "The JobQueue Name to process", Required = true)]
        public virtual String JobQueueName { get; set; }

        //[Option('t', "type", HelpText = "The Job QueueType (1 = timelimits)")]
        //public virtual JobQueueType QueueType { get; set; }

        [Option('p', "partition", HelpText = "The partition that this instance should process. 0 based index (0-9).", Default = 0)]
        public virtual Int32 Partition { get; set; }

        [Option('l', "level", HelpText = "The Logging Level (trace,debug,info,warn,error,fatal)")]
        public virtual String LogLevel { get; set; }

        [Option('s', "simulation", HelpText = "run in simulation mode")]
        public virtual String IsSimulationModeString { get; set; }

        public virtual Boolean IsSimulationMode
        {
            get { return IsSimulationModeString != "false"; }
        }

        [Option('o', "outputPath", Default = "..\\..\\BatchOutput\\")]
        public virtual String OutputPath { get; set; }

        [Option("loggingPath", Default = "..\\..\\logs\\batch\\")]
        public virtual String LoggingPath { get; set; }

        [Option('e', "environment", HelpText = "The environment to we are process against", Default = ApplicationEnvironment.Development)]
        public virtual ApplicationEnvironment Environment { get; set; }

        [Option('d', "processingDate", HelpText = "The processing date the application should use(mm/dd/yyyy)")]
        public String DateString { get; set; }
    }
}