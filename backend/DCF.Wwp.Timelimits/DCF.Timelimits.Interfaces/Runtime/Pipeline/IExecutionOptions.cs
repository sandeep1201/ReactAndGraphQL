using System;
using Polly.Retry;

namespace DCF.Core.Runtime.Pipeline
{
    public interface IExecutionOptions : ICloneable
    {
        Boolean ContinueOnFailure { get; set; }
        Int32 MaxConcurrency { get; set; }
        RetryPolicy RetryPolicy { get; set; }
        Boolean ThrowExceptionOnError { get; set; }

    }
}