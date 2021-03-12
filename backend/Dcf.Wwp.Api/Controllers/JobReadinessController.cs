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
    [Route("api/job-readiness")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class JobReadinessController : Controller
    {
        #region Properties

        private readonly IJobReadinessDomain _jobReadinessDomain;

        #endregion

        #region Methods

        public JobReadinessController(IJobReadinessDomain jobReadinessDomain)
        {
            _jobReadinessDomain = jobReadinessDomain;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}")]
        public IActionResult GetJobReadinessForPin(string pin)
        {
            var contract = _jobReadinessDomain.GetJobReadinessForPin(decimal.Parse(pin));
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("{pin}/{id}/{hasSaveErrors}")]
        public IActionResult UpsertJobReadiness([FromBody] JobReadinessContract jobReadinessContract, string pin, int id, bool hasSaveErrors)
        {
            var contract = _jobReadinessDomain.UpsertJobReadiness(jobReadinessContract, pin, id, hasSaveErrors);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion
    }
}
