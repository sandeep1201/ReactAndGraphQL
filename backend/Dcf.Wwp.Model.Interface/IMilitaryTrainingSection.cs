using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IMilitaryTrainingSection : ICommonDelModel, ICloneable
    {
        bool? DoesHaveTraining { get; set; }
        int? MilitaryRankId { get; set; }
        int? MilitaryBranchId { get; set; }
        string Rate { get; set; }
        DateTime? EnlistmentDate { get; set; }
        DateTime? DischargeDate { get; set; }
        bool? IsCurrentlyEnlisted { get; set; }
        int? MilitaryDischargeTypeId { get; set; }
        string SkillsAndTraining { get; set; }
        int? PolarLookupId { get; set; }
        string BenefitsDetails { get; set; }
        string Notes { get; set; }
        IMilitaryBranch MilitaryBranch { get; set; }
        IMilitaryDischargeType MilitaryDischargeType { get; set; }
        IMilitaryRank MilitaryRank { get; set; }
        IPolarLookup IsEligibleForBenefitsPolarLookup { get; set; }
    }
}
