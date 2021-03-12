using System.ComponentModel.DataAnnotations;


namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class MilitarySectionContract : BaseInformalAssessmentContract
    {
        public bool? HasTraining { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public int? RankId { get; set; }

        public string RankName { get; set; }

        [StringLength(200)]
        public string Rate { get; set; }

        public bool? IsCurrentlyEnlisted { get; set; }

        public string EnlistmentDate { get; set; }

        public string DischargeDate { get; set; }

        public int? DischargeTypeId { get; set; }

        public string DischargeTypeName { get; set; }

        [StringLength(400)]
        public string SkillsAndTraining { get; set; }

        public int? IsEligibleForBenefitsYesNoUnknown { get; set; }
        public string IsEligibleForBenefitsYesNoUnknownName { get; set; }

        [StringLength(400)]
        public string BenefitsDetails { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
