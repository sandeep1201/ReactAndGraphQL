using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PrintedEmployabilityPlanReportContract
    {
        public string                                HomeLanguageName { get; set; }
        public string                                Placement        { get; set; }
        public List<ActivityReportContract>          Activites        { get; set; }
        public List<EmploymentInfoReportContract>    Employment       { get; set; }
        public List<GoalReportContract>              Goals            { get; set; }
        public List<SupportiveServiceReportContract> Support          { get; set; }
        public List<ScheduleReportContract>          Schedule         { get; set; }
        public string                                WorkerPhone      { get; set; }
        public string                                WorkerEmail      { get; set; }
    }
}
