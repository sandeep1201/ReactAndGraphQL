using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/ia/transportation")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class TransportationController : BaseController
    {
        private readonly IAuthUser _authUser;

        public TransportationController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetSection(string pin)
        {
            try
            {
                var vm = new TransportationSectionViewModel(Repo, _authUser);
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
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }


        [ValidationResponseFilter]
        [HttpPost("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult PostSection(string pin, [FromBody] TransportationSectionContract model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new TransportationSectionViewModel(Repo, _authUser);
                    vm.InitializeFromPin(pin);

                    // Check if we have everything we need.
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

                    return BadRequest(new { error = "Concurrency error." });
                }
                catch (InvalidOperationException ex)
                {
                    Response.StatusCode = 422;
                    return Json(ex);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
