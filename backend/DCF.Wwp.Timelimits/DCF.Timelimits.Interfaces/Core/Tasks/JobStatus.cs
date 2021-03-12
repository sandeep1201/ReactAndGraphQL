using System;

namespace DCF.Common.Tasks
{
    public enum JobStatus : Int64
    {
        None = 0,
        ReadyForJobProcessing = 1,
        SelectedForJobProcessing = 4, // externalId is in the list of items the app is processing
        CreatingJobForProcessing = 16,
        QueuedForJobProcessing = 64, // Data has been queried and queued inside the apps queue
        JobIsProcessing = 256, // app has started processing queried data
        JobProcessingFailure = 1024,
        JobProcessingSuccess = 4096,
        IsReady = ReadyForJobProcessing | CreatingJobForProcessing | JobProcessingFailure // only when we can pick it back up. If someone else is processing it we don't want to start
    }
}