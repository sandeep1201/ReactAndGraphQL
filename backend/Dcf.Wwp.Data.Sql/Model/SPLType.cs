using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SPLType
    {
        #region Properties

        public string    Name         { get; set; }
        public string    Rating       { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
