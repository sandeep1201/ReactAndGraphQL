using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHourWageTypeBridge : BaseCommonModel, IWageHourWageTypeBridge, IEquatable<WageHourWageTypeBridge>
    {
        IWageHour IWageHourWageTypeBridge.WageHour
        {
            get => WageHour;

            set => WageHour = (WageHour) value;
        }

        IWageType IWageHourWageTypeBridge.WageType
        {
            get => WageType;

            set => WageType = (WageType) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new WageHourWageTypeBridge()
                        {
                            Id         = Id,
                            WageHourId = WageHourId,
                            WageTypeId = WageTypeId,
                            SortOrder  = SortOrder,
                            IsDeleted  = IsDeleted,
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WageHourWageTypeBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(WageHourWageTypeBridge other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(WageHourId, other.WageHourId))
                return false;
            if (!AdvEqual(WageTypeId, other.WageTypeId))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
