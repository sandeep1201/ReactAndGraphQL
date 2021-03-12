using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class MilitaryTrainingSection
    {
        #region Properties

        public int       ParticipantId           { get; set; }
        public bool?     DoesHaveTraining        { get; set; }
        public int?      MilitaryRankId          { get; set; }
        public int?      MilitaryBranchId        { get; set; }
        public string    Rate                    { get; set; }
        public int?      YearsEnlisted           { get; set; }
        public DateTime? EnlistmentDate          { get; set; }
        public DateTime? DischargeDate           { get; set; }
        public bool?     IsCurrentlyEnlisted     { get; set; }
        public int?      MilitaryDischargeTypeId { get; set; }
        public string    SkillsAndTraining       { get; set; }
        public string    Notes                   { get; set; }
        public int?      PolarLookupId           { get; set; }
        public string    BenefitsDetails         { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant           Participant                      { get; set; }
        public virtual MilitaryRank          MilitaryRank                     { get; set; }
        public virtual MilitaryBranch        MilitaryBranch                   { get; set; }
        public virtual MilitaryDischargeType MilitaryDischargeType            { get; set; }
        public virtual PolarLookup           IsEligibleForBenefitsPolarLookup { get; set; }

        #endregion
    }
}
