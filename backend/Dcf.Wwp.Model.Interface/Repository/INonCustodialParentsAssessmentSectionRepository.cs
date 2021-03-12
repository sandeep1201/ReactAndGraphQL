
namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialParentsAssessmentSectionRepository
    {
        INonCustodialParentsAssessmentSection NewNonCustodialParentsAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}
