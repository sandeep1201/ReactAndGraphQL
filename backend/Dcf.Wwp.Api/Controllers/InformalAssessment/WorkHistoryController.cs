using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/ia/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class WorkHistoryController : BaseController
    {
        private readonly IAuthUser _authUser;

        public WorkHistoryController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        [Authorize]
        public IActionResult GetSection(string pin)
        {
            try
            {
                var vm = new WorkHistorySectionViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid" });

                var data = vm.GetData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(PrepareExceptionForResult(ex));
            }
        }

        [ValidationResponseFilter]
        [HttpPost("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult PostSection(string pin, [FromBody] WorkHistorySectionContract model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new WorkHistorySectionViewModel(Repo, _authUser);
                    vm.InitializeFromPin(pin);

                    // Check if we have everything we need (a valid Pin and valid assessment to display).
                    if (!vm.IsPinValid)
                        return NoContent();

                    var hasupserted = vm.PostData(model, _authUser.Username);
                    if (hasupserted)
                    {
                        var data = vm.GetData();
                        return Ok(data);
                    }

                    return HttpConflict();
                }
                catch (Exception ex)
                {
                    return BadRequest(PrepareExceptionForResult(ex));
                }
            }

            return BadRequest(ModelState);
        }
    }
}
