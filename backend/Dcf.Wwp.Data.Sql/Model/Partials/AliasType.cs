using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class AliasType : BaseCommonModel, IAliasType, IEquatable<AliasType>
    {
        #region ICloneable

        public object Clone()
        {
            return new AliasType
                   {
                       Id        = Id,
                       IsDeleted = IsDeleted,
                       Name      = Name
                   };
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as AliasType;
            return obj != null && Equals(obj);
        }

        public bool Equals(AliasType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(Code, other.Code))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
