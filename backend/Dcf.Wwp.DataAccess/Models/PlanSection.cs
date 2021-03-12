using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PlanSection : BaseEntity
    {
        #region Properties

        public int      PlanId                { get; set; }
        public int      PlanSectionTypeId     { get; set; }
        public bool     IsNotNeeded           { get; set; }
        public string   ShortTermPlanOfAction { get; set; }
        public string   LongTermPlanOfAction  { get; set; }
        public bool     IsDeleted             { get; set; }
        public string   ModifiedBy            { get; set; }
        public DateTime ModifiedDate          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Plan                             Plan                 { get; set; }
        public virtual PlanSectionType                  PlanSectionType      { get; set; }
        public virtual ICollection<PlanSectionResource> PlanSectionResources { get; set; } = new List<PlanSectionResource>();

        #endregion
    }
}
