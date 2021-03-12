using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RoleAuthorization
    {
        #region Properties

        public int       RoleId          { get; set; }
        public int       AuthorizationId { get; set; }
        public int?      DeleteReasonId  { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Role          Role          { get; set; }
        public virtual Authorization Authorization { get; set; }

        #endregion
    }
}
