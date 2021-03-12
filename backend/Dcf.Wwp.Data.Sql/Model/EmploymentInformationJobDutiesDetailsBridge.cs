using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentInformationJobDutiesDetailsBridge
    {
        #region Properties

        public int?      EmploymentInformationId { get; set; }
        public int?      JobDutiesId             { get; set; }
        public int?      SortOrder               { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmploymentInformation EmploymentInformation { get; set; }
        public virtual JobDutiesDetail       JobDutiesDetail       { get; set; }

        #endregion
    }
}
