using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Goal : BaseEntity
    {
        #region Properties

        public int?      GoalTypeId       { get; set; }
        public DateTime? BeginDate        { get; set; }
        public DateTime? EndDate          { get; set; }
        public string    Name             { get; set; }
        public string    Details          { get; set; }
        public bool?     IsGoalEnded      { get; set; }
        public int?      GoalEndReasonId  { get; set; }
        public string    EndReasonDetails { get; set; }
        public bool      IsDeleted        { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime  ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual GoalType                                 GoalType                     { get; set; }
        public virtual GoalEndReason                            GoalEndReason                { get; set; }
        public virtual ICollection<GoalStep>                    GoalSteps                    { get; set; } = new List<GoalStep>();
        public virtual ICollection<EmployabilityPlanGoalBridge> EmployabilityPlanGoalBridges { get; set; } = new List<EmployabilityPlanGoalBridge>();

        #endregion
    }
}
