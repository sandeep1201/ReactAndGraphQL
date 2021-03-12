using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class NRSType : BaseCommonModel, INRSType, IEquatable<NRSType>
    {
        #region ICloneable

        public object Clone()
        {
            var et = new NRSType();
            et.Id        = this.Id;
            et.SortOrder = this.SortOrder;
            et.Rating    = this.Rating;
            et.IsDeleted = this.IsDeleted;
            et.Name      = this.Name;
            return et;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NRSType;
            return obj != null && Equals(obj);
        }

        public bool Equals(NRSType other)
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
            if (!AdvEqual(Rating, other.Rating))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
