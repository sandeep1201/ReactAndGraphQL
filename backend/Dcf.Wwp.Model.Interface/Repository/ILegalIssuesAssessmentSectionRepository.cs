namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ILegalIssuesAssessmentSectionRepository
    {
        ILegalIssuesAssessmentSection NewLegalIssuesAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}