
namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IChildYouthSupportsAssessmentSectionRepository
    {
        IChildYouthSupportsAssessmentSection NewChildYouthSupportsAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}