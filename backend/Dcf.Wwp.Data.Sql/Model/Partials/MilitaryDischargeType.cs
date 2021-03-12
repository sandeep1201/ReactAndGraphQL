using Dcf.Wwp.Model.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class MilitaryDischargeType : BaseCommonModel, IMilitaryDischargeType, IEquatable<MilitaryDischargeType>
    {
        #region ICloneable

        public new object Clone()
        {
            var md = new MilitaryDischargeType();

            md.Id        = this.Id;
            md.Name      = this.Name;
            md.SortOrder = this.SortOrder;

            return md;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as MilitaryDischargeType;
            return obj != null && Equals(obj);
        }

        public bool Equals(MilitaryDischargeType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)     &&
                   Name.Equals(other.Name) &&
                   SortOrder.Equals(other.SortOrder);
        }

        #endregion IEquatable<T>
    }
}
