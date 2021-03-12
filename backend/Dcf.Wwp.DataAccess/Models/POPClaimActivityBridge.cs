using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class POPClaimActivityBridge : BaseEntity
    {
        #region Properties

        public int      POPClaimId   { get; set; }
        public int      ActivityId   { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual POPClaim POPClaim { get; set; }
        public virtual Activity Activity { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
