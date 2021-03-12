using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWageHour : ICommonModel, ICloneable
    {
        DateTime?                            CurrentEffectiveDate            { get; set; }
        String                               CurrentPayTypeDetails           { get; set; }
        Decimal?                             CurrentAverageWeeklyHours       { get; set; }
        Decimal?                             CurrentPayRate                  { get; set; }
        Int32?                               CurrentPayRateIntervalId        { get; set; }
        Decimal?                             CurrentHourlySubsidyRate        { get; set; }
        Decimal?                             PastBeginPayRate                { get; set; }
        Int32?                               PastBeginPayRateIntervalId      { get; set; }
        Decimal?                             PastEndPayRate                  { get; set; }
        Int32?                               PastEndPayRateIntervalId        { get; set; }
        Boolean?                             IsUnchangedPastPayRateIndicator { get; set; }
        Int32?                               SortOrder                       { get; set; }
        Boolean                              IsDeleted                       { get; set; }
        Decimal?                             WorkSiteContribution            { get; set; }
        ICollection<IEmploymentInformation>  EmploymentInformations          { get; set; }
        ICollection<IWageHourHistory>        WageHourHistories               { get; set; }
        String                               ComputedCurrentWageRateUnit     { get; set; }
        Decimal?                             ComputedCurrentWageRateValue    { get; set; }
        String                               ComputedPastEndWageRateUnit     { get; set; }
        Decimal?                             ComputedPastEndWageRateValue    { get; set; }
        IIntervalType                        BeginRateIntervalType           { get; set; }
        IIntervalType                        CurrentPayIntervalType          { get; set; }
        IIntervalType                        EndRateIntervalType             { get; set; }
        ICollection<IWageHourWageTypeBridge> WageHourWageTypeBridges         { get; set; }
        ICollection<IWageHourWageTypeBridge> AllWageHourWageTypeBridges      { get; set; }
        ICollection<IWageHourHistory>        AllWageHourHistories            { get; set; }
    }
}
