using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierType : BaseCommonModel, IBarrierType, IEquatable<BarrierType>
    {
        ICollection<IBarrierDetail> IBarrierType.BarrierDetails
        {
            get => BarrierDetails.Cast<IBarrierDetail>().ToList();
            set => BarrierDetails = value.Cast<BarrierDetail>().ToList();
        }

        ICollection<IBarrierSubtype> IBarrierType.BarrierSubtypes
        {
            get => BarrierSubtypes.Cast<IBarrierSubtype>().ToList();
            set => BarrierSubtypes = value.Cast<BarrierSubtype>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var bt = new BarrierType();

            bt.Id        = Id;
            bt.SortOrder = SortOrder;
            bt.Name      = Name;

            return bt;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as BarrierType;

            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierType other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(SortOrder, other.SortOrder))
            {
                return false;
            }

            if (!AdvEqual(Name, other.Name))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
