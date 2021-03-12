using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels.ParticipantBarrierApp;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/participant-barriers")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ParticipantBarriersController : BaseController
    {
        #region Properties

        private readonly IAuthUser          _authUser;
        private readonly ITransactionDomain _transactionDomain;

        #endregion

        #region Methods

        public ParticipantBarriersController(IAuthUser authUser, IRepository repository, ITransactionDomain transactionDomain) : base(repository)
        {
            _authUser = authUser;
            _transactionDomain = transactionDomain;
        }

        [HttpGet("{pin}")]
        public IActionResult ParticipantBarriers(string pin)
        {
            try
            {
                var vm = new ParticipantBarrierDetailViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetParticipantBarrierLists();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        [HttpGet("{pin}/{id}")]
        public IActionResult ParticipantBarrierById(string pin, int id)
        {
            try
            {
                var vm = new ParticipantBarrierDetailViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid) return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetParticipantBarrierInfo(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        [HttpPost("{pin}/{id}"), ValidationResponseFilter]
        public IActionResult PostBarrierDetailInfo(string pin, int id, [FromBody] BarrierDetailContract model)
        {
            if (!_authUser.IsAuthenticated) return Unauthorized();

            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new ParticipantBarrierDetailViewModel(Repo, _authUser, _transactionDomain);
                    vm.InitializeFromPin(pin);

                    // Check if we have everything we need (a 
                    // valid Pin and valid assessment to display).
                    if (!vm.IsPinValid)
                    {
                        return BadRequest(new { error = "PIN is not valid." });
                    }

                    var response = vm.UpsertData(model, id, pin, _authUser.Username);

                    return (response.HasConcurrencyError ? HttpConflict() : Ok());
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("delete/{pin}/{id}")]
        public IActionResult DeleteParticipantBarrierDetail(string pin, int id)
        {
            if (!_authUser.IsAuthenticated) return Unauthorized();

            try
            {
                var vm = new ParticipantBarrierDetailViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                var didDelete = vm.DeleteParticipantBarrier(id, _authUser.Username);
                if (didDelete)
                    return Ok(); // 200
                else
                    return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        #endregion
    }
}
