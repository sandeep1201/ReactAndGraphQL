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
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
{
    [TestClass]
    public class PreCheckEnrollment : BaseUnitTest
    {
        [TestMethod]
        public void US1429_TMJ_Refered()
        {
            // Arrange.
            // Participant is trying to enroll into W-2.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();
            var mockRepo = NewMock<IRepository>();
            var w2Pep = new ParticipantEnrolledProgram
            {
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.WWP
            };

            var ep = new EnrolledProgram { ProgramCode = "WW" };
            w2Pep.EnrolledProgram = ep;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(w2Pep);

            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "Enrollment" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            // Participant has one TMJ Referred program.
            var part = new Participant();
            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId
            };
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockConf = NewMock<IConfidentialityChecker>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockConf.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreEnrollParticipant(1);

            // Assert.
            // We will append an error to the return contract stating "Individual is reffered for TMJ. TMJ. and W-2 cannot be co-enrolled."
            var index = contract.Errors.IndexOf("Individual is referred for TMJ. TMJ and W-2 cannot be co-enrolled.");
            Assert.IsTrue(index > -1);
            Assert.IsTrue(contract.Errors.Count == 1);
        }

        [TestMethod]
        public void US1429_TJ_Refered()
        {
            // Arrange.
            // Participant is trying to enroll into W-2.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();
            var mockRepo = NewMock<IRepository>();
            var w2Pep = new ParticipantEnrolledProgram
            {
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var ep = new EnrolledProgram { ProgramCode = "WW" };
            w2Pep.EnrolledProgram = ep;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(w2Pep);

            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "Enrollment" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            // Participant with one TJ Referred program.
            var part = new Participant();
            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransitionalJobsId,
                EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId
            };
            peps.Add(tmjPep);
            part.ParticipantEnrolledPrograms = peps;

            mockRepo.Setup(x => x.GetParticipant("123")).Returns(part);

            var mockAuthUser = NewMock<IAuthUser>();
            var mockTimeLimitService = NewMock<TimelimitService>();
            var mockDb2TimelimitService = NewMock<Db2TimelimitService>();

            // Act.
            var vm = new ParticipantViewModel("123", mockRepo.Object, mockAuthUser.Object, mockTimeLimitService.Object, mockDb2TimelimitService.Object, mockCountyAndTribe.Object);
            var contract = vm.PreEnrollParticipant(1);

            // Assert.
            // We will append an error to the return contract stating "Individual is reffered for TMJ. TMJ. and W-2 cannot be co-enrolled."
            var index = contract.Errors.IndexOf("Individual is referred for TMJ. TMJ and W-2 cannot be co-enrolled.");
            var index2 = contract.Errors.IndexOf("Individual is referred for TJ. TJ and W-2 cannot be co-enrolled.");
            Assert.IsTrue(index == -1);
            Assert.IsTrue(index2 > -1);
            Assert.IsTrue(contract.Errors.Count == 1);
        }

        [TestMethod]
        public void US1429_TMJ_Enrolled()
        {
            // Arrange.
            // Participant is trying to enroll into W-2.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();
            var mockRepo = NewMock<IRepository>();
            var pep = new ParticipantEnrolledProgram
            {
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var ep = new EnrolledProgram { ProgramCode = "WW" };
            pep.EnrolledProgram = ep;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);

            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "Enrollment" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            // Participant with one TMJ Enrolled program.
            var part = new Participant();
            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId,
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
            var contract = vm.PreEnrollParticipant(1);

            // Assert.
            // We will append an error to the return contract stating "Individual is enrolled for TMJ. TMJ. and W-2 cannot be co-enrolled."
            var index = contract.Errors.IndexOf("Individual is enrolled for TMJ. TMJ and W-2 cannot be co-enrolled.");
            Assert.IsTrue(index > -1);
        }

        [TestMethod]
        public void US1429_TJ_Enrolled()
        {
            // Arrange.
            var mockCountyAndTribe = NewMock<IReadOnlyCollection<ICountyAndTribe>>();
            // Participant is trying to enroll into W-2.
            var mockRepo = NewMock<IRepository>();
            var pep = new ParticipantEnrolledProgram
            {
                EnrolledProgramId = Model.Interface.Constants.EnrolledProgram.W2
            };

            var ep = new EnrolledProgram { ProgramCode = "WW" };
            pep.EnrolledProgram = ep;

            mockRepo.Setup(x => x.GetParticantEnrollment(1)).Returns(pep);

            mockRepo.Setup(x => x.GetRuleReasonsWhere(i => i.Category == "Enrollment" && i.SubCategory == "PreCheck")).Returns(CreateRulesList());

            // Participant with one TJ enrolled program.
            var part = new Participant();
            var peps = new List<ParticipantEnrolledProgram>();
            var tmjPep = new ParticipantEnrolledProgram
            {
                Id = 1,
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
            var contract = vm.PreEnrollParticipant(1);

            // Assert.
            // We will append an error to the return contract stating "Individual is enrolled for TMJ. TMJ. and W-2 cannot be co-enrolled."
            var index = contract.Errors.IndexOf("Individual is enrolled for TJ. TJ and W-2 cannot be co-enrolled.");
            Assert.IsTrue(index > -1);
            Assert.IsTrue(contract.Errors.Count == 1);
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
