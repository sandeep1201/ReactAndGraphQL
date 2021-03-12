using System;
using Dcf.Wwp.DataAccess.Base;
using System.Collections.Generic;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmployabilityPlanEmploymentInfoBridge : BaseEntity
    {
        #region Properties
        public int EmployabilityPlanId { get; set; }
        public int EmploymentInformationId { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan EmployabilityPlan { get; set; }
        public virtual EmploymentInformation EmploymentInformation { get; set; }


        #endregion
    }
}
