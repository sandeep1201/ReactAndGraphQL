using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/org-info")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class OrganizationInformationController : Controller
    {
        #region Properties

        private readonly IOrganizationInformationDomain _organizationInformationDomain;

        #endregion

        #region Methods

        public OrganizationInformationController(IOrganizationInformationDomain organizationInformationDomain)
        {
            _organizationInformationDomain = organizationInformationDomain;
        }

        [HttpGet("{progId}/{orgId}")]
        public IActionResult GetIOrganizationInformation(int progId, int orgId)
        {
            var contract = _organizationInformationDomain.GetIOrganizationInformation(progId, orgId);
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("{id}")]
        public IActionResult UpsertOrganizationInformation([FromBody] OrganizationInformationContract organizationInformationContract, int id)
        {
            var contract = _organizationInformationDomain.UpsertIOrganizationInformation(organizationInformationContract, id);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion
    }
}
