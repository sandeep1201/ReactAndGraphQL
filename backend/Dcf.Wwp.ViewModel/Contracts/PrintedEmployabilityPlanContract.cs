using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PrintedEmployabilityPlanContract
    {
        public EmployabilityPlanContract EmployabilityPlan { get; set; }
        public List<GoalContract> Goals { get; set; }
        public List<EpEmploymentContract> EmploymentInfo { get; set; }
        public List<ActivityContract> Activities { get; set; }
        public List<SupportiveServiceContract> SupportiveServices { get; set; }
        public ParticipantReportContract Participant { get; set; }
    }
}
