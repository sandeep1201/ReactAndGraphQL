using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivitySchedule
    {
        #region Properties

        public int       ActivityId          { get; set; }
        public DateTime? StartDate           { get; set; }
        public bool?     IsRecurring         { get; set; }
        public int?      FrequencyTypeId     { get; set; }
        public DateTime? PlannedEndDate      { get; set; }
        public bool      IsDeleted           { get; set; }
        public string    ModifiedBy          { get; set; }
        public DateTime  ModifiedDate        { get; set; }
        public DateTime? ActualEndDate       { get; set; }
        public decimal?  HoursPerDay         { get; set; }
        public int?      EmployabilityPlanId { get; set; }
        public TimeSpan? BeginTime           { get; set; }
        public TimeSpan? EndTime             { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Activity                                     Activity                         { get; set; }
        public virtual FrequencyType                                FrequencyType                    { get; set; }
        public virtual EmployabilityPlan                            EmployabilityPlan                { get; set; }
        public virtual ICollection<ActivityScheduleFrequencyBridge> ActivityScheduleFrequencyBridges { get; set; }

        #endregion
    }
}
