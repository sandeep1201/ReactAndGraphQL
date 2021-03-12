using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PlanSectionResource : BaseEntity
    {
        #region Properties

        public int      PlanSectionId { get; set; }
        public string   Resource      { get; set; }
        public bool     IsDeleted     { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual PlanSection PlanSection { get; set; }

        #endregion
    }
}
