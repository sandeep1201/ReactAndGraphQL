using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class SPLType : BaseCommonModel, ISPLType, IEquatable<SPLType>
    {
        #region ICloneable

        public object Clone()
        {
            var et = new SPLType();
            et.Id        = this.Id;
            et.Name      = this.Name;
            et.Rating    = this.Rating;
            et.SortOrder = this.SortOrder;
            et.IsDeleted = this.IsDeleted;

            return et;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as SPLType;
            return obj != null && Equals(obj);
        }

        public bool Equals(SPLType other)
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
