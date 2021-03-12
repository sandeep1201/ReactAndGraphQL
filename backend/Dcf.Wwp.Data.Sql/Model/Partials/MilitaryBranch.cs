using Dcf.Wwp.Model.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class MilitaryBranch : BaseCommonModel, IMilitaryBranch, IEquatable<MilitaryBranch>
    {
        #region ICloneable

        public new object Clone()
        {
            var m = new MilitaryBranch();

            m.Id   = this.Id;
            m.Name = this.Name;
            return m;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as MilitaryBranch;
            return obj != null && Equals(obj);
        }

        public bool Equals(MilitaryBranch other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
