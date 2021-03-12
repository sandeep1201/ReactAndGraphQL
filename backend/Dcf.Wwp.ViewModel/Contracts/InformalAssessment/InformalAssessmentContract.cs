using System;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class InformalAssessmentContract
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime? SubmitDate { get; set; }

        public LanguageSectionContract LanguagesSection { get; set; }

        public WorkHistorySectionContract WorkHistorySection { get; set; }

        public WorkProgramSectionContract WorkProgramSection { get; set; }

        public EducationHistorySectionContract EducationSection { get; set; }

        public PostSecondarySectionContract PostSecondarySection { get; set; }

        public MilitarySectionContract MilitarySection { get; set; }

        public HousingSectionContract HousingSection { get; set; }

        public TransportationSectionContract TransportationSection { get; set; }

        public LegalIssuesSectionContract LegalIssuesSection { get; set; }

        public ParticipantBarrierSectionContract ParticipantBarriersSection { get; set; }

        public ChildYouthSupportsSectionContract ChildYouthSupportsSection { get; set; }

        public FamilyBarriersSectionContract FamilyBarriersSection { get; set; }

        public NonCustodialParentAssessmentContract NonCustodialParentsSection { get; set; }

        public NonCustodialParentReferralAssessmentContract NonCustodialParentsReferralSection { get; set; }
    }
}