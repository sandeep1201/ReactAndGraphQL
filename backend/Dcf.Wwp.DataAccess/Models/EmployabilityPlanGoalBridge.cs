using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmployabilityPlanGoalBridge : BaseEntity
    {
        #region Properties

        public int       EmployabilityPlanId { get; set; }
        public int       GoalId              { get; set; }
        public bool      IsDeleted           { get; set; }
        public string    ModifiedBy          { get; set; }
        public DateTime  ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan EmployabilityPlan { get; set; }
        public virtual Goal              Goal              { get; set; }

        #endregion
    }
}
