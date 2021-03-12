using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageType : BaseCommonModel, IWageType, IEquatable<WageType>
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new WageType()
                        {
                            Id                 = this.Id,
                            Name               = this.Name,
                            DisablesOthersFlag = this.DisablesOthersFlag,
                            SortOrder          = this.SortOrder,
                            IsDeleted          = this.IsDeleted,
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WageType;
            return obj != null && Equals(obj);
        }

        public bool Equals(WageType other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(DisablesOthersFlag, other.DisablesOthersFlag))
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
