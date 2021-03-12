using Dcf.Wwp.Api.Controllers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.Api.Middleware;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Controller
{
    [TestClass]
    public class ParticipantActivityControllerTest
    {
        #region Properties

        private MockApiUser                   _apiUser;
        private MockParticipantActivityDomain _participantActivityDomain;
        private ParticipantActivityController _participantActivityController;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _apiUser                       = new MockApiUser { IsAuthenticated = true };
            _participantActivityDomain     = new MockParticipantActivityDomain();
            _participantActivityController = new ParticipantActivityController(_apiUser, _participantActivityDomain);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPins_CallsDomainGetParticipantActivitiesByPins()
        {
            _participantActivityController.GetParticipantActivitiesByPins("1234567890");
            Assert.IsTrue(_participantActivityDomain.HasGetParticipantActivitiesByPinsBeenCalled);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPins_ReturnsIActionResult()
        {
            Assert.IsNotNull(_participantActivityController.GetParticipantActivitiesByPins("1234567890"));
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ForUnAuthorized_ReturnsUnauthorizedMessage()
        {
            _apiUser.IsAuthenticated = false;
            var result = _participantActivityController.GetParticipantActivitiesByPins("1234567890");
            Assert.AreEqual("UnauthorizedResult", result.GetType().Name);
        }

        [TestMethod]
        public void Test_ForAuthorized_ReturnsNoContentMessage()
        {
            var result = _participantActivityController.Test();
            Assert.AreEqual("NoContentResult", result.GetType().Name);
        }

        [TestMethod]
        public void Test_ForUnAuthorized_ReturnsUnAuthorizedMessage()
        {
            _apiUser.IsAuthenticated = false;
            var result = _participantActivityController.Test();
            Assert.AreEqual("UnauthorizedResult", result.GetType().Name);
        }

        #endregion

        #region MockDomains

        private class MockParticipantActivityDomain : IParticipantActivityDomain
        {
            public bool HasGetParticipantActivitiesByPinsBeenCalled;

            public ParticipantActivitiesWebService GetParticipantActivitiesByPins(string pins)
            {
                HasGetParticipantActivitiesByPinsBeenCalled = true;
                return new ParticipantActivitiesWebService();
            }
        }

        #endregion
    }
}
