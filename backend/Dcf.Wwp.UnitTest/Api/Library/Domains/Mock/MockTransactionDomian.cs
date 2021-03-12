using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains.Mock
{
    public class MockTransactionDomain : ITransactionDomain
    {
        public int InsertCount;
        public TransactionContract Transaction;

        public List<TransactionContract> GetTransactions(int participantId)
        {
            throw new NotImplementedException();
        }

        public dynamic InsertTransaction(TransactionContract transactionContract, bool returnInterface = false)
        {
            InsertCount++;
            Transaction = transactionContract;

            return null;
        }
    }
}
