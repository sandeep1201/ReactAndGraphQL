namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEducationAssessmentSectionRepository
    {
        IEducationAssessmentSection NewEducationAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}