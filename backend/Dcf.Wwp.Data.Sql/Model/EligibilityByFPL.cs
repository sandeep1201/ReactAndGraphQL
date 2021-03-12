using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EligibilityByFPL
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

        #region Navigation properties

        #endregion
    }
}
