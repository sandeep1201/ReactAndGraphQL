using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierAssessmentSection
    {
        #region Properties

        public bool?     ReviewCompleted { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<InformalAssessment> InformalAssessments { get; set; }

        #endregion
    }
}
