using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class PepController : BaseController
    {
        private readonly IAuthUser _authUser;

        public PepController(IRepository repository, IAuthUser authUser) : base(repository)
        {
            _authUser = authUser;
        }


        [HttpGet("{pin}/program/{programCode?}")]
        [EnableCors("AllowAll")]
        public IActionResult GetParticipantEnrolledPrograms(string pin, string programCode)
        {
            var pepVm = new PepViewModel(Repo);
            var data  = pepVm.GetPepsByProgram(pin, programCode);

            return Ok(data);
        }
    }
}
