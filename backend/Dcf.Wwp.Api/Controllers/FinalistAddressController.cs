using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/finalist-address")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class FinalistAddress : BaseController
    {
        #region Properties
        private readonly IFinalistService _finalistService;

        #endregion

        public FinalistAddress(IRepository repository, IFinalistService finalistService) : base(repository)
        {
            _finalistService = finalistService;
        }

        [HttpPost("finalist")]
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

    }
}
