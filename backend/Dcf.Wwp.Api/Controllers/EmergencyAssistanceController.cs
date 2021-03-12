using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/ea")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class EmergencyAssistanceController : Controller
    {
        #region Properties

        private readonly IEmergencyAssistanceDomain _emergencyAssistanceDomain;

        #endregion

        #region Methods

        public EmergencyAssistanceController(IEmergencyAssistanceDomain emergencyAssistanceDomain)
        {
            _emergencyAssistanceDomain = emergencyAssistanceDomain;
        }

        [HttpGet("ea-group-confidential-participant/{pin}")]
        public IActionResult GetEAGroupConfidentialParticipant(string pin)
        {
            var contract = _emergencyAssistanceDomain.GetEAGroupConfidentialParticipant(pin);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("request-list/{pin}")]
        public IActionResult GetEAsForParticipant(string pin)
        {
            var contract = _emergencyAssistanceDomain.GetRequests(pin);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("request/{pin}/{id}")]
        public IActionResult GetEARequest(string pin, int id)
        {
            var contract = _emergencyAssistanceDomain.GetRequest(id);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("agMembers/{pin}/{id}")]
        public IActionResult GetAGMembers(string pin, int id)
        {
            var contract = _emergencyAssistanceDomain.GetAGMembers(pin, id);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("searchParticipant/{pin}")]
        public IActionResult SearchParticipant(string pin)
        {
            var contract = _emergencyAssistanceDomain.SearchParticipant(pin);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("request/agency-summary/{pin}/{id}")]
        public IActionResult GetEARequestAgencySummary(string pin, int id)
        {
            var contract = _emergencyAssistanceDomain.GetEARequestAgencySummary(id);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("payment/{id}")]
        public IActionResult GetPayment(int id)
        {
            var contract = _emergencyAssistanceDomain.GetPayment(id);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("ipv-list/{pin}")]
        public IActionResult GetIPVs(string pin)
        {
            var contract = _emergencyAssistanceDomain.GetIPVs(pin);
            var res      = Ok(contract);

            return (res);
        }

        [HttpGet("ipv/{id}")]
        public IActionResult GetIPV(int id)
        {
            var contract = _emergencyAssistanceDomain.GetIPV(id);
            var res      = Ok(contract);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("request/demographics/{pin}")]
        public IActionResult PostEADemographics(string pin, [FromBody] EADemographicsContract contract)
        {
            var result = _emergencyAssistanceDomain.UpsertDemographics(pin, contract);
            var res    = Ok(result);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("request/emergencytype/{pin}")]
        public IActionResult PostEAEmergencyType(string pin, [FromBody] EAEmergencyTypeContract contract)
        {
            var result = _emergencyAssistanceDomain.UpsertEmergencyType(contract);
            var res    = Ok(result);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("request/householdmembers/{pin}")]
        public IActionResult PostEAGroupMembers(string pin, [FromBody] EAGroupMembersContract contract)
        {
            var result = _emergencyAssistanceDomain.UpsertGroupMembers(contract);
            var res    = Ok(result);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("request/householdfinancials/{pin}")]
        public IActionResult PostEAHouseHoldFinancials(string pin, [FromBody] EAHouseHoldFinancialsContract contract)
        {
            var result = _emergencyAssistanceDomain.UpsertHouseHoldFinancials(contract);
            var res    = Ok(result);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("request/agencysummary/{pin}")]
        public IActionResult PostEAAgencySummary(string pin, [FromBody] EAAgencySummaryContract contract)
        {
            var result = _emergencyAssistanceDomain.UpsertAgencySummary(pin, contract);
            var res    = Ok(result);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("request/comment/{pin}/{requestId}")]
        public IActionResult PostEAComment(string pin, [FromBody] CommentContract contract, int requestId)
        {
            var result = _emergencyAssistanceDomain.UpsertComments(contract, requestId);
            var res    = Ok(result);

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("post-ipv/{pin}")]
        public IActionResult PostIPV(string pin, [FromBody] EAIPVContract contract)
        {
            _emergencyAssistanceDomain.UpsertIPV(contract, pin);
            var res    = Ok();

            return (res);
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpPost("post-payment/{pin}")]
        public IActionResult PostPayment(string pin, [FromBody] EAPaymentContract contract)
        {
            var result = _emergencyAssistanceDomain.UpsertPayment(contract);
            var res    = Ok(result);

            return (res);
        }

        #endregion
    }
}
