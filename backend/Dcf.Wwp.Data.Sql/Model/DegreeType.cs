using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DegreeType
    {
        #region Properties

        public string    Code         { get; set; }
        public string    Name         { get; set; }
        public int?      SortOrder    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<PostSecondaryDegree> PostSecondaryDegrees { get; set; }

        #endregion
    }
}
