using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Conviction
    {
        #region Properties

        public int?      LegalSectionId   { get; set; }
        public int?      ConvictionTypeID { get; set; }
        public bool?     IsUnknown        { get; set; }
        public DateTime? DateConvicted    { get; set; }
        public string    Details          { get; set; }
        public int?      DeleteReasonId   { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime? ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ConvictionType     ConvictionType     { get; set; }
        public virtual LegalIssuesSection LegalIssuesSection { get; set; }
        public virtual DeleteReason       DeleteReason       { get; set; }

        #endregion
    }
}
