using System;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Tasks;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobQueueItem : BaseEntity, IJobQueueItem
    {
        IJobQueue IJobQueueItem.JobQueue
        {
            get { return this.JobQueue; }

            set { this.JobQueue = value as JobQueue; }
        }

        JobStatus IJobQueueItem.JobStatus
        {
            get { return (JobStatus) this.JobStatusId; }

            set { this.JobStatusId = (Int32) value; }
        }
    }
}
