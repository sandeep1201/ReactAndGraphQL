using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels.WorkHistoryApp;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/employments")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class EmploymentController : BaseController
    {
        #region Properties

        private readonly IAuthUser                _authUser;
        private readonly IGoogleApi               _googleApi;
        private readonly IWeeklyHoursWorkedDomain _weeklyHoursWorkedDomain;
        private readonly ITransactionDomain       _transactionDomain;

        #endregion

        #region Methods

        public EmploymentController(IGoogleApi googleApi, IAuthUser authUser, IRepository repository, IWeeklyHoursWorkedDomain weeklyHoursWorkedDimain, ITransactionDomain transactionDomain) : base(repository)
        {
            _googleApi               = googleApi;
            _authUser                = authUser;
            _weeklyHoursWorkedDomain = weeklyHoursWorkedDimain;
            _transactionDomain       = transactionDomain;
        }

        [HttpGet("{pin}")]
        public IActionResult GetParticipantEmployments(string pin)
        {
            var vm = new EmploymentHistoryViewModel(_googleApi, Repo, _authUser, _weeklyHoursWorkedDomain, _transactionDomain);
            vm.InitializeFromPin(pin);

            if (!vm.IsPinValid)
                return BadRequest(new { error = "PIN is not valid." });

            var data = vm.GetParticipantEmployments();

            return Ok(data);
        }

        [HttpGet("{pin}/{id}")]
        public IActionResult EmploymentById(string pin, int id)
        {
            var vm = new EmploymentHistoryViewModel(_googleApi, Repo, _authUser, _weeklyHoursWorkedDomain, _transactionDomain);
            vm.InitializeFromPin(pin);

            if (!vm.IsPinValid)
                return BadRequest(new { error = "PIN is not valid." });

            var data = vm.GetParticipantEmploymentInfo(id);

            return Ok(data);
        }

        [HttpPost("{pin}/{id}"), ValidationResponseFilter]
        public IActionResult PostEmploymentInfo(string pin, int id, [FromBody] EmploymentInfoContract model)
        {
            if (!_authUser.IsAuthenticated)
                return Unauthorized();

            if (ModelState.IsValid && model != null)
            {
                var vm = new EmploymentHistoryViewModel(_googleApi, Repo, _authUser, _weeklyHoursWorkedDomain, _transactionDomain);
                vm.InitializeFromPin(pin);

                // Check if we have everything we need (a valid Pin and valid assessment to display).
                if (!vm.IsPinValid)
                {
                    return BadRequest(new { error = "PIN is not valid." });
                }

                var response = vm.UpsertData(model, id, pin, _authUser.Username);

                return response.HasConcurrencyError ? HttpConflict() : Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{pin}/predelete/{id}")]
        public IActionResult PreDeleteCheck(string pin, int id)
        {
            var vm = new EmploymentHistoryViewModel(_googleApi, Repo, _authUser, _weeklyHoursWorkedDomain, _transactionDomain);
            vm.InitializeFromPin(pin);
            var data = vm.PreDeleteCheck(pin, id);

            return Ok(data);
        }

        [HttpPost("{pin}/preAdd/{id}/{isHD}")]
        public IActionResult PreAddCheck(string pin, bool isHD, [FromBody] EmploymentInfoContract model)
        {
            var vm = new EmploymentHistoryViewModel(_googleApi, Repo, _authUser, _weeklyHoursWorkedDomain, _transactionDomain);
            vm.InitializeFromPin(pin);
            var data = vm.PreAddCheck(pin, isHD, model);

            return Ok(data);
        }


        [HttpDelete("delete/{pin}/{id}/{deleteReasonId}")]
        public IActionResult DeleteEmployment(string pin, int id, int deleteReasonId)
        {
            if (!_authUser.IsAuthenticated)
                return Unauthorized();

            try
            {
                var vm = new EmploymentHistoryViewModel(_googleApi, Repo, _authUser, _weeklyHoursWorkedDomain, _transactionDomain);
                vm.InitializeFromPin(pin);

                var didDelete = vm.DeleteEmployment(id, deleteReasonId, _authUser.Username);
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
