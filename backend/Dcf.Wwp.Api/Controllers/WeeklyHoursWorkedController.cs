using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/weekly-hours-worked")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class WeeklyHoursWorkedController : Controller
    {
        #region Properties

        private readonly IWeeklyHoursWorkedDomain _weeklyHoursWorkedDomain;

        #endregion

        #region Methods

        public WeeklyHoursWorkedController(IWeeklyHoursWorkedDomain weeklyHoursWorkedDomain)
        {
            _weeklyHoursWorkedDomain = weeklyHoursWorkedDomain;
        }

        [HttpGet("{pin}/{employmentInformationId}")]
        public IActionResult GetWeeklyHoursWorkedEntries(int employmentInformationId)
        {
            var contract = _weeklyHoursWorkedDomain.GetWeeklyHoursWorkedContracts(employmentInformationId);
            return Ok(contract);
        }

        [HttpGet("{pin}/{employmentInformationId}/{id}")]
        public IActionResult GetWeeklyHoursWorkedEntry(int id)
        {
            var contract = _weeklyHoursWorkedDomain.GetWeeklyHoursWorkedEntry(id);
            return Ok(contract);
        }

        [HttpPost("{pin}/{employmentInformationId}/{id}")]
        public IActionResult UpsertWeeklyHoursWorkedEntry([FromBody] WeeklyHoursWorkedContract weeklyHoursWorkedContract)
        {
            var contract = _weeklyHoursWorkedDomain.UpsertWeeklyHoursWorkedEntry(weeklyHoursWorkedContract);
            return Ok(contract);
        }

        [HttpDelete("{pin}/{employmentInformationId}/{id}")]
        public IActionResult DeleteWeeklyHoursWorkedEntry(int id)
        {
            var contract = _weeklyHoursWorkedDomain.DeleteWeeklyHoursWorkedEntry(id);
            return Ok(contract);
        }

        #endregion
    }
}
