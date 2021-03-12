using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActivitySchedule : BaseEntity
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
        public TimeSpan? BeginTime           { get; set; }
        public TimeSpan? EndTime             { get; set; }
        public int?      EmployabilityPlanId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Activity                                     Activity                         { get; set; }
        public virtual FrequencyType                                FrequencyType                    { get; set; }
        public virtual EmployabilityPlan                            EmployabilityPlan                { get; set; }
        public virtual ICollection<ActivityScheduleFrequencyBridge> ActivityScheduleFrequencyBridges { get; set; } = new List<ActivityScheduleFrequencyBridge>();

        #endregion

        #region Clone

        public ActivitySchedule Clone()
        {
            var a = new ActivitySchedule
                    {
                        Id                               = Id,
                        IsDeleted                        = IsDeleted,
                        ModifiedBy                       = ModifiedBy,
                        ModifiedDate                     = ModifiedDate,
                        RowVersion                       = RowVersion,
                        StartDate                        = StartDate,
                        IsRecurring                      = IsRecurring,
                        FrequencyTypeId                  = FrequencyTypeId,
                        PlannedEndDate                   = PlannedEndDate,
                        ActualEndDate                    = ActualEndDate,
                        HoursPerDay                      = HoursPerDay,
                        BeginTime                        = BeginTime,
                        EndTime                          = EndTime,
                        EmployabilityPlanId              = EmployabilityPlanId,
                        ActivityScheduleFrequencyBridges = ActivityScheduleFrequencyBridges.Select(i => i.Clone()).ToList()
                    };

            return a;
        }

        #endregion
    }
}
