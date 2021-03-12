using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [AllowAnonymous]
    public class LogController : BaseController
    {
        private readonly ILogger _logger;

        public LogController(IRepository repository) : base(repository)
        {
            _logger = Log.Logger.ForContext<LogController>();
        }

        [EnableCors("AllowAll")]
        [HttpPost]
        public IActionResult AddLog([FromBody] LogContract contract)
        {
            _logger.Write(contract.Level, contract.Message);

            return Ok();
        }
    }
}