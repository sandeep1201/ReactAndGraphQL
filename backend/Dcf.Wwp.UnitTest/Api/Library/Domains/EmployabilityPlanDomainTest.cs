using System;
using System.Collections.Generic;
using System.Linq;
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
using ActivityCompletionReason = Dcf.Wwp.Model.Interface.Constants.ActivityCompletionReason;
using ContractArea = Dcf.Wwp.DataAccess.Models.ContractArea;
using EnrolledProgram = Dcf.Wwp.DataAccess.Models.EnrolledProgram;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using RuleReason = Dcf.Wwp.DataAccess.Models.RuleReason;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class EmployabilityPlanDomainTest
    {
        private MockTransactionDomain                          _mockTransactionDomain;
        private MockPopClaimDomain                             _mockPopClaimDomain;
        private MockWorkerRepository                           _mockWorkerRepository;
        private MockParticipantRepository                      _mockParticipantRepository;
        private EmployabilityPlanDomain                        _employabilityPlanDomain;
        private MockEmployabilityPlanStatusRepository          _mockEmployabilityPlanStatusRepository;
        private MockParticipantEnrolledCutOverBridgeRepository _mockParticipantEnrolledCutOverBridgeRepository;
        private AuthUser                                       _authUser;
        private MockEmployabilityPlanRepository                _mockEmployabilityPlanRepository;
        private MockActivityDomain                             _mockActivityDomain;
        private MockGoalDomain                                 _mockGoalDomain;
        private MockActivityScheduleRepository                 _mockActivityScheduleRepository;
        private MockEpActivityBridgeRepository                 _mockEpActivityBridgeRepository;
        private MockEpGoalsBridgeRepository                    _mockEpGoalsBridgeRepository;
        private MockEpEmploymentInfoBridgeRepository           _mockEpEmploymentInfoBridgeRepository;
        private MockWorkerTaskListRepository                   _mockWorkerTaskListRepository;
        private MockOfficeRepository                           _mockOfficeRepository;
        private MockRuleReasonRepository                       _mockRuleReasonRepository;

        private static readonly Participant ParticipantWithEndDateOrPepInW2 = new Participant
                                                                              {
                                                                                  PinNumber         = 10000,
                                                                                  CareerAssessments = new List<CareerAssessment>(),
                                                                                  JobReadinesses    = new List<JobReadiness>(),
                                                                                  EducationExams    = new List<EducationExam>(),
                                                                                  WorkerTaskLists   = new List<WorkerTaskList>()
                                                                              };

        private static readonly Participant ParticipantWithOutEndDate = new Participant
                                                                        {
                                                                            PinNumber = 10500
                                                                        };

        private static readonly Participant ParticipantSubmit = new Participant
                                                                {
                                                                    PinNumber = 1234567890
                                                                };


        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionDomain                          = new MockTransactionDomain();
            _mockPopClaimDomain                             = new MockPopClaimDomain();
            _mockWorkerRepository                           = new MockWorkerRepository();
            _mockParticipantRepository                      = new MockParticipantRepository();
            _mockEmployabilityPlanRepository                = new MockEmployabilityPlanRepository();
            _mockEmployabilityPlanStatusRepository          = new MockEmployabilityPlanStatusRepository();
            _mockParticipantEnrolledCutOverBridgeRepository = new MockParticipantEnrolledCutOverBridgeRepository();
            _mockActivityDomain                             = new MockActivityDomain();
            _mockGoalDomain                                 = new MockGoalDomain();
            _mockActivityScheduleRepository                 = new MockActivityScheduleRepository();
            _authUser                                       = new AuthUser { WIUID = "1111" };
            _mockEpActivityBridgeRepository                 = new MockEpActivityBridgeRepository();
            _mockEpGoalsBridgeRepository                    = new MockEpGoalsBridgeRepository();
            _mockEpEmploymentInfoBridgeRepository           = new MockEpEmploymentInfoBridgeRepository();
            _mockWorkerTaskListRepository                   = new MockWorkerTaskListRepository();
            _mockOfficeRepository                           = new MockOfficeRepository();
            _mockRuleReasonRepository                       = new MockRuleReasonRepository();
            _employabilityPlanDomain                        = new EmployabilityPlanDomain(null, null, null, _mockEmployabilityPlanRepository,
                                                                                          _mockParticipantRepository, null, _mockWorkerRepository, new MockUnitOfWork(),
                                                                                          _authUser, _mockEpGoalsBridgeRepository, _mockEmployabilityPlanStatusRepository,
                                                                                          _mockEpActivityBridgeRepository, _mockActivityScheduleRepository,
                                                                                          _mockRuleReasonRepository, null, _mockEpEmploymentInfoBridgeRepository,
                                                                                          _mockOfficeRepository, null, null, _mockActivityDomain, _mockGoalDomain,
                                                                                          _mockTransactionDomain, _mockPopClaimDomain, null,
                                                                                          _mockParticipantEnrolledCutOverBridgeRepository,
                                                                                          new MockWorkerTaskStatusRepository(), new MockWorkerTaskCategoryRepository(),
                                                                                          _mockWorkerTaskListRepository, null);
        }

        [TestMethod]
        public void SubmitPlan_ValidEPId_InsertsFirstTransactions()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantSubmit.PinNumber.ToString(), 4);

            Assert.AreEqual(1, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void SubmitPlan_ValidEPIdWithoutActivityEndDate_InsertsTwoTransactions()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithOutEndDate.PinNumber.ToString(), 3);

            Assert.AreEqual(2, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void SubmitPlan_ValidEPIdWithActivityEndDate_InsertsThirdTransactions()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), 2);

            Assert.AreEqual(3, _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithCareerAssessment_DoNotInsertCareerAssesmentWorkerTask()
        {
            _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Participant.CareerAssessments = new List<CareerAssessment>
                                                                                                            {
                                                                                                                new CareerAssessment()
                                                                                                            };
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Id));
            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithCareerAssessmentInWorkerTask_DoNotInsertCareerAssesmentWorkerTask()
        {
            _mockWorkerTaskListRepository.IsCareerAssessmentTask = true;
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Id));
            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithNoCareerAssessment_InsertsCareerAssesmentWorkerTask()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsTrue(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Id));
            Assert.IsTrue(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithJobReadiness_DoNotInsertJobReadinessWorkerTask()
        {
            _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Participant.JobReadinesses = new List<JobReadiness>
                                                                                                         {
                                                                                                             new JobReadiness()
                                                                                                         };
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Id));
            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithJobReadinessInWorkerTask_DoNotInsertJobReadinessWorkerTask()
        {
            _mockWorkerTaskListRepository.IsJobReadinessTask = true;
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Id));
            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithNoJobReadiness_InsertsJobReadinessWorkerTask()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsTrue(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Id));
            Assert.IsTrue(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithTestScores_DoNotInsertTestScoresWorkerTask()
        {
            _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Participant.EducationExams = new List<EducationExam>
                                                                                                         {
                                                                                                             new EducationExam()
                                                                                                         };
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Id));
            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithTestScoresInWorkerTask_DoNotInsertTestScoresWorkerTask()
        {
            _mockWorkerTaskListRepository.IsTestScoresTask = true;
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Id));
            Assert.IsFalse(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Description));
        }

        [TestMethod]
        public void SubmitPlan_ParticipantWithNoTestScores_InsertsTestScoresWorkerTask()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsTrue(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.CategoryId  == MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Id));
            Assert.IsTrue(_mockWorkerTaskListRepository.AddedWorkerTaskLists.Any(i => i.TaskDetails == MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Description));
        }

        // Test for 2 or more scenario

        [TestMethod]
        public void SubmitPlan_ForVocationalTraining_CallsInsertSystemGeneratedPOPClaim()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsTrue(_mockPopClaimDomain.HasInsertSystemGeneratedPOPClaimBeenCalled);
            Assert.AreEqual(1, _mockPopClaimDomain.VocationalPOPClaimCallCount);
        }

        [TestMethod]
        public void SubmitPlan_ForVocationalTraining_CallsInsertSystemGeneratedPOPClaimOnlyOnce()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.AreEqual(1, _mockPopClaimDomain.VocationalPOPClaimCallCount);
        }

        [TestMethod]
        public void SubmitPlan_ForEducationalAttainment_CallsInsertSystemGeneratedPOPClaim()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.IsTrue(_mockPopClaimDomain.HasInsertSystemGeneratedPOPClaimBeenCalled);
            Assert.AreEqual(1, _mockPopClaimDomain.EducationalPOPClaimCallCount);
        }

        [TestMethod]
        public void SubmitPlan_ForEducationalAttainment_CallsInsertSystemGeneratedPOPClaimOnlyOnce()
        {
            _employabilityPlanDomain.SubmitPlan(ParticipantWithEndDateOrPepInW2.PinNumber.ToString(), _mockEmployabilityPlanRepository.EmployabilityPlanForWorkerTask.Id);

            Assert.AreEqual(1, _mockPopClaimDomain.EducationalPOPClaimCallCount);
        }

        [TestMethod]
        public void CarryOverActivities_ForSubsequentEPWithActivities_ReturnsTrue()
        {
            Assert.IsTrue(_employabilityPlanDomain.CarryOverActivities(new EmployabilityPlan(), _mockEpActivityBridgeRepository.FakeActivityBridge1.EmployabilityPlanId, DateTime.Today));
        }

        [TestMethod]
        public void CarryOverActivities_ForSubsequentEPWithOutActivities_ReturnsFalse()
        {
            Assert.IsFalse(_employabilityPlanDomain.CarryOverActivities(new EmployabilityPlan(), 1001, DateTime.Today));
        }

        [TestMethod]
        public void UpsertPlan_WithExistingContract_WillNotReturnErrorMessage()
        {
            var epContract = _employabilityPlanDomain.UpsertPlan(new EmployabilityPlanContract { Id = 1 }, ParticipantSubmit.PinNumber.ToString(), _mockEpActivityBridgeRepository.FakeActivityBridge1.EmployabilityPlanId);

            Assert.IsFalse(_mockRuleReasonRepository.GetManyHasBeenCalled);
            Assert.IsNull(epContract.ErrorMessage);
        }

        [TestMethod]
        public void UpsertPlan_WithOutActivitiesForSubsequesntEP_WillNotReturnErrorMessage()
        {
            var epContract = _employabilityPlanDomain.UpsertPlan(new EmployabilityPlanContract { CanSaveWithoutActivity = true }, ParticipantSubmit.PinNumber.ToString(), 1001);

            Assert.IsFalse(_mockRuleReasonRepository.GetManyHasBeenCalled);
            Assert.IsNull(epContract.ErrorMessage);
        }

        [TestMethod]
        public void UpsertPlan_WithActivitiesForSubsequesntEP_CallRuleReasonepositoryAndUpsertPlanReturnsErrorMessage()
        {
            var epContract = _employabilityPlanDomain.UpsertPlan(new EmployabilityPlanContract { CanSaveWithoutActivity = true }, ParticipantSubmit.PinNumber.ToString(), _mockEpActivityBridgeRepository.FakeActivityBridge1.EmployabilityPlanId);

            Assert.IsTrue(_mockRuleReasonRepository.GetManyHasBeenCalled);
            Assert.IsNotNull(epContract.ErrorMessage);
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

        private class MockPopClaimDomain : IPOPClaimDomain
        {
            public bool HasInsertSystemGeneratedPOPClaimBeenCalled;
            public int  VocationalPOPClaimCallCount;
            public int  EducationalPOPClaimCallCount;

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
                    VocationalPOPClaimCallCount++;

                if (popClaimType == POPClaimType.EducationalAttainmentCd)
                    EducationalPOPClaimCallCount++;

                return false;
            }
        }

        private class MockActivityDomain : IActivityDomain
        {
            public EmployabilityPlan GetRecentSubmittedEP(string pin, int epId, int enrolledProgramId, bool considerEnded = false)
            {
                return new EmployabilityPlan();
            }

            public ActivityContract GetActivity(int activityId, int employabilityPlanId)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> GetActivitiesForPin(string pin, bool fromEvents = false)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> GetActivitiesForPep(string pin, int pepId)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> GetActivitiesForEp(int epId)
            {
                throw new NotImplementedException();
            }

            public List<EventsContract> GetEvents(string pin, int epId, int programId, DateTime epBeginDate)
            {
                throw new NotImplementedException();
            }

            public bool DeleteActivity(string pin, int id, int epId, bool fromEndEp)
            {
                throw new NotImplementedException();
            }

            public void DeleteActivityAndSchedules(int epId, Activity activity)
            {
                throw new NotImplementedException();
            }

            public PreSavingActivityContract PreSaveActivity(string pin, int epId, string activityTypeId)
            {
                throw new NotImplementedException();
            }

            public ActivityContract UpsertActivity(ActivityContract activityContract, string pin, int epId, int subepId)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> UpsertElapsedActivity(List<ActivityContract> activityContracts, string pin, int epId, bool isDisenrollment, bool fromEpOverView)
            {
                throw new NotImplementedException();
            }

            public void EndActivityTransactionalSave(string xml, int? epId = null, string programCd = null)
            {
                throw new NotImplementedException();
            }

            public void EndStatus(int epId, decimal pin, IReadOnlyCollection<ActivityContract> contracts, DateTime updateTime)
            {
            }
        }

        private class MockGoalDomain : IGoalDomain
        {
            public GoalContract       GetGoal(int             id)
            {
                throw new NotImplementedException();
            }

            public List<GoalContract> GetGoalsForEP(int       epId)
            {
                throw new NotImplementedException();
            }

            public List<GoalContract> GetGoalsForPin(string   pin)
            {
                throw new NotImplementedException();
            }

            public bool               DeleteGoal(int          goalId,       int    epId, bool canCommit = true)
            {
                throw new NotImplementedException();
            }

            public GoalContract       UpsertGoal(GoalContract goalContract, string pin,  int  epId)
            {
                throw new NotImplementedException();
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

        private class MockEmployabilityPlanRepository : MockRepositoryBase<EmployabilityPlan>, IEmployabilityPlanRepository
        {
            public readonly EmployabilityPlan EmployabilityPlanForWorkerTask = new EmployabilityPlan
                                                                               {
                                                                                   Id            = 5,
                                                                                   ParticipantId = 500,
                                                                                   Participant = new Participant
                                                                                                 {
                                                                                                     PinNumber         = 10000,
                                                                                                     CareerAssessments = new List<CareerAssessment>(),
                                                                                                     JobReadinesses    = new List<JobReadiness>(),
                                                                                                     EducationExams    = new List<EducationExam>(),
                                                                                                     WorkerTaskLists   = new List<WorkerTaskList>(),
                                                                                                 },
                                                                                   ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                                                {
                                                                                                                    OfficeId = 1,
                                                                                                                    EnrolledProgram = new EnrolledProgram
                                                                                                                                      {
                                                                                                                                          ProgramCode = "WW"
                                                                                                                                      }
                                                                                                                },
                                                                                   EmploybilityPlanActivityBridges = new List<EmployabilityPlanActivityBridge>
                                                                                                                     {
                                                                                                                         new EmployabilityPlanActivityBridge
                                                                                                                         {
                                                                                                                             Activity = new Activity
                                                                                                                                        {
                                                                                                                                            StartDate = DateTime.Today,
                                                                                                                                            EndDate   = DateTime.Now,
                                                                                                                                            ActivityType = new ActivityType
                                                                                                                                                           {
                                                                                                                                                               Code = ActivityCode.JS
                                                                                                                                                           },
                                                                                                                                            ActivityCompletionReason = new Wwp.DataAccess.Models.ActivityCompletionReason
                                                                                                                                                                       {
                                                                                                                                                                           Name = ActivityCompletionReason.V
                                                                                                                                                                       }
                                                                                                                                        }
                                                                                                                         },
                                                                                                                         new EmployabilityPlanActivityBridge
                                                                                                                         {
                                                                                                                             Activity = new Activity
                                                                                                                                        {
                                                                                                                                            StartDate = DateTime.Today,
                                                                                                                                            EndDate   = DateTime.Now,
                                                                                                                                            ActivityType = new ActivityType
                                                                                                                                                           {
                                                                                                                                                               Code = ActivityCode.JS
                                                                                                                                                           },
                                                                                                                                            ActivityCompletionReason = new Wwp.DataAccess.Models.ActivityCompletionReason
                                                                                                                                                                       {
                                                                                                                                                                           Name = ActivityCompletionReason.V
                                                                                                                                                                       }
                                                                                                                                        }
                                                                                                                         },
                                                                                                                         new EmployabilityPlanActivityBridge
                                                                                                                         {
                                                                                                                             Activity = new Activity
                                                                                                                                        {
                                                                                                                                            StartDate = DateTime.Today,
                                                                                                                                            EndDate   = DateTime.Now,
                                                                                                                                            ActivityType = new ActivityType
                                                                                                                                                           {
                                                                                                                                                               Code = ActivityCode.GE
                                                                                                                                                           },
                                                                                                                                            ActivityCompletionReason = new Wwp.DataAccess.Models.ActivityCompletionReason
                                                                                                                                                                       {
                                                                                                                                                                           Name = ActivityCompletionReason.V
                                                                                                                                                                       }
                                                                                                                                        }
                                                                                                                         },
                                                                                                                         new EmployabilityPlanActivityBridge
                                                                                                                         {
                                                                                                                             Activity = new Activity
                                                                                                                                        {
                                                                                                                                            StartDate = DateTime.Today,
                                                                                                                                            EndDate   = DateTime.Now,
                                                                                                                                            ActivityType = new ActivityType
                                                                                                                                                           {
                                                                                                                                                               Code = ActivityCode.HE
                                                                                                                                                           },
                                                                                                                                            ActivityCompletionReason = new Wwp.DataAccess.Models.ActivityCompletionReason
                                                                                                                                                                       {
                                                                                                                                                                           Name = ActivityCompletionReason.V
                                                                                                                                                                       }
                                                                                                                                        }
                                                                                                                         }
                                                                                                                     },
                                                                                   Organization = new Organization()
                                                                               };


            public new IEnumerable<EmployabilityPlan> GetMany(Expression<Func<EmployabilityPlan, bool>> clause)
            {
                var activityWithoutEndDate = new EmployabilityPlanActivityBridge
                                             {
                                                 Activity = new Activity
                                                            {
                                                                StartDate = DateTime.Today,
                                                                ActivityType = new ActivityType
                                                                               {
                                                                                   Code = "TT"
                                                                               }
                                                            }
                                             };
                var activityWithEndDate = new EmployabilityPlanActivityBridge
                                          {
                                              Activity = new Activity
                                                         {
                                                             StartDate = DateTime.Today,
                                                             EndDate   = DateTime.Now,
                                                             ActivityType = new ActivityType
                                                                            {
                                                                                Code = ActivityCode.JS
                                                                            },
                                                             ActivityCompletionReason = new Wwp.DataAccess.Models.ActivityCompletionReason
                                                                                        {
                                                                                            Code = ActivityCompletionReason.V
                                                                                        }
                                                         }
                                          };
                var employabilityPlanWithActivityEndDate = new EmployabilityPlan
                                                           {
                                                               Id          = 2,
                                                               Participant = ParticipantWithEndDateOrPepInW2,
                                                               ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                            {
                                                                                                OfficeId = 1,
                                                                                                EnrolledProgram = new EnrolledProgram
                                                                                                                  {
                                                                                                                      ProgramCode = "WW"
                                                                                                                  }
                                                                                            },
                                                               EmploybilityPlanActivityBridges = new List<EmployabilityPlanActivityBridge> { activityWithEndDate }
                                                           };

                var employabilityPlanWithoutActivityEndDate = new EmployabilityPlan
                                                              {
                                                                  Id          = 3,
                                                                  Participant = ParticipantWithOutEndDate,
                                                                  ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                               {
                                                                                                   OfficeId = 1
                                                                                               },
                                                                  EmploybilityPlanActivityBridges = new List<EmployabilityPlanActivityBridge> { activityWithoutEndDate }
                                                              };

                var employabilityPlanForSubmit = new EmployabilityPlan
                                                 {
                                                     Id          = 4,
                                                     BeginDate   = DateTime.Today,
                                                     Participant = ParticipantSubmit,
                                                     ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                  {
                                                                                      OfficeId = 1
                                                                                  },
                                                     EmploybilityPlanActivityBridges = new List<EmployabilityPlanActivityBridge>()
                                                 };

                var mockEmployabilityPlans = new List<EmployabilityPlan> { employabilityPlanWithActivityEndDate, employabilityPlanWithoutActivityEndDate, employabilityPlanForSubmit, EmployabilityPlanForWorkerTask };

                return mockEmployabilityPlans.Where(clause.Compile());
            }
        }

        private class MockEmployabilityPlanStatusRepository : MockRepositoryBase<EmployabilityPlanStatusType>, IEmployabilityPlanStatusTypeRepository
        {
        }

        private class MockParticipantEnrolledCutOverBridgeRepository : MockRepositoryBase<ParticipantEnrolledProgramCutOverBridge>, IParticipantEnrolledProgramCutOverBridgeRepository
        {
        }

        private class MockActivityScheduleRepository : MockRepositoryBase<ActivitySchedule>, IActivityScheduleRepository
        {
        }

        private class MockEpGoalsBridgeRepository : MockRepositoryBase<EmployabilityPlanGoalBridge>, IEmployabilityPlanGoalBridgeRepository
        {
        }

        private class MockEpActivityBridgeRepository : MockRepositoryBase<EmployabilityPlanActivityBridge>, IEmployabilityPlanActivityBridgeRepository
        {
            public readonly EmployabilityPlanActivityBridge FakeActivityBridge1 = new EmployabilityPlanActivityBridge
                                                                         {
                                                                             EmployabilityPlanId = 1000,
                                                                             Activity            = new Activity
                                                                                                   {
                                                                                                       StartDate    = DateTime.Today,
                                                                                                       EndDate      = DateTime.Now,
                                                                                                       ActivityType = new ActivityType
                                                                                                                      {
                                                                                                                          Code = ActivityCode.JS
                                                                                                                      },
                                                                                                       ActivityCompletionReason = null
                                                                                                   }
                                                                         };

            public new IEnumerable<EmployabilityPlanActivityBridge> GetMany(Expression<Func<EmployabilityPlanActivityBridge, bool>> clause)
            {
                var mockEmployabilityPlanActivityBridge = new List<EmployabilityPlanActivityBridge> { FakeActivityBridge1 };

                return mockEmployabilityPlanActivityBridge.Where(clause.Compile());
            }
        }

        private class MockEpEmploymentInfoBridgeRepository : MockRepositoryBase<EmployabilityPlanEmploymentInfoBridge>, IEmployabilityPlanEmploymentInfoBridgeRepository
        {
        }

        private class MockOfficeRepository : MockRepositoryBase<Office>, IOfficeRepository
        {
            public new IEnumerable<Office> GetMany(Expression<Func<Office, bool>> clause)
            {
                return new List<Office> { new Office { ContractArea = new ContractArea { Organization = new Organization() }, InActivatedDate = DateTime.Now, ActiviatedDate = DateTime.Now } };
            }
        }

        private class MockRuleReasonRepository : MockRepositoryBase<RuleReason>, IRuleReasonRepository
        {
            public new RuleReason Get(Expression<Func<RuleReason, bool>> clause)
            {
                GetManyHasBeenCalled = true;
                return new RuleReason { Name = "Test" };
            }
        }
    }
}
