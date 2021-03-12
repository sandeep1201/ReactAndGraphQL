using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Api.Library.Domains.Mock;
using Dcf.Wwp.UnitTest.ConnectedServices;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnrolledProgram = Dcf.Wwp.Data.Sql.Model.EnrolledProgram;
using WorkerTaskStatus = Dcf.Wwp.Data.Sql.Model.WorkerTaskStatus;

namespace Dcf.Wwp.UnitTest.Api.Library.ViewModels
{
    [TestClass]
    public class ParticipantViewModelTest
    {
        #region Properties

        private          MockRepository        _mockRepository;
        private readonly IAuthUser             _authUser = new AuthUser();
        private          MockTransactionDomain _mockTransactionDomain;
        private          ParticipantViewModel  _participantViewModel;
        private          MockCwwIndService     _cwwIndSvc;

        private static readonly EnrolledProgramContract MockEnrolledProgramContract = new EnrolledProgramContract
                                                                                      {
                                                                                          Id = 1,
                                                                                          AssignedWorker = new WorkerContract
                                                                                                           {
                                                                                                               Id = 100
                                                                                                           }
                                                                                      };

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository        = new MockRepository();
            _mockTransactionDomain = new MockTransactionDomain();
            _cwwIndSvc             = new MockCwwIndService();

            _participantViewModel = new ParticipantViewModel(_mockRepository, _authUser, null, null, null, null, _cwwIndSvc, null, null, null, _mockTransactionDomain);
        }

        [TestMethod]
        public void EnrollParticipant_WithInvalidFeatureDate_DoNotInsertWorkerTask()
        {
            _mockRepository.FeatureToggleValue                                         = "2050-01-01";
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.IsNull(_mockRepository.WorkerTaskList);
        }

        [TestMethod]
        public void EnrollParticipant_WithValidFeatureDateButNotEnrolledInCForFCDP_DoNotInsertWorkerTask()
        {
            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.IsNull(_mockRepository.WorkerTaskList);
        }

