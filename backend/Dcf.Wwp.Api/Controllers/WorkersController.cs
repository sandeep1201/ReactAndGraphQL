using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class WorkersController : BaseController
    {
        private readonly IAuthUser _authUser;

        public WorkersController(IRepository repo, IAuthUser authUser) : base(repo)
        {
            _authUser = authUser;
        }

        [HttpGet("{orgCode}/{roleCode?}")]
        public IActionResult GetWorkersForOrganization(string orgCode, string roleCode)
        {
            var vm   = new WorkersViewModel(Repo, _authUser);
            var data = string.IsNullOrWhiteSpace(roleCode) ? vm.GetWorkersForOrganization(orgCode) : vm.GetWorkersForOrganization(orgCode, roleCode);

            return Ok(data);
        }
    }
}
