using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class MilitaryDischargeType
    {
        #region Properties

        public string    Name         { get; set; }
        public int       SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
