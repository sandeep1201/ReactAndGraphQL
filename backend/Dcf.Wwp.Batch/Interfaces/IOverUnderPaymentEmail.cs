using System;
using System.Collections.Generic;
using Dcf.Wwp.Batch.Models;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IOverUnderPaymentEmail
    {
        #region Properties

        #endregion

        #region Methods

        void SendEmail(string dbCatalog, List<OverUnderPaymentResult> overUnderPaymentsWithoutWorker);

        #endregion
    }
}
