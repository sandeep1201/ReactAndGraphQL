using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/employment-verification")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class EmploymentVerificationController : Controller
    {
        #region Properties

        private readonly IEmploymentVerificationDomain _employmentVerificationDomain;

        #endregion

        #region Methods

        public EmploymentVerificationController(IEmploymentVerificationDomain employmentVerificationDomain)
        {
           _employmentVerificationDomain = employmentVerificationDomain;
        }

        [HttpGet("{participantId}/{jobTypeId}/{enrollmentDate}")]
        public IActionResult GetTJTMJEmploymentsForParticipantByJobType(int participantId, int jobTypeId, DateTime enrollmentDate)
        {
            return Ok(_employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(participantId, jobTypeId, enrollmentDate));
        }
        
        [HttpPost("{pin}")]
        public IActionResult PostEmploymentVerification(string pin, [FromBody] List<EmploymentVerificationContract> contract)
        {
            _employmentVerificationDomain.UpsertEmploymentVerification(pin, contract);
            return Ok();
        }

        #endregion
    }
}
