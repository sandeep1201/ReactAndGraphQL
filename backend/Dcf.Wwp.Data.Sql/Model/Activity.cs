using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Activity
    {
        #region Properties

        public int       ActivityTypeId             { get; set; }
        public int?      ActivityLocationId         { get; set; }
        public int?      ActivityCompletionReasonId { get; set; }
        public string    Description                { get; set; }
        public string    Details                    { get; set; }
        public DateTime? StartDate                  { get; set; }
        public DateTime? EndDate                    { get; set; }
        public bool      IsDeleted                  { get; set; }
        public string    ModifiedBy                 { get; set; }
        public DateTime  ModifiedDate               { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ActivityLocation                             ActivityLocation                 { get; set; }
        public virtual ActivityType                                 ActivityType                     { get; set; }
        public virtual ICollection<ActivitySchedule>                ActivitySchedules                { get; set; }
        public virtual ICollection<ActivityContactBridge>           ActivityContactBridges           { get; set; }
        public virtual ICollection<EmployabilityPlanActivityBridge> EmployabilityPlanActivityBridges { get; set; }
        public virtual ICollection<NonSelfDirectedActivity>         NonSelfDirectedActivities        { get; set; }

        #endregion
    }
}
