using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using DCF.Timelimts.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.ObjectModel;

namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
{
    [TestClass]
    public class PreCheckDisEnrollment: BaseUnitTest
    {
     

        //[TestInitialize]
        //public static void InitializeTests()
        //{
        //    //... code that runs before each test
        //}

        [TestMethod]
        public void US1421_TMJ_TJ_CF_Disenrollment_Worker_End_Activities_Message()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };

            var pep = new ParticipantEnrolledProgram
            {
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.ActivityOpen = true;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);
            mockRepo.Setup(x => x.PreDisenrollmentErrors(pep.Participant.PinNumber, pep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            // Participant with one TJ enrolled program.
            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransitionalJobsId,
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Open activities/components exist for the participant. These must be ended prior to disenrollment."
            var index = contract.Errors.IndexOf("Open activities/components exist for the participant. These must be ended prior to disenrollment.");
            Assert.IsTrue(index > -1);
            Assert.IsTrue(contract.Errors.Count == 1);
        }

        [TestMethod]
        public void US1421_TMJ_TJ_CF_Disenrollment_Worker_End_Transactions_Message()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };


            var mockRepo = NewMock<IRepository>();
            var pep = new ParticipantEnrolledProgram
            {
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.TransactionExist = true;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);
            mockRepo.Setup(x => x.PreDisenrollmentErrors(pep.Participant.PinNumber, pep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransitionalJobsId,
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Transactions exists. These must be closed prior to disenrollment.
            var index = contract.Errors.IndexOf("Transactions exists. These must be closed prior to disenrollment.");
            Assert.IsTrue(index > -1);
            Assert.IsTrue(contract.Errors.Count == 1);
        }

        [TestMethod]
        public void US1421_TMJ_TJ_CF_Disenrollment_Worker_End_Placements_Message()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };

            var pep = new ParticipantEnrolledProgram
            {
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = true;
            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);
              mockRepo.Setup(x => x.PreDisenrollmentErrors(pep.Participant.PinNumber, pep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransitionalJobsId,
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Transactions exists. These must be closed prior to disenrollment.
            var index = contract.Errors.IndexOf("Open placements exist. These must be ended prior to disenrollment.");
            Assert.IsTrue(index > -1);
            Assert.IsTrue(contract.Errors.Count == 1);
        }

        [TestMethod]
        public void US1421_TMJ_TJ_CF_Disenrollment_Worker_End_Placements_Activities_Transactions_Message()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };

            var pep = new ParticipantEnrolledProgram
            {
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = true;
            precheck.TransactionExist = true;
            precheck.ActivityOpen = true;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);
            mockRepo.Setup(x => x.PreDisenrollmentErrors(pep.Participant.PinNumber, pep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransitionalJobsId,
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Transactions exists. These must be closed prior to disenrollment.
            var index = contract.Errors.IndexOf("Transactions exists. These must be closed prior to disenrollment.");
            Assert.IsTrue(index > -1);

            var index2 = contract.Errors.IndexOf("Open placements exist. These must be ended prior to disenrollment.");
            Assert.IsTrue(index2 > -1);

            var index3 = contract.Errors.IndexOf("Open activities/components exist for the participant. These must be ended prior to disenrollment.");
            Assert.IsTrue(index3 > -1);

            Assert.IsTrue(contract.Errors.Count == 3);
        }

        [TestMethod]
        public void PreDisenrollParticipant_TMJandCFEnrolledWithOpenBarrier_WarningForOpenBarriers()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
            {
                Id = 123,
                PinNumber = 123
            };

            var tmjPep = new ParticipantEnrolledProgram
                         {
                             Id                = 1,
                             Participant       = part,
                             EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
                             EnrolledProgram   = new EnrolledProgram()
                                                 {
                                                     ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode
                                                 },
                             EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
                         };

            var cfPep = new ParticipantEnrolledProgram
                        {
                            EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.ChildrenFirstId,
                            EnrolledProgram   = new EnrolledProgram()
                                                {
                                                    ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode
                                                },
                            EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId

                        };


            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = false;
            precheck.TransactionExist = false;
            precheck.ActivityOpen = false;

            var barrierDetails = new List<BarrierDetail>();
            var barrierDetail = new BarrierDetail();
            barrierDetail.IsDeleted = false;
            barrierDetail.EndDate = null;
            barrierDetails.Add(barrierDetail);

            mockRepo.Setup(x => x.BarrierDetailsByParticipantId(123)).Returns(barrierDetails);
            mockRepo.Setup(x => x.GetParticantEnrollment(tmjPep.Id)).Returns(tmjPep);
              mockRepo.Setup(x => x.PreDisenrollmentErrors(tmjPep.Participant.PinNumber, tmjPep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            var peps = new List<ParticipantEnrolledProgram>();
      
            peps.Add(tmjPep);
            peps.Add(cfPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Transactions exists. These must be closed prior to disenrollment.
            var index = contract.Errors.IndexOf("Transactions exists. These must be closed prior to disenrollment.");
            Assert.IsTrue(index == -1);


            Assert.IsTrue(contract.Errors.Count == 0);
            Assert.IsTrue(contract.Warnings.Count == 1);
        }

        [TestMethod]
        public void PreDisenrollParticipant_TMJandCFEnrolledWithClosedBarrier_NoWarningForClosedBarriers()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };

            var tmjPep = new ParticipantEnrolledProgram
                         {
                             Id                = 1,
                             Participant       = part,
                             EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
                             EnrolledProgram   = new EnrolledProgram()
                                                 {
                                                     ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode
                                                 },
                             EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
                         };

            var cfPep = new ParticipantEnrolledProgram
                        {
                            EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.ChildrenFirstId,
                            EnrolledProgram   = new EnrolledProgram()
                                                {
                                                    ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode
                                                },
                            EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId

                        };

            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = false;
            precheck.TransactionExist = false;
            precheck.ActivityOpen = false;

            var barrierDetails = new List<BarrierDetail>();
            var barrierDetail = new BarrierDetail();
            barrierDetail.EndDate = DateTime.Now;
            barrierDetails.Add(barrierDetail);

            mockRepo.Setup(x => x.BarrierDetailsByParticipantId(123)).Returns(barrierDetails);
            mockRepo.Setup(x => x.GetParticantEnrollment(tmjPep.Id)).Returns(tmjPep);
            mockRepo.Setup(x => x.PreDisenrollmentErrors(tmjPep.Participant.PinNumber, tmjPep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            var peps = new List<ParticipantEnrolledProgram>();
      
            peps.Add(tmjPep);
            peps.Add(cfPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // There shound 0 errors and warnings.
            Assert.IsTrue(contract.Errors.Count == 0);
            Assert.IsTrue(contract.Warnings.Count == 0);
        }

        [TestMethod]
        public void PreDisenrollParticipant_TMJandCFEnrolledWithOpenAccommodation_WarningForOpenAccommodation()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
            {
                Id = 123,
                PinNumber = 123
            };

            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                Participant = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
                EnrolledProgram = new EnrolledProgram()
                {
                    ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode
                },
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };

            var cfPep = new ParticipantEnrolledProgram
            {
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.ChildrenFirstId,
                EnrolledProgram = new EnrolledProgram()
                {
                    ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode
                },
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId

            };


            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = false;
            precheck.TransactionExist = false;
            precheck.ActivityOpen = false;

            var barrierDetails = new List<BarrierDetail>();
            var barrierDetail = new BarrierDetail();
            barrierDetail.IsAccommodationNeeded = true;
            var accommodation = new BarrierAccommodation();
            barrierDetail.IsDeleted = false;
            barrierDetail.EndDate = null;
  
            accommodation.Id = 1;
            barrierDetail.BarrierAccommodations.Add(accommodation);
            barrierDetails.Add(barrierDetail);

            mockRepo.Setup(x => x.BarrierDetailsByParticipantId(123)).Returns(barrierDetails);
            mockRepo.Setup(x => x.GetParticantEnrollment(tmjPep.Id)).Returns(tmjPep);
              mockRepo.Setup(x => x.PreDisenrollmentErrors(tmjPep.Participant.PinNumber, tmjPep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

  
        
            var peps = new List<ParticipantEnrolledProgram>();

            peps.Add(tmjPep);
            peps.Add(cfPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Transactions exists. These must be closed prior to disenrollment.
            var index = contract.Warnings.IndexOf("Review the participant's open accommodations, disenrollment will not auto end these");
            var index2 = contract.Warnings.IndexOf("Review the participant's open barriers, disenrollment will not auto close these barriers");
            Assert.IsTrue(index != -1);
            Assert.IsTrue(index2 != -1);

            Assert.IsTrue(contract.Errors.Count == 0);
            Assert.IsTrue(contract.Warnings.Count == 2);
        }

        [TestMethod]
        public void PreDisenrollParticipant_TMJandCFEnrolledWithClosedAccommodation_NoWarningForClosedAccommodation()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };


            var tmjPep = new ParticipantEnrolledProgram
            {
                Id                = 1,
                Participant       = part,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
                EnrolledProgram   = new EnrolledProgram()
                                    {
                                        ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode
                                    },
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };

            var cfPep = new ParticipantEnrolledProgram
            {
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.ChildrenFirstId,
                EnrolledProgram = new EnrolledProgram()
                {
                    ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode
                },
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId

            };


            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = false;
            precheck.TransactionExist = false;
            precheck.ActivityOpen = false;

            var barrierDetails = new List<BarrierDetail>();
            var barrierDetail = new BarrierDetail();
            barrierDetail.IsAccommodationNeeded = true;
            var accommodation = new BarrierAccommodation();
            barrierDetail.IsDeleted = false;
            barrierDetail.EndDate = null;

            accommodation.Id = 1;
            accommodation.EndDate =  DateTime.Now;
            barrierDetail.BarrierAccommodations.Add(accommodation);
            barrierDetails.Add(barrierDetail);

            mockRepo.Setup(x => x.BarrierDetailsByParticipantId(123)).Returns(barrierDetails);
            mockRepo.Setup(x => x.GetParticantEnrollment(tmjPep.Id)).Returns(tmjPep);
            mockRepo.Setup(x => x.PreDisenrollmentErrors(tmjPep.Participant.PinNumber, tmjPep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            var peps = new List<ParticipantEnrolledProgram>();

            peps.Add(tmjPep);
            peps.Add(cfPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            var index2 = contract.Warnings.IndexOf("Review the participant's open barriers, disenrollment will not auto close these barriers");
            Assert.IsTrue(index2 != -1);

            Assert.IsTrue(contract.Errors.Count == 0);
            Assert.IsTrue(contract.Warnings.Count == 1);
        }

        [TestMethod]
        public void US1469()
        {
            // Arrange.
            // Participant is diserolling tmj.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();

            var mockRepo = NewMock<IRepository>();

            var part = new Participant
                       {
                           Id        = 123,
                           PinNumber = 123
                       };


            var tmjPep = new ParticipantEnrolledProgram
                       {
                           Id = 1,
                           Participant = part,
                           EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
                           EnrolledProgram = new EnrolledProgram()
                                             {
                                                 ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode
                                             },
                           EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
            };

            var cfPep = new ParticipantEnrolledProgram
                       {
                           EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.ChildrenFirstId,
                           EnrolledProgram = new EnrolledProgram()
                                                       {
                                                            ProgramCode = Model.Interface.Constants.EnrolledProgram.CFProgramCode
                           },
                           EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId

            };

            var precheck = new SP_PreCheckDisenrollment_Result();
            precheck.PlacementOpen = false;
            precheck.TransactionExist = false;
            precheck.ActivityOpen = true;

            //var barrierDetails = new List<BarrierDetail>();
            //var barrierDetail = new BarrierDetail();
            //barrierDetails.Add(barrierDetail);

            //mockRepo.Setup(x => x.BarrierDetailsByParticipantId(123)).Returns(barrierDetails);
           // mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);
              mockRepo.Setup(x => x.PreDisenrollmentErrors(tmjPep.Participant.PinNumber, tmjPep.CASENumber)).Returns(precheck);
            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "DisEnroll" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

 
            var peps = new List<ParticipantEnrolledProgram>();

            peps.Add(cfPep);
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreDisenrollParticipant(1);

            // Assert.
            // Transactions exists. These must be closed prior to disenrollment.
            var index = contract.Errors.IndexOf("There are open activities for this participant. Close program specific activities prior to disenrollment.");
            Assert.IsTrue(index == -1);

            //var index2 = contract.Errors.IndexOf("Open placements exist. These must be ended prior to disenrollment.");
            //Assert.IsTrue(index2 == -1);

            //var index3 = contract.Errors.IndexOf("Open activities/components exist for the participant. These must be ended prior to disenrollment.");
            //Assert.IsTrue(index3 == -1);

            Assert.IsTrue(contract.Errors.Count == 0);
            Assert.IsTrue(contract.Warnings.Count == 1);
        }

        private List<RuleReason> CreateRulesList()
        {
            var rr = new RuleReason
                     {
                         Code = "TMJEW2",
                         Name = "Individual is enrolled for TMJ. TMJ and W-2 cannot be co-enrolled."
            };

            var rr1 = new RuleReason
                      {
                          Code = "TMJRW2",
                          Name = "Individual is referred for TMJ. TMJ and W-2 cannot be co-enrolled."
                      };

            var rr2 = new RuleReason
                      {
                          Code = "TJRW2",
                          Name = "Individual is referred for TJ. TJ and W-2 cannot be co-enrolled."
                      };

            var rr22 = new RuleReason
                      {
                          Code = "TJEW2",
                          Name = "Individual is enrolled for TJ. TJ and W-2 cannot be co-enrolled."
            };


            var rr3 = new RuleReason
                      {
                          Code = "DPCA",
                          Name = "Review the participant's open accommodations, disenrollment will not auto end these"
            };

            var rr4 = new RuleReason
                      {
                          Code = "DPCB",
                          Name = "Review the participant's open barriers, disenrollment will not auto close these barriers"
            };



            var rr5 = new RuleReason
                      {
                          Code = "DPCEA",
                          Name = "Open activities/components exist for the participant. These must be ended prior to disenrollment."
            };
            var rr6 = new RuleReason
                      {
                          Code = "DPCEP",
                          Name = "Open placements exist. These must be ended prior to disenrollment."
            };

            var rr7 = new RuleReason
                      {
                          Code = "DPCET",
                          Name = "Transactions exists. These must be closed prior to disenrollment."
            };

            var rr8 = new RuleReason
                      {
                          Code = "DPCWB",
                          Name = "There are one or more open barriers for this participant.  They will be ended upon disenrollment."
            };


            var rr9 = new RuleReason
                      {
                          Code = "DPAO",
                          Name = "There are open activities for this participant.Close program specific activities prior to disenrollment."
            };


         

            var rules = new List<RuleReason>
            {
                rr,
                rr1,
                rr2,
                rr3,
                rr4,
                rr5,
                rr6,
                rr7,
                rr8,
                rr22,
                rr9
            };

            return rules;
        }
    }
}

