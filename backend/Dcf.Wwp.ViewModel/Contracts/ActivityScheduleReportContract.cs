using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ActivityScheduleReportContract
    {
        public string ActivityScheduleNumber { get; set; }
        public string ActivityScheduleStartDate { get; set; }
        public string ActivitySchedulePlannedEndDate { get; set; }
        public string ActivityScheduleFrequencyTypeName { get; set; }
        public string ActivityScheduleHoursPerDay { get; set; }
        public string ActivityScheduleEndTime { get; set; }
        public string ActivityScheduleBeginTime { get; set; }
        public string ActivityScheduleEndDateType { get; set; }

    }
}
