using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialReferralChild
    {
        #region Properties

        public int       NonCustodialReferralParentId { get; set; }
        public string    FirstName                    { get; set; }
        public string    LastName                     { get; set; }
        public int?      ReferralContactIntervalId    { get; set; }
        public string    ContactIntervalDetails       { get; set; }
        public bool?     HasChildSupportOrder         { get; set; }
        public string    ChildSupportOrderDetails     { get; set; }
        public int?      DeleteReasonId               { get; set; }
        public string    ModifiedBy                   { get; set; }
        public DateTime? ModifiedDate                 { get; set; }

        #endregion

        #region Navigation Properties

        public virtual NonCustodialReferralParent NonCustodialReferralParent { get; set; }
        public virtual ReferralContactInterval    ReferralContactInterval    { get; set; }
        public virtual DeleteReason               DeleteReason               { get; set; }

        #endregion
    }
}
