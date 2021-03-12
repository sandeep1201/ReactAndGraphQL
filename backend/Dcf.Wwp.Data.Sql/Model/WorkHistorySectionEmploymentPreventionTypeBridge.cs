using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkHistorySectionEmploymentPreventionTypeBridge
    {
        #region Properties

        public int       WorkHistorySectionId       { get; set; }
        public int       EmploymentPreventionTypeId { get; set; }
        public bool      IsDeleted                  { get; set; }
        public string    ModifiedBy                 { get; set; }
        public DateTime? ModifiedDate               { get; set; }

        #endregion

        #region Navigation Properties

        public virtual WorkHistorySection       WorkHistorySection       { get; set; }
        public virtual EmploymentPreventionType EmploymentPreventionType { get; set; }

        #endregion
    }
}
