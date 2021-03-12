using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/participation-tracking")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ParticipationTrackingController : Controller
    {
        #region Properties

        private readonly IParticipationTrackingDomain _participationTrackingDomain;

        #endregion

        #region Methods

        public ParticipationTrackingController(IParticipationTrackingDomain participationTrackingDomain)
        {
            _participationTrackingDomain = participationTrackingDomain;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("{pin}/{participantId}/{startDate}/{endDate}/{isFromDetails}/{programCode?}")]
        public IActionResult GetParticipationTrackingDetails(string pin, int participantId, string startDate, string endDate, bool isFromDetails, string programCode)
        {
            var contract = _participationTrackingDomain.GetParticipationTrackingDetails(participantId, startDate, endDate, isFromDetails, programCode);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("{pin}/save/{programCode?}")]
        public IActionResult UpsertParticipationTrackingDetails(string pin, [FromBody] ParticipationTrackingContract participationTrackingContract, string programCode)
        {
            var contract = _participationTrackingDomain.UpsertParticipationTrackingDetails(participationTrackingContract, programCode);
            var res      = Ok(contract);

            return res;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpDelete("{pin}/{id}/{participantId}/delete")]
        public IActionResult DeleteParticipationTrackingDetails(string pin, int id)
        {
            var contract = _participationTrackingDomain.DeleteParticipationTrackingDetails(id);
            var res      = Ok(contract);

            return res;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("{pin}/{participantId}/{makeFullOrNoParticipation}/{startDate}/{endDate}/{programCode?}")]
        public IActionResult MakeFullOrNoParticipation(string pin, int participantId, string makeFullOrNoParticipation, string startDate, string endDate, [FromBody] List<ParticipationTrackingContract> participationTrackingContracts, string programCode)
        {
            var contract = _participationTrackingDomain.MakeFullOrNoParticipation(participantId, makeFullOrNoParticipation, startDate, endDate, participationTrackingContracts, programCode);
            var res      = Ok(contract);

            return res;
        }

        #endregion
    }
}
