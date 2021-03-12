using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/ia/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class WorkProgramController : BaseController
    {
        #region MyRegion

        private readonly IAuthUser  _authUser;
        private readonly IGoogleApi _googleApi;

        #endregion

        #region Methods

        public WorkProgramController(IGoogleApi googleApi, IAuthUser authUser, IRepository repository) : base(repository)
        {
            _googleApi = googleApi;
            _authUser  = authUser;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetSection(string pin)
        {
            try
            {
                var vm = new WorkProgramSectionViewModel(_googleApi, Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                {
                    return BadRequest(new { error = "PIN is not valid." });
                }

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
        public IActionResult PostSection(string pin, [FromBody] WorkProgramSectionContract model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new WorkProgramSectionViewModel(_googleApi, Repo, _authUser);
                    vm.InitializeFromPin(pin);

                    // Check if we have everything we need (a valid Pin and valid assessment to display).
                    if (!vm.IsPinValid)
                    {
                        return NoContent();
                    }

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

        #endregion
    }
}
