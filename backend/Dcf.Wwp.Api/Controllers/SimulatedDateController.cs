using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class SimulatedDateController : Controller
    {
        #region Properties

        private readonly ISimulatedDateDomain _simulatedDateDomain;

        #endregion

        #region Methods

        public SimulatedDateController(ISimulatedDateDomain simulatedDateDomain)
        {
            _simulatedDateDomain = simulatedDateDomain;
        }

        [HttpGet("api/current-date-override/log/{id}")]
        public IActionResult GetSimulatedDate(int id)
        {
            var contract = _simulatedDateDomain.GetSimulatedDate(id);
            var res      = Ok(contract);

            return res;
        }

        [HttpPost("api/current-date-override/log")]
        public IActionResult UpsertSimulatedDate([FromBody] SimulatedDateContract simulatedDateContract)
        {
            var contract = _simulatedDateDomain.UpsertSimulateDate(simulatedDateContract);
            var res      = Ok(contract);

            return res;
        }

        #endregion
    }
}
