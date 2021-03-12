using System;
using DCF.Common.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IJobQueueItem
    {
        Int32 Id { get; set; }
        String ExternalJobId { get; set; }
        Int32 JobQueueId { get; set; }
        Int32 JobStatusId { get; set; }
        Boolean? IsReady { get; set; }
        Boolean IsUrgent { get; set; }
        Int32? RetryCount { get; set; }
        Int32 MaxRetries { get; set; }
        DateTime? RetryTime { get; set; }
        String Notes { get; set; }
        DateTime? CreatedDate { get; set; }
        Boolean IsDeleted { get; set; }
        String ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Byte[] RowVersion { get; set; }
        String JobResult { get; set; }
        JobStatus JobStatus { get; set; }

        IJobQueue JobQueue { get; set; }
    }
}