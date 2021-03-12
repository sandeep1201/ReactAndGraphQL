using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ApprovalReason
    {
        #region Properties

        public string Name { get; set; }
        public string Code { get; set; }

        public DateTime? CreatedDate { get; set; }

        //public bool      IsDeleted    { get; set; } //TODO: already defined
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<TimeLimitExtension> TimeLimitExtensions { get; set; }
        public virtual ICollection<TimeLimitType>      TimeLimitTypes      { get; set; }

        #endregion
    }
}
