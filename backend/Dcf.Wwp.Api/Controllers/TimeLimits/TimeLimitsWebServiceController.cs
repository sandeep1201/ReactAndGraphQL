using System;
using DCF.Common.Logging;
using DCF.Timelimts.Service;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Api.Middleware;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProblemDetails = Dcf.Wwp.Api.Library.Utils.ProblemDetails;

namespace Dcf.Wwp.Api.Controllers.TimeLimits
{
    [Route("api/timelimits-summary")]
    public class TimeLimitsWebServiceController : Controller
    {
        private          IRepository        Repo    { get; }
        private          IApiUser           ApiUser { get; }
        private          IAuthUser          _authUser;
        private readonly ITimelimitService  _timelimitService;
        private readonly ILog               _logger = LogProvider.GetLogger(typeof(TimeLimitsWebServiceController));

        public TimeLimitsWebServiceController(IRepository repository, IApiUser apiUser, ITimelimitService timelimitService, IAuthUser authUser)
        {
            Repo               = repository;
            ApiUser            = apiUser;
            _timelimitService  = timelimitService;
            _authUser          = authUser;
        }

        [HttpGet("{pin}")]
        public IActionResult TimeLimitSummary(string pin)
        {
            if (!ApiUser.IsAuthenticated)
            {
                _logger.Warn("API key not valid, returning 401.");
                return Unauthorized();
            }

            try
            {
                if (string.IsNullOrEmpty(pin))
                {
                    _logger.Warn("PIN in the request is empty.");
                    return BadRequest(ProblemDetails.CreateBadRequestDetails("PIN in the request is empty"));
                }

                var jsonRequest = JsonConvert.SerializeObject(pin);
                _logger.Info($"REST WS TimeLimits Msg: {jsonRequest}");

                var vm   = new TimeLineViewModel(Repo, _authUser, _timelimitService);
                var data = vm.GetTimelineByPins(pin);
                data.MessageCode = "SUCCESS";
                return Ok(data);
            }
            catch (InvalidOperationException e)
            {
                _logger.ErrorException("TimeLimit Summary InvalidOperationException.", e);
                return StatusCode(400, ProblemDetails.CreateBadRequestDetails(e.Message));
            }
            catch (Exception e)
            {
                _logger.ErrorException("TimeLimit Summary Exception.", e);
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
