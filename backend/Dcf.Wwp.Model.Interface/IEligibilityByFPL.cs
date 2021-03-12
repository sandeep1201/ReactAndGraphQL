using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEligibilityByFPL
    {
        int       GroupSize      { get; set; }
        decimal?  Pct150PerMonth { get; set; }
        DateTime? EffectiveDate  { get; set; }
        DateTime? EndDate        { get; set; }
    }
}
