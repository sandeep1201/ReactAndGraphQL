using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/pin-comment")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class PinCommentController : Controller
    {
        #region Properties

        private readonly IPinCommentDomain _pinCommentDomain;

        #endregion

        #region Methods

        public PinCommentController(IPinCommentDomain pinCommentDomain)
        {
            _pinCommentDomain = pinCommentDomain;
        }

        [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("pin/{pin}")]
        public IActionResult GetPinComments(string pin)
        {
            var contract = _pinCommentDomain.GetPinComments(decimal.Parse(pin));
            var res      = Ok(contract);

            return (res);
        }

        // [ServiceFilter(typeof(ValidPinMustExistAttribute))]
        [HttpGet("pin/{pin}/{id}")]
        public IActionResult GetPinComment(int id)
        {
            var contract = _pinCommentDomain.GetPinComment(id);
            var res      = Ok(contract);

            return (res);
        }

        [HttpPost("pin/{pin}/{id}")]
        public IActionResult UpsertPinComment([FromBody] CommentContract pinCommentContract, string pin, int id)
        {
            var contract = _pinCommentDomain.UpsertPinComment(pinCommentContract, pin, id);
            var res      = Ok(contract);

            return Ok(res);
        }

        #endregion
    }
}
