using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class OtherJobInformation
    {
        #region Properties

        public string    ExpectedScheduleDetails { get; set; }
        public int?      JobSectorId             { get; set; }
        public int?      JobFoundMethodId        { get; set; }
        public string    JobFoundMethodDetails   { get; set; }
        public string    WorkerId                { get; set; }
        public int?      WorkProgramId           { get; set; }
        public int?      SortOrder               { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobSector                          JobSector              { get; set; }
        public virtual JobFoundMethod                     JobFoundMethod         { get; set; }
        public virtual WorkProgram                        WorkProgram            { get; set; }
        public virtual ICollection<EmploymentInformation> EmploymentInformations { get; set; }

        #endregion
    }
}
