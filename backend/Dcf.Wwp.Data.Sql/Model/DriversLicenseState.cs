using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DriversLicenseState : BaseEntity
    {
        #region Properties

        public int      StateId      { get; set; }
        public int      SortOrder    { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual State State { get; set; }

        #endregion
    }
}
