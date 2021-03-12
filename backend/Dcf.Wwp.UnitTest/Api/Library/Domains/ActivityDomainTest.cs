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
using Dcf.Wwp.UnitTest.Api.Library.Domains.Mock;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActivityCompletionReason = Dcf.Wwp.Model.Interface.Constants.ActivityCompletionReason;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class ActivityDomainTest
    {
        private ActivityDomain                  _activityDomain;
        private PrivateObject                   _privateActivityDomain;
        private MockTransactionDomain           _mockTransactionDomain;
        private MockWorkerRepository            _mockWorkerRepository;
        private MockActivityRepository          _mockActivityRepository;
        private MockEpaBridgeRepository         _mockEpaBridgeRepository;
        private MockEmployabilityPlanRepository _mockEmployabilityPlanRepository;
        private MockActivityScheduleRepository  _mockActivityScheduleRepository;
        private MockPopClaimDomain              _mockPopClaimDomain;
        private AuthUser                        _authUser;
        private ActivityContract                _activityContract;
        private List<ActivityContract>          _activityContracts;
        private object[]                        _endActivityTransactionArgs;
        private object[]                        _endActivityArgs;

        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionDomain           = new MockTransactionDomain();
            _mockWorkerRepository            = new MockWorkerRepository();
            _mockActivityRepository          = new MockActivityRepository();
            _mockEpaBridgeRepository         = new MockEpaBridgeRepository();
            _mockActivityScheduleRepository  = new MockActivityScheduleRepository();
            _mockEmployabilityPlanRepository = new MockEmployabilityPlanRepository();
            _mockPopClaimDomain              = new MockPopClaimDomain();
            _authUser                        = new AuthUser { WIUID = "1111" };
            _activityDomain                  = new ActivityDomain(_mockActivityRepository, null, null, null, _mockActivityScheduleRepository, _mockEpaBridgeRepository, null, _mockWorkerRepository, new MockUnitOfWork(), null, _authUser, null, null, null, _mockEmployabilityPlanRepository, null, null, null, _mockTransactionDomain, _mockPopClaimDomain, null);
            _privateActivityDomain           = new PrivateObject(_activityDomain);

            _activityContract = new ActivityContract
                                {
                                    Id                           = 1,
                                    EndDate                      = DateTime.Now.ToString("MM/dd/yyyy"),
                                    ActivityTypeCode             = ActivityCode.JS,
                                    ActivityCompletionReasonName = ActivityCompletionReason.V
                                };

            _activityContracts = new List<ActivityContract>
                                 {
                                     _activityContract
                                 };

            _endActivityTransactionArgs = new object[] { new EmployabilityPlan { ParticipantEnrolledProgram = new ParticipantEnrolledProgram() }, ActivityCode.JS, DateTime.Today, DateTime.Today };
            _endActivityArgs            = new object[] { _activityContract, new Activity { ActivityType     = new ActivityType() }, 0, 0, DateTime.Now };
        }

        [TestMethod]
        public void UpsertElapsedActivity_EmptyActivityContract_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _activityDomain.UpsertElapsedActivity(null, "", 0, false, false));
        }

        [TestMethod]
        public void UpsertElapsedActivity_ValidActivityContract_InsertsOneTransaction()
        {
            _activityDomain.UpsertElapsedActivity(_activityContracts, "0", 1, true, false);

            Assert.AreEqual(1, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void UpsertElapsedActivity_ForVocationalTraining_CallsInsertSystemGeneratedPOPClaim()
        {
            _activityContracts.Add(_activityContract);
            _activityDomain.UpsertElapsedActivity(_activityContracts, "0", 1, true, false);

            Assert.IsTrue(_mockPopClaimDomain.HasInsertSystemGeneratedPOPClaimBeenCalled);
            Assert.AreEqual(1, _mockPopClaimDomain.VoicationalPOPClaimCallCount);
        }

        [TestMethod]
        public void UpsertElapsedActivity_ForVocationalTraining_CallsInsertSystemGeneratedPOPClaimOnlyOnce()
        {
            _activityContracts.Add(_activityContract);
            _activityContracts.Add(_activityContract);
            _activityDomain.UpsertElapsedActivity(_activityContracts, "0", 1, true, false);

            Assert.AreEqual(1, _mockPopClaimDomain.VoicationalPOPClaimCallCount);
        }

        [TestMethod]
        public void UpsertElapsedActivity_ForEducationalAttainment_CallsInsertSystemGeneratedPOPClaim()
        {
            _activityContract.ActivityTypeCode = ActivityCode.HE;
            _activityContracts.Add(_activityContract);
            _activityDomain.UpsertElapsedActivity(_activityContracts, "0", 1, true, false);

            Assert.IsTrue(_mockPopClaimDomain.HasInsertSystemGeneratedPOPClaimBeenCalled);
            Assert.AreEqual(1, _mockPopClaimDomain.EduationalPOPClaimCallCount);
        }

        [TestMethod]
        public void UpsertElapsedActivity_ForEducationalAttainment_CallsInsertSystemGeneratedPOPClaimOnlyOnce()
        {
            _activityContract.ActivityTypeCode = ActivityCode.HE;
            _activityContracts.Add(_activityContract);
            _activityContracts.Add(_activityContract);
            _activityDomain.UpsertElapsedActivity(_activityContracts, "0", 1, true, false);

            Assert.AreEqual(1, _mockPopClaimDomain.EduationalPOPClaimCallCount);
        }

        [TestMethod]
        public void EndActivityTransaction_WithValidContract_InsertsTransaction()
        {
            _privateActivityDomain.Invoke("EndActivityTransaction", _endActivityTransactionArgs);

            Assert.IsTrue(_mockWorkerRepository.GetHasBeenCalled);
            Assert.AreEqual(1, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void EndActivityTransaction_WithValidContract_InsertsValidContract()
        {
            _privateActivityDomain.Invoke("EndActivityTransaction", _endActivityTransactionArgs);

            Assert.AreEqual(0,                            _mockTransactionDomain.Transaction.ParticipantId);
            Assert.AreEqual(0,                            _mockTransactionDomain.Transaction.WorkerId);
            Assert.AreEqual(0,                            _mockTransactionDomain.Transaction.OfficeId);
            Assert.AreEqual(DateTime.Today,               _mockTransactionDomain.Transaction.EffectiveDate);
            Assert.AreEqual(DateTime.Today,               _mockTransactionDomain.Transaction.CreatedDate);
            Assert.AreEqual(TransactionTypes.ActivityEnd, _mockTransactionDomain.Transaction.TransactionTypeCode);
            Assert.AreEqual(0,                            _mockTransactionDomain.Transaction.ParticipantId);
            Assert.AreEqual(_authUser.WIUID,              _mockTransactionDomain.Transaction.ModifiedBy);
            Assert.AreEqual(ActivityCode.JS,              _mockTransactionDomain.Transaction.StatusCode);
        }

        [TestMethod]
        public void EndActivity_WithCarriedOverActivityButEndDateGreaterThanEPBeginDate_DonotInsertsTransaction()
        {
            _activityContract.IsCarriedOver = true;
            _activityContract.EndDate       = "01/01/2050";
            _privateActivityDomain.Invoke("EndActivity", _endActivityArgs);

            Assert.AreEqual(0, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void EndActivity_WithCarriedOverActivityWithEndDateLessthanEPBeginDate_InsertsTransaction()
        {
            _activityContract.IsCarriedOver = true;
            _activityContract.EndDate       = "01/01/2015";
            _privateActivityDomain.Invoke("EndActivity", _endActivityArgs);

            Assert.AreEqual(1, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void EndActivity_WithoutCarriedOverActivity_DonotInsertTransaction()
        {
            _activityContract.IsCarriedOver = false;
            _activityContract.EndDate       = "01/01/2050";
            _privateActivityDomain.Invoke("EndActivity", _endActivityArgs);

            Assert.AreEqual(0, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void UpsertElapsedActivities_WithValidActivityContracts_InsertsTransactions()
        {
            _activityContract.ActivityTypeCode = ActivityCode.HE;
            _activityContracts.Add(_activityContract);
            _activityContracts.Add(_activityContract);
            _activityDomain.UpsertElapsedActivity(_activityContracts, "0", 1, true, false);

            Assert.AreEqual(3, _mockTransactionDomain.InsertCount);
        }


        private class MockWorkerRepository : MockRepositoryBase<Worker>, IWorkerRepository
        {
            public bool GetHasBeenCalled;

            public new Worker Get(Expression<Func<Worker, bool>> clause)
            {
                var worker       = new Worker();
                GetHasBeenCalled = true;
                return worker;
            }
        }

        private class MockActivityRepository : MockRepositoryBase<Activity>, IActivityRepository
        {
            public new Activity Get(Expression<Func<Activity, bool>> clause)
            {
                var employabilityPlanActivityBridge = new EmployabilityPlanActivityBridge
                                                      {
                                                          EmployabilityPlanId = 1,
                                                          EmployabilityPlan   = new EmployabilityPlan
                                                                                {
                                                                                    ParticipantId              = 1,
                                                                                    ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                                                 {
                                                                                                                     OfficeId = 1
                                                                                                                 }
                                                                                }
                                                      };
                return new Activity
                       {
                           Id           = 3,
                           ActivityType = new ActivityType
                                          {
                                              Code = ActivityCode.JS
                                          },
                           EmploybilityPlanActivityBridges = new List<EmployabilityPlanActivityBridge> { employabilityPlanActivityBridge }
                       };
            }
        }

        private class MockPopClaimDomain : IPOPClaimDomain
        {
            public bool HasInsertSystemGeneratedPOPClaimBeenCalled;
            public int  VoicationalPOPClaimCallCount;
            public int  EduationalPOPClaimCallCount;

            public List<POPClaimContract> GetPOPClaims(int participantId)
            {
                throw new NotImplementedException();
            }

            public List<POPClaimContract> GetPOPClaimsByAgency(string agencyCode = null)
            {
                throw new NotImplementedException();
            }

            public POPClaimContract GetPOPClaim(int i)
            {
                throw new NotImplementedException();
            }

            public List<POPClaimEmploymentContract> GetEmploymentsForPOP(string pin, int popClaimId)
            {
                throw new NotImplementedException();
            }

            public void UpsertPOPClaim(POPClaimContract contract, bool isSystemGenerated = false)
            {
                throw new NotImplementedException();
            }

            public PreAddingPOPClaimContract PreAddCheck(POPClaimContract contract)
            {
                throw new NotImplementedException();
            }

            public List<POPClaimContract> GetPOPClaimsWithStatuses(List<string> statuses, string agencyCode = null)
            {
                throw new NotImplementedException();
            }

            public bool InsertSystemGeneratedPOPClaim(EmployabilityPlan employabilityPlan, string activityTypeCode, string activityCompletionReasonCode, DateTime? activityEndDate, int activityId, string popClaimType)
            {
                HasInsertSystemGeneratedPOPClaimBeenCalled = true;

                if (popClaimType == POPClaimType.VocationalTrainingCd)
                    VoicationalPOPClaimCallCount++;

                if (popClaimType == POPClaimType.EducationalAttainmentCd)
                    EduationalPOPClaimCallCount++;

                return false;
            }
        }

        private class MockEpaBridgeRepository : MockRepositoryBase<EmployabilityPlanActivityBridge>, IEmployabilityPlanActivityBridgeRepository
        {
        }

        private class MockActivityScheduleRepository : MockRepositoryBase<ActivitySchedule>, IActivityScheduleRepository
        {
        }
    }
}
