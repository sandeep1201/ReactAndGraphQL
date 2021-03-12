using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class PostSecondaryDegree : BaseCommonModel, IPostSecondaryDegree, IEquatable<PostSecondaryDegree>
    {
        IDegreeType IPostSecondaryDegree.DegreeType
        {
            get => DegreeType;
            set => DegreeType = (DegreeType) value;
        }

        IPostSecondaryEducationSection IPostSecondaryDegree.PostSecondaryEducationSection
        {
            get => PostSecondaryEducationSection;
            set => PostSecondaryEducationSection = (PostSecondaryEducationSection) value;
        }

        #region ICloneable

        public object Clone()
        {
            var psd = new PostSecondaryDegree();

            psd.Id           = Id;
            psd.Name         = Name;
            psd.College      = College;
            psd.DegreeTypeId = DegreeTypeId;
            psd.YearAttained = YearAttained;
            psd.IsDeleted    = IsDeleted;
            psd.DegreeType   = (DegreeType) DegreeType?.Clone();

            // NOTE: We don't clone references to "parent" objects such as PostSecondaryEducationSection

            return psd;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as PostSecondaryDegree;
            return obj != null && Equals(obj);
        }

        public bool Equals(PostSecondaryDegree other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;


            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(College, other.College))
                return false;
            if (!AdvEqual(DegreeTypeId, other.DegreeTypeId))
                return false;
            if (!AdvEqual(YearAttained, other.YearAttained))
                return false;
            if (!AdvEqual(DegreeType, other.DegreeType))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
