using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EligibilityByFPL : BaseEntity
    {
        #region Properties

        public int       GroupSize      { get; set; }
        public decimal?  Pct150PerMonth { get; set; }
        public decimal?  Pct115PerMonth { get; set; }
        public DateTime? EffectiveDate  { get; set; }
        public DateTime? EndDate        { get; set; }
        public int       SortOrder      { get; set; }
        public bool      IsDeleted      { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime  ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

        #region Clone

        #endregion
    }
}
