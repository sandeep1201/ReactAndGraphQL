using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Middleware;
using DCF.Common.Logging;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProblemDetails = Dcf.Wwp.Api.Library.Utils.ProblemDetails;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/update-placement")]
    public class PlacementController : Controller
    {
        private          IApiUser                     ApiUser { get; }
        private readonly IParticipationTrackingDomain _participationTrackingDomain;
        private readonly ISpecialInitiativeRepository _specialInitiativeRepository;
        private readonly ILog                         _logger;

        public PlacementController(IApiUser apiUser, IParticipationTrackingDomain participationTrackingDomain, ISpecialInitiativeRepository specialInitiativeRepository)
        {
            ApiUser                      = apiUser;
            _participationTrackingDomain = participationTrackingDomain;
            _specialInitiativeRepository = specialInitiativeRepository;
            _logger                      = LogProvider.GetLogger(typeof(PlacementController));
        }

        [HttpPut("{batchKey?}")]
        public IActionResult UpdatePlacement(string batchKey, [FromBody] List<UpdatePlacement> updatePlacements)
        {
            var keyType = batchKey != null ? "Batch" : "API";
            var isAuthenticated = keyType        == "Batch"
                                      ? batchKey == _specialInitiativeRepository.Get(i => i.ParameterName == "BatchKey").ParameterValue
                                      : ApiUser.IsAuthenticated;

            if (!isAuthenticated)
            {
                _logger.Warn($"{keyType} key not valid, returning 401.");

                return Unauthorized();
            }

            try
            {
                if (updatePlacements == null || updatePlacements.Count == 0)
                {
                    _logger.Warn("Update Placement request is empty.");
                    return BadRequest(ProblemDetails.CreateBadRequestDetails("Update Placement request is empty"));
                }

                if (!ModelState.IsValid)
                {
                    _logger.Warn("update Placement request ModelState is invalid.");
                    return BadRequest(ProblemDetails.CreateBadRequestDetails(ModelState));
                }

                var jsonRequest = JsonConvert.SerializeObject(updatePlacements);
                _logger.Info($"REST WS Update Placement Msg: {jsonRequest}");

                var commitStatus = _participationTrackingDomain.UpdatePlacement(updatePlacements, keyType == "Batch" ? "WWP Batch" : "CWW");

                if (commitStatus != null && commitStatus.Status != "SUCCESS")
                {
                    var e = commitStatus.Exception;

                    _logger.ErrorException("Update Placement DBException.", e);
                    return StatusCode(500, ProblemDetails.CreateGatewayErrorDetails($"{(keyType == "Batch" ? $"{e.Message} {e.StackTrace}" : e.Message)}"));
                }
            }
            catch (InvalidOperationException e)
            {
                _logger.ErrorException("Update Placement InvalidOperationException.", e);
                return StatusCode(400, ProblemDetails.CreateBadRequestDetails($"{(keyType == "Batch" ? $"{e.Message} {e.StackTrace}" : e.Message)}"));
            }
            catch (Exception e)
            {
                _logger.ErrorException("Update Placement Exception.", e);
                return StatusCode(500, ProblemDetails.CreateServerErrorDetails($"{(keyType == "Batch" ? $"{e.Message} {e.StackTrace}" : e.Message)}"));
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
}
