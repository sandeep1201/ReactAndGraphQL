using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierTypeBarrierSubTypeBridge : BaseCommonModel, IBarrierTypeBarrierSubTypeBridge, IEquatable<BarrierTypeBarrierSubTypeBridge>
    {
        IBarrierSubtype IBarrierTypeBarrierSubTypeBridge.BarrierSubtype
        {
            get => BarrierSubtype;
            set => BarrierSubtype = (BarrierSubtype) value;
        }

        IBarrierDetail IBarrierTypeBarrierSubTypeBridge.BarrierDetail
        {
            get => BarrierDetail;
            set => BarrierDetail = (BarrierDetail) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new BarrierTypeBarrierSubTypeBridge
                        {
                            BarrierSubTypeId = BarrierSubTypeId,
                            BarrierSubtype   = (BarrierSubtype) BarrierSubtype?.Clone()
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as BarrierTypeBarrierSubTypeBridge;

            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierTypeBarrierSubTypeBridge other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check whether the products' properties are equal.           
            if (!AdvEqual(BarrierSubTypeId, other.BarrierSubTypeId))
            {
                return false;
            }

            if (!AdvEqual(BarrierSubtype, other.BarrierSubtype))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
