using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Role
    {
        #region Properties

        public string    Name            { get; set; }
        public string    Code            { get; set; }
        public int?      InheritedRoleId { get; set; }
        public int?      DeleteReasonId  { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<RoleAuthorization> RoleAuthorizations { get; set; }

        #endregion
    }
}
