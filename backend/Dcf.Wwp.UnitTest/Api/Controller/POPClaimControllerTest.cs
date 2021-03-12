using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Controllers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POPClaimStatusType = Dcf.Wwp.Model.Interface.Constants.POPClaimStatusType;

namespace Dcf.Wwp.UnitTest.Api.Controller
{
    [TestClass]
    public class POPClaimControllerTest
    {
        #region Properties

        private MockPOPClaimDomain _popClaimDomain;
        private POPClaimController _popClaimController;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _popClaimDomain     = new MockPOPClaimDomain();
            _popClaimController = new POPClaimController(_popClaimDomain);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void GetPopClaimsForParticipant_ValidParticipantId_ReturnsPopClaims()
        {
            _popClaimController.GetPopClaimsForParticipant(123);
        }

        [TestMethod]
        public void GetPOPClaimsForParticipant_ValidParticipantId_ReturnsIActionResult()
        {
            Assert.IsNotNull(_popClaimController.GetPopClaimsForParticipant(123));
        }

        [TestMethod]
        public void GetPOPClaimsForParticipant_ValidParticipantId_CallsGetPOPClaims()
        {
            _popClaimController.GetPopClaimsForParticipant(123);
            Assert.IsTrue(_popClaimDomain.hasGetPOPClaimsBeenCalled);
        }

        [TestMethod]
        public void GetPOPClaimsForParticipant_WithoutAParticipantId_CallsGetPOPClaimsWithoutParticipant()
        {
            _popClaimController.GetPOPClaimsWithoutParticipant();
            Assert.IsTrue(_popClaimDomain.hasGetPOPClaimsWithoutParticipantBeenCalled);
        }

        [TestMethod]
        public void GetPOPClaim_WithValidId()
        {
            _popClaimDomain.GetPOPClaim(123);
            Assert.IsTrue(_popClaimDomain.hasGetPOPClaimBeenCalled);
        }

        [TestMethod]
        public void GetPOPClaimsWithStatuses_WithStatuses_ReturnsIActionResult()
        {
            Assert.IsNotNull(_popClaimController.GetPOPClaimsWithStatuses("FSC", new List<string> { POPClaimStatusType.SubmitCd, POPClaimStatusType.ReviewCd, POPClaimStatusType.ReturnCd }));
        }

        [TestMethod]
        public void GetPOPClaimsWithStatuses_WithStatuses_CallsGetPOPClaimsWithStatuses()
        {
            _popClaimController.GetPOPClaimsWithStatuses("FSC", new List<string> { POPClaimStatusType.SubmitCd, POPClaimStatusType.ReviewCd, POPClaimStatusType.ReturnCd });
            Assert.IsTrue(_popClaimDomain.hasGetPOPClaimsWithStatusesBeenCalled);
        }

        [TestMethod]
        public void PostPOPClaim()
        {
            _popClaimDomain.UpsertPOPClaim(new POPClaimContract());
            Assert.IsTrue(_popClaimDomain.hasUpsertPOPClaimBeenCalled);
        }

        [TestMethod]
        public void GetEmploymentsForPOP_ValidPin_ReturnsEmploymentsForPin()
        {
            _popClaimController.GetEmploymentsForPOP("123", 1);
        }

        [TestMethod]
        public void GetEmploymentsForPOP_ValidPin_ReturnsIActionResult()
        {
            _popClaimController.GetEmploymentsForPOP("123", 1);
        }

        [TestMethod]
        public void GetPOPClaimEmploymentsForParticipant_ValidParticipantId_CallsGetEmploymentsForPOP()
        {
            _popClaimController.GetEmploymentsForPOP("123", 1);
            Assert.IsTrue(_popClaimDomain.hasGetEmploymentsForPOPBeenCalled);
        }

        [TestMethod]
        public void PreAddCheckBeingCalledAndReturnsActionResult()
        {
            Assert.IsInstanceOfType(_popClaimController.PreAddCheck("123", new POPClaimContract()), typeof(IActionResult));
        }

        [TestMethod]
        public void PreAddCheckCallsTheDomainMethod()
        {
            _popClaimController.PreAddCheck("123", new POPClaimContract());
            Assert.IsTrue(_popClaimDomain.hasPreAddCheckBeenCalled);
        }

        #endregion

        #region Domain

        private class MockPOPClaimDomain : IPOPClaimDomain
        {
            public bool hasGetPOPClaimsBeenCalled;
            public bool hasGetPOPClaimBeenCalled;
            public bool hasUpsertPOPClaimBeenCalled;
            public bool hasGetEmploymentsForPOPBeenCalled;
            public bool hasPreAddCheckBeenCalled;
            public bool hasGetPOPClaimsWithoutParticipantBeenCalled;
            public bool hasGetPOPClaimsWithStatusesBeenCalled;

            public List<POPClaimContract> GetPOPClaims(int participantId)
            {
                hasGetPOPClaimsBeenCalled = true;
                return null;
            }

            public List<POPClaimContract> GetPOPClaimsByAgency(string agencyCode = null)
            {
                hasGetPOPClaimsWithoutParticipantBeenCalled = true;
                return null;
            }


            public void UpsertPOPClaim(POPClaimContract contract, bool isSystemGenerated = false)
            {
                hasUpsertPOPClaimBeenCalled = true;
            }

            public POPClaimContract GetPOPClaim(int i)
            {
                hasGetPOPClaimBeenCalled = true;
                return new POPClaimContract();
            }

            public List<POPClaimEmploymentContract> GetEmploymentsForPOP(string pin, int popClaimId)
            {
                hasGetEmploymentsForPOPBeenCalled = true;
                return null;
            }

            public PreAddingPOPClaimContract PreAddCheck(POPClaimContract contract)
            {
                hasPreAddCheckBeenCalled = true;
                return new PreAddingPOPClaimContract();
            }

            public List<POPClaimContract> GetPOPClaimsWithStatuses(List<string> statuses, string agencyCode = null)
            {
                hasGetPOPClaimsWithStatusesBeenCalled = true;
                return new List<POPClaimContract>();
            }

            public bool InsertSystemGeneratedPOPClaim(EmployabilityPlan employabilityPlan, string activityTypeCode, string activityCompletionReasonCode, DateTime? activityEndDate, int activityId, string popClaimType)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
