using System.Collections.Generic;
using Dcf.Wwp.Api.Controllers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Controller
{
    [TestClass]
    public class PlanControllerTest
    {
        #region Properties

        private MockPlanDomain _planDomain;
        private PlanController _planController;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _planDomain     = new MockPlanDomain();
            _planController = new PlanController(_planDomain);
        }

        [TestMethod]
        public void GetW2PlansByParticipantId_ValidParticipantId_CallsDomainGetW2PlansByParticipantId()
        {
            _planController.GetW2PlansByParticipantId(123);
            Assert.IsTrue(_planDomain.HasGetW2PlansByParticipantIdBeenCalled);
        }

        [TestMethod]
        public void GetW2PlansByParticipantId_ValidParticipantId_ReturnsIActionResult()
        {
            Assert.IsNotNull(_planController.GetW2PlansByParticipantId(123));
        }

        #endregion

        #region MockDomains

        private class MockPlanDomain : IPlanDomain
        {
            public bool HasGetW2PlansByParticipantIdBeenCalled;

            public List<PlanContract> GetW2PlansByParticipantId(int id)
            {
                HasGetW2PlansByParticipantIdBeenCalled = true;
                return null;
            }
        }

        #endregion
    }
}
