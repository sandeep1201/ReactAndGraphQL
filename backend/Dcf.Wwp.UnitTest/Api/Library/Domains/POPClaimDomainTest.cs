using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Api.AutoMapper;
using Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActivityCompletionReason = Dcf.Wwp.Model.Interface.Constants.ActivityCompletionReason;
using POPClaimStatusType = Dcf.Wwp.Model.Interface.Constants.POPClaimStatusType;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class POPClaimDomainTest
    {
        #region Properties

        private MockPOPClaimRepository                   _popClaimRepository;
        private MockPOPClaimEmploymentBridgeRepository   _popClaimEmploymentBridgeRepository;
        private MockPOPClaimActivityBridgeRepository     _popClaimActivityBridgeRepository;
        private POPClaimDomain                           _popClaimDomain;
        private MockParticipantEnrolledProgramRepository _pepRepository;
        private MockOrganizationRepository               _organizationRepository;
        private MockEmployabilityPlanRepository          _employabilityPlanRepository;
        private MockParticipantPlacementRepository       _participantPlacementRepository;
        private MockPOPClaimStatusTypeRepository         _popClaimStatusTypeRepository;
        private MockPOPClaimStatusRepository             _popClaimStatusRepository;
        private MockRuleReasonRepository                 _ruleReasonRepository;
        private POPClaimContract                         _popClaimContract;
        private POPClaimEmploymentContract               _popClaimEmploymentContract;
        private MockRuleRepository                       _ruleRepository;
        private MockWIUIDToFullNameModifiedByResolver    _WIUIDToFullNameModifiedByResolver;
        private MapperBaseTest                           _mapperBaseTest;
        private MapperConfiguration                      _config;
        private IMapper                                  _mockMapper;
        private MockPOPClaimTypeRepository               _popClaimTypeRepository;
        private MockEmploymentInformationRepository      _employmentInformationRepository;
        private IAuthUser                                _authUser;
        private MockPOPClaimHighWageRepository           _popClaimHighWageRepository;
        private string                                   _modifiedBy;
        private DateTime                                 _modifiedDate;
        private WorkerTaskListDomain                     _workerTaskListDomain;
        private MockWorkerTaskCategoryRepository         _workerTaskCategoryRepository;
        private MockWorkerRepository                     _workerRepository;
        private MockParticipantRepository                _participantRepository;
        private MockWorkerTaskListRepository             _mockWorkerTaskListRepository;
        private MockSpecialInitiativeRepository          _mockfeatureToggleRepository;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _popClaimRepository                         = new MockPOPClaimRepository();
            _popClaimEmploymentBridgeRepository         = new MockPOPClaimEmploymentBridgeRepository();
            _popClaimActivityBridgeRepository           = new MockPOPClaimActivityBridgeRepository();
            _pepRepository                              = new MockParticipantEnrolledProgramRepository();
            _organizationRepository                     = new MockOrganizationRepository();
            _employabilityPlanRepository                = new MockEmployabilityPlanRepository();
            _participantPlacementRepository             = new MockParticipantPlacementRepository();
            _ruleReasonRepository                       = new MockRuleReasonRepository();
            _ruleRepository                             = new MockRuleRepository();
            _WIUIDToFullNameModifiedByResolver          = new MockWIUIDToFullNameModifiedByResolver();
            _mapperBaseTest                             = new MapperBaseTest();
            _popClaimTypeRepository                     = new MockPOPClaimTypeRepository();
            _employmentInformationRepository            = new MockEmploymentInformationRepository();
            _authUser                                   = new AuthUser { WIUID = "1111", Authorizations = new List<string> { Authorization.canAccessProgram_WW }, AgencyCode = AgencyCode.FSC };
            _popClaimHighWageRepository                 = new MockPOPClaimHighWageRepository();
            _popClaimStatusTypeRepository               = new MockPOPClaimStatusTypeRepository();
            _popClaimStatusRepository                   = new MockPOPClaimStatusRepository();
            _popClaimRepository.IsPreAddCheck           = true;
            _pepRepository.IsPOPClaim                   = true;
            _organizationRepository.IsPOPClaim          = true;
            _employabilityPlanRepository.IsPOPClaim     = true;
            _ruleReasonRepository.IsPOPClaim            = true;
            _employmentInformationRepository.IsPOPClaim = true;
            _modifiedBy                                 = "WWP";
            _modifiedDate                               = DateTime.Now;
            _workerTaskCategoryRepository               = new MockWorkerTaskCategoryRepository();
            _workerRepository                           = new MockWorkerRepository();
            _participantRepository                      = new MockParticipantRepository();
            _mockWorkerTaskListRepository               = new MockWorkerTaskListRepository();
            _workerTaskListDomain                       = new WorkerTaskListDomain(_mockWorkerTaskListRepository, _workerRepository, new MockWorkerStatusRepository(), _authUser, new MockUnitOfWork());
            _mockfeatureToggleRepository                = new MockSpecialInitiativeRepository();


            _popClaimContract = new POPClaimContract
                                {
                                    PinNumber            = 12345678890,
                                    ParticipantId        = 1,
                                    OrganizationId       = 1,
                                    POPClaimTypeId       = 1,
                                    ClaimPeriodBeginDate = new DateTime(2020, 08, 20),
                                    POPClaimEmployments  = new List<POPClaimEmploymentContract>()
                                };

            _popClaimEmploymentContract = new POPClaimEmploymentContract
                                          {
                                              EmploymentInformationId = 1,
                                              IsPrimary               = true,
                                              IsSelected              = true,
                                              JobBeginDate            = "08/19/2020",
                                              HoursWorked             = 110,
                                              Earnings                = 870,
                                          };

            _config = new MapperConfiguration(cfg =>
                                              {
                                                  cfg.AddProfile<POPClaimProfile>();

                                                  cfg.ConstructServicesUsing(i =>
                                                                             {
                                                                                 if (i.Name.Contains(nameof(WIUIDToFullNameModifiedByResolver)))
                                                                                     return _WIUIDToFullNameModifiedByResolver;

                                                                                 return null;
                                                                             });
                                              });

            _mockMapper = _config.CreateMapper();

            _popClaimDomain = new POPClaimDomain(new MockUnitOfWork(), _authUser, _mockMapper, _popClaimRepository, _popClaimEmploymentBridgeRepository, _popClaimActivityBridgeRepository, _popClaimStatusTypeRepository, _popClaimStatusRepository, _employmentInformationRepository,
                                                 _pepRepository, _organizationRepository, _employabilityPlanRepository, _participantPlacementRepository,
                                                 _ruleReasonRepository, _ruleRepository, _popClaimTypeRepository, _popClaimHighWageRepository, _workerTaskListDomain, _workerTaskCategoryRepository, _participantRepository, _mockfeatureToggleRepository);
        }

        [TestMethod]
        public void POPClaimProfileTestMappingEmptyEntityToContractForValueEquality()
        {
            var entity   = new POPClaimContract();
            var contract = _mockMapper.Map<POPClaimContract>(entity);

            _mapperBaseTest.AssertAllPropertiesMapped(entity, contract);

            Assert.AreEqual(_WIUIDToFullNameModifiedByResolver.ResolveString, "Success");
        }

        [TestMethod]
        public void GetPOPClaims_WithId_ReturnsPOPClaimsContract()
        {
            Assert.IsNotNull(_popClaimDomain.GetPOPClaims(123));
        }

        [TestMethod]
        public void GetPOPClaimsByAgency_WithOutAParticipantId_CallsGetQueryable()
        {
            _popClaimDomain.GetPOPClaimsByAgency();

            Assert.IsTrue(_popClaimRepository.HasGetAsQueryableBeenCalled);
        }

        [TestMethod]
        public void GetPOPClaimsWithStatuses_WithStatuses_CallsGetQueryable()
        {
            _popClaimDomain.GetPOPClaimsWithStatuses(new List<string> { POPClaimStatusType.SubmitCd, POPClaimStatusType.ReviewCd, POPClaimStatusType.ReturnCd });

            Assert.IsTrue(_popClaimRepository.HasGetAsQueryableBeenCalled);
        }

        [TestMethod]
        public void GetPOPClaims_WithId_CallsGetMany()
        {
            _popClaimDomain.GetPOPClaims(123);

            Assert.IsTrue(_popClaimRepository.HasGetManyBeenCalled);
            Assert.IsTrue(_popClaimRepository.HasGetManyBeenCalled);
        }

        [TestMethod]
        public void GetPOPClaim_WithId_CallsGet()
        {
            _popClaimDomain.GetPOPClaim(1);

            Assert.IsTrue(_popClaimRepository.HasGetBeenCalled);
        }

        [TestMethod]
        public void UpsertPOPClaim_WithoutContract_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _popClaimDomain.UpsertPOPClaim(null));
        }

        [TestMethod]
        public void UpsertPOPClaim_WithContract_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _popClaimDomain.UpsertPOPClaim(new POPClaimContract()));
        }

        [TestMethod]
        public void UpsertPOPClaim_WithActualContractButEmptyEmploymentsAndStatuses_ThrowsException()
        {
            var POPClaimEmployments = new List<POPClaimEmploymentContract>();
            var POPClaimStatuses    =  new List<POPClaimStatusContract>();
            var contract            = new POPClaimContract
                                      {
                                          Id                   = 0,
                                          ParticipantId        = 1,
                                          OrganizationId       = 6 ,
                                          POPClaimTypeId       = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                                          ClaimPeriodBeginDate = Convert.ToDateTime("10/12/2020"),
                                          POPClaimStatuses     = POPClaimStatuses,
                                          POPClaimEmployments  = POPClaimEmployments
                                      };

            Assert.ThrowsException<ArgumentNullException>(() => _popClaimDomain.UpsertPOPClaim(contract));
        }

        [TestMethod]
        public void UpsertPOPClaim_WithActualContractOfPOPClaimEmployments()
        {
            var popClaimEmployments = new List<POPClaimEmploymentContract>();
            var popClaimStatuses    = new List<POPClaimStatusContract>();
            popClaimEmployments.Add(new POPClaimEmploymentContract
                                    {
                                        Id                      = 0,
                                        EmploymentInformationId = 123,
                                        IsPrimary               = true,
                                        IsSelected              = true,
                                        HoursWorked             = 10,
                                        Earnings                = 12
                                    });
            var contract = new POPClaimContract
                           {
                               Id                   = 0,
                               ParticipantId        = 123,
                               OrganizationId       = 6,
                               POPClaimTypeId       = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                               ClaimPeriodBeginDate = Convert.ToDateTime("10/12/2020"),
                               POPClaimStatuses     = popClaimStatuses,
                               POPClaimEmployments  = popClaimEmployments
                           };

            _popClaimDomain.UpsertPOPEmployments(contract, new POPClaim(), _modifiedBy, _modifiedDate);

            Assert.IsTrue(_popClaimEmploymentBridgeRepository.HasDeleteRangeBeenCalled);
            Assert.IsTrue(_popClaimEmploymentBridgeRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void UpsertPOPClaim_WithActualContractOfPOPClaimActivity()
        {
            var popClaimStatuses = new List<POPClaimStatusContract>();
            var contract         = new POPClaimContract
                                   {
                                       Id                   = 0,
                                       ParticipantId        = 123,
                                       OrganizationId       = 6,
                                       POPClaimTypeId       = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                                       ClaimPeriodBeginDate = Convert.ToDateTime("10/12/2020"),
                                       POPClaimStatuses     = popClaimStatuses,
                                       ActivityId           = 2
                                   };

            _popClaimDomain.InsertPOPActivities(contract, new POPClaim(), _modifiedBy, _modifiedDate);

            Assert.IsTrue(_popClaimActivityBridgeRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void UpsertOrUpdatePOPStatus_WithoutContract_ThrowsException()
        {
            _popClaimDomain.UpsertPOPClaimStatus(new POPClaimContract(), new POPClaim(), _modifiedBy, _modifiedDate);
            Assert.IsTrue(_popClaimStatusRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void UpsertOrUpdatePOPEmployments_WithoutContract__ThrowsException()
        {
            Assert.ThrowsException<NullReferenceException>(() => _popClaimDomain.UpsertPOPEmployments(null, null, _modifiedBy, _modifiedDate));
        }

        [TestMethod]
        public void UpsertOrUpdatePOPEmployments_WithContractButWithNullValues__ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _popClaimDomain.UpsertPOPEmployments(new POPClaimContract(), new POPClaim(), _modifiedBy, _modifiedDate));
        }

        [TestMethod]
        public void InsertPOPActivities_WithoutContract__ThrowsException()
        {
            Assert.ThrowsException<NullReferenceException>(() => _popClaimDomain.InsertPOPActivities(null, new POPClaim(), _modifiedBy, _modifiedDate));
        }

        [TestMethod]
        public void InsertPOPActivities_WithContractButWithNullValues__ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _popClaimDomain.InsertPOPActivities(new POPClaimContract(), new POPClaim(), _modifiedBy, _modifiedDate));
        }

        [TestMethod]
        public void UpsertWorkerTaskList_WhenNewPOPClaim_hasBeenSubmitted()
        {
            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.SubmitCd;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.IsTrue(_mockWorkerTaskListRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void InsertPOPClaim_ShouldCreate_WorkerTask()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.SubmitCd;
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);

            Assert.IsTrue(_mockWorkerTaskListRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void InsertPOPClaim_InvalidContractWithNoClaimType_WillNotCreateWorkerTask()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);

            Assert.IsFalse(_mockWorkerTaskListRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void UpdatePOPClaim_ShouldNotCreate_WorkerTask()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimContract.Id       = 123;
            _popClaimContract.IsSubmit = false;
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);

            Assert.IsFalse(_mockWorkerTaskListRepository.HasAddBeenCalled);
        }

        [TestMethod]
        [DynamicData(nameof(GetContractForCreateWorkerTaskItemTest), DynamicDataSourceType.Method)]
        public void CreateWorkerTaskItem_ValidVariousContracts_InsertsNewWorkerTask(ContractForCreateWorkerTaskItemTest testData)
        {
            _popClaimContract.ClaimStatusTypeCode = testData.ClaimStatusTypeCode;
            _popClaimContract.POPClaimTypeId      = testData.POPClaimTypeId;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(testData.WorkerTaskId,           _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(_popClaimContract.ParticipantId, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().ParticipantId);
            Assert.AreEqual(testData.WorkerTaskDescription,  _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
            Assert.IsTrue(_mockWorkerTaskListRepository.HasAddBeenCalled);
        }


        [TestMethod]
        public void CreateWorkerTaskItem_ForJobAttainmentPOPClaimWithSubmitted_InsertsJobAttainmentInitiatedWorkerTask()
        {
            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.SubmitCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JobAttainmentInitiationTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JobAttainmentInitiationTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }


        [TestMethod]
        public void CreateWorkerTaskItem_ForJobAttainmentPOPClaimWithDeniedByDCFAdjudicatorOrApprover_InsertsJobAttainmentDeniedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.DeniedCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JobAttainmentDeniedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JobAttainmentDeniedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void CreateWorkerTaskItem_ForJobAttainmentPOPClaimWithDeniedByDCFAdjudicatorOrApprover_InsertsJobAttainmentReturnedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.ReturnCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JobAttainmentReturnedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JobAttainmentReturnedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }


        [TestMethod]
        public void CreateWorkerTaskItem_ForJobRetentionPOPClaimWithDeniedByDCFAdjudicatorOrApprover_InsertsJobRetainmentReturnedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.ReturnCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JRReturnedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JRReturnedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void CreateWorkerTaskItem_ForJobRetentionPOPClaimWithDeniedByDCFAdjudicatorOrApprover_InsertsJobRetainmentDeniedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.DeniedCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JRDeniedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JRDeniedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void CreateWorkerTaskItem_ForLongTermPOPClaimWithDeniedByDCFAdjudicatorOrApprover_InsertsLongTermDeniedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.DeniedCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.LTJADeniedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.LTJADeniedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void CreateWorkerTaskItem_ForLongTermPOPClaimWithReturnedByDCFAdjudicatorOrApprover_InsertsLongTermReturnedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.ReturnCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.LTJAReturnedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.LTJAReturnedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void CreateWorkerTaskItem_ForHighWagePOPClaimWithDeniedByDCFAdjudicatorOrApprover_InsertsHighWageDeniedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.DeniedCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JAHWDeniedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JAHWDeniedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void CreateWorkerTaskItem_ForHighWagePOPClaimWithReturnedByDCFAdjudicatorOrApprover_InsertsHighWageReturnedWorkerTask()
        {
            _authUser.AgencyCode     = AgencyCode.DCF;
            _authUser.Authorizations = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit, Authorization.canAccessProgram_WW };

            _popClaimContract.ClaimStatusTypeCode = POPClaimStatusType.ReturnCd;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimDomain.CreateWorkerTaskItem(_popClaimContract);

            Assert.AreEqual(MockWorkerTaskCategoryRepository.JAHWReturnedTask.Id,          _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().CategoryId);
            Assert.AreEqual(MockWorkerTaskCategoryRepository.JAHWReturnedTask.Description, _mockWorkerTaskListRepository.AddedWorkerTaskLists.First().TaskDetails);
        }

        [TestMethod]
        public void TestPreAddCheckConfiguresRulesEngineCorrectlyForJobRetention()
        {
            _popClaimContract.POPClaimTypeId = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.PreAddCheck(_popClaimContract);

            var ruleSet = _ruleRepository.GetRuleSets().SelectMany(i => i.Rules).ToList();

            Assert.IsTrue(ruleSet.Select(i => i.Tags).All(i => i.Contains(RuleReason.JobRetentionPOPClaimAdd)));
            Assert.AreEqual(7, ruleSet.Select(i => i.Tags).Count());
            Assert.IsTrue(_ruleRepository.HasCreateSessionBeenCalled);
            Assert.AreEqual(16, _ruleRepository.InsertedFacts.Count);
            _ruleRepository.InsertedFacts.ForEach(i => Assert.IsNotNull(i.Value));
            Assert.IsTrue(_ruleRepository.FiredRules.Count >= 0);
        }

        [TestMethod]
        public void TestPreAddCheckConfiguresRulesEngineCorrectlyForJobAttainment()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.PreAddCheck(_popClaimContract);

            var ruleSet = _ruleRepository.GetRuleSets().SelectMany(i => i.Rules).ToList();

            Assert.IsTrue(ruleSet.Select(i => i.Tags).All(i => i.Contains(RuleReason.JobAttainmentPOPClaimAdd)));
            Assert.AreEqual(7, ruleSet.Select(i => i.Tags).Count());
            Assert.IsTrue(_ruleRepository.HasCreateSessionBeenCalled);
            Assert.AreEqual(16, _ruleRepository.InsertedFacts.Count);
            _ruleRepository.InsertedFacts.ForEach(i => Assert.IsNotNull(i.Value));
            Assert.IsTrue(_ruleRepository.FiredRules.Count >= 0);
        }


        [TestMethod]
        public void ValidateEmploymentDaysAndLeaveOfAbsences_WhenThereIsLessThan93DaysOfEmploymentForOneSelectedEmployments_ReturnsFalse()
        {
            _popClaimContract.POPClaimTypeCode     = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId       = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate = new DateTime(2020, 08, 19);
            _popClaimRepository.IsCheckingJRRules  = true;

            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(!_popClaimDomain.ValidateEmploymentDaysAndLeaveOfAbsence(_popClaimContract.POPClaimEmployments, 1234567890));
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPE9314));
        }

        [TestMethod]
        public void ValidateEmploymentDaysAndLeaveOfAbsences_WhenThereIsEqualtoOrMoreThan93DaysOfEmploymentForOneSelectedEmployments_ReturnsTrue()
        {
            _employmentInformationRepository.lengthOfEmploymentMoreThan93 = true;
            _popClaimContract.POPClaimTypeCode                            = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId                              = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate                        = new DateTime(2020, 08, 19);
            _popClaimRepository.IsCheckingJRRules                         = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.ValidateEmploymentDaysAndLeaveOfAbsence(_popClaimContract.POPClaimEmployments, 1234567890));
            Assert.IsTrue(!_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPE9314));
        }

        [TestMethod]
        public void ValidateEmploymentDaysLessThan93_WhenThereAreMultipleEmployments_ReturnsFalse()
        {
            _popClaimRepository.IsPOPClaim                          = true;
            _employmentInformationRepository.addMultipleEmployments = true;
            _popClaimContract.POPClaimTypeCode                      = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId                        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate                  = new DateTime(2020, 08, 19);
            _popClaimRepository.IsCheckingJRRules                   = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(!_popClaimDomain.ValidateEmploymentDaysAndLeaveOfAbsence(_popClaimContract.POPClaimEmployments, 1234567890));
            Assert.IsTrue(_popClaimRepository.ExecFunctionHasBeenCalled);
            Assert.IsTrue(!_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPE9314));
        }

        [TestMethod]
        public void ValidateEmploymentDaysLessThan93_WhenThereAreMultipleEmploymentsGapBetweenThemisGreaterThan14_ReturnsFalse()
        {
            _popClaimRepository.IsPOPClaim                           = true;
            _employmentInformationRepository.addMultipleEmployments  = true;
            _popClaimContract.POPClaimTypeCode                       = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId                         = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate                   = new DateTime(2020, 08, 19);
            _popClaimRepository.IsGapBetweenEmploymentsGreaterThan14 = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimRepository.IsCheckingJRRules = true;

            Assert.IsTrue(!_popClaimDomain.ValidateEmploymentDaysAndLeaveOfAbsence(_popClaimContract.POPClaimEmployments, 1234567890));
            Assert.IsTrue(_popClaimRepository.ExecFunctionHasBeenCalled);
            Assert.IsTrue(!_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPE9314));
        }


        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsJobAttainmentInPast12Months()
        {
            _popClaimContract.POPClaimTypeCode     = POPClaimType.JobAttainmentCd;
            _popClaimContract.ClaimPeriodBeginDate = new DateTime(2020, 08, 19);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJA12));
        }

        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsJobAttainmentInPast12MonthsAndSubmittingANewJobAttainmentWithHighWage()
        {
            _popClaimContract.POPClaimTypeId       = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode     = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.ClaimPeriodBeginDate = new DateTime(2020, 08, 19);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJA12));
        }

        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsJobAttainmentWithHighWageInPast12MonthsAndSubmittingANewJobAttainment()
        {
            _popClaimContract.POPClaimTypeId        = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode      = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.ClaimPeriodBeginDate  = new DateTime(2020, 08, 19);
            _popClaimRepository.IsPreAddCheck       = true;
            _popClaimRepository.IsCheckingJAHWRules = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJA12));
        }

        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsJobAttainmentWithHighWageInPast12Months()
        {
            _popClaimContract.POPClaimTypeId        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode      = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.ClaimPeriodBeginDate  = new DateTime(2020, 08, 19);
            _popClaimRepository.IsPreAddCheck       = true;
            _popClaimRepository.IsCheckingJAHWRules = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJA12));
        }

        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsJobRetentionInPast12Months()
        {
            _popClaimRepository.IsCheckingJRRules  = true;
            _popClaimContract.POPClaimTypeCode     = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId       = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate = new DateTime(2020, 08, 19);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJA12));
        }

        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsJobRetentionInPast12MonthsInDB2()
        {
            _popClaimRepository.IsCheckingJRRules  = true;
            _popClaimRepository.ReturnValueofSP    = true;
            _popClaimContract.POPClaimTypeCode     = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId       = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate = new DateTime(2018, 08, 19);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJA12));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEANForJobRetentionErrorWhenThereIsNoEnrollmentBeforePrimaryJob()
        {
            _popClaimRepository.IsCheckingJRRules = true;
            _popClaimContract.POPClaimTypeCode    = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPAEN));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEANErrorWhenThereIsNoEnrollmentBeforePrimaryJob()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPAEN));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPUPPLErrorForJobRetentionWhenThereIsNoUpFrontActivityOrPlacementBeforePrimaryJob()
        {
            _popClaimRepository.IsCheckingJRRules = true;
            _popClaimContract.POPClaimTypeCode    = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId      = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckShouldNotReturnPOPUPPLErrorForJobRetention_WhenTodayIsBeforeSixMonthsFromCutOverDate()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckShouldReturnPOPUPPLErrorForJobRetention_WhenTodayIsAfterSixMonthsFromCutOverDate()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckShouldNotReturnPOPUPPLErrorForJobRetention_WhenTodayIsAfterSixMonthsFromCutOverDateAndUpFrontActivityExists()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2020";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _employabilityPlanRepository.IsPOPClaim = true;
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckShouldNotReturnPOPUPPLError_WhenTodayIsBeforeSixMonthsFromPhase2AndCutOverDateIsNull()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _pepRepository.IsPOPClaim      = true;
            _pepRepository.AddCutOverEntry = true;
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheck_ReturnsPOPUPPLErrorWhenThereIsNoUpFrontActivityOrPlacementBeforePrimaryJobAndFeatureToggleBeforeSixMonthsAndNoCutOverEntry()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _mockfeatureToggleRepository.IsFeatureToggleDateOlderThanSixMonthsFromToday = false;
            _pepRepository.AddCutOverEntry                                              = true;
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheck_ReturnsPOPUPPLErrorWhenThereIsNoUpFrontActivityOrPlacementBeforePrimaryAndWhenTodayIsAfterSixMonthsFromPhase2AndCutOverDateIsNull()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPUPPLErrorWhenThereIsNoUpFrontActivityOrPlacementBeforePrimaryJob()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPPEBDPOPCPBDErrorWhenPrimaryYearOrClaimPeriodYearIsNotCurrent()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            var errorCodes = new List<string> { RuleReason.POPPEBD, RuleReason.POPCPBD };

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Count(i => errorCodes.Contains(i)) == 2);
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPJB31ErrorWhenPrimaryJobDidNotLastFor31Days()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate   = "08/30/2019";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJB31));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPJB31ErrorWhenPrimaryJobDidNotLastFor31DaysWithCDODate()
        {
            _authUser.CDODate                        = new DateTime(2019, 08, 30);
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate   = null;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJB31));
        }

        [TestMethod]
        public void PreAddCheckNotReturnsPOPJB31ErrorWhenPrimaryJobDidLastFor31DaysWithCDODate()
        {
            _authUser.CDODate                        = new DateTime(2019, 09, 30);
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate   = null;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsFalse(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJB31));
        }


        [TestMethod]
        public void PreAddCheckReturnsPOPMHEErrorForJobRetentionWhenTotalHoursAndEarningsReachesMax()
        {
            _popClaimRepository.IsCheckingJRRules    = true;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.JRPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.JRPOPClaimType.Id;
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.HoursWorked  = 329;
            _popClaimEmploymentContract.Earnings     = 2609;
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPMHE));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPMHEErrorWhenTotalHoursAndEarningsReachesMax()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.HoursWorked  = 109;
            _popClaimEmploymentContract.Earnings     = 869;
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPMHE));
        }

        #region LongTerm JA related Tests

        [TestMethod]
        public void TestPreAddCheckConfiguresRulesEngineCorrectlyForLongTermParticipantJobAttainment()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimContract.POPClaimTypeId = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id;
            _popClaimDomain.PreAddCheck(_popClaimContract);

            var ruleSet = _ruleRepository.GetRuleSets().SelectMany(i => i.Rules).ToList();

            Assert.IsTrue(ruleSet.Select(i => i.Tags).All(i => i.Contains(RuleReason.LongTermParticipantJobAttainment)));
            Assert.AreEqual(8, ruleSet.Select(i => i.Tags).Count());
            Assert.IsTrue(_ruleRepository.HasCreateSessionBeenCalled);
            Assert.AreEqual(16, _ruleRepository.InsertedFacts.Count);
            _ruleRepository.InsertedFacts.ForEach(i => Assert.IsNotNull(i.Value));
            Assert.IsTrue(_ruleRepository.FiredRules.Count >= 0);
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPTLErrorForLongTermJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimRepository.IsCheckingLPJARules  = true;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.LPJAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPTL));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPMHEErrorWhenTotalHoursAndEarningsReachesMaxForLongTermJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.HoursWorked  = 109;
            _popClaimEmploymentContract.Earnings     = 869;
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPMHE));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPJB31ErrorWhenPrimaryJobDidNotLastFor31DaysForLongTermJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate   = "08/30/2019";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJB31));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPUPPLErrorWhenThereIsNoUpFrontActivityOrPlacementBeforePrimaryJobForLongTermJAPOPType()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPPEBDPOPCPBDErrorWhenPrimaryYearOrClaimPeriodYearIsNotCurrentForLongTermJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            var errorCodes = new List<string> { RuleReason.POPPEBD, RuleReason.POPCPBD };

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Count(i => errorCodes.Contains(i)) == 2);
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEANErrorWhenThereIsNoEnrollmentBeforePrimaryJobForLongTermJAPOPType()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPAEN));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEPSPEBDErrorForLongTermJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimRepository.IsCheckingLPJARules  = true;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.LPJAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPEPSPEBD));
        }

        [TestMethod]
        public void PreAddCheckReturnsErrorWhenThereIsLongTermJAEver()
        {
            _popClaimContract.POPClaimTypeId        = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode      = MockPOPClaimTypeRepository.LPJAPOPClaimType.Code;
            _popClaimRepository.IsCheckingLPJARules = true;
            _popClaimContract.ClaimPeriodBeginDate  = new DateTime(2020, 08, 19);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPLPJA));
        }

        #endregion

        #region High Wage JA related Tests

        [TestMethod]
        public void TestPreAddCheckConfiguresRulesEngineCorrectlyForHighWageJobAttainment()
        {
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimContract.POPClaimTypeId = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimDomain.PreAddCheck(_popClaimContract);

            var ruleSet = _ruleRepository.GetRuleSets().SelectMany(i => i.Rules).ToList();

            Assert.IsTrue(ruleSet.Select(i => i.Tags).All(i => i.Contains(RuleReason.JobAttainmentWithHighWage)));
            Assert.AreEqual(7, ruleSet.Select(i => i.Tags).Count());
            Assert.IsTrue(_ruleRepository.HasCreateSessionBeenCalled);
            Assert.AreEqual(16, _ruleRepository.InsertedFacts.Count);
            _ruleRepository.InsertedFacts.ForEach(i => Assert.IsNotNull(i.Value));
            Assert.IsTrue(_ruleRepository.FiredRules.Count >= 0);
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPPEBDPOPCPBDErrorWhenPrimaryYearOrClaimPeriodYearIsNotCurrentForHighWageJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            var errorCodes = new List<string> { RuleReason.POPPEBD, RuleReason.POPCPBD };

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Count(i => errorCodes.Contains(i)) == 2);
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEANErrorWhenThereIsNoEnrollmentBeforePrimaryJobForHighWageJAPOPType()
        {
            _popClaimContract.POPClaimTypeId   = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPAEN));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPUPPLErrorWhenThereIsNoUpFrontActivityOrPlacementBeforePrimaryJobForHighWageJAPOPType()
        {
            _popClaimContract.POPClaimTypeId   = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPUPPL));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPJB31ErrorWhenPrimaryJobDidNotLastFor31DaysForHighWageJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate   = "08/30/2019";
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPJB31));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPPEWErrorWhenPrimaryJobDidWageRateDidNotMeetTheMinimumForHighWageJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate       = "08/30/2019";
            _popClaimEmploymentContract.StartingWage     = 15.0M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimTypeCode           = "HWJA";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPPEW));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPPEWNoErrorWhenPrimaryJobDidWageRateDidNotMeetTheMinimumForHighWageJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2019";
            _popClaimEmploymentContract.JobEndDate       = "08/30/2019";
            _popClaimEmploymentContract.StartingWage     = 20.0M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(!_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPPEW));
        }

        [TestMethod]
        public void UpsertPOPClaimWithJAPOPType_whichAlsoSatisfiesJAHWValidations_ShouldOnlyCreateJAHWPOPClaim()
        {
            _popClaimEmploymentContract.StartingWage     = 20.0M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _employabilityPlanRepository.IsPOPClaim      = true;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2021, 02, 20);
            _popClaimEmploymentContract.JobBeginDate     = "01/10/2021";
            _authUser.CDODate                            = new DateTime(2021, 02, 10);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);

            Assert.AreEqual(MockPOPClaimTypeRepository.JAHWPOPClaimType.Id, _popClaimRepository.AddedPOPClaimsLists.First().POPClaimTypeId);
        }

        [TestMethod]
        public void UpsertPOPClaimWithJAPOPType_whichdoesNotSatisfiesJAHWValidations_ShouldOnlyCreateJAPOPClaim()
        {
            _popClaimEmploymentContract.StartingWage     = 15.0M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _employabilityPlanRepository.IsPOPClaim      = true;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2021, 02, 20);
            _popClaimEmploymentContract.JobBeginDate     = "01/10/2021";
            _authUser.CDODate                            = new DateTime(2021, 02, 10);
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);

            Assert.AreEqual(MockPOPClaimTypeRepository.JAPOPClaimType.Id, _popClaimRepository.AddedPOPClaimsLists.First().POPClaimTypeId);
        }

        #endregion

        [TestMethod]
        public void PreAddCheckPassesWhenAllConditionsMet()
        {
            _popClaimContract.ClaimStatusTypeCode      = POPClaimType.JobAttainmentCd;
            _popClaimContract.ClaimPeriodBeginDate     = DateTime.Today;
            _participantPlacementRepository.IsPOPClaim = true;
            _popClaimEmploymentContract.JobBeginDate   = "08/25/2021";
            _popClaimEmploymentContract.JobEndDate     = "09/30/2021";
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).CanAddPOPClaim);
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEPSPEBDErrorForHighWageJAPOPType()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimRepository.IsCheckingJAHWRules  = true;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPEPSPEBD));
        }

        [TestMethod]
        public void PreAddCheckNotReturnsPOPEPSPEBDErrorForHighWageJAPOPType_WhenTheFeatureToggleDateisWIthInSixMonthsAndNotCutOverDate()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimRepository.IsCheckingJAHWRules  = true;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _mockfeatureToggleRepository.IsFeatureToggleDateOlderThanSixMonthsFromToday = false;
            _pepRepository.AddCutOverEntry                                              = false;

            Assert.IsFalse(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPEPSPEBD));
        }

        [TestMethod]
        public void PreAddCheckReturnsPOPEPSPEBDErrorForHighWageJAPOPType_WhenTheFeatureToggleDateisWIthInSixMonthsAndThereisCutOverDate()
        {
            _popClaimEmploymentContract.JobBeginDate = "08/21/2019";
            _popClaimEmploymentContract.IsSelected   = true;
            _popClaimRepository.IsCheckingJAHWRules  = true;
            _popClaimContract.POPClaimTypeCode       = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId         = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _mockfeatureToggleRepository.IsFeatureToggleDateOlderThanSixMonthsFromToday = false;
            _pepRepository.AddCutOverEntry                                              = true;

            Assert.IsTrue(_popClaimDomain.PreAddCheck(_popClaimContract).ErrorCodes.Contains(RuleReason.POPEPSPEBD));
        }

        [TestMethod]
        public void ValidateAutoCreationOfHighWagePOPClaimWhenAJobAttainmentPOPClaimIsBeingAdded_ShouldNotAddHighWagePOPClaim()
        {
            _employmentInformationRepository.IsPOPClaim  = true;
            _employabilityPlanRepository.IsPOPClaim      = true;
            _participantPlacementRepository.IsPOPClaim   = true;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2020, 08, 21);
            _popClaimEmploymentContract.StartingWage     = 16.00M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2020";

            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.ValidatePreCheckForJobAttainmentWithHighWagePOPClaim(_popClaimContract);

            Assert.IsFalse(_popClaimRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void UpsertPOPClaim_WithIsprocessedSettoTrue_shouldTurnisProcessToFalse()
        {
            _employmentInformationRepository.IsPOPClaim  = true;
            _employabilityPlanRepository.IsPOPClaim      = true;
            _participantPlacementRepository.IsPOPClaim   = true;
            _popClaimContract.Id                         = 1;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2020, 08, 21);
            _popClaimEmploymentContract.StartingWage     = 16.00M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2020";
            _popClaimRepository.ValueForIsProcessed      = true;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);
            Assert.IsFalse(_popClaimRepository.UpdatedPOPClaim.IsProcessed != null && (bool) _popClaimRepository.UpdatedPOPClaim.IsProcessed);
        }

        [TestMethod]
        public void UpsertPOPClaim_WithIsprocessedSettoNull_shouldStayNull()
        {
            _employmentInformationRepository.IsPOPClaim  = true;
            _employabilityPlanRepository.IsPOPClaim      = true;
            _participantPlacementRepository.IsPOPClaim   = true;
            _popClaimContract.Id                         = 1;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2020, 08, 21);
            _popClaimEmploymentContract.StartingWage     = 16.00M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2020";
            _popClaimRepository.ValueForIsProcessed      = null;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);
            Assert.IsNull(_popClaimRepository.UpdatedPOPClaim.IsProcessed);
        }

        [TestMethod]
        public void UpsertPOPClaim_WithIsprocessedSettoFalse_shouldStayFalse()
        {
            _employmentInformationRepository.IsPOPClaim  = true;
            _employabilityPlanRepository.IsPOPClaim      = true;
            _participantPlacementRepository.IsPOPClaim   = true;
            _popClaimContract.Id                         = 1;
            _popClaimContract.POPClaimTypeCode           = MockPOPClaimTypeRepository.JAPOPClaimType.Code;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2020, 08, 21);
            _popClaimEmploymentContract.StartingWage     = 16.00M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2020";
            _popClaimRepository.ValueForIsProcessed      = false;
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.UpsertPOPClaim(_popClaimContract);
            Assert.IsFalse(_popClaimRepository.UpdatedPOPClaim.IsProcessed != null && (bool) _popClaimRepository.UpdatedPOPClaim.IsProcessed);
        }

        [TestMethod]
        public void InsertSystemGeneratedPOPClaim_ForVocationalTraining_ReturnsTrueWhenConditionsNotMet()
        {
            var employabilityPlan = new EmployabilityPlan
                                    {
                                        ParticipantId = 1,
                                        Participant   = new Participant
                                                        {
                                                            Id        = 1,
                                                            PinNumber = 1234567890
                                                        },
                                        ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                     {
                                                                         EnrolledProgram = new Wwp.DataAccess.Models.EnrolledProgram
                                                                                           {
                                                                                               ProgramCode = "CF"
                                                                                           }
                                                                     },
                                        Organization = new Organization
                                                       {
                                                           EntsecAgencyCode = "FSC"
                                                       }
                                    };

            _employmentInformationRepository.IsPOPClaim  = true;
            _employabilityPlanRepository.IsPOPClaim      = true;
            _participantPlacementRepository.IsPOPClaim   = true;
            _popClaimContract.POPClaimTypeId             = MockPOPClaimTypeRepository.JAPOPClaimType.Id;
            _popClaimContract.ClaimPeriodBeginDate       = new DateTime(2020, 08, 21);
            _popClaimEmploymentContract.StartingWage     = 17.00M;
            _popClaimEmploymentContract.StartingWageUnit = "Hour";
            _popClaimEmploymentContract.JobBeginDate     = "08/21/2020";

            // Tests for Participant Not Enrolled in W2
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, null, null, null, 0, POPClaimType.VocationalTrainingCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, null, null, null, 0, POPClaimType.EducationalAttainmentCd));

            // Tests for Invalid or Null Activity Code
            employabilityPlan.ParticipantEnrolledProgram.EnrolledProgram.ProgramCode = "WW";
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, null,            null, null, 0, POPClaimType.VocationalTrainingCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.AA, null, null, 0, POPClaimType.VocationalTrainingCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, null,            null, null, 0, POPClaimType.EducationalAttainmentCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.AA, null, null, 0, POPClaimType.EducationalAttainmentCd));

            // Tests for Invalid or Null Activity Complete Reason
            _popClaimContract.POPClaimEmployments.Add(_popClaimEmploymentContract);
            _popClaimDomain.ValidatePreCheckForJobAttainmentWithHighWagePOPClaim(_popClaimContract);
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.GE, null,                       null, 0, POPClaimType.VocationalTrainingCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.JS, ActivityCompletionReason.T, null, 0, POPClaimType.VocationalTrainingCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.JS, null,                       null, 0, POPClaimType.EducationalAttainmentCd));
            Assert.IsTrue(_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.GE, ActivityCompletionReason.T, null, 0, POPClaimType.EducationalAttainmentCd));

            // Test to check if  GetPOPClaimStatues been called(this only gets called when all above conditions were met)
            employabilityPlan.ParticipantEnrolledProgram.EnrolledProgram.ProgramCode = "WW";
            _popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.JS, ActivityCompletionReason.V, null, 0, POPClaimType.VocationalTrainingCd);
            Assert.IsTrue(_popClaimRepository.HasAddBeenCalled);
        }

        [TestMethod]
        public void InsertSystemGeneratedPOPClaim_ReturnsFalse_WhenConditionsMet()
        {
            _popClaimRepository.ForSystemGeneratedClaim = true;
            var employabilityPlan                       = new EmployabilityPlan
                                                          {
                                                              ParticipantId = 1,
                                                              Participant   = new Participant
                                                                              {
                                                                                  Id        = 1,
                                                                                  PinNumber = 1234567890
                                                                              },
                                                              ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                           {
                                                                                               EnrolledProgram = new Wwp.DataAccess.Models.EnrolledProgram
                                                                                                                 {
                                                                                                                     ProgramCode = "ww"
                                                                                                                 }
                                                                                           },
                                                              Organization = new Organization
                                                                             {
                                                                                 EntsecAgencyCode = "FSC"
                                                                             }
                                                          };

            Assert.IsTrue(!_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.JS, ActivityCompletionReason.V, null, 0, POPClaimType.VocationalTrainingCd));
            Assert.IsTrue(!_popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, ActivityCode.GE, ActivityCompletionReason.V, null, 0, POPClaimType.EducationalAttainmentCd));
        }


        private static IEnumerable<ContractForCreateWorkerTaskItemTest[]> GetContractForCreateWorkerTaskItemTest()
        {
            return new[]
                   {
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.SubmitCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JobAttainmentInitiationTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JobAttainmentInitiationTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.DeniedCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JobAttainmentDeniedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JobAttainmentDeniedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.ReturnCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JobAttainmentReturnedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JobAttainmentReturnedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JRPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.SubmitCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JRInitiationTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JRInitiationTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JRPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.DeniedCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JRDeniedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JRDeniedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JRPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.ReturnCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JRReturnedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JRReturnedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.SubmitCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.LTJAInitiationTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.LTJAInitiationTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.DeniedCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.LTJADeniedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.LTJADeniedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.ReturnCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.LTJAReturnedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.LTJAReturnedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.SubmitCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JAHWInitiationTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JAHWInitiationTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.DeniedCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JAHWDeniedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JAHWDeniedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.ReturnCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.JAHWReturnedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.JAHWReturnedTask.Description
                       },

                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.EAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.SubmitCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.EAInitiationTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.EAInitiationTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.EAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.DeniedCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.EADeniedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.EADeniedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.EAPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.ReturnCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.EAReturnedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.EAReturnedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.VTPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.SubmitCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.VTInitiationTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.VTInitiationTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.VTPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.DeniedCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.VTDeniedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.VTDeniedTask.Description
                       },
                       new ContractForCreateWorkerTaskItemTest
                       {
                           POPClaimTypeId        = MockPOPClaimTypeRepository.VTPOPClaimType.Id,
                           ClaimStatusTypeCode   = POPClaimStatusType.ReturnCd,
                           WorkerTaskId          = MockWorkerTaskCategoryRepository.VTReturnedTask.Id,
                           WorkerTaskDescription = MockWorkerTaskCategoryRepository.VTReturnedTask.Description
                       },
                   }.Select(i => new[] { i });
        }

        #endregion

        #region Repositories

        private class MockPOPClaimRepository : MockRepositoryBase<POPClaim>, IPOPClaimRepository
        {
            #region Properties

            public          bool           ForSystemGeneratedClaim;
            public          bool           HasGetManyBeenCalled;
            public          bool           HasGetBeenCalled;
            public          bool           HasAddBeenCalled;
            public          bool           IsPreAddCheck;
            public          bool           HasGetAsQueryableBeenCalled;
            public          bool           IsCheckingJRRules;
            public          bool           IsCheckingLPJARules;
            public          bool           IsCheckingJAHWRules;
            public          bool           ReturnValueofSP;
            public          bool?          ValueForIsProcessed;
            public readonly List<POPClaim> AddedPOPClaimsLists = new List<POPClaim>();

            public POPClaim UpdatedPOPClaim { get; set; }

            #endregion

            #region Methods

            public new IEnumerable<POPClaim> GetMany(Expression<Func<POPClaim, bool>> clause)
            {
                HasGetManyBeenCalled = true;

                var popClaims = new List<POPClaim>();

                if (IsPreAddCheck)
                {
                    POPClaim popClaim;
                    if (IsCheckingJRRules)
                    {
                        popClaim = new POPClaim
                                   {
                                       ParticipantId        = 1,
                                       OrganizationId       = 1,
                                       ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                       POPClaimTypeId       = 2,
                                       POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                              {
                                                                  Code        = MockPOPClaimTypeRepository.JRPOPClaimType.Code,
                                                                  Id          = MockPOPClaimTypeRepository.JRPOPClaimType.Id,
                                                                  Description = MockPOPClaimTypeRepository.JRPOPClaimType.Description
                                                              },
                                       Participant = new Participant

                                                     {
                                                         Id         = 1,
                                                         FirstName  = "Mock",
                                                         LastName   = "Mock",
                                                         TimeLimits = new List<TimeLimit>
                                                                      {
                                                                          new TimeLimit
                                                                          {
                                                                              ParticipantId  = 1,
                                                                              EffectiveMonth = new DateTime(2020, 11, 20),
                                                                              StateTimelimit = false,
                                                                          }
                                                                      }
                                                     }
                                   };
                    }
                    else
                        if (IsCheckingLPJARules)
                        {
                            popClaim = new POPClaim
                                       {
                                           ParticipantId        = 1,
                                           OrganizationId       = 1,
                                           ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                           POPClaimTypeId       = 3,
                                           POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                                  {
                                                                      Code        = MockPOPClaimTypeRepository.LPJAPOPClaimType.Code,
                                                                      Id          = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id,
                                                                      Description = MockPOPClaimTypeRepository.LPJAPOPClaimType.Description
                                                                  },
                                           Participant = new Participant

                                                         {
                                                             Id         = 1,
                                                             FirstName  = "Mock",
                                                             LastName   = "Mock",
                                                             TimeLimits = new List<TimeLimit>
                                                                          {
                                                                              new TimeLimit
                                                                              {
                                                                                  ParticipantId  = 1,
                                                                                  EffectiveMonth = new DateTime(2020, 11, 20),
                                                                                  StateTimelimit = false,
                                                                              }
                                                                          }
                                                         }
                                       };
                        }
                        else
                            if (IsCheckingJAHWRules)
                            {
                                popClaim = new POPClaim
                                           {
                                               ParticipantId        = 1,
                                               OrganizationId       = 1,
                                               ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                               POPClaimTypeId       = 4,
                                               POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                                      {
                                                                          Code        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code,
                                                                          Id          = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id,
                                                                          Description = MockPOPClaimTypeRepository.JAHWPOPClaimType.Description
                                                                      },
                                               Participant = new Participant

                                                             {
                                                                 Id         = 1,
                                                                 FirstName  = "Mock",
                                                                 LastName   = "Mock",
                                                                 TimeLimits = new List<TimeLimit>
                                                                              {
                                                                                  new TimeLimit
                                                                                  {
                                                                                      ParticipantId  = 1,
                                                                                      EffectiveMonth = new DateTime(2020, 11, 20),
                                                                                      StateTimelimit = false,
                                                                                  }
                                                                              }
                                                             }
                                           };
                            }
                            else
                            {
                                popClaim = new POPClaim
                                           {
                                               ParticipantId        = 1,
                                               OrganizationId       = 1,
                                               ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                               POPClaimTypeId       = 1,
                                               POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                                      {
                                                                          Code        = MockPOPClaimTypeRepository.JAPOPClaimType.Code,
                                                                          Id          = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                                                                          Description = MockPOPClaimTypeRepository.JAPOPClaimType.Description
                                                                      },
                                               IsProcessed = ValueForIsProcessed,
                                               Participant = new Participant

                                                             {
                                                                 Id        = 1,
                                                                 FirstName = "Mock",
                                                                 LastName  = "Mock",
                                                             }
                                           };
                            }


                    popClaims.Add(popClaim);
                }

                return popClaims;
            }

            public new POPClaim Get(Expression<Func<POPClaim, bool>> clause)
            {
                HasGetBeenCalled = true;
                POPClaim popClaim;
                if (IsCheckingLPJARules)
                {
                    popClaim = new POPClaim
                               {
                                   ParticipantId        = 1,
                                   OrganizationId       = 1,
                                   ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                   POPClaimTypeId       = 3,
                                   POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                          {
                                                              Code        = MockPOPClaimTypeRepository.LPJAPOPClaimType.Code,
                                                              Id          = MockPOPClaimTypeRepository.LPJAPOPClaimType.Id,
                                                              Description = MockPOPClaimTypeRepository.LPJAPOPClaimType.Description
                                                          },
                                   Participant = new Participant

                                                 {
                                                     Id         = 1,
                                                     FirstName  = "Mock",
                                                     LastName   = "Mock",
                                                     TimeLimits = new List<TimeLimit>
                                                                  {
                                                                      new TimeLimit
                                                                      {
                                                                          ParticipantId  = 1,
                                                                          EffectiveMonth = new DateTime(2020, 11, 20),
                                                                          StateTimelimit = false,
                                                                      }
                                                                  }
                                                 }
                               };
                }
                else
                    if (IsCheckingJRRules)
                    {
                        popClaim = new POPClaim
                                   {
                                       ParticipantId        = 1,
                                       OrganizationId       = 1,
                                       ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                       POPClaimTypeId       = 3,
                                       POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                              {
                                                                  Code        = MockPOPClaimTypeRepository.JRPOPClaimType.Code,
                                                                  Id          = MockPOPClaimTypeRepository.JRPOPClaimType.Id,
                                                                  Description = MockPOPClaimTypeRepository.JRPOPClaimType.Description
                                                              },
                                       Participant = new Participant

                                                     {
                                                         Id         = 1,
                                                         FirstName  = "Mock",
                                                         LastName   = "Mock",
                                                         TimeLimits = new List<TimeLimit>
                                                                      {
                                                                          new TimeLimit
                                                                          {
                                                                              ParticipantId  = 1,
                                                                              EffectiveMonth = new DateTime(2020, 11, 20),
                                                                              StateTimelimit = false,
                                                                          }
                                                                      }
                                                     }
                                   };
                    }
                    else
                        if (IsCheckingJAHWRules)
                        {
                            popClaim = new POPClaim
                                       {
                                           ParticipantId        = 1,
                                           OrganizationId       = 1,
                                           ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                           POPClaimTypeId       = 3,
                                           POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                                  {
                                                                      Code        = MockPOPClaimTypeRepository.JAHWPOPClaimType.Code,
                                                                      Id          = MockPOPClaimTypeRepository.JAHWPOPClaimType.Id,
                                                                      Description = MockPOPClaimTypeRepository.JAHWPOPClaimType.Description
                                                                  },
                                           Participant = new Participant

                                                         {
                                                             Id         = 1,
                                                             FirstName  = "Mock",
                                                             LastName   = "Mock",
                                                             TimeLimits = new List<TimeLimit>
                                                                          {
                                                                              new TimeLimit
                                                                              {
                                                                                  ParticipantId  = 1,
                                                                                  EffectiveMonth = new DateTime(2020, 11, 20),
                                                                                  StateTimelimit = false,
                                                                              }
                                                                          }
                                                         }
                                       };
                        }
                        else
                        {
                            popClaim = new POPClaim
                                       {
                                           ParticipantId        = 1,
                                           OrganizationId       = 1,
                                           ClaimPeriodBeginDate = new DateTime(2019, 08, 20),
                                           POPClaimTypeId       = 3,
                                           IsProcessed          = ValueForIsProcessed,
                                           POPClaimType         = new Wwp.DataAccess.Models.POPClaimType
                                                                  {
                                                                      Code        = MockPOPClaimTypeRepository.JAPOPClaimType.Code,
                                                                      Id          = MockPOPClaimTypeRepository.JAPOPClaimType.Id,
                                                                      Description = MockPOPClaimTypeRepository.JAPOPClaimType.Description
                                                                  },
                                           Participant = new Participant

                                                         {
                                                             Id         = 1,
                                                             FirstName  = "Mock",
                                                             LastName   = "Mock",
                                                             TimeLimits = new List<TimeLimit>
                                                                          {
                                                                              new TimeLimit
                                                                              {
                                                                                  ParticipantId  = 1,
                                                                                  EffectiveMonth = new DateTime(2020, 11, 20),
                                                                                  StateTimelimit = false,
                                                                              }
                                                                          }
                                                         }
                                       };
                        }


                return popClaim;
            }

            public new void Add(POPClaim contract)
            {
                HasAddBeenCalled = true;
                AddedPOPClaimsLists.Add(contract);
            }

            public new void Update(POPClaim contract)
            {
                UpdatedPOPClaim = contract;
            }

            public new IQueryable<POPClaim> GetAsQueryable(bool withTracking = true)
            {
                HasGetAsQueryableBeenCalled = true;

                var popClaims = new List<POPClaim>();

                if (ForSystemGeneratedClaim)
                {
                    var popClaim = new POPClaim
                                   {
                                       ParticipantId = 0,
                                       POPClaimType  = new Wwp.DataAccess.Models.POPClaimType
                                                       {
                                                           Code = POPClaimType.EducationalAttainmentCd
                                                       },
                                       POPClaimStatuses = new List<POPClaimStatus>
                                                          {
                                                              new POPClaimStatus
                                                              {
                                                                  Id                 = 1,
                                                                  POPClaimStatusType = new Wwp.DataAccess.Models.POPClaimStatusType
                                                                                       {
                                                                                           Code = POPClaimStatusType.ApproveCd
                                                                                       },
                                                              }
                                                          },
                                       Organization = new Organization
                                                      {
                                                          EntsecAgencyCode = "FSC"
                                                      },
                                       ClaimPeriodBeginDate = DateTime.Today,
                                       ModifiedDate         = DateTime.Today,
                                       IsDeleted            = false
                                   };

                    popClaim.POPClaimStatuses.ForEach(i => i.POPClaim = popClaim);
                    popClaims.Add(popClaim);
                }

                return popClaims.AsQueryable();
            }

            public new IList<TC> ExecStoredProc<TC>(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp")
            {
                return new List<TC> { (TC) Convert.ChangeType(new POPClaimDomain.CheckDB2ClaimTypExists { PinNumber = "12345678", HasClaimTypExists = ReturnValueofSP }, typeof(POPClaimDomain.CheckDB2ClaimTypExists)) };
            }

            #endregion
        }

        private class MockPOPClaimStatusTypeRepository : MockRepositoryBase<Wwp.DataAccess.Models.POPClaimStatusType>, IPOPClaimStatusTypeRepository
        {
        }

        private class MockPOPClaimStatusRepository : MockRepositoryBase<POPClaimStatus>, IPOPClaimStatusRepository
        {
            public bool HasAddBeenCalled;

            public new void Add(POPClaimStatus contract)
            {
                HasAddBeenCalled = true;
            }
        }

        private class MockPOPClaimTypeRepository : MockRepositoryBase<Wwp.DataAccess.Models.POPClaimType>, IPOPClaimTypeRepository
        {
            #region Properties

            public static readonly Wwp.DataAccess.Models.POPClaimType LPJAPOPClaimType = new Wwp.DataAccess.Models.POPClaimType
                                                                                         {
                                                                                             Code        = POPClaimType.LongTermCd,
                                                                                             Id          = 3,
                                                                                             Description = "Long-Term Participant Job Attainment"
                                                                                         };

            public static readonly Wwp.DataAccess.Models.POPClaimType JAPOPClaimType = new Wwp.DataAccess.Models.POPClaimType
                                                                                       {
                                                                                           Code        = POPClaimType.JobAttainmentCd,
                                                                                           Id          = 1,
                                                                                           Description = "Job Attainment"
                                                                                       };

            public static readonly Wwp.DataAccess.Models.POPClaimType JRPOPClaimType = new Wwp.DataAccess.Models.POPClaimType
                                                                                       {
                                                                                           Code        = POPClaimType.JobRetentionCd,
                                                                                           Id          = 2,
                                                                                           Description = "Job Retention"
                                                                                       };

            public static readonly Wwp.DataAccess.Models.POPClaimType JAHWPOPClaimType = new Wwp.DataAccess.Models.POPClaimType
                                                                                         {
                                                                                             Code        = POPClaimType.JobAttainmentWithHighWageCd,
                                                                                             Id          = 4,
                                                                                             Description = "High Wage Job Attainment",
                                                                                         };

            public static readonly Wwp.DataAccess.Models.POPClaimType EAPOPClaimType = new Wwp.DataAccess.Models.POPClaimType
                                                                                       {
                                                                                           Code        = POPClaimType.EducationalAttainmentCd,
                                                                                           Id          = 5,
                                                                                           Description = "Educational Attainment"
                                                                                       };

            public static readonly Wwp.DataAccess.Models.POPClaimType VTPOPClaimType = new Wwp.DataAccess.Models.POPClaimType
                                                                                       {
                                                                                           Code        = POPClaimType.VocationalTrainingCd,
                                                                                           Id          = 6,
                                                                                           Description = "Vocational Training"
                                                                                       };

            #endregion

            #region Methods

            public new Wwp.DataAccess.Models.POPClaimType Get(Expression<Func<Wwp.DataAccess.Models.POPClaimType, bool>> clause)
            {
                var popClaimCategoryClaimType = new List<Wwp.DataAccess.Models.POPClaimType> { LPJAPOPClaimType, JAPOPClaimType, JRPOPClaimType, JAHWPOPClaimType, EAPOPClaimType, VTPOPClaimType };

                return popClaimCategoryClaimType.FirstOrDefault(clause.Compile());
            }

            #endregion
        }


        private class MockPOPClaimEmploymentBridgeRepository : MockRepositoryBase<POPClaimEmploymentBridge>, IPOPClaimEmploymentBridgeRepository
        {
            public bool HasAddBeenCalled;
            public bool HasDeleteRangeBeenCalled;

            public new IEnumerable<POPClaimEmploymentBridge> GetMany(Expression<Func<POPClaimEmploymentBridge, bool>> clause)
            {
                return new List<POPClaimEmploymentBridge>();
            }

            public new POPClaimEmploymentBridge Get(Expression<Func<POPClaimEmploymentBridge, bool>> clause)
            {
                return new POPClaimEmploymentBridge();
            }

            public new void Add(POPClaimEmploymentBridge contract)
            {
                HasAddBeenCalled = true;
            }

            public new void Update(POPClaimEmploymentBridge contract)
            {
            }

            public new void DeleteRange(IEnumerable<POPClaimEmploymentBridge> entities)
            {
                HasDeleteRangeBeenCalled = true;
            }
        }

        #endregion
    }

    public class MockPOPClaimActivityBridgeRepository : MockRepositoryBase<POPClaimActivityBridge>, IPOPClaimActivityBridgeRepository
    {
        public bool HasAddBeenCalled;

        public new void Add(POPClaimActivityBridge contract)
        {
            HasAddBeenCalled = true;
        }
    }

    public class MockPOPClaimStatusTypeRepository : MockRepositoryBase<Wwp.DataAccess.Models.POPClaimStatusType>, IPOPClaimStatusTypeRepository
    {
        public bool HasAddBeenCalled;

        public new Wwp.DataAccess.Models.POPClaimStatusType Get(Expression<Func<Wwp.DataAccess.Models.POPClaimStatusType, bool>> clause)
        {
            return new Wwp.DataAccess.Models.POPClaimStatusType
                   {
                       Id = 1
                   };
        }
    }

    public  class MockPOPClaimHighWageRepository : MockRepositoryBase<POPClaimHighWage>, IPOPClaimHighWageRepository
    {
        #region Properties

        public bool getHasBeenCalled;

        #endregion

        #region Methods

        public new POPClaimHighWage Get(Expression<Func<POPClaimHighWage, bool>> clause)
        {
            getHasBeenCalled     = true;
            var popClaimHighWage = new POPClaimHighWage()
                                   {
                                       OrganizationId = 1,
                                       StartingWage   = 16.40M,
                                   };
            return popClaimHighWage;
        }

        #endregion
    }

    public class MockWorkerRepository : MockRepositoryBase<Worker>, IWorkerRepository
    {
        public new Worker Get(Expression<Func<Worker, bool>> clause)
        {
            var worker = new Worker();

            return worker;
        }
    }
}
