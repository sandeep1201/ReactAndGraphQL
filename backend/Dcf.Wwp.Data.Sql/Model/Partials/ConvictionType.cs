using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ConvictionType : BaseEntity, IConvictionType, IEquatable<ConvictionType>
    {
        ICollection<IConviction> IConvictionType.Convictions
        {
            get { return Convictions.Cast<IConviction>().ToList(); }

            set { Convictions = value.Cast<Conviction>().ToList(); }
        }

        ICollection<IPendingCharge> IConvictionType.PendingCharges
        {
            get { return PendingCharges.Cast<IPendingCharge>().ToList(); }

            set { PendingCharges = value.Cast<PendingCharge>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var ct = new ConvictionType();

            ct.Id        = this.Id;
            ct.SortOrder = this.SortOrder;
            ct.Name      = this.Name;
            return ct;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ConvictionType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ConvictionType other)
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
