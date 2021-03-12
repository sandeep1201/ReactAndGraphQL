using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Api.Middleware;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProblemDetails = Dcf.Wwp.Api.Library.Utils.ProblemDetails;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/reassign-fep")]
    public class ReassignFepController : Controller
    {
        private IRepository Repo { get; }
        private IApiUser ApiUser { get; }

        private readonly ILog _logger = LogProvider.GetLogger(typeof(ReassignFepController));


        public ReassignFepController(IRepository repository, IApiUser apiUser)
        {
            Repo = repository;
            ApiUser = apiUser;
        }

        [HttpPut]
        public IActionResult ReassignFep([FromBody] ReassignFepRequest request)
        {
            if (!ApiUser.IsAuthenticated)
            {
                _logger.Warn("API key not valid, returning 401.");
                return Unauthorized();
            }

            try
            {
                if (request == null)
                {
                    _logger.Warn("Reassign FEP request is empty.");
                    return BadRequest(ProblemDetails.CreateBadRequestDetails("Reassign FEP request is empty"));
                }

                if (!ModelState.IsValid)
                {
                    _logger.Warn("Reassign FEP request ModelState is invalid.");
                    return BadRequest(ProblemDetails.CreateBadRequestDetails(ModelState));
                }

                var jsonRequest = JsonConvert.SerializeObject(request);
                _logger.Info($"REST WS ReassignFEP Msg: {jsonRequest}");

                var vm = new ReassignFepViewModel(Repo);
                vm.UpdatePins(request.NewFepMainframeId, request.ParticipantPins);
            }
            catch (InvalidOperationException e)
            {
                _logger.ErrorException("Reassign FEP InvalidOperationException.", e);
                return StatusCode(400, ProblemDetails.CreateBadRequestDetails(e.Message));
            }
            catch (Exception e)
            {
                _logger.ErrorException("Reassign FEP Exception.", e);
                return StatusCode(500, ProblemDetails.CreateServerErrorDetails(e.Message));
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Test()
        {
            if (!ApiUser.IsAuthenticated)
                return Unauthorized();

            return NoContent();
        }
    }

    public class ReassignFepRequest
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OldFepMainframeId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string NewFepMainframeId { get; set; }

        [AtLeastOne(ErrorMessage = "At least one PIN is required.")]
        [Required]
        public List<string> ParticipantPins { get; set; }
    }
}
