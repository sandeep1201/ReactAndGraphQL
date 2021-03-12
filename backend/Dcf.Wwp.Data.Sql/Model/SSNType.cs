using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SSNType
    {
        #region Properties

        public string    Name              { get; set; }
        public bool?     IsDetailsRequired { get; set; }
        public bool      IsDeleted         { get; set; }
        public DateTime? CreatedDate       { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<AKA> AKAs { get; set; }

        #endregion
    }
}
