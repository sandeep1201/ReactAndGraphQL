using System;
using System.Diagnostics;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChildYouthSupportsAssessmentSection : BaseAssessmentSection, IChildYouthSupportsAssessmentSection, IEquatable<ChildYouthSupportsAssessmentSection>
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new ChildYouthSupportsAssessmentSection();
            base.CloneAssessmentSectionProperites(clone);

            // NOTE: We don't clone references to "parent" objects or other reference collections.

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

#pragma warning disable 659
        public override bool Equals(object other)
#pragma warning restore 659
        {
            if (other == null)
                return false;

            var obj = other as ChildYouthSupportsAssessmentSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(ChildYouthSupportsAssessmentSection other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the base properties are equal.
            if (!base.AssessmentSectionProperitesEqual(other))
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

            Debug.Assert(cloned is IChildYouthSupportsAssessmentSection, "cloned is not IChildYouthSupportsAssessmentSection");

            var clone = (IChildYouthSupportsAssessmentSection) cloned;
        }

        #endregion IComplexModel
    }
}
