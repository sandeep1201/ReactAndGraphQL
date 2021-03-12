using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyBarriersDetail : BaseCommonModel, IFamilyBarriersDetail
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new FamilyBarriersDetail();

            clone.Id           = this.Id;
            clone.ModifiedDate = this.ModifiedDate;
            clone.ModifiedBy   = this.ModifiedBy;
            clone.Details      = this.Details;
            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as FamilyBarriersDetail;
            return obj != null && Equals(obj);
        }

        public bool Equals(FamilyBarriersDetail other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
