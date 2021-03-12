using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.ActionFilters;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class AgencyController : BaseController
    {
        private readonly IAuthUser _authUser;

        public AgencyController(IRepository repo, IAuthUser authUser) : base(repo)
        {
            _authUser = authUser;
        }

        [HttpGet("{agencycode}/program-workers/{programCode?}")]
        public IActionResult GetWorkersByAgency(string agencyCode, string programCode)
        {
            if (string.IsNullOrEmpty(agencyCode))
                throw new EntityNotFoundException("Agency Code is not provided");

            var agencyVm = new AgencyViewModel(Repo, _authUser);
            var data     = agencyVm.GetWorkersByAgencyCode(agencyCode, programCode);

            if (data == null)
                throw new EntityNotFoundException("Something went wrong");

            return Ok(data);
        }

        [HttpGet("{agencycode}/workers/{authcode}")]
        public IActionResult CheckEnrollmentAuthorization(string agencyCode, string authcode)
        {
            if (String.IsNullOrEmpty(agencyCode))
                throw new EntityNotFoundException("Agency Code is not provided");

            var agencyVm = new AgencyViewModel(Repo, _authUser);
            var data     = agencyVm.GetWorkersByAuthorization(agencyCode, authcode);

            if (data == null)
                throw new EntityNotFoundException("Couldn't access data");

            return Ok(data);
        }

        [HttpGet("{officeNumber}/transfer-destinations")]
        public IActionResult TransferDestinations(string officeNumber)
        {
            if (String.IsNullOrEmpty(officeNumber))
                throw new EntityNotFoundException("Office Number is not provided");

            var vm   = new AgencyViewModel(Repo, _authUser);
            var data = vm.GetTransferDestinations(officeNumber);

            return Ok(data);
        }


        [HttpGet("my-offices")]
        public IActionResult GetOfficesWithAccess()
        {
            var vm   = new AgencyViewModel(Repo, _authUser);
            var data = vm.GetOfficesWithAccess(_authUser.Username);

            return Ok(data);
        }

        [HttpGet("agencies")]
        public IActionResult GetAgencies()
        {
            var vm   = new AgencyViewModel(Repo, _authUser);
            var data = vm.GetAgencies();

            return Ok(data);
        }
    }
}
