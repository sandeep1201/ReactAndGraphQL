using System;
using DCF.Common.Tasks;
using MediatR;

namespace DCF.Timelimits.Core.Tasks
{
    public interface IBatchTask  : IRequest
    {
        /// <summary>
        /// Gets or sets the external job identifier.
        /// this is the ID in the external system
        /// </summary>
        /// <value>The external job identifier.</value>
        String ExternalJobId { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>The external identifier.</value>
        Int32 JobId { get; set; }

        Boolean ReadyToProcess();

        Int32 RetryCount { get; set; }

        Int32 MaxRetries { get; set; }

        DateTime? RetryTime { get; set; }

        JobStatus? Status { get; set; }
        String GetItemIdentifier();
    }

    public interface IBatchTask<T> : IBatchTask, IRequest<T>
    {
        T Result { get; set; }
    }
}