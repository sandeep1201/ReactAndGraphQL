using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobQueue
    {
        #region Properties

        public string    Name                       { get; set; }
        public int       QueueType                  { get; set; }
        public int       Partition                  { get; set; }
        public int?      ItemsToProcessConcurrently { get; set; }
        public int?      DefaultMaxRetries          { get; set; }
        public int?      DefaultRetryTimeBuffer     { get; set; }
        public int?      DefaultSleepTime           { get; set; }
        public bool      IsActive                   { get; set; }
        public bool      IsSimulation               { get; set; }
        public DateTime  CreatedDate                { get; set; }
        public bool      IsDeleted                  { get; set; }
        public string    ModifiedBy                 { get; set; }
        public DateTime? ModifiedDate               { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<JobQueueItem>        JobQueueItems         { get; set; }
        public virtual ICollection<JobQueueItemHistory> JobQueueItemHistories { get; set; }

        #endregion
    }
}
