using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EACommentType : BaseEntity
    {
        #region Properties

        public string    Code            { get; set; }
        public string    Name            { get; set; }
        public DateTime  EffectiveDate   { get; set; }
        public DateTime? EndDate         { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime  ModifiedDate    { get; set; }
        public bool      IsSystemUseOnly { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<EACommentTypeBridge> EaCommentTypeBridges { get; set; } = new List<EACommentTypeBridge>();

        #endregion
    }
}
