using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/career-assessment")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class CareerAssessmentController : Controller
    {
        #region Properties

        private readonly ICareerAssessmentDomain _careerAssessmentDomain;

        #endregion

        #region Methods

        public CareerAssessmentController(ICareerAssessmentDomain careerAssessmentDomain)
        {
            _careerAssessmentDomain = careerAssessmentDomain;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}")]
        public IActionResult GetCareerAssessmentsForPin(string pin)
        {
            var contract = _careerAssessmentDomain.GetCareerAssessmentsForPin(decimal.Parse(pin));
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/{id}")]
        public IActionResult GetCareerAssessment(string pin, int id)
        {
            var contract = _careerAssessmentDomain.GetCareerAssessment(id);
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("{pin}/save")]
        public IActionResult UpsertCareerAssessment([FromBody] CareerAssessmentContract careerAssessmentContract, string pin)
        {
            var contract = _careerAssessmentDomain.UpsertCareerAssessment(careerAssessmentContract, pin);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion
    }
}
