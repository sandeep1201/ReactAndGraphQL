using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IOverUnderPaymentEmail
    {
        void SendEmail(string dbCatalog, List<OverUnderPaymentResult> overUnderPaymentsWithoutWorker);
    }
}
