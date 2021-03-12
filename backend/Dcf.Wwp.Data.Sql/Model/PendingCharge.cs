using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PendingCharge
    {
        #region Properties

        public int?      LegalSectionId   { get; set; }
        public int?      ConvictionTypeID { get; set; }
        public DateTime? ChargeDate       { get; set; }
        public bool?     IsUnknown        { get; set; }
        public string    Details          { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime? ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual LegalIssuesSection LegalIssuesSection { get; set; }
        public virtual ConvictionType     ConvictionType     { get; set; }

        #endregion
    }
}
