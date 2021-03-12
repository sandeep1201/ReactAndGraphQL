using System;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public class DcfException : Exception
    {
        #region Properties

        public int ErrorCode { get; }

        #endregion

        #region Methods

        public DcfException (string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        #endregion
    }
}
