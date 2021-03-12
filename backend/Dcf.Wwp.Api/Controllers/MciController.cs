using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class MciController : Controller
    {
        #region Properties

        private readonly IClientRegistrationViewModel _clientRegDomain;

        #endregion

        #region Methods

        public MciController (IClientRegistrationViewModel clientRegDomain)
        {
            _clientRegDomain = clientRegDomain;
        }

        [HttpPost("search")]
        public IActionResult MciSearch([FromBody] DemographicSearchContract contract)
        {
            var matchData = _clientRegDomain.GetClearanceSearchResults(contract); // Phase II

            //var crVm = new ClientRegistrationViewModel(_mciSvc, _cwwIndSvc, _cwwKeySvc, _googleApi, _repository, _authUser);
            //var matches = crVm.GetClearanceSearchResults(contract);

            return Ok(matchData);
        }

        #endregion
    }
}
