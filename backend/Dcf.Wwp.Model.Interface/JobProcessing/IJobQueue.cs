using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IJobQueue
    {
        int                        Id                         { get; set; }
        string                     Name                       { get; set; }
        int                        QueueType                  { get; set; }
        int?                       ItemsToProcessConcurrently { get; set; }
        int?                       DefaultMaxRetries          { get; set; }
        int?                       DefaultRetryTimeBuffer     { get; set; }
        int?                       DefaultSleepTime           { get; set; }
        DateTime                   CreatedDate                { get; set; }
        bool                       IsActive                   { get; set; }
        bool                       IsDeleted                  { get; set; }
        string                     ModifiedBy                 { get; set; }
        DateTime?                  ModifiedDate               { get; set; }
        byte[]                     RowVersion                 { get; set; }
        bool                       IsSimulation               { get; set; }
        ICollection<IJobQueueItem> JobQueueItems              { get; set; }
    }
}
