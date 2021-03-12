using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityScheduleFrequencyBridge
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
    }
}
