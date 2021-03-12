using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class MilitaryTrainingSection : BaseCommonModel, IMilitaryTrainingSection, IEquatable<MilitaryTrainingSection>
    {
        IMilitaryBranch IMilitaryTrainingSection.MilitaryBranch
        {
            get { return MilitaryBranch; }
            set { MilitaryBranch = (MilitaryBranch) value; }
        }

        IMilitaryRank IMilitaryTrainingSection.MilitaryRank
        {
            get { return MilitaryRank; }
            set { MilitaryRank = (MilitaryRank) value; }
        }

        IMilitaryDischargeType IMilitaryTrainingSection.MilitaryDischargeType
        {
            get { return MilitaryDischargeType; }
            set { MilitaryDischargeType = (MilitaryDischargeType) value; }
        }

        IPolarLookup IMilitaryTrainingSection.IsEligibleForBenefitsPolarLookup
        {
            get { return IsEligibleForBenefitsPolarLookup; }
            set { IsEligibleForBenefitsPolarLookup = (PolarLookup) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var ms = new MilitaryTrainingSection();

            ms.Id                      = this.Id;
            ms.DoesHaveTraining        = this.DoesHaveTraining;
            ms.MilitaryBranchId        = this.MilitaryBranchId;
            ms.MilitaryRankId          = this.MilitaryRankId;
            ms.Rate                    = this.Rate;
            ms.MilitaryDischargeTypeId = this.MilitaryDischargeTypeId;
            ms.EnlistmentDate          = this.EnlistmentDate;
            ms.IsCurrentlyEnlisted     = this.IsCurrentlyEnlisted;
            ms.DischargeDate           = this.DischargeDate;
            ms.PolarLookupId           = this.PolarLookupId;
            ms.BenefitsDetails         = this.BenefitsDetails;
            ms.SkillsAndTraining       = this.SkillsAndTraining;
            ms.Notes                   = this.Notes;

            ms.MilitaryDischargeType            = (MilitaryDischargeType) this.MilitaryDischargeType?.Clone();
            ms.MilitaryRank                     = (MilitaryRank) this.MilitaryRank?.Clone();
            ms.MilitaryBranch                   = (MilitaryBranch) this.MilitaryBranch?.Clone();
            ms.IsEligibleForBenefitsPolarLookup = (PolarLookup) this.IsEligibleForBenefitsPolarLookup?.Clone();

            return ms;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as MilitaryTrainingSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(MilitaryTrainingSection other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(DoesHaveTraining, other.DoesHaveTraining))
                return false;
            if (!AdvEqual(MilitaryRankId, other.MilitaryRankId))
                return false;
            if (!AdvEqual(MilitaryBranchId, other.MilitaryBranchId))
                return false;
            if (!AdvEqual(Rate, other.Rate))
                return false;
            if (!AdvEqual(EnlistmentDate, other.EnlistmentDate))
                return false;
            if (!AdvEqual(DischargeDate, other.DischargeDate))
                return false;
            if (!AdvEqual(IsCurrentlyEnlisted, other.IsCurrentlyEnlisted))
                return false;
            if (!AdvEqual(MilitaryDischargeTypeId, other.MilitaryDischargeTypeId))
                return false;
            if (!AdvEqual(SkillsAndTraining, other.SkillsAndTraining))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(PolarLookupId, other.PolarLookupId))
                return false;
            if (!AdvEqual(BenefitsDetails, other.BenefitsDetails))
                return false;

            // Are Equal.
            return true;
        }

        #endregion IEquatable<T>
    }
}
