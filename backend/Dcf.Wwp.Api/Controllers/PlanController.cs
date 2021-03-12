using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/w-2-plans")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class PlanController : Controller
    {
        #region Properties

        private readonly IPlanDomain _planDomain;

        #endregion

        #region Methods

        public PlanController(IPlanDomain planDomain)
        {
            _planDomain = planDomain;
        }

        [HttpGet("{id}")]
        public IActionResult GetW2PlansByParticipantId(int id)
        {
            var contract = _planDomain.GetW2PlansByParticipantId(id);
            return Ok(contract);
        }

        [HttpPost("section/{participantId}")]
        public IActionResult UpsertW2PlansSection(int participantId, [FromBody] PlanSectionContract w2PlansSectionContract)
        {
            return Ok(_planDomain.UpsertPlanSection(w2PlansSectionContract, participantId));
        }

        #endregion
    }
}
