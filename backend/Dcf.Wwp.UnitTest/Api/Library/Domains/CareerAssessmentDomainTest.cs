using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class CareerAssessmentDomainTest
    {
        private CareerAssessmentDomain         _careerAssessmentDomain;
        private MockTransactionDomain          _mockTransactionDomain;
        private MockWorkerRepository           _mockWorkerRepository;
        private MockParticipantRepository      _mockParticipantRepository;
        private MockCareerAssessmentRepository _mockCareerAssessmentRepository;

        private AuthUser _authUser;

        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionDomain          = new MockTransactionDomain();
            _mockWorkerRepository           = new MockWorkerRepository();
            _mockParticipantRepository      = new MockParticipantRepository();
            _mockCareerAssessmentRepository = new MockCareerAssessmentRepository();
            _authUser                       = new AuthUser { WIUID = "1111", Authorizations = new List<string> { Authorization.canAccessProgram_WW }, AgencyCode = AgencyCode.FSC };
            _careerAssessmentDomain         = new CareerAssessmentDomain(_mockParticipantRepository, _mockCareerAssessmentRepository, null, new MockUnitOfWork(), _authUser, _mockWorkerRepository, _mockTransactionDomain);
        }

        [TestMethod]
        public void UpsertCareerAssessment_ValidAssessmentContract_InsertsTransaction()
        {
            _careerAssessmentDomain.UpsertCareerAssessment(new CareerAssessmentContract(), "1234567890");

            Assert.AreEqual(1, _mockTransactionDomain.InsertCount);
        }

        private class MockTransactionDomain : ITransactionDomain
        {
            public int InsertCount;

            public List<TransactionContract> GetTransactions(int participantId)
            {
                throw new NotImplementedException();
            }

            public dynamic InsertTransaction(TransactionContract transactionContract, bool returnInterface = false)
            {
                InsertCount++;
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

        private class MockCareerAssessmentRepository : MockRepositoryBase<CareerAssessment>, ICareerAssessmentRepository
        {
        }
    }
}
