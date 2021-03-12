using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EducationAssessmentSection
    {
        #region Properties

        public bool?     ReviewCompleted { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        // this is really tied to only ONE informal assement, but this is the
        // way they modeled it in database when they inverted the relationship.

        public virtual ICollection<InformalAssessment> InformalAssessments { get; set; }

        #endregion
    }
}
