using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Timelimts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DCF.Timelimits.Rules.Domain;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Dcf.Wwp.Api.Controllers.TimeLimits
{
    [Route("api/[controller]")]
    public class TimelimitsController : BaseController
    {
        private IAuthUser _authUser;
        private readonly ITimelimitService _timelimitService;
        private readonly IDb2TimelimitService _db2TimelimitService;

        public TimelimitsController(IRepository repository, IAuthUser authUser, ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService) : base(repository)
        {
            this._authUser = authUser;
            this._timelimitService = timelimitService;
            this._db2TimelimitService = db2TimelimitService;
        }

        [HttpGet("{pin}")]
        public IActionResult GetTimeline(string pin)
        {
            var vm = new TimeLineViewModel(this.Repo, this._authUser, this._timelimitService);
            var data = vm.GetTimelineByPin(pin);
            return this.Ok(data);
        }

        [ValidationResponseFilter]
        [HttpPost("{pin}/months")]
        public IActionResult SaveTimeLimitMonths(String pin, [FromBody] IEnumerable<TimelineMonthContract> model)
        {
            var vm = new TimeLimitViewModel(this.Repo, this._authUser, this._timelimitService, this._db2TimelimitService);
            vm.InitializeFromPin(pin);

            var savedData = vm.UpsertData(model, pin).ToList();

            return this.Ok(savedData.Select(x => x.UpdatedModel));
        }

        [ValidationResponseFilter]
        [HttpPost("{pin}/month")]
        public IActionResult SaveTimeLimitMonth(String pin, [FromBody] TimelineMonthContract model)
        {
            var vm = new TimeLimitViewModel(this.Repo, this._authUser, this._timelimitService, this._db2TimelimitService);
            vm.InitializeFromPin(pin);

            var savedData = vm.UpsertData(model, pin);

            //if (savedData.HasConcurrencyError)
            //{
            //    return this.BadRequest(new {error = "Concurrency error"});
            //}

            //if (!savedData.ErrorMessage.IsNullOrWhiteSpace())
            //{
            //    return this.BadRequest(savedData.ErrorMessage);
            //}

            return this.Ok(savedData.UpdatedModel);

        }

        [HttpPost("{pin}/month/history")]
        public IActionResult GetTimeliineMonthHistory(String pin, [FromBody] DateTime? model)
        {
            var results = new List<TimelineMonthContract>();

            if (ModelState.IsValid && model.HasValue)
            {
                var vm = new TimeLimitViewModel(this.Repo, this._authUser, this._timelimitService, this._db2TimelimitService);
                var data = vm.GeTimelineMonthHistory(model.Value, pin);
                if (data != null)
                {
                    results.AddRange(data);
                }
            }
            return Ok(results);
        }

        [HttpGet("states")]
        public IActionResult GetStates()
        {
            var vm = new TimeLimitViewModel(this.Repo, this._authUser, this._timelimitService, this._db2TimelimitService);

            var data = vm.GetStates();
            return this.Ok(data);
        }

        [HttpGet("month/change-reasons")]
        public IActionResult GetReasonsForChange()
        {
            var vm = new TimeLimitViewModel(this.Repo, this._authUser, this._timelimitService, this._db2TimelimitService);

            var data = vm.GetChangeReasons();
            return this.Ok(data);
        }

        [HttpGet("{pin}/snapshot")]
        [AllowAnonymous]
        public IActionResult SaveTimelimitsSummary(String pin)
        {
            var vm = new TimeLineViewModel(this.Repo, this._authUser, this._timelimitService);
            var data = vm.GetTimelineSnapshot(pin,true);
            return this.Ok(data);
        }

        [HttpGet("{pin}/snapshot/test")]
        [AllowAnonymous]
        public IActionResult GetTimelimitsSummary(String pin)
        {
            var vm = new TimeLineViewModel(this.Repo, this._authUser, this._timelimitService);
            var data = vm.GetTimelineSnapshot(pin,false);
            return this.Ok(data);
        }

        [HttpGet("{pin}/t0459/counts/{id}")]
        [AllowAnonymous]
        public IActionResult GetT0459Counts(String pin, Int32 id)
        {
            var vm = new TimeLineViewModel(this.Repo,this._authUser,this._timelimitService);
            var data = vm.GetT0459Counts(pin, id);
            return this.Ok(data);
        }

        [HttpGet("{pin}/t0459/latest")]
        [AllowAnonymous]
        public IActionResult GetLatestTicksForEachClockType(decimal pin)
        {
            //var dPin = Decimal.Parse(pinNum);
            var data = this.Repo.GetLatestW2LimitsMonthsForEachClockType(pin);
            return this.Ok(data);
        }

        [HttpGet("{pin}/t0459/latest/{clocktype}")]
        [AllowAnonymous]
        public IActionResult GetLatestTickForbyClockType(decimal pin, string clocktype)
        {
            //var dPin = Decimal.Parse(pinNum);
            var data = this.Repo.GetLatestW2LimitsByClockType(pin, (ClockTypes)Enum.Parse(typeof(ClockTypes), clocktype,true));
            if (data != null)
            {
                return this.Ok(data);

            }
            else
            {
                return this.Ok(new {});

            }
        }
    }
}
