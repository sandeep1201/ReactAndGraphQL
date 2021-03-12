using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IInformalAssessment : ICommonDelModel
    {
        int ParticipantId { get; set; }
        int? LanguageAssessmentSectionId { get; set; }
        int? WorkHistoryAssessmentSectionId { get; set; }
        int? WorkProgramAssessmentSectionId { get; set; }
        int? EducationAssessmentSectionId { get; set; }
        int? PostSecondaryEducationAssessmentSectionId { get; set; }
        int? MilitaryTrainingAssessmentSectionId { get; set; }
        int? HousingAssessmentSectionId { get; set; }
        int? TransportationAssessmentSectionId { get; set; }
        int? LegalIssuesAssessmentSectionId { get; set; }
        int? BarriersAssessmentSectionId { get; set; }
        int? ChildYouthSupportsAssessmentSectionId { get; set; }
        int? FamilyBarriersAssessmentSectionId { get; set; }
        int? NonCustodialParentsAssessmentSectionId { get; set; }
        int? NonCustodialParentsReferralAssessmentSectionId { get; set; }

        DateTime? EndDate { get; set; }
        DateTime CreatedDate { get; set; }

        IAssessmentType AssessmentType { get; set; }
        IParticipant Participant { get; set; }

        ILanguageAssessmentSection LanguageAssessmentSection { get; set; }
        IWorkHistoryAssessmentSection WorkHistoryAssessmentSection { get; set; }
        IWorkProgramAssessmentSection WorkProgramAssessmentSection { get; set; }
        IEducationAssessmentSection EducationAssessmentSection { get; set; }
        IPostSecondaryEducationAssessmentSection PostSecondaryEducationAssessmentSection { get; set; }
        IMilitaryTrainingAssessmentSection MilitaryTrainingAssessmentSection { get; set; }
        IHousingAssessmentSection HousingAssessmentSection { get; set; }
        ITransportationAssessmentSection TransportationAssessmentSection { get; set; }
        ILegalIssuesAssessmentSection LegalIssuesAssessmentSection { get; set; }
        IBarrierAssessmentSection BarrierAssessmentSection { get; set; }
        IChildYouthSupportsAssessmentSection ChildYouthSupportsAssessmentSection { get; set; }
        IFamilyBarriersAssessmentSection FamilyBarriersAssessmentSection { get; set; }
        INonCustodialParentsAssessmentSection NonCustodialParentsAssessmentSection { get; set; }
        INonCustodialParentsReferralAssessmentSection NonCustodialParentsReferralAssessmentSection { get; set; }
    }
}
