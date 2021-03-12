using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Tasks;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobQueue : BaseEntity, IJobQueue
    {
        ICollection<IJobQueueItem> IJobQueue.JobQueueItems
        {
            get { return this.JobQueueItems.Cast<IJobQueueItem>().ToList(); }
            set { this.JobQueueItems = value as ICollection<JobQueueItem>; }
        }
    }
}
