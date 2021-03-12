using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentInformationBenefitsOfferedTypeBridge
    {
        #region Properties

        public int?      EmploymentInformationId { get; set; }
        public int?      BenefitsOfferedTypeId   { get; set; }
        public int?      SortOrder               { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual BenefitsOfferedType   BenefitsOfferedType   { get; set; }
        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
