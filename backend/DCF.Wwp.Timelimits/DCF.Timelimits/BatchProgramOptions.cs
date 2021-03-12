using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using DCF.Common.Tasks;

namespace DCF.Timelimits
{
    public class BatchProgramOptions
    {
        [Option('c',"concurrency",HelpText = "Item to process concurrently.", DefaultValue = 1)]
        public Int32 ItemsToProcessesConcurrently { get; set; }

        [Option('n')]
        public String ApplicationInstanceName { get; set; }
        
        [Option("queue",HelpText="The JobQueue Name")]
        public Int32 JobQueueName { get; set; }

        [Option('t',HelpText="The Job QueueType",DefaultValue = JobQueueType.EvaluateTimeLimits)]
        public JobQueueType QueueType { get; set; }

        [Option('p',"The partition that this instance should process")]
        public Int32? Partition { get; set; }
    }
}
