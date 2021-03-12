using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/auxiliary")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class AuxiliaryController : Controller
    {
        #region Properties

        private readonly IAuxiliaryDomain _auxiliaryDomain;

        #endregion

        #region Methods

        public AuxiliaryController(IAuxiliaryDomain auxiliaryDomain)
        {
            _auxiliaryDomain = auxiliaryDomain;
        }

        [HttpGet("list/{participantId?}")]
        public IActionResult GetAuxiliariesForParticipant(int? participantId)
        {
            var contract = _auxiliaryDomain.GetAuxiliaries(participantId);
            return Ok(contract);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuxiliary(int id)
        {
            var contract = _auxiliaryDomain.GetAuxiliary(id);
            return Ok(contract);
        }

        [HttpGet("{pin}/{participantId}/paymentDetails/{participationPeriod}/{year}")]
        public IActionResult GetDetails(string pin, int participantId, string participationPeriod, short year)
        {
            var contract = _auxiliaryDomain.GetDetailsBasedOnParticipationPeriod(pin, participantId, participationPeriod, year);
            return Ok(contract);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("{pin}")]
        public IActionResult PostAuxiliary(string pin, [FromBody] AuxiliaryContract contract)
        {
            _auxiliaryDomain.UpsertAuxiliary(contract);
            return Ok();
        }

        #endregion
    }
}
