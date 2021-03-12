using System;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Middleware;
using DCF.Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProblemDetails = Dcf.Wwp.Api.Library.Utils.ProblemDetails;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/participant-activity")]
    public class ParticipantActivityController : Controller
    {
        private          IApiUser ApiUser { get; }
        private readonly ILog     _logger = LogProvider.GetLogger(typeof(ParticipantActivityController));

        private readonly IParticipantActivityDomain _participantActivityDomain;

        public ParticipantActivityController(IApiUser apiUser, IParticipantActivityDomain participantActivityDomain)
        {
            ApiUser                    = apiUser;
            _participantActivityDomain = participantActivityDomain;
        }
        
        [HttpGet("{pins}")]
        public IActionResult GetParticipantActivitiesByPins(string pins)
        {
            if (!ApiUser.IsAuthenticated)
            {
                _logger.Warn("API key not valid, returning 401.");
                return Unauthorized();
            }

            try
            {
                if (string.IsNullOrEmpty(pins))
                {
                    _logger.Warn("PIN in the request is empty.");
                    return BadRequest(ProblemDetails.CreateBadRequestDetails("PIN in the request is empty"));
                }

                var jsonRequest = JsonConvert.SerializeObject(pins);
                _logger.Info($"REST WS Activities Msg: {jsonRequest}");

                var data = _participantActivityDomain.GetParticipantActivitiesByPins(pins);
                data.MessageCode = "SUCCESS";
                return Ok(data);
            }
            catch (InvalidOperationException e)
            {
                _logger.ErrorException("Participant Activities InvalidOperationException.", e);
                return StatusCode(400, ProblemDetails.CreateBadRequestDetails(e.Message));
            }
            catch (Exception e)
            {
                _logger.ErrorException("Participant Activities Exception.", e);
                return StatusCode(500, ProblemDetails.CreateServerErrorDetails(e.Message));
            }
        }

        [HttpGet]
        public IActionResult Test()
        {
            if (!ApiUser.IsAuthenticated)
                return Unauthorized();

            return NoContent();
        }
    }
}
