using System;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Api.Library.ViewModels.ContactsApp;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/contacts")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class WorkerContactInfoController : Controller
    {
        #region Properties

        private readonly IWorkerContactInfoDomain _workerContactInfoDomain;

        #endregion

        #region Methods

        public WorkerContactInfoController(IWorkerContactInfoDomain workerContactInfoDomain)
        {
            _workerContactInfoDomain = workerContactInfoDomain;
        }

        [HttpGet("worker")]
        public IActionResult WorkerContactById()
        {
            try
            {
                var contract = _workerContactInfoDomain.GetContactInfo();
                var res = Ok(contract);

                return (res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        [HttpPost("contactInfo")]
        public IActionResult UpsertContactInformation([FromBody] WorkerInfoContract workerInfoContract)
        {
            var contract = _workerContactInfoDomain.UpsertIContactInformation(workerInfoContract);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion
    }
}
