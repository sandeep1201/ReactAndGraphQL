namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWorkProgramAssessmentSectionRepository
    {
        IWorkProgramAssessmentSection NewWorkProgramAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}