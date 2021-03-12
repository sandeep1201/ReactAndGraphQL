using System;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.ViewModels.History;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class HelpController : BaseController
    {
        private readonly IAuthUser _authUser;

        public HelpController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{feature}")]
        [EnableCors("AllowAll")]
        public IActionResult GetFeatureUrl(string feature)
        {
            try
            {
                var data = Repo.GetFeatureUrl(feature);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = $"Something unexpected happened. {e}" });
            }
        }
    }
}
