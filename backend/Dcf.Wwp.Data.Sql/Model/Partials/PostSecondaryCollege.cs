using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class PostSecondaryCollege : BaseCommonModel, IPostSecondaryCollege, IEquatable<PostSecondaryCollege>
    {
        IPostSecondaryEducationSection IPostSecondaryCollege.PostSecondaryEducationSection
        {
            get => PostSecondaryEducationSection;
            set => PostSecondaryEducationSection = (PostSecondaryEducationSection) value;
        }

        ISchoolCollegeEstablishment IPostSecondaryCollege.SchoolCollegeEstablishment
        {
            get => SchoolCollegeEstablishment;
            set => SchoolCollegeEstablishment = (SchoolCollegeEstablishment) value;
        }

        #region ICloneable

        public object Clone()
        {
            var pse = new PostSecondaryCollege();

            pse.Id                           = Id;
            pse.SchoolCollegeEstablishmentId = SchoolCollegeEstablishmentId;
            pse.HasGraduated                 = HasGraduated;
            pse.LastYearAttended             = LastYearAttended;
            pse.Semesters                    = Semesters;
            pse.Credits                      = Credits;
            pse.Details                      = Details;
            pse.IsDeleted                    = IsDeleted;
            pse.SchoolCollegeEstablishment   = (SchoolCollegeEstablishment) SchoolCollegeEstablishment?.Clone();

            // NOTE: We don't clone references to "parent" objects such as PostSecondaryEducationSection

            return pse;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as PostSecondaryCollege;
            return obj != null && Equals(obj);
        }

        public bool Equals(PostSecondaryCollege other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(SchoolCollegeEstablishmentId, other.SchoolCollegeEstablishmentId))
                return false;
            if (!AdvEqual(HasGraduated, other.HasGraduated))
                return false;
            if (!AdvEqual(LastYearAttended, other.LastYearAttended))
                return false;
            if (!AdvEqual(Semesters, other.Semesters))
                return false;
            if (!AdvEqual(Credits, other.Credits))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(SchoolCollegeEstablishment, other.SchoolCollegeEstablishment))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
