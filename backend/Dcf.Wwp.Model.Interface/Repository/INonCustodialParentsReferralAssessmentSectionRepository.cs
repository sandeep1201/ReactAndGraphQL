namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialParentsReferralAssessmentSectionRepository
    {
        INonCustodialParentsReferralAssessmentSection NewNonCustodialParentsReferralAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}