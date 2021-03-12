using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentVerification
    {
        #region Properties

        public int      EmploymentInformationId    { get; set; }
        public bool     IsVerified                 { get; set; }
        public DateTime CreatedDate                { get; set; }
        public string   ModifiedBy                 { get; set; }
        public DateTime ModifiedDate               { get; set; }
        public int?     NumberOfDaysAtVerification { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
