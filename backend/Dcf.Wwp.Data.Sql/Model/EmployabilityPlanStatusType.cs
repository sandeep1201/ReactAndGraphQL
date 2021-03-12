using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlanStatusType
    {
        #region Properties

        public string   Name         { get; set; }
        public int      SortOrder    { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Nav Props

        public virtual ICollection<EmployabilityPlan> EmployabilityPlans { get; set; }

        #endregion
    }
}
