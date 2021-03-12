using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Frequency
    {
        #region Properties

        public string   Code         { get; set; }
        public string   Name         { get; set; }
        public int      SortOrder    { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string   ShortName    { get; set; }

        #endregion

        #region Methods

        public virtual ICollection<ActivityScheduleFrequencyBridge> MRActivityScheduleFrequencyBridges { get; set; }
        public virtual ICollection<ActivityScheduleFrequencyBridge> WKActivityScheduleFrequencyBridges { get; set; }

        #endregion
    }
}
