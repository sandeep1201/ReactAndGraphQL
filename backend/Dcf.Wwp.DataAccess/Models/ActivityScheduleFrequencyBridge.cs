using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActivityScheduleFrequencyBridge : BaseEntity
    {
        #region Properties

        public int?      ActivityScheduleId { get; set; }
        public int?      WKFrequencyId      { get; set; }
        public int?      MRFrequencyId      { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ActivitySchedule ActivitySchedule { get; set; }
        public virtual Frequency        WKFrequency      { get; set; }
        public virtual Frequency        MRFrequency      { get; set; }

        #endregion

        #region Clone

        public ActivityScheduleFrequencyBridge Clone()
        {
            var a = new ActivityScheduleFrequencyBridge
                    {
                        Id                 = Id,
                        ActivityScheduleId = ActivityScheduleId,
                        WKFrequencyId      = WKFrequencyId,
                        MRFrequencyId      = MRFrequencyId,
                        IsDeleted          = IsDeleted,
                        ModifiedBy         = ModifiedBy,
                        ModifiedDate       = ModifiedDate,
                        RowVersion         = RowVersion
                    };

            return a;
        }

        #endregion
    }
}
