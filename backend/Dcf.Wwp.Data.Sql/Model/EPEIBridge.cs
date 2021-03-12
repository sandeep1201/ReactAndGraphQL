using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EPEIBridge
    {
        #region Properties

        public int?     EmployabilityPlanId     { get; set; }
        public int?     EmploymentInformationId { get; set; }
        public bool     IsDeleted               { get; set; }
        public string   ModifiedBy              { get; set; }
        public DateTime ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan     EmployabilityPlan     { get; set; }
        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
