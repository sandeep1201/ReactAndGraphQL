using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Controllers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Controller
{
    [TestClass]
    public class EmployabilityPlanControllerTest
    {
        #region Properties

        private MockActivityDomain           _activityDomain;
        private MockEmployabilityPlanDomain  _employabilityPlanDomain;
        private MockGoalDomain               _goalDomain;
        private MockSupportiveServicesDomain _supportiveServicesDomain;
        private MockEPEmploymentsDomain      _epEmploymentsDomain;
        private EmployabilityPlanController  _employabilityPlanController;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _activityDomain              = new MockActivityDomain();
            _employabilityPlanDomain     = new MockEmployabilityPlanDomain();
            _goalDomain                  = new MockGoalDomain();
            _supportiveServicesDomain    = new MockSupportiveServicesDomain();
            _epEmploymentsDomain         = new MockEPEmploymentsDomain();
            _employabilityPlanController = new EmployabilityPlanController(_activityDomain, _employabilityPlanDomain, _goalDomain, _supportiveServicesDomain, _epEmploymentsDomain);
        }

        [TestMethod]
        public void GetChildCareAuthorizationsByPin_ValidPin_ReturnsIActionResult()
        {
            Assert.IsNotNull(_employabilityPlanDomain.GetChildCareAuthorizationsByPin("1001"));
        }

        [TestMethod]
        public void GetChildCareAuthorizationsByPin_ValidPin_CallsGetChildCareAuthorizationsByPin()
        {
            _employabilityPlanDomain.GetChildCareAuthorizationsByPin("1001");
            Assert.IsTrue(_employabilityPlanDomain.GetCCAuthorizationsHasBeenCalled);
        }

        #endregion

        #region MockActivityDomain

        private class MockActivityDomain : IActivityDomain
        {
            public EmployabilityPlan GetRecentSubmittedEP(string pin, int epId, int enrolledProgramId, bool considerEnded = false)
            {
                throw new NotImplementedException();
            }

            public ActivityContract GetActivity(int activityId, int employabilityPlanId)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> GetActivitiesForPin(string pin, bool fromEvents = false)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> GetActivitiesForPep(string pin, int pepId)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> GetActivitiesForEp(int epId)
            {
                throw new NotImplementedException();
            }

            public List<EventsContract> GetEvents(string pin, int epId, int programId, DateTime epBeginDate)
            {
                throw new NotImplementedException();
            }

            public bool DeleteActivity(string pin, int id, int epId, bool fromEndEp)
            {
                throw new NotImplementedException();
            }

            public void DeleteActivityAndSchedules(int epId, Activity activity)
            {
                throw new NotImplementedException();
            }

            public PreSavingActivityContract PreSaveActivity(string pin, int epId, string activityTypeId)
            {
                throw new NotImplementedException();
            }

            public ActivityContract UpsertActivity(ActivityContract activityContract, string pin, int epId, int subepId)
            {
                throw new NotImplementedException();
            }

            public List<ActivityContract> UpsertElapsedActivity(List<ActivityContract> activityContracts, string pin, int epId, bool isDisenrollment, bool fromEpOverView)
            {
                throw new NotImplementedException();
            }

            public void EndActivityTransactionalSave(string xml, int? epId = null, string programCd = null)
            {
                throw new NotImplementedException();
            }

            public void EndStatus(int epId, decimal pin, IReadOnlyCollection<ActivityContract> contracts, DateTime updateTime)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region MockEmployabilityPlanDomain

        private class MockEmployabilityPlanDomain : IEmployabilityPlanDomain
        {
            public bool GetCCAuthorizationsHasBeenCalled;

            public List<EmployabilityPlanContract> GetEmployabilityPlans(string pin)
            {
                throw new NotImplementedException();
            }

            public DocumentResponse GetDocumentsForPin(string pin)
            {
                throw new NotImplementedException();
            }

            public EmployabilityPlanContract GetPlanById(int epId)
            {
                throw new NotImplementedException();
            }

            public bool DeletePlan(string pin, int epId, bool isVoid, bool isAutoDeleted, bool epTransfer)
            {
                throw new NotImplementedException();
            }

            public EmployabilityPlanContract SubmitPlan(string pin, int epId)
            {
                throw new NotImplementedException();
            }

            public EndEPContract EndEP(EndEPContract contract, string pin, int epId)
            {
                throw new NotImplementedException();
            }

            public PreCheckEPContract PreSaveCheck(int partId, bool submittingEP, EmployabilityPlanContract contract)
            {
                throw new NotImplementedException();
            }

            public EmployabilityPlanContract UpsertPlan(EmployabilityPlanContract employabilityPlanContract, string pin, int subsequentEPId)
            {
                throw new NotImplementedException();
            }

            public void EPTransfer(IParticipantEnrolledProgram pep, DateTime modifiedDate)
            {
                throw new NotImplementedException();
            }

            public string GetChildCareAuthorizationsByPin(string pin)
            {
                GetCCAuthorizationsHasBeenCalled = true;
                return "{\"effectivePeriod\":\"12/01/2020 - 06/30/2021\",\"periods\":[{\"month\":\"December\",\"year\":2020,\"children\":[{\"name\":\"David Warner\",\"hours\":120}]},{\"month\":\"January\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120},{\"name\":\"William Smith\",\"hours\":120}]},{\"month\":\"February\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120},{\"name\":\"William Smith\",\"hours\":120},{\"name\":\"Chris Gayle\",\"hours\":120}]},{\"month\":\"March\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120},{\"name\":\"Sachin Tendlukar\",\"hours\":120}]},{\"month\":\"April\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120}]}]}";
            }
        }

        #endregion

        #region MockGoalDomain

        private class MockGoalDomain : IGoalDomain
        {
            public GoalContract GetGoal(int id)
            {
                throw new NotImplementedException();
            }

            public List<GoalContract> GetGoalsForEP(int epId)
            {
                throw new NotImplementedException();
            }

            public List<GoalContract> GetGoalsForPin(string pin)
            {
                throw new NotImplementedException();
            }

            public bool DeleteGoal(int goalId, int epId, bool canCommit = true)
            {
                throw new NotImplementedException();
            }

            public GoalContract UpsertGoal(GoalContract goalContract, string pin, int epId)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region MockSupportiveServicesDomain

        private class MockSupportiveServicesDomain : ISupportiveServicesDomain
        {
            public List<SupportiveServiceContract> GetSupportiveServicesForEP(int epId)
            {
                throw new NotImplementedException();
            }

            public List<SupportiveServiceContract> Upsert(List<SupportiveServiceContract> supportiveServiceContract, int epId)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region MockEpEmploymentsDomain

        private class MockEPEmploymentsDomain : IEpEmploymentsDomain
        {
            public List<EpEmploymentContract> GetEmploymentsForEp(string pin, int epId, DateTime epBeginDate, string programCd)
            {
                throw new NotImplementedException();
            }

            public List<EpEmploymentContract> UpsertEpEmployment(List<EpEmploymentContract> employmentsContract, string pin, int epId)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
