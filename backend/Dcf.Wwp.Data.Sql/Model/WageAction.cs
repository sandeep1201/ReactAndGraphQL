using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageAction : BaseEntity
    {
        #region Properties

        public string    Name         { get; set; }
        public string    ActionType   { get; set; }
        public bool?     IsRequired   { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
