using System;
using System.Collections.Generic;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using DCF.Timelimits.Rules.Domain;
using Newtonsoft.Json;

namespace DCF.Timelimits.Tasks
{
    public class ProcessTimelimitEvaluationContext : TimelineTaskContext<ProcessTimelimitEvaluationResult>
    {
        public ITimeLimit CreatedMonth { get; set; }
        public List<SpTimelimitPlacementSummaryReturnModel> PlacementData { get; set; } = new List<SpTimelimitPlacementSummaryReturnModel>();

        public List<AssistanceGroupMember> OtherAssistanceGroupMembers { get;set; } = new List<AssistanceGroupMember>();

    }

    public class ProcessTimelimitEvaluationResult : TimelineTaskResult
    {
        public IT0459_IN_W2_LIMITS t0459Record { get; set; }
        public Decimal PinNumber { get; set; }
        public SpTimelimitPlacementSummaryReturnModel ClosedPlacement { get; set; }
        public Boolean? ClosedSuccesfully { get; set; }
        public ITimeLimitSummary Snapshot { get; set; }
        public List<IT0459_IN_W2_LIMITS> CorrectedTicks { get; set; } = new List<IT0459_IN_W2_LIMITS>();
        public Boolean? PrimaryOutOfStateTicks { get; set; }
        public Boolean? PrimaryOutOfClockTicks { get; set; }
        public Boolean? SecondaryOutOfStateTicks { get; set; }
        public Int32? OtherStateParentRemaining { get; set; }

        public Boolean ShouldClose { get; set; }
        public Placement LastPlacement { get; set; }

        #region Database fields for auditing

        public Decimal? CaseNumber { get; set; }
        public DateTime? DatabaseDate { get; set; }
        public String InputUserId { get; set; }
        public DateTime? ExistingEpisodeBeginDate { get; set; }
        public String ExistingFepId {get; set; }
        public DateTime?  ExistingEpisodeEndDate { get; set; }
        public String ExistingPlacementCode {get; set; }
        public DateTime? ExistingPlacementBeginDate { get; set; }
        public Int32? ClockUsed { get; set; }
        public Int32? ClockMax { get; set; }
        public Int32? ClockRemaining { get; set; }
        public Int32? StateRemaining {get; set; }
        public Int32? ClockLimit { get; set; }
        public Boolean? PlacementIsOpen { get; set; }
        public String PlacementType { get; set; }

        #endregion
    }
}