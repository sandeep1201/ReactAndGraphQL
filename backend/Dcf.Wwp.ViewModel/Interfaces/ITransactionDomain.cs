using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ITransactionDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<TransactionContract> GetTransactions(int                   participantId);
        dynamic                   InsertTransaction(TransactionContract transactionContract, bool returnInterface = false);

        #endregion
    }
}
