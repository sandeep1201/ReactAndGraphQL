using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHourHistoryWageTypeBridge
    {
        #region Properties

        public int?      WageHourHistoryId { get; set; }
        public int?      WageTypeId        { get; set; }
        public int?      SortOrder         { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual WageHourHistory WageHourHistory { get; set; }
        public virtual WageType        WageType        { get; set; }

        #endregion
    }
}
