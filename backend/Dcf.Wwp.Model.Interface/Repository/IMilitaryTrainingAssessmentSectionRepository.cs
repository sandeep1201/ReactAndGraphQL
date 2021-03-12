namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IMilitaryTrainingAssessmentSectionRepository
    {
        IMilitaryTrainingAssessmentSection NewMilitaryTrainingAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}