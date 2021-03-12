using System;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.ViewModels.History;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class HistoryController : BaseController
    {
        private readonly IAuthUser _authUser;

        public HistoryController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("ia/{section}/{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetAssessmentHistory(string section, string pin)
        {
            var vm = new AssessmentHistoryViewModel(Repo, _authUser);
            vm.InitializeFromPin(pin);

            if (!vm.IsPinValid)
                return BadRequest(new { error = "PIN is not valid." });

            var data = vm.GetSectionHistory(section, pin, id: null);
            return Ok(data);
        }

        [HttpGet("app/{pin}/{section}/{id}")]
        [EnableCors("AllowAll")]
        public IActionResult GetAppAssessmentHistory(string section, string pin, int? id)
        {
            var vm = new AssessmentHistoryViewModel(Repo, _authUser);
            vm.InitializeFromPin(pin);

            if (!vm.IsPinValid)
                return BadRequest(new { error = "PIN is not valid." });

            var data = vm.GetAppSectionHistory(section, pin: null, id: id);
            return Ok(data);
        }
    }
}
