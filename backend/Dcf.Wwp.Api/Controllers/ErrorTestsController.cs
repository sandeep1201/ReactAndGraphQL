using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;


namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    public class ErrorTestsController : BaseController
    {
        public ErrorTestsController(IRepository repository) : base(repository)
        {
        }

        [HttpGet("")]
        [HttpGet("500")]
        [AllowAnonymous]
        public void Test500()
        {
            throw new DCFApplicationException();
        }

        [HttpGet("400")]
        [AllowAnonymous]
        public void Test400()
        {
            throw new DCFValidationException("Whoopsy!");
        }

        [HttpGet("401")]
        [AllowAnonymous]
        public void Test401()
        {
            throw new DCFAuthorizationException();
        }

        [HttpGet("403")]
        [AllowAnonymous]
        public void Test403()
        {
            throw new DCFAuthorizationException() { HttpStatusCode = HttpStatusCode.Forbidden };
        }

        [HttpGet("500f")]
        [AllowAnonymous]
        public void TestFriendly500()
        {
            throw new UserFriendlyException();
        }
    }
}
