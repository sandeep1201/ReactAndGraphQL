using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    //[Route("api/ia/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class InformalAssessmentController : BaseController
    {
        private readonly IAuthUser          _authUser;
        private readonly ITransactionDomain _transactionDomain;

        public InformalAssessmentController(IRepository repository, IAuthUser authUser, ITransactionDomain transactionDomain) : base(repository)
        {
            _authUser          = authUser;
            _transactionDomain = transactionDomain;
        }

        [HttpGet("api/ia/{pin}")]
        [EnableCors("AllowAll")]
        [Authorize]
        public IActionResult GetAssessment(string pin)
        {
            try
            {
                var vm = new InformalAssessmentViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid" });

                var data = vm.GetAssessment();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(PrepareExceptionForResult(ex));
            }
        }

        [HttpPost("api/ia/new/{pin}")]
        [EnableCors("AllowAll")]
        [Authorize]
        public IActionResult CreateNewAssessment(string pin)
        {
            var vm = new InformalAssessmentViewModel(Repo, _authUser, _transactionDomain);
            vm.InitializeFromPin(pin);
            if (!vm.IsPinValid)
                return BadRequest(new { error = "PIN is not valid" });
            var data = vm.NewAssessment();

            return Ok(data);
        }


        [HttpPost("api/ia/submit/{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult SubmitAssessment(string pin)
        {
            if (!_authUser.IsAuthenticated)
                return Unauthorized();

            try
            {
                var vm = new InformalAssessmentViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);
                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid" });
                var data = vm.SubmitAssessment();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(PrepareExceptionForResult(ex));
            }
        }
    }
}
