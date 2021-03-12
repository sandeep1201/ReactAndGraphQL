using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Authorization
    {
        #region Properties

        public string Name { get; set; }

        //public bool      IsDeleted      { get; set; } //TODO: already defined
        public int?      DeleteReasonId { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
