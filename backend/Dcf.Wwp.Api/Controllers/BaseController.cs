using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

using Dcf.Wwp.Api.Core.Http;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Authorize]
    public class BaseController : Controller
    {
        protected IRepository Repo { get; }

        protected BaseController(IRepository repository)
        {
            Repo = repository;
        }

        [AllowAnonymous]
        public IActionResult HttpConflict()
        {
            return new HttpConflictResult();
        }

        protected object PrepareExceptionForResult(Exception ex)
        {
            return ex;
        }
    }
}