using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class SupportiveService : BaseEntity
    {
        #region Properties

        public int       EmployabilityPlanId     { get; set; }
        public int       SupportiveServiceTypeId { get; set; }
        public string    Details                 { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Methods

        public virtual EmployabilityPlan     EmployabilityPlan     { get; set; }
        public virtual SupportiveServiceType SupportiveServiceType { get; set; }

        #endregion
    }
}
