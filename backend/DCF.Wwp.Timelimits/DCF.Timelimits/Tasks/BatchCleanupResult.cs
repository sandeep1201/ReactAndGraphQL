using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace DCF.Timelimits.Tasks
{
    public class BatchCleanupContext : TimelineTaskContext<BatchCleanupResult>
    {
        public List<IT0459_IN_W2_LIMITS> TicksToUpdate { get; set; } = new List<IT0459_IN_W2_LIMITS>();

    }

    public class BatchCleanupResult : TimelineTaskResult
    {
        public List<IT0459_IN_W2_LIMITS> NewTicks { get; set; } = new List<IT0459_IN_W2_LIMITS>();
        public List<IT0459_IN_W2_LIMITS> OldTicks { get; set; }
    }
}