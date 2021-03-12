using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CourtDate
    {
        #region Properties

        public int?      LegalSectionId { get; set; }
        public bool?     IsUnknown      { get; set; }
        public string    Location       { get; set; }
        public DateTime? Date           { get; set; }
        public string    Details        { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual LegalIssuesSection LegalIssuesSection { get; set; }

        #endregion
    }
}
