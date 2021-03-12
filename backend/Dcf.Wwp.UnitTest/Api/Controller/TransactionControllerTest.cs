using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.UnitTest.Api.Controller
{
    [TestClass]
    public class TransactionControllerTest
    {
        private const int ValidTestParticipantId = 1234567890;
        private const int ParticipantWithNoTransactions = 14585890;

        [TestMethod]
        public void GetTransactionsByParticipant_ValidId_ReturnsTypeOfTransactionContract()
        {
            var transactionController = new TransactionController(new MockTransactionDomain());
            var result = transactionController.GetTransactionsByParticipant(ValidTestParticipantId) as OkObjectResult;
            Assert.IsInstanceOfType(result?.Value, typeof(List<TransactionContract>));
        }

        [TestMethod]
        public void GetTransactionsByParticipant_PinHasNoTransactions_ReturnsEmptyList()
        {
            var transactionController = new TransactionController(new MockTransactionDomain());
            var result = (transactionController.GetTransactionsByParticipant(ParticipantWithNoTransactions) as OkObjectResult)?.Value as IEnumerable<TransactionContract>;
            Assert.IsFalse(result?.Any() ?? false);
        }

        [TestMethod]
        public void GetTransactionsByParticipant_ValidId_ReturnsTransactions()
        {
            var transactionController = new TransactionController(new MockTransactionDomain());
            var result                = (transactionController.GetTransactionsByParticipant(ValidTestParticipantId) as OkObjectResult)?.Value as IEnumerable<TransactionContract>;
            Assert.IsTrue(result?.Any() ?? false);
        }

        private class MockTransactionDomain : ITransactionDomain
        {
            public List<TransactionContract> GetTransactions(int participantId)
            {
                var result = new List<TransactionContract>();
                if (participantId == ValidTestParticipantId) result.Add(new TransactionContract());
                return result;
            }
            
            public dynamic InsertTransaction(TransactionContract transactionContract, bool returnInterface = false)
            {
                throw new System.NotImplementedException();
            }
        }
    }


}

