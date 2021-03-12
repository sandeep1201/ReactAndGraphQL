using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class InformalAssessment : BaseEntity, IInformalAssessment
    {
        IAssessmentType IInformalAssessment.AssessmentType
        {
            get { return AssessmentType; }

            set { AssessmentType = (AssessmentType) value; }
        }

        IParticipant IInformalAssessment.Participant
        {
            get { return Participant; }

            set { Participant = (Participant) value; }
        }

        #region Assessment Sections

        ILanguageAssessmentSection IInformalAssessment.LanguageAssessmentSection
        {
            get { return LanguageAssessmentSection; }

            set { LanguageAssessmentSection = (LanguageAssessmentSection) value; }
        }

        IWorkHistoryAssessmentSection IInformalAssessment.WorkHistoryAssessmentSection
        {
            get { return WorkHistoryAssessmentSection; }

            set { WorkHistoryAssessmentSection = (WorkHistoryAssessmentSection) value; }
        }

        IWorkProgramAssessmentSection IInformalAssessment.WorkProgramAssessmentSection
        {
            get { return WorkProgramAssessmentSection; }

            set { WorkProgramAssessmentSection = (WorkProgramAssessmentSection) value; }
        }

        #endregion Assessment Sections


        IEducationAssessmentSection IInformalAssessment.EducationAssessmentSection
        {
            get { return EducationAssessmentSection; }

            set { EducationAssessmentSection = (EducationAssessmentSection) value; }
        }

        IMilitaryTrainingAssessmentSection IInformalAssessment.MilitaryTrainingAssessmentSection
        {
            get { return MilitaryTrainingAssessmentSection; }

            set { MilitaryTrainingAssessmentSection = (MilitaryTrainingAssessmentSection) value; }
        }

        IChildYouthSupportsAssessmentSection IInformalAssessment.ChildYouthSupportsAssessmentSection
        {
            get { return ChildYouthSupportsAssessmentSection; }

            set { ChildYouthSupportsAssessmentSection = (ChildYouthSupportsAssessmentSection) value; }
        }

        ITransportationAssessmentSection IInformalAssessment.TransportationAssessmentSection
        {
            get { return TransportationAssessmentSection; }

            set { TransportationAssessmentSection = (TransportationAssessmentSection) value; }
        }

        ILegalIssuesAssessmentSection IInformalAssessment.LegalIssuesAssessmentSection
        {
            get { return LegalIssuesAssessmentSection; }

            set { LegalIssuesAssessmentSection = (LegalIssuesAssessmentSection) value; }
        }

        IHousingAssessmentSection IInformalAssessment.HousingAssessmentSection
        {
            get { return HousingAssessmentSection; }

            set { HousingAssessmentSection = (HousingAssessmentSection) value; }
        }

        IFamilyBarriersAssessmentSection IInformalAssessment.FamilyBarriersAssessmentSection
        {
            get { return FamilyBarriersAssessmentSection; }
            set { FamilyBarriersAssessmentSection = (FamilyBarriersAssessmentSection) value; }
        }

        IBarrierAssessmentSection IInformalAssessment.BarrierAssessmentSection
        {
            get { return BarrierAssessmentSection; }
            set { BarrierAssessmentSection = (BarrierAssessmentSection) value; }
        }

        IPostSecondaryEducationAssessmentSection IInformalAssessment.PostSecondaryEducationAssessmentSection
        {
            get { return PostSecondaryEducationAssessmentSection; }
            set { PostSecondaryEducationAssessmentSection = (PostSecondaryEducationAssessmentSection) value; }
        }

        INonCustodialParentsAssessmentSection IInformalAssessment.NonCustodialParentsAssessmentSection
        {
            get { return NonCustodialParentsAssessmentSection; }
            set { NonCustodialParentsAssessmentSection = (NonCustodialParentsAssessmentSection) value; }
        }

        INonCustodialParentsReferralAssessmentSection IInformalAssessment.NonCustodialParentsReferralAssessmentSection
        {
            get { return NonCustodialParentsReferralAssessmentSection; }
            set { NonCustodialParentsReferralAssessmentSection = (NonCustodialParentsReferralAssessmentSection) value; }
        }
    }
}
