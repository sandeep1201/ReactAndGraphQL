using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DCF.Common.Extensions;

namespace DCF.Timelimits
{
    public abstract class PartitionThrottle<T> where T : new()
    {
        private ConcurrentDictionary<Int32, SemaphoreSlim> _queues;

        public Int32 PartitionQueueSize { get; }
        protected readonly SemaphoreSlim _masterQueue;

        protected readonly TimeSpan _timeout;

        public PartitionThrottle(Int32 partitionQueueSize, int? maxQueueSize = null, TimeSpan? timeout = null)
        {
            this.PartitionQueueSize = partitionQueueSize < 1 ?  1 : partitionQueueSize;
            if (maxQueueSize.HasValue)
            {
                this._masterQueue = new SemaphoreSlim(0, maxQueueSize.Value);
            }
            else
            {
                this._masterQueue = new SemaphoreSlim(0);
            }

            this._timeout = timeout.GetValueOrDefault(TimeSpan.FromTicks(-1));
            this._queues = new ConcurrentDictionary<Int32, SemaphoreSlim>();
        }

        public virtual IDisposable Wait(T identifier, CancellationToken token = default(CancellationToken))
        {
            // First wait on the master Queue to open a spot
            this._masterQueue.Wait(this._timeout, token);
            return this.GetQueue(identifier);
        }

        public virtual async Task<IDisposable> WaitAsync(T identifier, CancellationToken token = default(CancellationToken))
        {
            // First wait on the master Queue to open a spot
            await this._masterQueue.WaitAsync(this._timeout, token).ConfigureAwait(false);
            return this.GetQueueAsync(identifier, token);
        }

        protected virtual PartitionThrottleHandle GetQueue(T identifier, CancellationToken token = default(CancellationToken))
        {
            var queue = this.FindQueue(identifier);
            queue.Wait(this._timeout, token);
            return this.CreateHandle(queue, this._masterQueue);
        }

        protected virtual async Task<PartitionThrottleHandle> GetQueueAsync(T identifier, CancellationToken token = default(CancellationToken))
        {
            var queue = this.FindQueue(identifier);
            await queue.WaitAsync(this._timeout, token).ConfigureAwait(false);
            return this.CreateHandle(queue, this._masterQueue);
        }

        protected virtual SemaphoreSlim FindQueue(T identifier)
        {
            var key = this.GetQueueKey(identifier);
            var semaphore = this._queues.GetOrAdd(key, ()=> new SemaphoreSlim(0, this.PartitionQueueSize));
            return semaphore;
        }

        protected abstract Int32 GetQueueKey(T identifier);

        protected virtual PartitionThrottleHandle CreateHandle(SemaphoreSlim queue, SemaphoreSlim master)
        {
            return new PartitionThrottleHandle(queue,master);
        }

        public Int32 MasterCount()
        {
            return this._masterQueue.CurrentCount;
        }

        public Int32? QueueCount(T identifier)
        {
            var queue = this.FindQueue(identifier);
            return queue?.CurrentCount;
        }
    }
}