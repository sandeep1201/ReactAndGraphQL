using System.Collections.Generic;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/pop-claim")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class POPClaimController : Controller
    {
        #region Properties

        private readonly IPOPClaimDomain _popClaimDomain;

        #endregion

        #region MyRegion

        public POPClaimController(IPOPClaimDomain popClaimDomain)
        {
            _popClaimDomain = popClaimDomain;
        }

        #endregion

        [HttpGet("list/{participantId}")]
        public IActionResult GetPopClaimsForParticipant(int participantId)
        {
            var popClaims = _popClaimDomain.GetPOPClaims(participantId);
            return Ok(popClaims);
        }

        [HttpGet("list")]
        public IActionResult GetPOPClaimsWithoutParticipant()
        {
            var popClaims = _popClaimDomain.GetPOPClaimsByAgency();
            return Ok(popClaims);
        }

        [HttpGet("{id}")]
        public IActionResult GetPOPClaim(int id)
        {
            var contract = _popClaimDomain.GetPOPClaim(id);
            return Ok(contract);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("{pin}")]
        public IActionResult PostPOPClaim(string pin, [FromBody] POPClaimContract contract)
        {
            _popClaimDomain.UpsertPOPClaim(contract);
            return Ok();
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/pop-claim-employments/{popClaimId}")]
        public IActionResult GetEmploymentsForPOP(string pin, int popClaimId)
        {
            return Ok(_popClaimDomain.GetEmploymentsForPOP(pin, popClaimId));
        }

        [HttpPost("{pin}/preAdd/{id}")]
        public IActionResult PreAddCheck(string pin, [FromBody] POPClaimContract contract)
        {
            return Ok(_popClaimDomain.PreAddCheck(contract));
        }

        [HttpPost("list/{agencyCode?}")]
        public IActionResult GetPOPClaimsWithStatuses(string agencyCode, [FromBody] List<string> statuses)
        {
            return Ok(_popClaimDomain.GetPOPClaimsWithStatuses(statuses, agencyCode));
        }
    }
}
