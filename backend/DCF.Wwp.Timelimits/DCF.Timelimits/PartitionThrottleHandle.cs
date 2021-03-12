using System;
using System.Threading;

namespace DCF.Timelimits
{
    public sealed class PartitionThrottleHandle : IDisposable
    {
        private SemaphoreSlim _throttle;
        private SemaphoreSlim _master;

        public PartitionThrottleHandle(SemaphoreSlim throttle, SemaphoreSlim master = null)
        {
            this._throttle = throttle;
            this._master = master;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: Should we throw an error if release doesn't do anything?
                    this._throttle.Release();
                    this._master?.Release();
                }

                this.disposedValue = true;
            }
        }


        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion

    }
}