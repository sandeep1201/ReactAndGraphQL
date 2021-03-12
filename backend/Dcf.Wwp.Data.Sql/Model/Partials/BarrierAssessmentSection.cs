using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierAssessmentSection : BaseAssessmentSection, IBarrierAssessmentSection, IEquatable<BarrierAssessmentSection>
    {
        ICollection<IInformalAssessment> IBarrierAssessmentSection.InformalAssessments
        {
            get => InformalAssessments.Cast<IInformalAssessment>().ToList();
            set => InformalAssessments = value.Cast<InformalAssessment>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new BarrierAssessmentSection();
            CloneAssessmentSectionProperites(clone);

            // NOTE: We don't clone references to "parent" objects or other reference collections.

            return clone;
        }

        #endregion

        #region IEquatable<T>

#pragma warning disable 659
        public override bool Equals(object other)
#pragma warning restore 659
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as BarrierAssessmentSection;

            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierAssessmentSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check whether the base properties are equal.
            if (!AssessmentSectionProperitesEqual(other))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
