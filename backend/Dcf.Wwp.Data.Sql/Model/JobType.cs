using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobType
    {
        #region Properties

        public string    Name                        { get; set; }
        public bool?     IsRequired                  { get; set; }
        public bool?     IsUsedForEmploymentOfRecord { get; set; }
        public int?      SortOrder                   { set; get; }
        public bool      IsDeleted                   { get; set; }
        public string    ModifiedBy                  { get; set; }
        public DateTime? ModifiedDate                { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<EmploymentInformation>      EmploymentInformations      { get; set; }
        public virtual ICollection<JobTypeLeavingReasonBridge> JobTypeLeavingReasonBridges { get; set; }

        #endregion
    }
}
