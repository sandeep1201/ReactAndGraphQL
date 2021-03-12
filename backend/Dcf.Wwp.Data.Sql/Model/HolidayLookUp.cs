using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class HolidayLookUp : BaseEntity
    {
        #region Properties

        public string    Year             { get; set; }
        public DateTime? Date             { get; set; }
        public bool      IsCARESHoliday   { get; set; }
        public bool      IsFederalHoliday { get; set; }
        public bool      IsDeleted        { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime? ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
