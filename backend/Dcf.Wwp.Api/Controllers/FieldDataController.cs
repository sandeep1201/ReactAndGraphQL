using System;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.DataAccess.Contexts;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class FieldDataController : BaseController
    {
        private readonly IAuthUser _authUser;
        private readonly EPContext _referenceDataContext;

        public FieldDataController(IRepository repository, IAuthUser authUser, EPContext referenceDataContext) : base(repository)
        {
            _authUser             = authUser;
            _referenceDataContext = referenceDataContext;
        }

        [HttpGet("{fieldName}/{options?}/{subOptions?}")]
        [EnableCors("AllowAll")]
        public IActionResult GetData(string fieldName, string options, string subOptions)
        {
            try
            {
                var fd   = new ReferenceDataViewModel(Repo, _authUser, _referenceDataContext);
                var data = fd.GetData2(fieldName, options, subOptions);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("delete-reasons/{repeater}")]
        [EnableCors("AllowAll")]
        public IActionResult GetDeleteReasons(string repeater)
        {
            try
            {
                var fd   = new ReferenceDataViewModel(Repo, _authUser, _referenceDataContext);
                var data = fd.GetDeleteReasons(repeater);

                return Ok(data);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 422;
                return Json(ex);
            }
        }
    }
}
