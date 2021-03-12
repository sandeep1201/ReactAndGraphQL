namespace DCF.Core.Runtime.Pipeline
{
    public enum PipelineNodeStatus
    {
        None,
        Created,
        WaitingForActivation,
        WaitingToRun,
        Running,
        WaitingForChildrenToComplete,
        RanToCompletion,
        RanToCompletionWithErrors,
        Canceled,
        Faulted,
    }
}