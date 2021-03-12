using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DCF.Common.Tasks;
using DCF.Timelimits.Core.Tasks;

namespace Dcf.Wwp.Model.Interface.Services
{
    public interface IJobQueueService : IDisposable
    {
        Task CreateJobAsync(IBatchTask task, IJobQueue queue, String createdBy,  CancellationToken token = default(CancellationToken));
        Task<IJobQueue> GetJobQueueAsync(String queueName, CancellationToken token = default(CancellationToken));
        Task UpdateJobAsync<T>(IBatchTask<T> task, JobStatus status, Boolean? isUrgent = null, CancellationToken token = default(CancellationToken));
    }
}
