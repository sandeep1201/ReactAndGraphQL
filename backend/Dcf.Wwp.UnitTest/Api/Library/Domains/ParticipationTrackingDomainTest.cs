using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Api.Library.Domains.Mock;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class ParticipationTrackingDomainTest
    {
        private readonly Participant _participant = new Participant
                                                    {
                                                        Id = 12345,
                                                        ParticipantEnrolledPrograms = new List<ParticipantEnrolledProgram>
                                                                                      {
                                                                                          new ParticipantEnrolledProgram
                                                                                          {
                                                                                              EnrolledProgram = new Wwp.DataAccess.Models.EnrolledProgram
                                                                                                                {
                                                                                                                    ProgramCode = "WW"
                                                                                                                },
                                                                                              EnrolledProgramStatusCodeId = 2,
                                                                                              WorkerId                    = 1
                                                                                          }
                                                                                      }
                                                    };

        private readonly UpdatePlacement _updatePlacementContract = new UpdatePlacement
                                                                    {
                                                                        PlacementStartDate = DateTime.Today.AddDays(-5)
                                                                    };

        private MockTransactionDomain                   _mockTransactionDomain;
        private ParticipationTrackingDomain             _participationTrackingDomain;
        private PrivateObject                           _privateParticipationTrackingDomain;
        private MockWorkerTaskCategoryRepository        _workerTaskCategoryRepository;
        private MockWorkerTaskStatusRepository          _workerTaskStatusRepository;
        private MockWorkerTaskListRepository            _workerTaskListRepository;
        private MockParticipantPaymentHistoryRepository _participantPaymentHistoryRepository;
        private object[]                                _args;
        private MockDatabaseConfiguration               _dbConfig;
        private IAuthUser                               _authUser;

        private const    int      WorkerId      = 123;
        private const    string   ModifiedBy    = "CWW";
        private readonly DateTime _modifiedDate = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionDomain               = new MockTransactionDomain();
            _workerTaskCategoryRepository        = new MockWorkerTaskCategoryRepository();
            _workerTaskStatusRepository          = new MockWorkerTaskStatusRepository();
            _workerTaskListRepository            = new MockWorkerTaskListRepository();
            _participantPaymentHistoryRepository = new MockParticipantPaymentHistoryRepository();
            _dbConfig                            = new MockDatabaseConfiguration();
            _authUser                            = new AuthUser { WIUID = "1111" };
            _args                                = new object[] { new ParticipantPaymentHistory(), 0.0m, 0.0m, _authUser.WIUID, DateTime.Today };
            _participationTrackingDomain = new ParticipationTrackingDomain(null, null, null, null, null, null, null, null, _workerTaskListRepository, _workerTaskCategoryRepository, _participantPaymentHistoryRepository, null, null, null,
                                                                           _workerTaskStatusRepository, null, null, null, null, null, null, _dbConfig, null, _authUser, null, _mockTransactionDomain);
            _privateParticipationTrackingDomain = new PrivateObject(_participationTrackingDomain);

            _participant.InformalAssessments = new List<InformalAssessment>
                                               {
                                                   new InformalAssessment
                                                   {
                                                       EndDate = DateTime.Today.AddDays(-15)
                                                   }
                                               };
        }

        [TestMethod]
        public void InsertTransaction_DoesNotSetOfficeIdAndNotCallInsertTransaction_WhenNoPEP()
        {
            _participant.ParticipantEnrolledPrograms = null;

            Assert.AreEqual(default(int), _participationTrackingDomain.InsertTransaction(_participant, null, _updatePlacementContract, null, DateTime.Now).OfficeId);
            Assert.AreEqual(0,            _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void InsertTransaction_MapsAllValuesCorrectly()
        {
            var transactionContract = _participationTrackingDomain.InsertTransaction(_participant, WorkerId, _updatePlacementContract, ModifiedBy, _modifiedDate);

            Assert.AreEqual(_participant.Id,                             transactionContract.ParticipantId);
            Assert.AreEqual(WorkerId,                                    transactionContract.WorkerId);
            Assert.AreEqual(_updatePlacementContract.PlacementStartDate, transactionContract.EffectiveDate);
            Assert.AreEqual(_modifiedDate,                               transactionContract.CreatedDate);
            Assert.AreEqual(TransactionTypes.Placement,                  transactionContract.TransactionTypeCode);
            Assert.AreEqual(ModifiedBy,                                  transactionContract.ModifiedBy);
        }

        [TestMethod]
        public void InsertTransaction_MapsEffectiveDateAsPlacementEndDate_WhenEndDateIsNotNull()
        {
            _updatePlacementContract.PlacementEndDate = DateTime.Today;

            var transactionContract = _participationTrackingDomain.InsertTransaction(_participant, WorkerId, _updatePlacementContract, ModifiedBy, _modifiedDate);

            Assert.AreEqual(_updatePlacementContract.PlacementEndDate, transactionContract.EffectiveDate);
        }

        [TestMethod]
        public void InsertTransaction_MapsAllValuesCorrectly_CallsInsertTransaction_WithPEP()
        {
            var pep1 = new ParticipantEnrolledProgram
                       {
                           ReferralDate = new DateTime(2020, 05, 01),
                           OfficeId     = 1
                       };
            var pep2 = new ParticipantEnrolledProgram
                       {
                           ReferralDate = new DateTime(2020, 01, 01),
                           OfficeId     = 5
                       };
            _participant.ParticipantEnrolledPrograms = new List<ParticipantEnrolledProgram> { pep1, pep2 };

            var transactionContract = _participationTrackingDomain.InsertTransaction(_participant, WorkerId, _updatePlacementContract, ModifiedBy, _modifiedDate);

            Assert.AreEqual(pep1.OfficeId, transactionContract.OfficeId);
            Assert.AreEqual(1,             _mockTransactionDomain.InsertCount);
        }

        [TestMethod]
        public void InsertWorkerTask_WhenPEPAndIAAreNull_SkipsInsert()
        {
            _participant.ParticipantEnrolledPrograms = null;
            _participant.InformalAssessments         = null;
            _participationTrackingDomain.InsertWorkerTask(_participant, _updatePlacementContract, ModifiedBy, _modifiedDate, WorkerTaskCategoryCodes.PlacementChangeIACode);

            Assert.AreEqual(0, _workerTaskListRepository.AddedWorkerTaskLists.Count);
        }

        [TestMethod]
        public void InsertWorkerTask_WhenIAIsNull_SkipsInsert()
        {
            _participant.InformalAssessments = null;
            _participationTrackingDomain.InsertWorkerTask(_participant, _updatePlacementContract, ModifiedBy, _modifiedDate, WorkerTaskCategoryCodes.PlacementChangeIACode);

            Assert.AreEqual(0, _workerTaskListRepository.AddedWorkerTaskLists.Count);
        }

        [TestMethod]
        public void InsertWorkerTask_WhenPEPIsNullAndNoInformalAssessmentWithin10Days_SkipsInsert()
        {
            _participant.ParticipantEnrolledPrograms = null;
            _participant.InformalAssessments.Add(new InformalAssessment
                                                 {
                                                     EndDate = DateTime.Today.AddDays(-16)
                                                 });
            _participationTrackingDomain.InsertWorkerTask(_participant, _updatePlacementContract, ModifiedBy, _modifiedDate, WorkerTaskCategoryCodes.PlacementChangeIACode);

            Assert.AreEqual(0, _workerTaskListRepository.AddedWorkerTaskLists.Count);
        }

        [TestMethod]
        public void InsertWorkerTask_WhenInformalAssessmentIsWithin10Days_SkipsInsert()
        {
            _participant.InformalAssessments.Add(new InformalAssessment
                                                 {
                                                     EndDate = DateTime.Today.AddDays(-14)
                                                 });
            _participant.InformalAssessments.Add(new InformalAssessment
                                                 {
                                                     EndDate = null
                                                 });
            _participationTrackingDomain.InsertWorkerTask(_participant, _updatePlacementContract, ModifiedBy, _modifiedDate, WorkerTaskCategoryCodes.PlacementChangeIACode);

            Assert.AreEqual(0, _workerTaskListRepository.AddedWorkerTaskLists.Count);
        }

        [TestMethod]
        public void InsertWorkerTask_WhenNoInformalAssessmentWithin10Days_CallsWorkerTaskCategoryGet()
        {
            PrepareParticipantContractAndCallInsertWorkerTask();

            Assert.IsTrue(_workerTaskCategoryRepository.HasGetBeenCalled);
        }

        [TestMethod]
        public void InsertWorkerTask_WhenNoInformalAssessmentWithin10Days_InsertsTask()
        {
            PrepareParticipantContractAndCallInsertWorkerTask();

            Assert.AreEqual(1, _workerTaskListRepository.AddedWorkerTaskLists.Count);
        }

        [TestMethod]
        public void AddParticipantPaymentHistory_CallsParticipantPaymentHistoryRepositoryNewAndAdd()
        {
            _privateParticipationTrackingDomain.Invoke("AddParticipantPaymentHistory", _args);

            Assert.IsTrue(_participantPaymentHistoryRepository.NewHasBeenCalled);
            Assert.IsTrue(_participantPaymentHistoryRepository.AddHasBeenCalled);
            Assert.AreEqual(1, _participantPaymentHistoryRepository.AddCount);
        }

        [TestMethod]
        public void AddParticipantPaymentHistory_WhenDelayedPaymentHasDrugFelonPenalty_CalculatesNewDrugFelonPenalty()
        {
            const decimal newBasePayment = 618.0m;

            _args[1] = newBasePayment;
            _args[2] = 0.10m;

            _privateParticipationTrackingDomain.Invoke("AddParticipantPaymentHistory", _args);

            Assert.AreEqual(Math.Floor(decimal.Multiply(newBasePayment, 0.10m)), _participantPaymentHistoryRepository.NewValue.DrugFelonPenalty);
        }

        [TestMethod]
        public void AddParticipantPaymentHistory_WhenDelayedPaymentHasNoDrugFelonPenalty_DoesNotCalculatesDrugFelonPenalty()
        {
            _privateParticipationTrackingDomain.Invoke("AddParticipantPaymentHistory", _args);

            Assert.AreEqual(_args[0].GetType().GetProperty("DrugFelonPenalty")?.GetValue(_args[0]), _participantPaymentHistoryRepository.NewValue.DrugFelonPenalty);
        }

        [TestMethod]
        public void AddParticipantPaymentHistory_MapsValuesFromArgsToPaymentHistory()
        {
            _args[0].GetType().GetProperty("EffectiveMonth")?.SetValue(_args[0], 202011, null);

            _privateParticipationTrackingDomain.Invoke("AddParticipantPaymentHistory", _args);

            var participantPayment = _participantPaymentHistoryRepository.NewValue.BaseW2Payment             - _participantPaymentHistoryRepository.NewValue.DrugFelonPenalty -
                                     _participantPaymentHistoryRepository.NewValue.Recoupment                - _participantPaymentHistoryRepository.NewValue.LearnFarePenalty -
                                     _participantPaymentHistoryRepository.NewValue.NonParticipationReduction - _participantPaymentHistoryRepository.NewValue.VendorPayment;

            Assert.AreEqual(_args[0].GetType().GetProperty("CaseNumber")?.GetValue(_args[0]),             _participantPaymentHistoryRepository.NewValue.CaseNumber);
            Assert.AreEqual(_args[0].GetType().GetProperty("EffectiveMonth")?.GetValue(_args[0]),         _participantPaymentHistoryRepository.NewValue.EffectiveMonth);
            Assert.AreEqual(_args[0].GetType().GetProperty("ParticipationBeginDate")?.GetValue(_args[0]), _participantPaymentHistoryRepository.NewValue.ParticipationBeginDate);
            Assert.AreEqual(_args[0].GetType().GetProperty("ParticipationEndDate")?.GetValue(_args[0]),   _participantPaymentHistoryRepository.NewValue.ParticipationEndDate);
            Assert.AreEqual(_args[1],                                                                     _participantPaymentHistoryRepository.NewValue.BaseW2Payment);
            Assert.AreEqual(participantPayment,                                                           _participantPaymentHistoryRepository.NewValue.ParticipantPayment);
            Assert.AreEqual(_args[3],                                                                     _participantPaymentHistoryRepository.NewValue.ModifiedBy);
            Assert.AreEqual(_args[4],                                                                     _participantPaymentHistoryRepository.NewValue.ModifiedDate);
        }

        private void PrepareParticipantContractAndCallInsertWorkerTask()
        {
            _participant.InformalAssessments.Add(new InformalAssessment
                                                 {
                                                     EndDate = DateTime.Today.AddDays(-16)
                                                 });
            _participationTrackingDomain.InsertWorkerTask(_participant, _updatePlacementContract, ModifiedBy, _modifiedDate, WorkerTaskCategoryCodes.PlacementChangeIACode);
        }
    }

    public class MockParticipantPaymentHistoryRepository : MockRepositoryBase<ParticipantPaymentHistory>, IParticipantPaymentHistoryRepository
    {
    }
}
