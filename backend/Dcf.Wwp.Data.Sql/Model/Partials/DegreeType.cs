using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class DegreeType : BaseEntity, IDegreeType, IEquatable<DegreeType>
    {
        ICollection<IPostSecondaryDegree> IDegreeType.PostSecondaryDegrees
        {
            get { return PostSecondaryDegrees.Cast<IPostSecondaryDegree>().ToList(); }

            set { PostSecondaryDegrees = value.Cast<PostSecondaryDegree>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var dt = new DegreeType();

            dt.Id   = this.Id;
            dt.Code = this.Code;
            dt.Name = this.Name;
            return dt;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as DegreeType;
            return obj != null && Equals(obj);
        }

        public bool Equals(DegreeType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)     &&
                   Code.Equals(other.Code) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
