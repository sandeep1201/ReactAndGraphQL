using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobDutiesDetail
    {
        #region Properties

        public string    Details      { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<EmploymentInformationJobDutiesDetailsBridge> EmploymentInformationJobDutiesDetailsBridges { get; set; }

        #endregion
    }
}
