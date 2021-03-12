using System;
using System.Collections.Generic;
using DCF.Common.Tasks;

namespace DCF.Timelimits.Core.Tasks
{
    public class BatchTaskResultBase : IBatchTaskResult
    {
        public JobStatus Status { get; set; }
        public List<Exception> Errors { get; set; } = new List<Exception>();

    }
}