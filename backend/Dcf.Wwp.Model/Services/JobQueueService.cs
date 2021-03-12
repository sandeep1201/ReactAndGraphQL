using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Services;
using DCF.Common.Logging;
using DCF.Common.Tasks;
using DCF.Timelimits.Core.Tasks;
using EnumsNET;

namespace Dcf.Wwp.Model.Services
{
    public class JobQueueService : IJobQueueService
    {
        private readonly WwpEntities _dbContext;
        private ILog _logger = LogProvider.GetLogger(typeof(IJobQueueService));

        public JobQueueService(WwpEntities dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task CreateJobAsync<T>(IBatchTask<T> task, IJobQueue queue, String createdBy, CancellationToken token = default(CancellationToken))
        {
            var jobQueueItem = this._dbContext.JobQueueItems.Create();
            this._dbContext.JobQueueItems.Add(jobQueueItem);

            jobQueueItem.ExternalJobId = task.ExternalJobId;
            jobQueueItem.JobQueueId = queue.Id;
            ((IJobQueueItem)jobQueueItem).JobStatus = JobStatus.CreatingJobForProcessing;
            jobQueueItem.ModifiedBy = createdBy;
            await this._dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            task.JobId = jobQueueItem.Id;
        }

        //public async Task<IJobQueue> GetJobQueueAsync(String queueName, JobQueueType queueType, Int32 partition, CancellationToken token = default(CancellationToken))
        public async Task<IJobQueue> GetJobQueueAsync(String queueName, CancellationToken token = default(CancellationToken))
        {
            var  tmp = await this._dbContext.JobQueues.Where(x => x.IsActive && !x.IsDeleted && x.Name == queueName).FirstOrDefaultAsync(token).ConfigureAwait(false);

            return (tmp);

            //if (item == null)
            //{
            //    this._logger.DebugFormat($"Queue:\"{queueName}\" not found for type \"{queueType}\" and partition: {partition}. Creating new JobQueue ");
            //    item = this._dbContext.JobQueues.Create();
            //    this._dbContext.Entry(item).State = EntityState.Added;
            //    item.IsActive = true;
            //    item.IsDeleted = false;
            //    item.Name = queueName;
            //    item.QueueType = queueType;
            //    item.CreatedDate = DateTime.Now;
            //    item.ModifiedBy = "WWPBatch";
            //    item.Partition = partition;

            //    if (items.Any())
            //    {
            //        var existingItem = items.GetMax(x => x.CreatedDate);
            //        item.ItemsToProcessConcurrently = existingItem.ItemsToProcessConcurrently;
            //        item.DefaultMaxRetries = existingItem.DefaultMaxRetries;
            //        item.DefaultRetryTimeBuffer = existingItem.DefaultRetryTimeBuffer;
            //        item.DefaultSleepTime = existingItem.DefaultSleepTime;
            //        item.IsSimulation = existingItem.IsSimulation;
            //    }
            //    else
            //    {
            //        item.ItemsToProcessConcurrently = 1;
            //        item.IsSimulation = false;
            //    }

            //    await this._dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            //}

            //return item;

        }



        public async Task<IEnumerable<IJobQueueItem>> GetRetryJobs(Int32 jobQueueId, Int32? partition = null, CancellationToken token = default(CancellationToken))
        {

            var jobQueue = await this._dbContext.JobQueues.FirstOrDefaultAsync(x => x.Id == jobQueueId && x.IsDeleted == false && x.IsActive, token).ConfigureAwait(false);
            if (jobQueue == null)
            {
                return new List<IJobQueueItem>();
            }
            var jobQuery = this._dbContext.JobQueueItems.Where(x => x.JobQueueId == jobQueue.Id && x.IsDeleted == false && x.IsReady == true && x.RetryCount < x.MaxRetries && x.RetryTime > DateTime.Now);
            if (partition.HasValue)
            {
                jobQuery = jobQuery.Where(x => x.JobQueue.Partition == partition.Value);
            }

            var jobQueueItems = await jobQuery.ToListAsync(token).ConfigureAwait(false);
            foreach (var job in jobQueueItems)
            {
                job.RetryCount = job.RetryCount.GetValueOrDefault() + 1;
                job.RetryTime = DateTime.Now.Add(TimeSpan.FromSeconds(jobQueue.DefaultRetryTimeBuffer.GetValueOrDefault()));
            }

            await this._dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            return jobQueueItems;

        }

        public async Task UpdateJobAsync<T>(IBatchTask<T> task, JobStatus status, Boolean? isUrgent = null, CancellationToken token = default(CancellationToken))
        {

            var jobQueueItem = await this._dbContext.JobQueueItems.FirstOrDefaultAsync(x => x.Id == task.JobId, token).ConfigureAwait(false);
            jobQueueItem.JobStatusId = (Int32)status;
            jobQueueItem.ModifiedDate = DateTime.Now;
            if (isUrgent.HasValue)
                jobQueueItem.IsUrgent = isUrgent.Value;

            jobQueueItem.IsReady = status.HasAnyFlags(JobStatus.IsReady);
            //jobQueueItem.JobResult = task?.Result?.ToJson();

            await this.SaveChangesAsync(token).ConfigureAwait(false);

            task.Status = status;
        }

        private async Task SaveChangesAsync(CancellationToken token)
        {
            try
            {
                await this._dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));

                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._dbContext?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
