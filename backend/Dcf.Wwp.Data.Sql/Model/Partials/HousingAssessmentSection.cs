﻿using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class HousingAssessmentSection : BaseAssessmentSection, IHousingAssessmentSection, IEquatable<HousingAssessmentSection>
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new HousingAssessmentSection();
            base.CloneAssessmentSectionProperites(clone);

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

            var obj = other as HousingAssessmentSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(HousingAssessmentSection other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the base properties are equal.
            if (!base.AssessmentSectionProperitesEqual(other))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}