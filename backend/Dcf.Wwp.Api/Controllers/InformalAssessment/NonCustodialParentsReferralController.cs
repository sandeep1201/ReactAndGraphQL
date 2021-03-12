using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/ia/non-custodial-parents-referral")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class NonCustodialParentsReferralController : BaseController
    {
        private readonly IAuthUser _authUser;

        public NonCustodialParentsReferralController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetSection(string pin)
        {
            var vm = new NonCustodialParentsReferralSectionViewModel(Repo, _authUser);
            vm.InitializeFromPin(pin);

            if (!vm.IsPinValid)
                throw new EntityNotFoundException("PIN is not valid");

            var data = vm.GetData();

            if (data == null)
                throw new EntityNotFoundException("Data not found");

            return Ok(data);
        }


        [ValidationResponseFilter]
        [HttpPost("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult PostSection(string pin, [FromBody] NonCustodialParentReferralAssessmentContract model)
        {
            if (!ModelState.IsValid || model == null) return BadRequest(ModelState);

            var vm = new NonCustodialParentsReferralSectionViewModel(Repo, _authUser);
            vm.InitializeFromPin(pin);
            if (!vm.IsPinValid)
                throw new EntityNotFoundException("PIN is not valid");

            var hasupserted = vm.PostData(model, _authUser.Username);

            if (!hasupserted) return HttpConflict();

            // When we update the data model and save it there are sometime
            // issues getting the updated on the same context.  The work
            // around is to reset the context before getting the data.
            Repo.ResetContext();

            // We will re-use the Get request to be sure we return the data
            // in a consistent manner.
            return GetSection(pin);
        }
    }
}
