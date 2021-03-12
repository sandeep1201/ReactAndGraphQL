using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/ia/family-barriers")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class FamilyBarriersController : BaseController
    {
        private readonly IAuthUser _authUser;

        public FamilyBarriersController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetSection(string pin)
        {
            try
            {
                var vm = new FamilyBarriersSectionViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

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
        public IActionResult PostSection(string pin, [FromBody] FamilyBarriersSectionContract model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new FamilyBarriersSectionViewModel(Repo, _authUser);
                    vm.InitializeFromPin(pin);
                    if (!vm.IsPinValid)
                        return BadRequest(new { error = "Pin is not valid" });

                    var hasupserted = vm.PostData(model, _authUser.Username);
                    if (hasupserted)
                    {
                        // When we update the data model and save it there are sometime
                        // issues getting the updated on the same context.  The work
                        // around is to reset the context before getting the data.
                        Repo.ResetContext();

                        // We will re-use the Get request to be sure we return the data
                        // in a consistent manner.
                        return GetSection(pin);
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
