using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PinCommentType : BaseEntity
    {
        #region Properties

        public string    Name          { get; set; }
        public DateTime  EffectiveDate { get; set; }
        public DateTime? EndDate       { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime  ModifiedDate  { get; set; }
        public bool SystemUseOnly { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<PCCTBridge> PCCTBridges { get; set; } = new List<PCCTBridge>();

        #endregion
    }
}
