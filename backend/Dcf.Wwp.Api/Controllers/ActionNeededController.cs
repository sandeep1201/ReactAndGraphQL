using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/action-needed")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ActionNeededController : BaseController
    {
        private readonly IAuthUser _authUser;

        public ActionNeededController(IRepository repository, IAuthUser authUser) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetAllActionNeededForPin(string pin)
        {
            try
            {
                var vm = new ActionNeededViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetActionNeeded();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{pin}/{page}")]
        [EnableCors("AllowAll")]
        public IActionResult GetActionNeededForPinAndPage(string pin, string page)
        {
            try
            {
                var vm = new ActionNeededViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                // HACK: some kind of issue in front end... this is a temp fix.
                if (page == "child-and-youth-supports") page = "child-youth-supports";

                var data = vm.GetActionNeededForPage(page);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{pin}/{page}")]
        [EnableCors("AllowAll")]
        public IActionResult PostActionNeeded(string pin, string page, [FromBody] ActionNeededContract contract)
        {
            try
            {
                var vm = new ActionNeededViewModel(Repo, _authUser);

                vm.InitializeFromPin(pin);
                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                // HACK: some kind of issue in front end... this is a temp fix.
                if (page == "child-and-youth-supports") page = "child-youth-supports";

                vm.UpsertActionNeeded(page, contract, _authUser.Username);

                // When we update the data model and save it there are sometime
                // issues getting the updated on the same context.  The work
                // around is to reset the context before getting the data.
                Repo.ResetContext();

                // We will re-use the Get request to be sure we return the data
                // in a consistent manner.
                return GetActionNeededForPinAndPage(pin, page);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

    [Route("api/action-needed-task")]
    [EnableCors("AllowAll")]
    public class ActionNeededTaskController : BaseController
    {
        private readonly IAuthUser _authUser;

        public ActionNeededTaskController(IRepository repository, IAuthUser authUser) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}/{taskId}")]
        [EnableCors("AllowAll")]
        public IActionResult GetActionNeededTask(string pin, int taskId)
        {
            try
            {
                var vm = new ActionNeededViewModel(Repo, _authUser);

                var data = vm.GetActionNeededTaskForPinById(pin, taskId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult PostActionNeededTask(string pin, [FromBody] ActionNeededTaskContract contract)
        {
            try
            {
                var vm = new ActionNeededViewModel(Repo, _authUser);

                var taskId = vm.UpsertActionNeededTask(pin, contract, _authUser.Username);

                // When we update the data model and save it there are sometime
                // issues getting the updated on the same context.  The work
                // around is to reset the context before getting the data.
                Repo.ResetContext();

                // We will re-use the Get request to be sure we return the data
                // in a consistent manner.
                return GetActionNeededTask(pin, taskId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{pin}/{taskId}")]
        [EnableCors("AllowAll")]
        public IActionResult DeleteActionNeededTask(string pin, int taskId)
        {
            try
            {
                var vm = new ActionNeededViewModel(Repo, _authUser);

                vm.DeleteActionNeededTask(pin, taskId, _authUser.Username);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
