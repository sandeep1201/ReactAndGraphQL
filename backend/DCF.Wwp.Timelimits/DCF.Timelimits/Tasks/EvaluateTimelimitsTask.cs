using System;
using System.Collections.Generic;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Definitions;
using DCF.Timelimits.Rules.Domain;
using MediatR;

namespace DCF.Timelimits.Tasks
{
    

    public class EvaluateTimelimitsTaskContext : TimelineTaskContext<EvaluateTimelimitsTaskResult>
    {
        public DateTime MonthToProcess { get; set; }

        public override String GetItemIdentifier()
        {
            return $"{this.JobId}-{this.MonthToProcess:MMMM-yyyy}-{this.PinNumber}";
        }


        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<AssistanceGroupMember> AssitanceGroupMembers { get; set; } = new List<AssistanceGroupMember>();
        public override EvaluateTimelimitsTaskResult Result { get; set; } = new EvaluateTimelimitsTaskResult();
    }

    public class EvaluateTimelimitsTaskResult : TimelineTaskResult
    {
        public DateTime MonthProcessed { get; set; }
        public TimelineMonth EvaluatedData { get; set; }
        public RuleContext RuleContext { get; set; } = new RuleContext();
        public List<OpcTick> OtherParentData { get; set; } = new List<OpcTick>();
        public Decimal PinNumber { get; set; }

    }
}
