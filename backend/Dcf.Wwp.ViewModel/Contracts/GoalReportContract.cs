using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GoalReportContract
    {
        public string GoalTypeName { get; set; }
        public string GoalName { get; set; }
        public string GoalDetails { get; set; }
        public string GoalBegin { get; set; }
        public bool HasGoalStep { get; set; }
        public List<GoalStepReportContract> GoalStep { get; set; }
    }
}
