using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Accommodation : BaseCommonModel, IAccommodation, IEquatable<Accommodation>
    {
        ICollection<IBarrierAccommodation> IAccommodation.BarrierAccommodations
        {
            get => BarrierAccommodations.Cast<IBarrierAccommodation>().ToList();
            set => BarrierAccommodations = (ICollection<BarrierAccommodation>) value;
        }

        #region ICloneable

        public new object Clone()
        {
            var ac = new Accommodation();

            ac.Id        = this.Id;
            ac.SortOrder = this.SortOrder;
            ac.Name      = this.Name;
            return ac;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Accommodation;
            return obj != null && Equals(obj);
        }

        public bool Equals(Accommodation other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
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
