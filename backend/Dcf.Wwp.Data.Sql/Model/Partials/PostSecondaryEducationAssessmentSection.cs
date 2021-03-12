using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PostSecondaryEducationAssessmentSection : BaseCommonModel, IPostSecondaryEducationAssessmentSection, IEquatable<PostSecondaryEducationAssessmentSection>
    {
        ICollection<IInformalAssessment> IPostSecondaryEducationAssessmentSection.InformalAssessments
        {
            get { return InformalAssessments.Cast<IInformalAssessment>().ToList(); }
            set { InformalAssessments = value.Cast<InformalAssessment>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new PostSecondaryEducationAssessmentSection()
                        {
                            Id              = this.Id,
                            ReviewCompleted = this.ReviewCompleted,
                            ActionDetails   = this.ActionDetails,
                            IsDeleted       = this.IsDeleted
                        };

            // NOTE: We don't clone references to "parent" objects or other reference collections.

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as PostSecondaryEducationAssessmentSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(PostSecondaryEducationAssessmentSection other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ReviewCompleted, other.ReviewCompleted))
                return false;
            if (!AdvEqual(ActionDetails, other.ActionDetails))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
