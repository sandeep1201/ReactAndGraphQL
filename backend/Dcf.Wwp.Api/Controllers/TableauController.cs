using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class TableauController : Controller
    {
        #region Properties

        private readonly ITableauApi _tableauApi;

        #endregion

        #region Methods

        public TableauController(ITableauApi tableauApi)
        {
            _tableauApi = tableauApi;
        }

        [HttpGet]
        [Route("trustedticket")]
        public IActionResult GetTrustedTicket()
        {
            var data = _tableauApi.GetTrustedTicket();

            return Ok(data);
        }

        #endregion
    }
}
