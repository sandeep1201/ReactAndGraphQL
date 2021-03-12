using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.ActionFilters;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/client-registration")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ClientRegistrationController : BaseController
    {
        #region Properties

        private readonly IClientRegistrationViewModel _clientRegDomain; // Phase II
        private readonly IFinalistService             _finalistService;

        #endregion

        public ClientRegistrationController(IClientRegistrationViewModel clientRegDomain, IRepository repository, IFinalistService finalistService) : base(repository)
        {
            _clientRegDomain = clientRegDomain;
            _finalistService = finalistService;
        }

        [HttpPost("{pin}/finalist")]
        public IActionResult VerifyAddress([FromBody] FinalistAddressContract contract)
        {
            IActionResult res;

            if (contract == null)
            {
                res = BadRequest();
            }
            else
            {
                var data = _finalistService.GetAnalyzeAddress(contract);
                res = Ok(data);
            }

            return (res);
        }

        [HttpPost]
        public IActionResult UpsertClientRegistration([FromBody] ClientRegistrationContract contract)
        {
            IActionResult res = null;

            if (contract == null)
            {
                res = BadRequest();
            }
            else
            {
                try
                {
                    var data = _clientRegDomain.UpsertClientRegistration(contract);
                    res = Ok(data);
                }
                catch (UserFriendlyException ex)
                {
                    var statusContract = new StatusContract();
                    statusContract.ErrorMessages.Add(ex.Message);
                    res = Ok(statusContract);
                }
            }

            return (res);
        }

        [HttpPost("{mciId}")]
        public IActionResult GetClientReg(long mciId, [FromBody] DemographicResultContract contract)
        {
            var data = _clientRegDomain.GetClientRegistration(mciId, contract);

            return Ok(data);
        }
    }
}
