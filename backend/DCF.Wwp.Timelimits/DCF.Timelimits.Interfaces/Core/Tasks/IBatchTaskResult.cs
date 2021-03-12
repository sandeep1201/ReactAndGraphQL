using System;
using System.Collections.Generic;
using DCF.Common.Tasks;

namespace DCF.Timelimits.Core.Tasks
{
    public interface IBatchTaskResult
    {
        List<Exception> Errors { get; set; }

        JobStatus Status { get; set; }
    }
}