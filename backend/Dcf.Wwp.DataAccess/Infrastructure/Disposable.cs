using System;

namespace Dcf.Wwp.DataAccess.Infrastructure
{
    public class Disposable : IDisposable
    {
        #region Properties

        private bool _isDisposed;

        #endregion

        #region Methods

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }

            _isDisposed = true;
        }

        protected virtual void DisposeCore()
        {
        }

        #endregion
    }
}
