using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class HousingSituation : BaseCommonModel, IHousingSituation, IEquatable<HousingSituation>
    {
        #region ICloneable

        public new object Clone()
        {
            var hs = new HousingSituation();

            hs.Id        = this.Id;
            hs.SortOrder = this.SortOrder;
            hs.Name      = this.Name;
            return hs;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as HousingSituation;
            return obj != null && Equals(obj);
        }

        public bool Equals(HousingSituation other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
