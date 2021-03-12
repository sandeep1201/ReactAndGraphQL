using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Api.Library.Domains.Mock;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class JobReadinessDomainTest
    {
        #region Properties

        private JobReadinessDomain         _jobReadinessDomain;
        private MockTransactionDomain      _mockTransactionDomain;
        private MockWorkerRepository       _mockWorkerRepository;
        private MockParticipantRepository  _mockParticipantRepository;
        private MockJobReadinessRepository _mockJobReadinessRepository;
        private AuthUser                   _authUser;

        private static readonly JRWorkPreferencesContract JrWorkPreferenceContract = new JRWorkPreferencesContract
                                                                                     {
                                                                                         WorkShiftIds = new List<int> { 1 }
                                                                                     };

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionDomain      = new MockTransactionDomain();
            _mockWorkerRepository       = new MockWorkerRepository();
            _mockParticipantRepository  = new MockParticipantRepository();
            _mockJobReadinessRepository = new MockJobReadinessRepository();
            _authUser                   = new AuthUser { WIUID = "1111", Authorizations = new List<string> { Authorization.canAccessProgram_WW }, AgencyCode = AgencyCode.FSC };
            _jobReadinessDomain         = new JobReadinessDomain(_mockParticipantRepository, _mockJobReadinessRepository, new MockJrApplicationInfoRepository(), new MockJrContactInfoRepository(), new MockJrHistoryInfoRepository(), new MockJrInterviewInfoRepository(), new MockJrWorkPreferencesRepository(), null, new MockUnitOfWork(), _authUser, _mockWorkerRepository, new MockActionNeededDomain(), _mockTransactionDomain);
        }

        [TestMethod]
        public void UpsertJobReadiness_ValidJobReadinessContractWithSaveErrors_DoesNotInsertsTransaction()
        {
            var jobReadinessContract = new JobReadinessContract { JrApplicationInfo = new JRApplicationInfoContract(), JrContactInfo = new JRContactInfoContract(), JrHistoryInfo = new JRHistoryInfoContract(), JrInterviewInfo = new JRInterviewInfoContract(), JrWorkPreferences = JrWorkPreferenceContract };
            _jobReadinessDomain.UpsertJobReadiness(jobReadinessContract, "123456789", 0, true);

            Assert.AreEqual(0, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void UpsertJobReadiness_ValidJobReadinessContractWithNoSaveErrors_InsertsTransaction()
        {
            var jobReadinessContract = new JobReadinessContract { JrApplicationInfo = new JRApplicationInfoContract(), JrContactInfo = new JRContactInfoContract(), JrHistoryInfo = new JRHistoryInfoContract(), JrInterviewInfo = new JRInterviewInfoContract(), JrWorkPreferences = JrWorkPreferenceContract };
            _jobReadinessDomain.UpsertJobReadiness(jobReadinessContract, "1234567890", 0, false);

            Assert.AreEqual(1, _mockTransactionDomain.InsertCount);
        }

        #endregion

        #region Repositories

        private class MockActionNeededDomain : IActionNeededDomain
        {
            public ActionNeededContract GetActionNeededContract(Participant participant, string page)
            {
                return new ActionNeededContract();
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

        private class MockJobReadinessRepository : MockRepositoryBase<JobReadiness>, IJobReadinessRepository
        {
        }

        private class MockJrApplicationInfoRepository : MockRepositoryBase<JRApplicationInfo>, IJRApplicationInfoRepository
        {
        }

        private class MockJrContactInfoRepository : MockRepositoryBase<JRContactInfo>, IJRContactInfoRepository
        {
        }

        private class MockJrHistoryInfoRepository : MockRepositoryBase<JRHistoryInfo>, IJRHistoryInfoRepository
        {
        }

        private class MockJrInterviewInfoRepository : MockRepositoryBase<JRInterviewInfo>, IJRInterviewInfoRepository
        {
        }

        private class MockJrWorkPreferencesRepository : MockRepositoryBase<JRWorkPreferences>, IJRWorkPreferencesRepository
        {
        }

        #endregion
    }
}
