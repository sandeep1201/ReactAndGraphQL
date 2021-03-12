using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmploymentVerification : BaseEntity
    {
        #region Properties

        public int      EmploymentInformationId    { get; set; }
        public bool     IsVerified                 { get; set; }
        public DateTime CreatedDate                { get; set; }
        public bool     IsDeleted                  { get; set; }
        public string   ModifiedBy                 { get; set; }
        public DateTime ModifiedDate               { get; set; }
        public int?     NumberOfDaysAtVerification { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
