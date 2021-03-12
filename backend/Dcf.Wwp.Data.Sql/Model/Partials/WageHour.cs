using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHour : BaseCommonModel, IWageHour, IEquatable<WageHour>
    {
        ICollection<IEmploymentInformation> IWageHour.EmploymentInformations
        {
            get => EmploymentInformations.Cast<IEmploymentInformation>().ToList();
            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        IIntervalType IWageHour.CurrentPayIntervalType
        {
            get => CurrentPayIntervalType;
            set => CurrentPayIntervalType = (IntervalType) value;
        }

        IIntervalType IWageHour.BeginRateIntervalType
        {
            get => BeginRateIntervalType;
            set => BeginRateIntervalType = (IntervalType) value;
        }

        IIntervalType IWageHour.EndRateIntervalType
        {
            get => EndRateIntervalType;
            set => EndRateIntervalType = (IntervalType) value;
        }

        ICollection<IWageHourWageTypeBridge> IWageHour.WageHourWageTypeBridges
        {
            get => (from x in WageHourWageTypeBridges where x.IsDeleted == false select x).Cast<IWageHourWageTypeBridge>().ToList();
            set => WageHourWageTypeBridges = value.Cast<WageHourWageTypeBridge>().ToList();
        }

        ICollection<IWageHourWageTypeBridge> IWageHour.AllWageHourWageTypeBridges
        {
            get => WageHourWageTypeBridges.Cast<IWageHourWageTypeBridge>().ToList();
            set => WageHourWageTypeBridges = value.Cast<WageHourWageTypeBridge>().ToList();
        }

        ICollection<IWageHourHistory> IWageHour.WageHourHistories
        {
            get => (from x in WageHourHistories where x.IsDeleted == false select x).Cast<IWageHourHistory>().ToList();
            set => WageHourHistories = value.Cast<WageHourHistory>().ToList();
        }

        ICollection<IWageHourHistory> IWageHour.AllWageHourHistories
        {
            get => WageHourHistories.Cast<IWageHourHistory>().ToList();
            set => WageHourHistories = value.Cast<WageHourHistory>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var wh = new WageHour();

            wh.Id                              = Id;
            wh.CurrentEffectiveDate            = CurrentEffectiveDate;
            wh.CurrentPayTypeDetails           = CurrentPayTypeDetails;
            wh.CurrentAverageWeeklyHours       = CurrentAverageWeeklyHours;
            wh.CurrentPayRate                  = CurrentPayRate;
            wh.CurrentPayRateIntervalId        = CurrentPayRateIntervalId;
            wh.CurrentHourlySubsidyRate        = CurrentHourlySubsidyRate;
            wh.PastBeginPayRate                = PastBeginPayRate;
            wh.PastBeginPayRateIntervalId      = PastBeginPayRateIntervalId;
            wh.PastEndPayRate                  = PastEndPayRate;
            wh.PastEndPayRateIntervalId        = PastEndPayRateIntervalId;
            wh.IsUnchangedPastPayRateIndicator = IsUnchangedPastPayRateIndicator;
            wh.SortOrder                       = SortOrder;
            wh.CurrentHourlySubsidyRate        = CurrentHourlySubsidyRate;
            wh.ComputedCurrentWageRateUnit     = ComputedCurrentWageRateUnit;
            wh.ComputedCurrentWageRateValue    = ComputedCurrentWageRateValue;
            wh.ComputedPastEndWageRateUnit     = ComputedPastEndWageRateUnit;
            wh.ComputedPastEndWageRateValue    = ComputedPastEndWageRateValue;
            wh.CurrentPayIntervalType          = (IntervalType) CurrentPayIntervalType?.Clone();
            wh.BeginRateIntervalType           = (IntervalType) BeginRateIntervalType?.Clone();
            wh.EndRateIntervalType             = (IntervalType) EndRateIntervalType?.Clone();
            wh.WageHourWageTypeBridges         = WageHourWageTypeBridges.Select(x => (WageHourWageTypeBridge) x.Clone()).ToList();
            wh.WageHourHistories               = WageHourHistories.Select(x => (WageHourHistory) x.Clone()).ToList();

            return wh;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WageHour;
            return obj != null && Equals(obj);
        }

        public bool Equals(WageHour other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(CurrentEffectiveDate, other.CurrentEffectiveDate))
                return false;
            if (!AdvEqual(CurrentPayTypeDetails, other.CurrentPayTypeDetails))
                return false;
            if (!AdvEqual(CurrentAverageWeeklyHours, other.CurrentAverageWeeklyHours))
                return false;
            if (!AdvEqual(CurrentPayRate, other.CurrentPayRate))
                return false;
            if (!AdvEqual(CurrentPayRateIntervalId, other.CurrentPayRateIntervalId))
                return false;
            if (!AdvEqual(CurrentHourlySubsidyRate, other.CurrentHourlySubsidyRate))
                return false;
            if (!AdvEqual(PastBeginPayRate, other.PastBeginPayRate))
                return false;
            if (!AdvEqual(PastBeginPayRateIntervalId, other.PastBeginPayRateIntervalId))
                return false;
            if (!AdvEqual(PastEndPayRate, other.PastEndPayRate))
                return false;
            if (!AdvEqual(PastEndPayRateIntervalId, other.PastEndPayRateIntervalId))
                return false;
            if (!AdvEqual(IsUnchangedPastPayRateIndicator, other.IsUnchangedPastPayRateIndicator))
                return false;
            if (!AdvEqual(CurrentPayIntervalType, other.CurrentPayIntervalType))
                return false;
            if (!AdvEqual(BeginRateIntervalType, other.BeginRateIntervalType))
                return false;
            if (!AdvEqual(EndRateIntervalType, other.EndRateIntervalType))
                return false;
            if (AreBothNotNull(WageHourHistories, other.WageHourHistories) && !WageHourHistories.OrderBy(x => x.Id).SequenceEqual(other.WageHourHistories.OrderBy(x => x.Id)))
                return false;
            if (AreBothNotNull(WageHourWageTypeBridges, other.WageHourWageTypeBridges) && !WageHourWageTypeBridges.OrderBy(x => x.Id).SequenceEqual(other.WageHourWageTypeBridges.OrderBy(x => x.Id)))
                return false;
            if (!AdvEqual(ComputedCurrentWageRateUnit, other.ComputedCurrentWageRateUnit))
                return false;
            if (!AdvEqual(ComputedCurrentWageRateValue, other.ComputedCurrentWageRateValue))
                return false;
            if (!AdvEqual(ComputedPastEndWageRateUnit, other.ComputedPastEndWageRateUnit))
                return false;
            if (!AdvEqual(ComputedPastEndWageRateValue, other.ComputedPastEndWageRateValue))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
