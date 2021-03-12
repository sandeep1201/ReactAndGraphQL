using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Document : BaseEntity
    {
        #region Properties

        public int      Id                  { get; set; }
        public int      EmployabilityPlanId { get; set; }
        public DateTime UploadedDate        { get; set; }
        public bool     IsScanned           { get; set; }
        public bool     IsDeleted           { get; set; }
        public string   ModifiedBy          { get; set; }
        public DateTime ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan EmployabilityPlan { get; set; }

        #endregion
    }
}
