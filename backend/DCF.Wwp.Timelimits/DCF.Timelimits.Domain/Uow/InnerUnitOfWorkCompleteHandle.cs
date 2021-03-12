using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DCF.Common.Exceptions;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// This handle is used for innet unit of work scopes.
    /// A inner unit of work scope actually uses outer unit of work scope
    /// and has no effect on <see cref="IUnitOfWorkCompleteHandle.Complete"/> call.
    /// But if it's not called, an exception is thrown at end of the UOW to rollback the UOW.
    /// </summary>
    internal class InnerUnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
    {
        public const String DidNotCallCompleteMethodExceptionMessage = "Did not call Complete method of a unit of work.";

        private volatile Boolean _isCompleteCalled;
        private volatile Boolean _isDisposed;

        public void Complete()
        {
            this._isCompleteCalled = true;
        }

        public async Task CompleteAsync()
        {
            this._isCompleteCalled = true;           
        }

        public void Dispose()
        {
            if (this._isDisposed)
            {
                return;
            }

            this._isDisposed = true;

            if (!this._isCompleteCalled)
            {
                if (InnerUnitOfWorkCompleteHandle.HasException())
                {
                    return;
                }

                throw new DCFApplicationException(InnerUnitOfWorkCompleteHandle.DidNotCallCompleteMethodExceptionMessage);
            }
        }
        
        private static Boolean HasException()
        {
            try
            {
                return Marshal.GetExceptionCode() != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}