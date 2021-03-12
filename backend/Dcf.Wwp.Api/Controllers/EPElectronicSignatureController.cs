using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Dcf.Wwp.Api.Middleware;
using DCF.Common.Logging;
using Dcf.Wwp.TelerikReport.Library.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/electronic-signature")]
    public class EPElectronicSignatureController : Controller
    {
        private          IApiUser              ApiUser { get; }
        private readonly ILog                  _logger = LogProvider.GetLogger(typeof(EPElectronicSignatureController));
        private readonly IEmploymentPlanDomain _employmentPlanDomain;
        private          string                _resultFile;


        public EPElectronicSignatureController(IApiUser apiUser, IEmploymentPlanDomain employmentPlanDomain)
        {
            ApiUser               = apiUser;
            _employmentPlanDomain = employmentPlanDomain;
        }

        [HttpPut]
        public IActionResult ElectronicSignature([FromBody] EPElectronicSignatureRequest request)
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
                    _logger.Warn("Update EP Sign request is empty.");
                    return BadRequest(ElectronicSignatureProblemDetails.CreateBadRequestDetails("Update EP Sign request is empty"));
                }

                if (!ModelState.IsValid)
                {
                    _logger.Warn("EP Sign request ModelState is invalid.");
                    return BadRequest(ElectronicSignatureProblemDetails.CreateBadRequestDetails(ModelState));
                }

                var jsonRequest = JsonConvert.SerializeObject(request);
                _logger.Info($"REST WS EP Sign Msg: {jsonRequest}");

                _resultFile = $"EmployabilityPlan_{request.PinNumber}_{request.EpId}.pdf";

                var isUploaded = _employmentPlanDomain.AppendPdf(request.PinNumber.ToString(CultureInfo.InvariantCulture), request.EpId, request.FirstName, request.MiddleInitialName, request.LastName, DateTime.Parse(request.SignedDate), _resultFile);

                if (System.IO.File.Exists(_resultFile)) System.IO.File.Delete(_resultFile);

                if (!isUploaded)
                {
                    _logger.Warn($"The supplied EP Id '{request.EpId}' is not known to WWP");
                    throw new InvalidOperationException($"The supplied EP Id '{request.EpId}' is not known to WWP.");
                }
            }
            catch (InvalidOperationException e)
            {
                if (System.IO.File.Exists(_resultFile)) System.IO.File.Delete(_resultFile);
                _logger.ErrorException("Update EP Sign InvalidOperationException.", e);
                return StatusCode(400, ElectronicSignatureProblemDetails.CreateBadRequestDetails(e.Message));
            }
            catch (Exception e)
            {
                if (System.IO.File.Exists(_resultFile)) System.IO.File.Delete(_resultFile);
                _logger.ErrorException("Update EP Sign Exception.", e);
                return StatusCode(500, ElectronicSignatureProblemDetails.CreateServerErrorDetails(e.Message));
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

    public class EPElectronicSignatureRequest
    {
        public decimal PinNumber         { get; set; }
        public string  FirstName         { get; set; }
        public string  MiddleInitialName { get; set; }
        public string  LastName          { get; set; }
        public string  SuffixName        { get; set; }

        [Required(ErrorMessage = "Signed Date is not provided")]
        public string SignedDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EpId is not Provided")]
        public int EpId { get; set; }
    }

    #region Problem Details code taken from .NET Core 2.1 code... TODO: refactore this once we move to 2.1+

    /// <summary>
    /// A machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
    /// </summary>
    public class ElectronicSignatureProblemDetails
    {
        /// <summary>
        /// A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when
        /// dereferenced, it provide human-readable documentation for the problem type
        /// (e.g., using HTML [W3C.REC-html5-20141028]).  When this member is not present, its value is assumed to be
        /// "about:blank".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to occurrence
        /// of the problem, except for purposes of localization(e.g., using proactive content negotiation;
        /// see[RFC7231], Section 3.4).
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence of the problem.It may or may not yield further information if dereferenced.
        /// </summary>
        public string Instance { get; set; }

        public static ElectronicSignatureProblemDetails CreateServerErrorDetails(string detail)
        {
            return CreateProblemDetails("https://httpstatuses.com/500", "Internal Server Error", 500, detail, string.Empty);
        }

        public static ElectronicSignatureProblemDetails CreateBadRequestDetails(string detail)
        {
            return CreateProblemDetails("https://httpstatuses.com/400", "Bad Request", 400, detail, string.Empty);
        }

        public static ElectronicSignatureProblemDetails CreateBadRequestDetails(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            // This shoudl always be the case...
            if (modelState.Count > 0)
            {
                var detail = string.Join(" ", modelState.Keys.SelectMany(k => modelState[k].Errors.Select(e => $"{k} - {e.ErrorMessage}")).ToArray());

                return CreateProblemDetails("https://httpstatuses.com/400", "Bad Request", 400, detail, string.Empty);
            }

            if (modelState.IsValid)
                throw new Exception("CreateBadRequestDetails called in error.");

            // Handle a generic problem.
            return CreateProblemDetails("https://httpstatuses.com/400", "Bad Request", 400, "Model state is invalid", string.Empty);
        }

        private static ElectronicSignatureProblemDetails CreateProblemDetails(string type, string title, int? status, string detail, string instance)
        {
            return new ElectronicSignatureProblemDetails
                   {
                       Type     = type,
                       Title    = title,
                       Status   = status,
                       Detail   = detail,
                       Instance = instance
                   };
        }
    }

    #endregion
}
