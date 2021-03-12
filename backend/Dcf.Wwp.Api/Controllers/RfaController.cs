using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.DataAccess.Contexts;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class RfaController : BaseController
    {
        private readonly IAuthUser          _authUser;
        private readonly EPContext          _referenceDataContext;
        private readonly ITransactionDomain _transactionDomain;

        public RfaController(IRepository repository, IAuthUser authUser, EPContext referenceDataContext, ITransactionDomain transactionDomain) : base(repository)
        {
            _authUser             = authUser;
            _referenceDataContext = referenceDataContext;
            _transactionDomain    = transactionDomain;
        }

        [HttpGet("api/pin/{pin}/rfa/{id}")]
        [EnableCors("AllowAll")]
        public IActionResult GetRfa(string pin, int id)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var data = vm.GetRfa(pin, id);

            if (data == null)
            {
                //throw new EntityNotFoundException("Data not found");
                return BadRequest("Data not found");
            }

            return Ok(data);
        }

        [ValidationResponseFilter]
        [HttpPost("api/pin/{pin}/rfa/{id}")]
        [EnableCors("AllowAll")]
        public IActionResult PostRfa(string pin, int id, [FromBody] RequestForAssistanceContract contract)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            // We only need to do a pre-check when the RFA is new.
            if (contract.IsNew())
            {
                var data = vm.PreCheckRulesEngineBased(pin, contract);

                if (data.Errors.Any() || data.Warnings.Any())
                {
                    return BadRequest(data);
                }
            }

            var hasUpserted = vm.PostRfa(contract, ref id);

            if (!hasUpserted) return HttpConflict();

            // When we update the data model and save it there are sometime
            // issues getting the updated on the same context.  The work
            // around is to reset the context before getting the data.
            Repo.ResetContext();

            // We will re-use the Get request to be sure we return the data
            // in a consistent manner.
            return GetRfa(pin, id);
        }

        [ValidationResponseFilter]
        [HttpPut("api/pin/{pin}/rfa/{id}/{status}")]
        [EnableCors("AllowAll")]
        public IActionResult SetRfaStatus(string pin, int id, string status)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            vm.ChangeRfaStatus(pin, id, status);

            return GetRfa(pin, id);
        }

        [ValidationResponseFilter]
        [HttpPost("api/pin/{pin}/rfa/{id}/eligibility")]
        [EnableCors("AllowAll")]
        public IActionResult DetermineEligibility(string pin, int id, [FromBody] RequestForAssistanceContract model)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var result = vm.DetermineEligibility(id, model, _authUser);

            return Ok(result);
        }

        [HttpGet("api/pin/{pin}/rfa")]
        [EnableCors("AllowAll")]
        public IActionResult RfasForPin(string pin)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var data = vm.RequestForAssistanceSummariesForParticipant(pin);

            return Ok(data);
        }

        [HttpGet("api/pin/{pin}/sortedrfa")]
        [EnableCors("AllowAll")]
        public IActionResult SortedRfasForPin(string pin)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var data = vm.RequestForAssistanceSummariesForParticipant(pin).OrderByDescending(i => i.StatusDate).ThenBy(i => i.StatusId);

            return Ok(data);
        }

        [HttpGet("api/pin/{pin}/oldrfas")]
        [EnableCors("AllowAll")]
        public IActionResult OldRfasForPin(string pin)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var data = vm.RequestForAssistanceOldSummariesForParticipant(pin);
            return Ok(data);
        }

        [HttpPost("api/pin/{pin}/rfa/precheck")]
        [EnableCors("AllowAll")]
        public IActionResult PreCheck(string pin, [FromBody] RequestForAssistanceContract model)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var data = vm.PreCheckRulesEngineBased(pin, model);

            return Ok(data);
        }

        [HttpPost("api/pin/{pin}/rfa/validate/{rule}")]
        [EnableCors("AllowAll")]
        public IActionResult Validate(string pin, string rule, [FromBody] RequestForAssistanceContract model)
        {
            var vm = new RequestForAssistanceViewModel(Repo, _authUser, _referenceDataContext, _transactionDomain);
            vm.InitializeFromPin(pin);

            var data = vm.Validate(pin, rule, model);

            return Ok(data);
        }
    }
}
