using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class PostSecondaryEducationSection : BaseEntity, IPostSecondaryEducationSection,
                                                         IEquatable<PostSecondaryEducationSection>
    {
        ICollection<IPostSecondaryCollege> IPostSecondaryEducationSection.PostSecondaryColleges
        {
            get => (from x in PostSecondaryColleges where x.IsDeleted == false select x).Cast<IPostSecondaryCollege>()
                                                                                        .ToList();

            set => PostSecondaryColleges = value.Cast<PostSecondaryCollege>().ToList();
        }

        ICollection<IPostSecondaryCollege> IPostSecondaryEducationSection.AllPostSecondaryColleges
        {
            get => PostSecondaryColleges.Cast<IPostSecondaryCollege>().ToList();

            set => PostSecondaryColleges = value.Cast<PostSecondaryCollege>().ToList();
        }

        ICollection<IPostSecondaryDegree> IPostSecondaryEducationSection.PostSecondaryDegrees
        {
            get => (from x in PostSecondaryDegrees where x.IsDeleted == false select x).Cast<IPostSecondaryDegree>()
                                                                                       .ToList();

            set => PostSecondaryDegrees = value.Cast<PostSecondaryDegree>().ToList();
        }

        ICollection<IPostSecondaryDegree> IPostSecondaryEducationSection.AllPostSecondaryDegrees
        {
            get => PostSecondaryDegrees.Cast<IPostSecondaryDegree>().ToList();

            set => PostSecondaryDegrees = (ICollection<PostSecondaryDegree>) value;
        }

        ICollection<IPostSecondaryLicense> IPostSecondaryEducationSection.PostSecondaryLicenses
        {
            get => (from x in PostSecondaryLicenses where x.IsDeleted == false select x).Cast<IPostSecondaryLicense>()
                                                                                        .ToList();

            set => PostSecondaryLicenses = value.Cast<PostSecondaryLicense>().ToList();
        }

        ICollection<IPostSecondaryLicense> IPostSecondaryEducationSection.AllPostSecondaryLicenses
        {
            get => PostSecondaryLicenses.Cast<IPostSecondaryLicense>().ToList();

            set => PostSecondaryLicenses = value.Cast<PostSecondaryLicense>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var pse = new PostSecondaryEducationSection();

            pse.Id                                = Id;
            pse.DidAttendCollege                  = DidAttendCollege;
            pse.IsWorkingOnLicensesOrCertificates = IsWorkingOnLicensesOrCertificates;
            pse.DoesHaveDegrees                   = DoesHaveDegrees;
            pse.Notes                             = Notes;
            pse.PostSecondaryColleges             = PostSecondaryColleges.Select(x => (PostSecondaryCollege) x.Clone()).ToList();
            pse.PostSecondaryDegrees              = PostSecondaryDegrees.Select(y => (PostSecondaryDegree) y.Clone()).ToList();
            pse.PostSecondaryLicenses             = PostSecondaryLicenses.Select(z => (PostSecondaryLicense) z.Clone()).ToList();

            return pse;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as PostSecondaryEducationSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(PostSecondaryEducationSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(DidAttendCollege, other.DidAttendCollege))
                return false;
            if (!AdvEqual(IsWorkingOnLicensesOrCertificates, other.IsWorkingOnLicensesOrCertificates))
                return false;
            if (!AdvEqual(DoesHaveDegrees, other.DoesHaveDegrees))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (AreBothNotNull(PostSecondaryColleges, other.PostSecondaryColleges) &&
                !PostSecondaryColleges.OrderBy(x => x.Id).SequenceEqual(other.PostSecondaryColleges.OrderBy(x => x.Id)))
                return false;
            if (AreBothNotNull(PostSecondaryDegrees, other.PostSecondaryDegrees) &&
                !PostSecondaryDegrees.OrderBy(x => x.Id).SequenceEqual(other.PostSecondaryDegrees.OrderBy(x => x.Id)))
                return false;
            if (AreBothNotNull(PostSecondaryLicenses, other.PostSecondaryLicenses) &&
                !PostSecondaryLicenses.OrderBy(x => x.Id).SequenceEqual(other.PostSecondaryLicenses.OrderBy(x => x.Id)))
                return false;

            return true;
        }

        #endregion IEquatable<T>

        #region IComplexModel

        public void SetModifiedOnComplexProperties<T>(T cloned, string user, DateTime modDate)
            where T : class, ICloneable, ICommonModel
        {
            // We don't need to set modified on null objects.
            if (cloned == null) return;

            Debug.Assert(cloned is IPostSecondaryEducationSection, "cloned is not IChildYouthSection");

            var clone = (IPostSecondaryEducationSection) cloned;

            if (AreBothNotNull(PostSecondaryColleges, clone.PostSecondaryColleges))
            {
                var first  = PostSecondaryColleges.OrderBy(x => x.Id).ToList();
                var second = clone.PostSecondaryColleges.OrderBy(x => x.Id).ToList();

                var i = 0;
                foreach (var cysc1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (cysc1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (i < second.Count)
                        {
                            var cysc2 = second[i];

                            if (!cysc1.Equals(cysc2))
                            {
                                cysc1.ModifiedBy   = user;
                                cysc1.ModifiedDate = modDate;
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            cysc1.ModifiedBy   = user;
                            cysc1.ModifiedDate = modDate;
                        }
                    }

                    i++;
                }
            }

            if (AreBothNotNull(PostSecondaryDegrees, clone.PostSecondaryDegrees))
            {
                var first  = PostSecondaryDegrees.OrderBy(x => x.Id).ToList();
                var second = clone.PostSecondaryDegrees.OrderBy(x => x.Id).ToList();

                var j = 0;
                foreach (var cysc1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (cysc1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (j < second.Count)
                        {
                            var cysc2 = second[j];

                            if (!cysc1.Equals(cysc2))
                            {
                                cysc1.ModifiedBy   = user;
                                cysc1.ModifiedDate = modDate;
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            cysc1.ModifiedBy   = user;
                            cysc1.ModifiedDate = modDate;
                        }
                    }

                    j++;
                }
            }

            if (AreBothNotNull(PostSecondaryLicenses, clone.PostSecondaryLicenses))
            {
                var first  = PostSecondaryLicenses.OrderBy(x => x.Id).ToList();
                var second = clone.PostSecondaryLicenses.OrderBy(x => x.Id).ToList();

                var k = 0;
                foreach (var cysc1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (cysc1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (k < second.Count)
                        {
                            var cysc2 = second[k];

                            if (!cysc1.Equals(cysc2))
                            {
                                cysc1.ModifiedBy   = user;
                                cysc1.ModifiedDate = modDate;
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            cysc1.ModifiedBy   = user;
                            cysc1.ModifiedDate = modDate;
                        }
                    }

                    k++;
                }
            }
        }

        #endregion IComplexModel
    }
}
