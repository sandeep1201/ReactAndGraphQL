using System.Collections.Generic;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractArea = Dcf.Wwp.Data.Sql.Model.ContractArea;
using EnrolledProgram = Dcf.Wwp.Data.Sql.Model.EnrolledProgram;
using WorkerTaskStatus = Dcf.Wwp.Data.Sql.Model.WorkerTaskStatus;

namespace Dcf.Wwp.UnitTest.Api.Library.ViewModels
{
    [TestClass]
    public class ReassignFepViewModelTest
    {
        #region Properties

        private MockRepository       _mockRepository;
        private ReassignFepViewModel _reassignFepViewModel;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new MockRepository();

            _reassignFepViewModel = new ReassignFepViewModel(_mockRepository);
        }


        [TestMethod]
        public void EnrollParticipant_WithInvalidFeatureDate_DoNotInsertWorkerTask()
        {
            _mockRepository.MockParticipantEnrolledProgram.EnrolledProgram.ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode;
            List<string> arrlist = new List<string>();
            arrlist.Add("1234567890");
            var mainFrameId = "TEMP12";

            _reassignFepViewModel.UpdatePins(mainFrameId, arrlist);
            Assert.IsNotNull(_mockRepository.WorkerTaskList);
            Assert.AreEqual(_mockRepository.WorkerTaskList.TaskDetails, WorkerTaskCategoryCodes.PinReassignCode);
        }

        #endregion

        private class MockRepository : MockPhase1Repository
        {
            public  IWorkerTaskList WorkerTaskList;
            private string          FeatureToggleValue = "2020-01-01";

            public readonly ParticipantEnrolledProgram MockParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                        {
                                                                                            EnrolledProgramId           = Model.Interface.Constants.EnrolledProgram.WW,
                                                                                            EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId,
                                                                                            WorkerId                    = 2,
                                                                                            Participant = new Participant
                                                                                                          {
                                                                                                              PinNumber                                = 1234567890,
                                                                                                              ParticipantEnrolledProgramCutOverBridges = new List<ParticipantEnrolledProgramCutOverBridge>(),
                                                                                                              WorkerTaskLists                          = new List<WorkerTaskList>()
                                                                                                          },
                                                                                            Office = new Office
                                                                                                     {
                                                                                                         ContractArea =     new ContractArea
                                                                                                                            {
                                                                                                                                OrganizationId = 1,
                                                                                                                            }
                                                                                                     },
                                                                                            EnrolledProgram = new EnrolledProgram
                                                                                                              {
                                                                                                                  ProgramCode = null,
                                                                                                                  ProgramType = "Eligibility",
                                                                                                              },
                                                                                        };

            public readonly WorkerTaskStatus WorkerTaskStatus = new WorkerTaskStatus
                                                                {
                                                                    Id   = 2,
                                                                    Code = Model.Interface.Constants.WorkerTaskStatus.Open
                                                                };


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

            public override IParticipant GetParticipant(string pin)
            {
                return new Participant
                       {
                           PinNumber = 1234567890
                       };
            }

            public override IParticipantEnrolledProgram GetPepById(int id)
            {
                return MockParticipantEnrolledProgram;
            }

            public override IWorker WorkerByMainframeId(string mfUserId)
            {
                return new Worker
                       {
                           Roles          = "FEP",
                           OrganizationId = 1,
                           Id             = 1,
                       };
            }

            public override IEnumerable<IParticipantEnrolledProgram> GetPepRecordsForPin(decimal pin)
            {
                return new List<ParticipantEnrolledProgram>
                       {
                           MockParticipantEnrolledProgram
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

            public override void NewWorkerTask(IWorkerTaskList workerTaskList)
            {
                WorkerTaskList = workerTaskList;
            }
        }
    }
}
