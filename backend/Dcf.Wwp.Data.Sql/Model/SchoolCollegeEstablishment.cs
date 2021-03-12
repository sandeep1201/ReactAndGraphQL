using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SchoolCollegeEstablishment
    {
        #region Properties

        public string    Name         { get; set; }
        public string    Street       { get; set; }
        public int?      CityId       { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual City                              City                  { get; set; }
        public virtual ICollection<PostSecondaryCollege> PostSecondaryColleges { get; set; }
        public virtual ICollection<EducationSection>     EducationSections     { get; set; }

        #endregion
    }
}
