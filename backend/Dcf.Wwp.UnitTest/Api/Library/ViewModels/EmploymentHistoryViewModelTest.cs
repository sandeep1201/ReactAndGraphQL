using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.ViewModels.WorkHistoryApp;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Api.Library.Domains.Mock;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.ViewModels
{
    [TestClass]
    public class EmploymentHistoryViewModelTest
    {
        private          MockRepository             _mockRepository;
        private readonly IAuthUser                  _authUser = new AuthUser();
        private          MockTransactionDomain      _mockTransactionDomain;
        private          EmploymentHistoryViewModel _employmentHistoryViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository             = new MockRepository();
            _mockTransactionDomain      = new MockTransactionDomain();
            _employmentHistoryViewModel = new EmploymentHistoryViewModel(null, _mockRepository, _authUser, null, _mockTransactionDomain) { Participant = new MockParticipantForPhase1 { EmployabilityPlans = new List<IEmployabilityPlan>() } } ;
        }

        [TestMethod]
        public void GetParticipantEmployments_WhenNoEmploymentVerifications_ReturnsIsVerifiedFalseForAllEmployments()
        {
            var employmentInformation = new EmploymentInformation
                                        {
                                            EmploymentVerifications = new List<EmploymentVerification>()
                                        };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation };
            var employments                                                   = _employmentHistoryViewModel.GetParticipantEmployments();
            Assert.IsFalse(employments.First().IsVerified);
        }

        [TestMethod]
        public void GetParticipantEmployments_WithVerifiedEmployments_ReturnsIsVerifiedTrueForThatEmployments()
        {
            var employmentInformation = new EmploymentInformation
                                        {
                                            EmploymentVerifications = new List<EmploymentVerification> { new EmploymentVerification { IsVerified = true } }
                                        };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation };
            var employments                                                   = _employmentHistoryViewModel.GetParticipantEmployments();
            Assert.IsTrue(employments.First().IsVerified);
        }

        [TestMethod]
        public void GetParticipantEmployments_WithUnVerifiedEmployments_ReturnsIsVerifiedFalseForThatEmployments()
        {
            var employmentInformation = new EmploymentInformation
                                        {
                                            EmploymentVerifications = new List<EmploymentVerification> { new EmploymentVerification { IsVerified = false } }
                                        };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation };
            var employments                                                   = _employmentHistoryViewModel.GetParticipantEmployments();
            Assert.IsFalse(employments.First().IsVerified);
        }

        [TestMethod]
        public void GetParticipantEmployments_WithValidEmployments_ReturnsDifferentIsVerifiedForDifferentEmployments()
        {
            var employmentInformation1 = new EmploymentInformation
                                         {
                                             Id                      = 1,
                                             EmploymentVerifications = new List<EmploymentVerification> { new EmploymentVerification { IsVerified = false } }
                                         };
            var employmentInformation2 = new EmploymentInformation
                                         {
                                             Id                      = 2,
                                             EmploymentVerifications = new List<EmploymentVerification> { new EmploymentVerification { IsVerified = true } }
                                         };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation1, employmentInformation2 };
            var employments                                                   = _employmentHistoryViewModel.GetParticipantEmployments().ToList();
            Assert.IsFalse(employments.First(x => x.Id == employmentInformation1.Id).IsVerified);
            Assert.IsTrue(employments.First(x => x.Id  == employmentInformation2.Id).IsVerified);
        }

        [TestMethod]
        public void GetParticipantEmploymentInfo_WhenNoEmploymentVerification_ReturnsIsVerifiedFalse()
        {
            var employmentInformation = new EmploymentInformation
                                        {
                                            EmploymentVerifications = new List<EmploymentVerification>()
                                        };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation };
            var employment                                                    = _employmentHistoryViewModel.GetParticipantEmploymentInfo(0);
            Assert.IsFalse(employment.IsVerified);
        }

        [TestMethod]
        public void GetParticipantEmploymentInfo_WithVerifiedEmployment_ReturnsIsVerifiedTrue()
        {
            var employmentInformation = new EmploymentInformation
                                        {
                                            EmploymentVerifications = new List<EmploymentVerification> { new EmploymentVerification { IsVerified = true } }
                                        };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation };
            var employment                                                    = _employmentHistoryViewModel.GetParticipantEmploymentInfo(0);
            Assert.IsTrue(employment.IsVerified);
        }

        [TestMethod]
        public void GetParticipantEmploymentInfo_WithUnVerifiedEmployment_ReturnsIsVerifiedFalse()
        {
            var employmentInformation = new EmploymentInformation
                                        {
                                            EmploymentVerifications = new List<EmploymentVerification> { new EmploymentVerification { IsVerified = false } }
                                        };
            _employmentHistoryViewModel.Participant.AllEmploymentInformations = new List<IEmploymentInformation> { employmentInformation };
            var employment                                                    = _employmentHistoryViewModel.GetParticipantEmploymentInfo(0);
            Assert.IsFalse(employment.IsVerified);
        }
    }
}
