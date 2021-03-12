using Dcf.Wwp.Model.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ChildCareArrangement : BaseCommonModel, IChildCareArrangement, IEquatable<ChildCareArrangement>
    {
        #region ICloneable

        public object Clone()
        {
            var cca = new ChildCareArrangement
                      {
                          Id        = this.Id,
                          Name      = this.Name,
                          SortOrder = this.SortOrder,
                          IsDeleted = this.IsDeleted
                      };

            return cca;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ChildCareArrangement;
            return obj != null && Equals(obj);
        }

        public bool Equals(ChildCareArrangement other)
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
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
