using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHourHistoryWageTypeBridge : BaseCommonModel, IWageHourHistoryWageTypeBridge, IEquatable<WageHourHistoryWageTypeBridge>
    {
        IWageHourHistory IWageHourHistoryWageTypeBridge.WageHourHistory
        {
            get => WageHourHistory;
            set => WageHourHistory = (WageHourHistory) value;
        }

        IWageType IWageHourHistoryWageTypeBridge.WageType
        {
            get => WageType;
            set => WageType = (WageType) value;
        }

        #region ICloneable

        public new object Clone()
        {
            var clone = new WageHourHistoryWageTypeBridge();

            clone.Id                = Id;
            clone.WageHourHistoryId = WageHourHistoryId;
            clone.WageTypeId        = WageTypeId;
            clone.SortOrder         = SortOrder;
            clone.IsDeleted         = IsDeleted;

            clone.WageType = (WageType) WageType?.Clone();

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WageHourHistoryWageTypeBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(WageHourHistoryWageTypeBridge other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(WageHourHistoryId, other.WageHourHistoryId))
                return false;
            if (!AdvEqual(WageTypeId, other.WageTypeId))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            if (!AdvEqual(WageHourHistory, other.WageHourHistory))
                return false;
            if (!AdvEqual(WageType, other.WageType))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
