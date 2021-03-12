using System;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.TimeLimits
{
    [Route("api/[controller]")]
    public class AssistanceGroupController : BaseController
    {
        private IAuthUser _authUser;

        public AssistanceGroupController(IRepository repository, IAuthUser authUser) : base(repository)
        {
            this._authUser = authUser;
        }

        [HttpGet("{pin}")]
        public IActionResult GetAssistanceGroup(string pin)
        {
                var vm = new AssistanceGroupViewModel(this.Repo, this._authUser);
                var data = vm.GetParticipantAssistanceGroupByPin(pin);

                return this.Ok(data);
        }

        
    }
}