using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/employability-plan")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class EmployabilityPlanController : Controller
    {
        #region Properties

        private readonly IActivityDomain           _activityDomain;
        private readonly IEmployabilityPlanDomain  _epDomain;
        private readonly IGoalDomain               _goalDomain;
        private readonly ISupportiveServicesDomain _supportiveServicesDomain;
        private readonly IEpEmploymentsDomain      _epEmploymentsDomain;

        #endregion

        #region Methods

        public EmployabilityPlanController(IActivityDomain           activityDomain,
                                           IEmployabilityPlanDomain  epDomain,
                                           IGoalDomain               goalDomain,
                                           ISupportiveServicesDomain supportiveServicesDomain,
                                           IEpEmploymentsDomain      epEmploymentsDomain)
        {
            _activityDomain           = activityDomain;
            _epDomain                 = epDomain;
            _goalDomain               = goalDomain;
            _supportiveServicesDomain = supportiveServicesDomain;
            _epEmploymentsDomain      = epEmploymentsDomain;
        }

        #region EPs

        [HttpGet("{pin}")]
        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        public IActionResult GetPlans(string pin)
        {
            var contract = _epDomain.GetEmployabilityPlans(pin);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("{pin}/employment-plan/{epid}")]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        public IActionResult GetPlanById(int epId)
        {
            var contract = _epDomain.GetPlanById(epId);
            var res      = Ok(contract);

            return (res);
        }

        [HttpDelete("delete/{pin}/employment-plan/{epId}/{isAutoDeleted}")]
        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        public IActionResult DeletePlan(string pin, int epId, bool isAutoDeleted)
        {
            var contract = _epDomain.DeletePlan(pin, epId, false, isAutoDeleted, false);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("{pin}/employment-plan/presave/{partId}/{submittingEP}")]
        public IActionResult PreSaveEP(string pin, int partId, bool submittingEP, [FromBody] EmployabilityPlanContract employabilityPlanContract)
        {
            var contract = _epDomain.PreSaveCheck(partId, submittingEP, employabilityPlanContract);

            return Ok(contract);
        }


        [HttpPost("{pin}/{id}/{subsequentEPId}")]
        public IActionResult UpsertEmployabilityPlan(string pin, [FromBody] EmployabilityPlanContract employabilityPlanContract, int subsequentEPId)
        {
            var contract = _epDomain.UpsertPlan(employabilityPlanContract, pin, subsequentEPId);
            var res      = Ok(contract);

            return Ok(res);
        }

        [HttpPost("{pin}/employment-plan/submit/{epId}")]
        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        public IActionResult SubmitPlan(string pin, int epId /*, [FromBody] PrintedEmployabilityPlanContract model*/)
        {
            var contract = _epDomain.SubmitPlan(pin, epId);
            var res      = Ok(contract);

            return Ok(res);

            // ToDo: Uncomment the piece below when we are ready to upload while submitting EP (move it to domain and wrap it in a transaction)
            //var cb = new ConfigurationBuilder()
            //         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //         .AddEnvironmentVariables()
            //         .AddJsonFile("EmployabilityPlanStockText.json");
            //var cbb       = cb.Build();
            //var stockText = cbb.Get<PrintedEPStockTextConfig>();
            //var report    = _employmentPlanDomain.GetEmploymentPlanPdf(pin, model, stockText);
            //var caseNumber = model.Participant?.Programs?.First(i => i.EnrolledProgramId == model.EmployabilityPlan.EnrolledProgramId)
            //                      .CaseNumber
            //                      .GetValueOrDefault() ?? 0;

            //var isUploaded = _fileUploadDomain.UploadEPDoc(pin, model.EmployabilityPlan.Id, caseNumber, report.FileStream);
        }

        [HttpPost("{pin}/employment-plan/end-ep/{epId}")]
        public IActionResult EndEP(string pin, [FromBody] EndEPContract endContract, int epId)
        {
            var contract = _epDomain.EndEP(endContract, pin, epId);
            var res      = Ok(contract);

            return res;
        }

        #endregion

        #region Events

        // schedules are handled by/under Activities (see ActivityDomain.cs)

        [HttpGet("{pin}/get-events/{epId}/{programId}/{epBeginDate}")]
        public IActionResult GetEvents(string pin, int epId, int programId, DateTime epBeginDate)
        {
            var contract = _activityDomain.GetEvents(pin, epId, programId, epBeginDate);
            var res      = Ok(contract);

            return (res);
        }

        #endregion

        #region Activities

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/{epId}/activities")]
        public IActionResult GetActivities(string pin, int epId)
        {
            var contract = _activityDomain.GetActivitiesForEp(epId);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/activity/{id}/{epId}")]
        public IActionResult GetActivity(string pin, int id, int epId)
        {
            var contract = _activityDomain.GetActivity(id, epId);
            var res      = Ok(contract);

            return Ok(res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/activities/{fromEvents}")]
        public IActionResult GetActivitiesForPin(string pin, bool fromEvents)
        {
            var contract = _activityDomain.GetActivitiesForPin(pin, fromEvents);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/activities/pep/{pepId}")]
        public IActionResult GetActivitiesForPep(string pin, int pepId)
        {
            var contract = _activityDomain.GetActivitiesForPep(pin, pepId);
            var res      = Ok(contract);

            return (res);
        }

        [HttpDelete("delete/{pin}/employment-plan/{epId}/activity/{id}/{fromEndEp}")]
        public IActionResult DeleteActivity(string pin, int id, int epId, bool fromEndEp)
        {
            var contract = _activityDomain.DeleteActivity(pin, id, epId, fromEndEp);

            var res = Ok(contract);
            return Ok(res);
        }

        [HttpPost("{pin}/employment-plan/{epId}/activity/{subepId}")]
        public IActionResult UpsertActivity(string pin, [FromBody] ActivityContract activityContract, int epId, int subepId)
        {
            var contract = _activityDomain.UpsertActivity(activityContract, pin, epId, subepId);
            var res      = Ok(contract);

            return Ok(res);
        }

        [HttpPost("{pin}/employment-plan/{epId}/elapsed-activities/{fromEpOverView}")]
        public IActionResult UpsertElapsedActivity(string pin, [FromBody] List<ActivityContract> activityContracts, int epId, bool fromEpOverView)
        {
            var contract = _activityDomain.UpsertElapsedActivity(activityContracts, pin, epId, false, fromEpOverView);
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("{pin}/employment-plan/activity/presave/{epId}/{activityTypeId}")]
        public IActionResult ActivityPreCheck(string pin, int epId, string activityTypeId)
        {
            var contract = _activityDomain.PreSaveActivity(pin, epId, activityTypeId);

            return Ok(contract);
        }

        #endregion

        #region Goals

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/{epId}/goals")]
        public IActionResult GetGoals(string pin, int epId)
        {
            var contract = _goalDomain.GetGoalsForEP(epId);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        //[ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/goal/{id}")]
        public IActionResult GetGoal(string pin, int id)
        {
            var contract = _goalDomain.GetGoal(id);
            var res      = Ok(contract);

            return Ok(res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/goals")]
        public IActionResult GetGoalsForPin(string pin)
        {
            var contract = _goalDomain.GetGoalsForPin(pin);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpDelete("delete/{pin}/employment-plan/{epId}/goal/{id}")]
        public IActionResult DeleteGoal(string pin, int id, int epId)
        {
            var contract = _goalDomain.DeleteGoal(id, epId);

            var res = Ok(contract);
            return Ok(res);
        }

        [HttpPost("{pin}/employment-plan/{epId}/goal")]
        public IActionResult UpsertGoal(string pin, [FromBody] GoalContract goalContract, int epId)
        {
            var contract = _goalDomain.UpsertGoal(goalContract, pin, epId);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion

        #region Employment

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/{epId}/employment/{epBeginDate}/{programCd}")]
        public IActionResult GetEmploymentsForEp(string pin, int epId, string epBeginDate, string programCd)
        {
            var contract = _epEmploymentsDomain.GetEmploymentsForEp(pin, epId, DateTime.Parse(epBeginDate), programCd);
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("{pin}/employment-plan/{epId}/employment")]
        public IActionResult UpsertEmployment(string pin, [FromBody] List<EpEmploymentContract> employmentsContract, int epId)
        {
            var contract = _epEmploymentsDomain.UpsertEpEmployment(employmentsContract, pin, epId);
            var res      = Ok(contract);
            return (res);
        }

        #endregion

        #region Supportive Services

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [ServiceFilter(typeof(ValidEpIdMustExistAttribute))]
        [HttpGet("{pin}/employment-plan/{epId}/supportive-service")]
        public IActionResult GetSupportiveServices(string pin, int epId)
        {
            var contract = _supportiveServicesDomain.GetSupportiveServicesForEP(epId);
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("{pin}/employment-plan/{epId}/supportive-service")]
        public IActionResult UpsertSupportiveServices(string pin, int epId, [FromBody] List<SupportiveServiceContract> supportiveServiceContract)
        {
            var contract = _supportiveServicesDomain.Upsert(supportiveServiceContract, epId);
            var res      = Ok(contract);

            return (res);
        }

        #endregion

        #region Child Care Authorizations

        [HttpGet("{pin}/child-care-authorizations")]
        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        public IActionResult GetChildCareAuthorizationsByPin(string pin)
        {
            var contract = _epDomain.GetChildCareAuthorizationsByPin(pin);
            var res      = Ok(contract);

            return (res);
        }

        #endregion

        #endregion
    }
}
