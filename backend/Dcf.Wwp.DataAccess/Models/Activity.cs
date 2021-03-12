using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Activity : BaseEntity
    {
        #region Properties

        public int       ActivityTypeId             { get; set; }
        public string    Description                { get; set; }
        public int?      ActivityLocationId         { get; set; }
        public string    Details                    { get; set; }
        public bool      IsDeleted                  { get; set; }
        public string    ModifiedBy                 { get; set; }
        public DateTime  ModifiedDate               { get; set; }
        public int?      ActivityCompletionReasonId { get; set; }
        public DateTime? EndDate                    { get; set; }
        public DateTime? StartDate                  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ActivityType                                 ActivityType                    { get; set; }
        public virtual ActivityLocation                             ActivityLocation                { get; set; }
        public virtual ActivityCompletionReason                     ActivityCompletionReason        { get; set; }
        public virtual ICollection<ActivitySchedule>                ActivitySchedules               { get; set; } = new List<ActivitySchedule>();
        public virtual ICollection<NonSelfDirectedActivity>         NonSelfDirectedActivities       { get; set; } = new List<NonSelfDirectedActivity>();
        public virtual ICollection<ActivityContactBridge>           ActivityContactBridges          { get; set; } = new List<ActivityContactBridge>();
        public virtual ICollection<EmployabilityPlanActivityBridge> EmploybilityPlanActivityBridges { get; set; } = new List<EmployabilityPlanActivityBridge>();
        public virtual ICollection<ParticipationEntry>              ParticipationEntries            { get; set; } = new List<ParticipationEntry>();
        public virtual ICollection<CFParticipationEntry>            CFParticipationEntries          { get; set; } = new List<CFParticipationEntry>();
        public virtual ICollection<ParticipationEntryHistory>       ParticipationEntryHistories     { get; set; } = new List<ParticipationEntryHistory>();
        public virtual ICollection<POPClaimActivityBridge>          POPClaimActivityBridges         { get; set; } = new List<POPClaimActivityBridge>();

        #endregion

        #region Clone

        public Activity Clone()
        {
            var a = new Activity
                    {
                        ActivityType                    = ActivityType?.Clone(),
                        ActivityTypeId                  = ActivityTypeId,
                        Description                     = Description,
                        ActivityLocation                = ActivityLocation?.Clone(),
                        ActivityLocationId              = ActivityLocationId,
                        Details                         = Details,
                        IsDeleted                       = IsDeleted,
                        ModifiedBy                      = ModifiedBy,
                        ModifiedDate                    = ModifiedDate,
                        RowVersion                      = RowVersion,
                        ActivityCompletionReason        = ActivityCompletionReason?.Clone(),
                        ActivityCompletionReasonId      = ActivityCompletionReasonId,
                        EndDate                         = EndDate,
                        StartDate                       = StartDate,
                        ActivityContactBridges          = ActivityContactBridges?.Select(x => x.Clone()).ToList(),
                        NonSelfDirectedActivities       = NonSelfDirectedActivities?.Select(x => x.Clone()).ToList(),
                        EmploybilityPlanActivityBridges = EmploybilityPlanActivityBridges?.Select(x => x.Clone()).ToList(),
                        ActivitySchedules               = ActivitySchedules?.Select(x => x.Clone()).ToList()
                    };


            return a;
        }

        #endregion
    }
}
