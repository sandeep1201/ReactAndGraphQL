using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class POPClaimStatus : BaseEntity
    {
        #region Properties

        public int      POPClaimId           { get; set; }
        public int      POPClaimStatusTypeId { get; set; }
        public DateTime POPClaimStatusDate   { get; set; }
        public string   Details              { get; set; }
        public bool     IsDeleted            { get; set; }
        public string   ModifiedBy           { get; set; }
        public DateTime ModifiedDate         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual POPClaim           POPClaim           { get; set; }
        public virtual POPClaimStatusType POPClaimStatusType { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
