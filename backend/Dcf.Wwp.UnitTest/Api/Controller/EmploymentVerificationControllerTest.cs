using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Controllers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Controller
{
    [TestClass]
    public class EmploymentVerificationControllerTest
    {
        #region Properties

        private MockEmploymentVerificationDomain _employmentVerificationDomain;
        private EmploymentVerificationController _employmentVerificationController;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _employmentVerificationDomain = new MockEmploymentVerificationDomain();
            _employmentVerificationController = new EmploymentVerificationController(_employmentVerificationDomain);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_ValidParticipantIdAndJobType_ReturnsIActionResult()
        {
            Assert.IsNotNull(_employmentVerificationController.GetTJTMJEmploymentsForParticipantByJobType(123, 1, DateTime.Now.AddDays(-100)));
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_ValidParticipantIdAndJobType_CallsGetTJTMJEmploymentsForParticipantByJobType()
        {
            _employmentVerificationController.GetTJTMJEmploymentsForParticipantByJobType(123, 1, DateTime.Now.AddDays(-100));
            Assert.IsTrue(_employmentVerificationDomain.HasGetTjtmjEmploymentsBeenCalled);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_ValidEmploymentVerificationContract_ReturnsIActionResult()
        {
            Assert.IsNotNull(_employmentVerificationController.PostEmploymentVerification("123", new List<EmploymentVerificationContract>()));
        }

        [TestMethod]
        public void UpsertEmploymentVerification_ValidEmploymentVerificationContract_CallsGetTJTMJEmploymentsForParticipantByJobType()
        {
            _employmentVerificationController.PostEmploymentVerification("123", new List<EmploymentVerificationContract>());
            Assert.IsTrue(_employmentVerificationDomain.HasPostTjtmjEmploymentsBeenCalled);
        }

        #endregion

        private class MockEmploymentVerificationDomain : IEmploymentVerificationDomain
        {
            public bool HasGetTjtmjEmploymentsBeenCalled;
            public bool HasPostTjtmjEmploymentsBeenCalled;

            public List<EmploymentVerificationContract> GetTJTMJEmploymentsForParticipantByJobType(int participantId, int jobTypeId, DateTime enrollmentDate)
            {
                HasGetTjtmjEmploymentsBeenCalled = true;
                return new List<EmploymentVerificationContract>();
            }

            public void UpsertEmploymentVerification(string pin, List<EmploymentVerificationContract> contract)
            {
                HasPostTjtmjEmploymentsBeenCalled = true;
            }
        }
    }
}
