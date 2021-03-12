using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers
{
    /// <summary>
    /// The OfficeController class contains the API for all things Office Structure related including
    /// the Organizations, Contract Areas and Workers.
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class OfficeController : BaseController
    {
        private readonly IAuthUser _authUser;

        public OfficeController(IRepository repo, IAuthUser authUser) : base(repo)
        {
            _authUser = authUser;
        }

        [HttpGet("{programCode}/{wiuid}")]
        public IActionResult GetOfficesForProgramAndWorker(string programCode, string wiuid)
        {
            var vm   = new OfficeViewModel(Repo, _authUser);
            var data = vm.GetOfficesForProgramAndWorker(programCode, wiuid);

            return Ok(data);
        }

        [HttpGet("program/{programCode}/county/{countyOrTribeId}")]
        public IActionResult GetOfficeForProgramAndCounty(string programCode, int countyOrTribeId)
        {
            var vm   = new OfficeViewModel(Repo, _authUser);
            var data = vm.GetOfficeByCountyAndProgramCode(countyOrTribeId, programCode);

            if (data == null)
                return NoContent();

            return Ok(data);
        }

        [HttpGet("{programCode}/{workerMfId}/{sourceOfficeNumber}")]
        public IActionResult GetTransferOfficesByProgramWorkerSourceOffice(string programCode, string workerMfId, int sourceOfficeNumber)
        {
            var vm   = new OfficeViewModel(Repo, _authUser);
            var data = vm.GetTransferOfficesByProgramWorkerSourceOffice(programCode, workerMfId, sourceOfficeNumber);

            return Ok(data);
        }
    }
}
