using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmployabilityPlanActivityBridge : BaseEntity
    {
        #region Properties

        public int      EmployabilityPlanId { get; set; }
        public int      ActivityId          { get; set; }
        public bool     IsDeleted           { get; set; }
        public string   ModifiedBy          { get; set; }
        public DateTime ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan EmployabilityPlan { get; set; }
        public virtual Activity          Activity          { get; set; }

        #endregion

        #region Clone

        public EmployabilityPlanActivityBridge Clone()
        {
            var a = new EmployabilityPlanActivityBridge
                    {
                        Id                  = Id,
                        IsDeleted           = IsDeleted,
                        ModifiedBy          = ModifiedBy,
                        ModifiedDate        = ModifiedDate,
                        RowVersion          = RowVersion,
                        ActivityId          = ActivityId,
                        EmployabilityPlanId = EmployabilityPlanId
                    };

            return a;
        }

        #endregion
    }
}
