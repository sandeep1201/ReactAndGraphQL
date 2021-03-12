using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWageHourHistory : ICommonDelModel, ICloneable
    {
        Decimal?      HourlySubsidyRate     { get; set; }
        DateTime?     EffectiveDate         { get; set; }
        String        PayTypeDetails        { get; set; }
        Decimal?      AverageWeeklyHours    { get; set; }
        Decimal?      PayRate               { get; set; }
        Int32?        PayRateIntervalId     { get; set; }
        Int32?        SortOrder             { get; set; }
        String        ComputedWageRateUnit  { get; set; }
        Decimal?      ComputedWageRateValue { get; set; }
        Decimal?      WorkSiteContribution  { get; set; }
        IIntervalType IntervalType          { get; set; }
        IWageHour     WageHour              { get; set; }

        ICollection<IWageHourHistoryWageTypeBridge> WageHourHistoryWageTypeBridges    { get; set; }
        ICollection<IWageHourHistoryWageTypeBridge> AllWageHourHistoryWageTypeBridges { get; set; }
    }
}
