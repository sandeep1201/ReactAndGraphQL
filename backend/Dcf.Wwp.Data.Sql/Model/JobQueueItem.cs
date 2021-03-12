using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobQueueItem
    {
        #region Properties

        public string    ExternalJobId { get; set; }
        public int       JobQueueId    { get; set; }
        public int       JobStatusId   { get; set; }
        public bool?     IsReady       { get; set; }
        public bool      IsUrgent      { get; set; }
        public int?      RetryCount    { get; set; }
        public int       MaxRetries    { get; set; }
        public DateTime? RetryTime     { get; set; }
        public string    Notes         { get; set; }
        public string    JobResult     { get; set; }
        public DateTime? CreatedDate   { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobQueue JobQueue { get; set; }

        #endregion
    }
}
