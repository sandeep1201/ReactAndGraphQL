using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlanActivityBridge
    {
        #region Properties

        public int       EmployabilityPlanId { get; set; }
        public int       ActivityId          { get; set; }
        public bool      IsDeleted           { get; set; }
        public string    ModifiedBy          { get; set; }
        public DateTime  ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan EmployabilityPlan { get; set; }
        public virtual Activity          Activity          { get; set; }

        #endregion
    }
}
