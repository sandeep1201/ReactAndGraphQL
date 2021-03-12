using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JRWorkPreferences : BaseEntity
    {
        #region Properties

        public int       JobReadinessId                     { get; set; }
        public string    KindOfJobDetails                   { get; set; }
        public string    JobInterestDetails                 { get; set; }
        public string    TrainingNeededForJobDetails        { get; set; }
        public string    SomeOtherPlacesJobAvailableDetails { get; set; }
        public bool?     SomeOtherPlacesJobAvailableUnknown { get; set; }
        public string    SituationsToAvoidDetails           { get; set; }
        public TimeSpan? WorkScheduleBeginTime              { get; set; }
        public TimeSpan? WorkScheduleEndTime                { get; set; }
        public string    WorkScheduleDetails                { get; set; }
        public string    TravelTimeToWork                   { get; set; }
        public string    DistanceHomeToWork                 { get; set; }
        public bool      IsDeleted                          { get; set; }
        public string    ModifiedBy                         { get; set; }
        public DateTime  ModifiedDate                       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobReadiness                             JobReadiness                 { get; set; }
        public virtual ICollection<JRWorkPreferenceShiftBridge> JrWorkPreferenceShiftBridges { get; set; } = new List<JRWorkPreferenceShiftBridge>();

        #endregion

        #region Clone

        #endregion
    }
}
