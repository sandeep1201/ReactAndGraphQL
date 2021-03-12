using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHourHistory : BaseCommonModel, IWageHourHistory, IEquatable<WageHourHistory>
    {
        ICollection<IWageHourHistoryWageTypeBridge> IWageHourHistory.WageHourHistoryWageTypeBridges
        {
            get => (from x in WageHourHistoryWageTypeBridges where x.IsDeleted == false select x).Cast<IWageHourHistoryWageTypeBridge>().ToList();
            set => WageHourHistoryWageTypeBridges = value.Cast<WageHourHistoryWageTypeBridge>().ToList();
        }

        ICollection<IWageHourHistoryWageTypeBridge> IWageHourHistory.AllWageHourHistoryWageTypeBridges
        {
            get => WageHourHistoryWageTypeBridges.Cast<IWageHourHistoryWageTypeBridge>().ToList();
            set => WageHourHistoryWageTypeBridges = value.Cast<WageHourHistoryWageTypeBridge>().ToList();
        }

        IWageHour IWageHourHistory.WageHour
        {
            get => WageHour;
            set => WageHour = (WageHour) value;
        }

        IIntervalType IWageHourHistory.IntervalType
        {
            get => IntervalType;
            set => IntervalType = (IntervalType) value;
        }

        #region ICloneable

        public new object Clone()
        {
            var whh = new WageHourHistory();

            whh.Id                             = Id;
            whh.WageHourId                     = WageHourId;
            whh.HourlySubsidyRate              = HourlySubsidyRate;
            whh.EffectiveDate                  = EffectiveDate;
            whh.PayTypeDetails                 = PayTypeDetails;
            whh.AverageWeeklyHours             = AverageWeeklyHours;
            whh.PayRate                        = PayRate;
            whh.PayRateIntervalId              = PayRateIntervalId;
            whh.ComputedWageRateUnit           = ComputedWageRateUnit;
            whh.ComputedWageRateValue          = ComputedWageRateValue;
            whh.IntervalType                   = (IntervalType) IntervalType?.Clone();
            whh.WageHourHistoryWageTypeBridges = WageHourHistoryWageTypeBridges.Select(x => (WageHourHistoryWageTypeBridge) x.Clone()).ToList();
            whh.IsDeleted                      = IsDeleted;

            return whh;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WageHourHistory;
            return obj != null && Equals(obj);
        }

        public bool Equals(WageHourHistory other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(WageHourId, other.WageHourId))
                return false;
            if (!AdvEqual(HourlySubsidyRate, other.HourlySubsidyRate))
                return false;
            if (!AdvEqual(EffectiveDate, other.EffectiveDate))
                return false;
            if (!AdvEqual(PayTypeDetails, other.PayTypeDetails))
                return false;
            if (!AdvEqual(AverageWeeklyHours, other.AverageWeeklyHours))
                return false;
            if (!AdvEqual(PayRate, other.PayRate))
                return false;
            if (!AdvEqual(PayRateIntervalId, other.PayRateIntervalId))
                return false;
            if (!AdvEqual(IntervalType, other.IntervalType))
                return false;
            if (!AdvEqual(ComputedWageRateUnit, other.ComputedWageRateUnit))
                return false;
            if (!AdvEqual(ComputedWageRateValue, other.ComputedWageRateValue))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
