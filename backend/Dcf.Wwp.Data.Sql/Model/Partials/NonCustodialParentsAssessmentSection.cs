using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    partial class NonCustodialParentsAssessmentSection : BaseCommonModel, INonCustodialParentsAssessmentSection, IEquatable<NonCustodialParentsAssessmentSection>
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialParentsAssessmentSection()
                        {
                            Id              = this.Id,
                            ReviewCompleted = this.ReviewCompleted,
                            ActionDetails   = this.ActionDetails,
                            IsDeleted       = this.IsDeleted,
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonCustodialParentsAssessmentSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialParentsAssessmentSection other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ReviewCompleted, other.ReviewCompleted))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
