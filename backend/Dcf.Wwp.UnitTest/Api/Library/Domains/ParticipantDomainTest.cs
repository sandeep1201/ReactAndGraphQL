using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class ParticipantDomainTest
    {
        private MockTransactionDomain             _mockTransactionDomain;
        private MockWorkerRepository              _mockWorkerRepository;
        private MockParticipantRepository         _mockParticipantRepository;
        private ParticipantDomain                 _participantDomain;
        private AuthUser                          _authUser;
        private ParticipantStatusContract         _participantStatusContract;
        private MockParticipationStatusRepository _mockParticipationStatusRepository;


        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionDomain             = new MockTransactionDomain();
            _mockWorkerRepository              = new MockWorkerRepository();
            _mockParticipationStatusRepository = new MockParticipationStatusRepository();
            _mockParticipantRepository         = new MockParticipantRepository();
            _authUser                          = new AuthUser { WIUID                              = "1111" };
            _participantStatusContract         = new ParticipantStatusContract { ParticipantId = 1, EnrolledProgramId = MockParticipantRepository.ParticipantEnrolledProgramId };
            _participantDomain                 = new ParticipantDomain(_mockParticipantRepository, _mockParticipationStatusRepository, new MockUnitOfWork(), _authUser, null, null, null, null, null, null, null, null, null,
                                                                       _mockTransactionDomain, _mockWorkerRepository);
        }

        [TestMethod]
        public void AddStatus_ParticipantStatusContractIsEmpty_ThrowsException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _participantDomain.AddStatus(new ParticipantStatusContract { ParticipantId = 1 }));
        }

        [TestMethod]
        public void AddStatus_ValidParticipantStatusContract_InsertsIransaction()
        {
            _participantDomain.AddStatus(_participantStatusContract);
            Assert.IsTrue(_mockTransactionDomain.HasInsertTransactionBeenCalled);
        }

        [TestMethod]
        public void AddStatus_ValidParticipantHasEnrolledProgram_GetsOfficeIdForParticipantStatusContract()
        {
            _participantDomain.AddStatus(_participantStatusContract);
            Assert.AreEqual(MockParticipantRepository.ParticipantEnrolledOfficeId, _mockTransactionDomain.OfficeId);
        }

        [TestMethod]
        public void UpdateStatus_ParticipantStatusContractIsEmpty_ThrowsException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _participantDomain.UpdateStatus(new ParticipantStatusContract { ParticipantId = 1 }));
        }

        [TestMethod]
        public void UpdateStatus_InValidParticipantStatusContract_InsertsIransaction()
        {
            _participantDomain.UpdateStatus(_participantStatusContract);
            Assert.IsFalse(_mockTransactionDomain.HasInsertTransactionBeenCalled);
            Assert.IsTrue(_mockParticipationStatusRepository.UpdateHasBeenCalled);
        }

        [TestMethod]
        public void UpdateStatus_ValidParticipantStatusContract_InsertsIransaction()
        {
            _participantStatusContract.EndDate = DateTime.Today;
            _participantDomain.UpdateStatus(_participantStatusContract);
            Assert.IsTrue(_mockTransactionDomain.HasInsertTransactionBeenCalled);
            Assert.IsTrue(_mockParticipationStatusRepository.UpdateHasBeenCalled);
        }

        private class MockTransactionDomain : ITransactionDomain
        {
            public bool HasInsertTransactionBeenCalled;
            public int  OfficeId;

            public List<TransactionContract> GetTransactions(int participantId)
            {
                throw new NotImplementedException();
            }

            public dynamic InsertTransaction(TransactionContract transactionContract, bool returnInterface = false)
            {
                HasInsertTransactionBeenCalled = true;
                OfficeId                       = transactionContract.OfficeId;
                return null;
            }
        }

        private class MockWorkerRepository : MockRepositoryBase<Worker>, IWorkerRepository
        {
            public new Worker Get(Expression<Func<Worker, bool>> clause)
            {
                var worker = new Worker();

                return worker;
            }
        }
    }
}
