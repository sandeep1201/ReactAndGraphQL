namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWorkHistoryAssessmentSectionRepository
    {
        IWorkHistoryAssessmentSection NewWorkHistoryAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}