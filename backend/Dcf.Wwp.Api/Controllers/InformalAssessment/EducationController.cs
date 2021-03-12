using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Interfaces;
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
    public class EducationController : BaseController
    {
        #region Properties

        private readonly IAuthUser          _authUser;
        private readonly IGoogleApi         _googleApi;
        private readonly ITransactionDomain _transactionDomain;

        #endregion

        #region Methods

        public EducationController(IGoogleApi googleApi, IAuthUser authUser, IRepository repository, ITransactionDomain transactionDomain) : base(repository)
        {
            _googleApi         = googleApi;
            _authUser          = authUser;
            _transactionDomain = transactionDomain;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetSection(string pin)
        {
            try
            {
                var vm = new EducationSectionViewModel(_googleApi, Repo, _authUser, _transactionDomain);
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
        public IActionResult PostSection(string pin, [FromBody] EducationHistorySectionContract model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new EducationSectionViewModel(_googleApi, Repo, _authUser, _transactionDomain);
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

        #endregion
    }
}
