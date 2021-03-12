using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/drug-screening")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class DrugScreeningController : Controller
    {
        #region Properties

        private readonly IDrugScreeningDomain _drugScreeningDomain;

        #endregion

        #region Methods

        public DrugScreeningController(IDrugScreeningDomain drugScreeningDomain)
        {
            _drugScreeningDomain = drugScreeningDomain;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("get/{pin}")]
        public IActionResult GetDrugScreeningForPin(string pin)
        {
            var contract = _drugScreeningDomain.GetDrugScreeningForPin(decimal.Parse(pin));
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("post/{pin}")]
        public IActionResult UpsertDrugScreening([FromBody] DrugScreeningContract drugScreeningContract, string pin)
        {
            var contract = _drugScreeningDomain.UpsertDrugScreening(drugScreeningContract, pin);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion
    }
}
