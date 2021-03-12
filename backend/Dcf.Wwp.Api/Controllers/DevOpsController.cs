using System;
using Dcf.Wwp.ConnectedServices.Logging;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [AllowAnonymous]
    public class DevOpsController : BaseController
    {
        private readonly IAuthUser _authUser;

        public DevOpsController(IAuthUser authUser, IRepository repository) : base(repository)
        {
            _authUser = authUser;
        }

        [HttpGet("status")]
        [EnableCors("AllowAll")]
        public IActionResult GetStatus()
        {
            var status = new { version = Program.Version.ToString(), dbServer = Repo.Server, dbInstance = Repo.Database, dbUser = Repo.UserId, authUser = _authUser.Username };
            return Ok(status);
        }

        //[HttpGet("db")]
        //[EnableCors("AllowAll")]
        //public IActionResult GetDbInfo()
        //{
        //    var status = new { server = Repo.Server, instance = Repo.Database, user = Repo.UserId, auth = Repo.Pass };
        //    return Ok(status);
        //}

        [HttpGet("loglevel/{logEventLevel?}")]
        [EnableCors("AllowAll")]
        public IActionResult SetLogLevel(LogEventLevel logEventLevel = LogEventLevel.Debug)
        {
            Startup.LevelSwitch.MinimumLevel = logEventLevel;
            return Ok(new { message = $"Log Level changed to {logEventLevel} ({(int) logEventLevel}) " });
        }

        [HttpGet("log/setupinfo/{flush?}")]
        public IActionResult SetLogLevel(bool flush = false)
        {
            var data = Startup.sb.ToString();

            if (flush)
            {
                Startup.sb.Clear();
            }

            return Ok(new { message = data });
        }

        [HttpGet("throw")]
        [EnableCors("AllowAll")]
        public IActionResult ThrowExeption(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Throwing a test exception.";
            }

            throw new Exception(message);
        }

        [HttpGet("soaploglevel/{newLevel}")]
        [EnableCors("AllowAll")]
        public IActionResult SetSoapLogLevel(string newLevel)
        {
            var currentLevel = WebServicesLogger.GetLevel();
            WebServicesLogger.SetLevel(newLevel);

            return Ok(new { message = $"WCF SOAP Log Level changed from {currentLevel} to {newLevel.ToUpper()}" });
        }
    }
}
