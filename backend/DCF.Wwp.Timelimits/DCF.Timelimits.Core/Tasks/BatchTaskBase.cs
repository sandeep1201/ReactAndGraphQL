using System;
using DCF.Common.Tasks;

namespace DCF.Timelimits.Core.Tasks
{
    public abstract class BatchTaskBase<T> : IBatchTask<T>
    {
        /// <summary>
        /// Gets or sets the external job identifier.
        /// this is the ID in the external system
        /// </summary>
        /// <value>The external job identifier.</value>
        public virtual String ExternalJobId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Gets or sets the internal identifier.
        /// </summary>
        /// <value>The external identifier.</value>
        public Int32 JobId { get; set; } 

        public abstract Boolean ReadyToProcess();

        public Int32 RetryCount { get; set; }

        public Int32 MaxRetries { get; set; } = 5;

        public DateTime? RetryTime { get; set; }

        public JobStatus? Status { get; set; }

        public virtual String GetItemIdentifier()
        {
            return $"{this.JobId}-{this.RetryCount +1} / { this.MaxRetries }  ";
        }

        public abstract T Result { get; set; }
    }

    //public class ProcessParticipantTimelimitBatchTask : BatchTaskBase<TimelineMonth>
    //{
    //    public ProcessParticipantTimelimitBatchTask(Boolean isActive)
    //    {
    //        IsActive = isActive;
    //    }

    //    public Boolean IsActive { get; set; }

    //    public override Boolean ReadyToProcess()
    //    {
    //        return IsActive;
    //    }

    //    public override String GetItemIdentifier()
    //    {
    //        return $"{this.JobId}-{this.EvaluationMonth:MMMM-yyyy}-{this.PinNumber}";
    //    }

    //    public override TimelineMonth Result { get; internal set; }

    //    public DateTime EvaluationMonth { get; set; }

    //    public Decimal PinNumber { get; set; }
    //}
}