        [TestMethod]
        public void EnrollParticipant_EnrolledInChildrenFirst_InsertsNewWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.IsNotNull(_mockRepository.WorkerTaskList);
        }

        [TestMethod]
        public void EnrollParticipant_EnrolledInFCDP_InsertsNewWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.FCDPProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.IsNotNull(_mockRepository.WorkerTaskList);
        }

        [TestMethod]
        public void EnrollParticipant_EnrolledInFCDP_GetsFCDPWorkerTaskCategory()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.FCDPProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.AreEqual(WorkerTaskCategoryCodes.FCDPEnrolledCode, _mockRepository.WorkerTaskList.TaskDetails);
        }

        [TestMethod]
        public void EnrollParticipant_EnrolledInW2_GetsW2WorkerTaskCategory()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.AreEqual(WorkerTaskCategoryCodes.PinReassignCode, _mockRepository.WorkerTaskList.TaskDetails);
        }

        [TestMethod]
        public void EnrollParticipant_EnrolledInW2_GetsLFWorkerTaskCategory()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.LFProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.AreEqual(WorkerTaskCategoryCodes.PinReassignCode, _mockRepository.WorkerTaskList.TaskDetails);
        }

        [TestMethod]
        public void EnrollParticipant_EnrolledInCForFCDP_ReturnsValidWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode;

            _participantViewModel.EnrollParticipant(MockEnrolledProgramContract);
            Assert.AreEqual(50,                                                           _mockRepository.WorkerTaskList.CategoryId);
            Assert.AreEqual(WorkerTaskCategoryCodes.ChildrenFirstEnrolledCode,            _mockRepository.WorkerTaskList.TaskDetails);
            Assert.AreEqual(_mockRepository.WorkerTaskStatus.Id,                          _mockRepository.WorkerTaskList.WorkerTaskStatusId);
            Assert.AreEqual(0,                                                            _mockRepository.WorkerTaskList.WorkerId);
            Assert.AreEqual(_mockRepository.MockParticipantEnrolledProgram.ParticipantId, _mockRepository.WorkerTaskList.ParticipantId);
            Assert.AreEqual(true,                                                         _mockRepository.WorkerTaskList.IsSystemGenerated);
        }

        [TestMethod]
        public void CreateAndAddNewWorkerTask_WithValidContract_ShouldCreateNewWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode;
            var newWorker    = _mockRepository.WorkerById(MockEnrolledProgramContract.AssignedWorker.Id.GetValueOrDefault());
            var pep          = _mockRepository.GetPepById(MockEnrolledProgramContract.Id.GetValueOrDefault());
            var modifiedDate = DateTime.Now;
            _participantViewModel.CreateAndAddNewWorkerTask(newWorker, pep, modifiedDate);
            Assert.AreEqual(WorkerTaskCategoryCodes.PinReassignCode, _mockRepository.WorkerTaskList.TaskDetails);
        }

        [TestMethod]
        public void ReassignParticipantToWorker_WithValidW2Contract_ShouldCreateNewWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode;
            _participantViewModel.ReassignParticipantToWorker(MockEnrolledProgramContract);
            Assert.AreEqual(WorkerTaskCategoryCodes.PinReassignCode, _mockRepository.WorkerTaskList.TaskDetails);
        }

        [TestMethod]
        public void ReassignParticipantToWorker_WithValidLFContract_ShouldCreateNewWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.LFProgramCode;
            _participantViewModel.ReassignParticipantToWorker(MockEnrolledProgramContract);
            Assert.AreEqual(WorkerTaskCategoryCodes.PinReassignCode, _mockRepository.WorkerTaskList.TaskDetails);
        }

        [TestMethod]
        public void ReassignParticipantToWorker_WithCWWThrowingError_ShouldThrowAnError()
        {
            _cwwIndSvc.invokeException                                                 = true;
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode;
            Assert.ThrowsException<Exception>(() => _participantViewModel.ReassignParticipantToWorker(MockEnrolledProgramContract));
        }

        #endregion
    }

    public class MockRepository : MockPhase1Repository
    {
        public IWorkerTaskList WorkerTaskList;

        public readonly ParticipantEnrolledProgram MockParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                    {
                                                                                        Participant = new Participant
                                                                                                      {
                                                                                                          PinNumber                                = 1234567890,
                                                                                                          ParticipantEnrolledProgramCutOverBridges = new List<ParticipantEnrolledProgramCutOverBridge>()
                                                                                                      },
                                                                                        Office = new Office(),
                                                                                        EnrolledProgram = new EnrolledProgram
                                                                                                          {
                                                                                                              ProgramCode = null
                                                                                                          }
                                                                                    };

        public readonly WorkerTaskStatus WorkerTaskStatus = new WorkerTaskStatus
                                                            {
                                                                Id   = 2,
                                                                Code = Model.Interface.Constants.WorkerTaskStatus.Open
                                                            };

        public string FeatureToggleValue = "2020-01-01";

        public override void NewWorkerTask(IWorkerTaskList workerTaskList)
        {
            WorkerTaskList = workerTaskList;
        }

        public override IParticipantEnrolledProgram GetPepById(int id)
        {
            return MockParticipantEnrolledProgram;
        }

        public override IEnumerable<ISpecialInitiative> GetFeatureValue(string featureName)
        {
            return new List<SpecialInitiative>
                   {
                       new SpecialInitiative
                       {
                           ParameterValue = FeatureToggleValue
                       }
                   };
        }

        public override IWorkerTaskCategory GetWorkerTaskCategory(string code)
        {
            return new WorkerTaskCategory
                   {
                       Id          = 50,
                       Description = code
                   };
        }

        public override IWorkerTaskStatus GetWorkerTaskStatus(string code)
        {
            return WorkerTaskStatus;
        }

        public override IWorker WorkerByWIUID(string wiuid)
        {
            return new Worker();
        }

        public override IWorker WorkerById(int id)
        {
            return new Worker();
        }

        public override ISP_MostRecentFEPFromDB2_Result GetMostRecentFepDetails(string pin)
        {
            return new SP_MostRecentFEPFromDB2_Result
                   {
                       MostRecentMFFepId = "TEMP12",
                       Id                = 1
                   };
        }

        public override void ReassignW2CaseManagerInDB2(decimal? pinNumber, string FepId)
        {
        }
    }
}
