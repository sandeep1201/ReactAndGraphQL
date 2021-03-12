using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [AllowAnonymous]
    public class DevOps2Controller : Controller
    {
        private readonly IDevOpsDomain _devOpsDomain;
        private readonly IAuthUser     _authUser;

        public DevOps2Controller(IDevOpsDomain devOpsDomain, IAuthUser authUser)
        {
            _devOpsDomain = devOpsDomain;
            _authUser     = authUser;
        }

        [HttpGet("status2")]
        public IActionResult GetStatus2()
        {
            var res = _devOpsDomain.GetStatus2();

            return Ok(res);
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var model = _devOpsDomain.GetStatus();

            var res = new
                      {
                          model.AppVersion,
                          model.DbServer,
                          model.DbInstance,
                          DbUserId        = model.DbUser,
                          LogSerilogLevel = model.LogLevelApp,
                          LogWCFLevel     = model.LogLevelWcf,
                          AuthUser        = _authUser.Username,
                          MFUserId        = _authUser.MainFrameId,
                      };

            return Ok(res);
        }

        [HttpGet("dbStatus")]
        public IActionResult GetDbInfo()
        {
            var model = _devOpsDomain.GetDbInfo();

            var res = new
                      {
                          model.DbServer,
                          model.DbDatabase,
                          DbUserId    = model.DbUser,
                          DbUserPass  = "".PadRight(10, '*'), //TODO: looks good ~ lol
                          MaxPoolSize = model.MaxPoolSize,
                          Timeout     = model.Timeout
                      };

            return Ok(res);
        }

        [HttpGet("appLogLevel")]
        public IActionResult GetAppLogLevel()
        {
            var model = _devOpsDomain.GetAppLogLevel();
            var res   = new { message = $"App Log Level set to {model} ({(int) model}) " };

            return Ok(res);
        }

        [HttpGet("appLogLevel/{logEventLevel?}")]
        public IActionResult SetAppLogLevel(LogEventLevel logEventLevel = LogEventLevel.Debug)
        {
            var model = _devOpsDomain.SetAppLogLevel(logEventLevel);
            var res   = new { message = $"App Log Level changed from {model.AppLogLevel} to {model.NewAppLogLevel}" };

            return Ok(res);
        }

        [HttpGet("appLogLevel/setupinfo/{flush?}")]
        public IActionResult SetAppLogLevel(bool flush = false)
        {
            var data = Startup.sb.ToString();

            if (flush)
            {
                Startup.sb.Clear();
            }

            return Ok(new { message = data });
        }

        [HttpGet("WcfLogLevel")]
        public IActionResult GetWcfLogLevel()
        {
            var model = _devOpsDomain.GetWcfLogLevel();
            var res   = new { message = $"WCF SOAP Log Level set to {model.WcfLogLevel.ToUpper()}" };

            return Ok(res);
        }

        [HttpGet("WcfLogLevel/{newLevel}")]
        public IActionResult SetWcfLogLevel(string newLevel)
        {
            var model = _devOpsDomain.SetWcfLogLevel(newLevel);
            var res   = new { message = $"WCF SOAP Log Level changed from {model.WcfLogLevel} to {model.NewWcfLogLevel}" };

            return Ok(res);
        }

        [HttpGet("throw")]
        public IActionResult ThrowExeption()
        {
            _devOpsDomain.ThrowExeption();

            return Ok(new { message = "This message won't be seen" });
        }

        [HttpGet("throw/{message}")]
        public IActionResult ThrowExeption(string message)
        {
            _devOpsDomain.ThrowExeption(message);

            return Ok(new { message = "This message won't be seen" });
        }
    }
}
