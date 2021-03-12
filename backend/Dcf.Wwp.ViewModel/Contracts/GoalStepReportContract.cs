using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GoalStepReportContract
    {
        public string GoalStepDetails { get; set; }
        public bool? GoalStepIsGoalStepCompleted { get; set; }
    }
}
