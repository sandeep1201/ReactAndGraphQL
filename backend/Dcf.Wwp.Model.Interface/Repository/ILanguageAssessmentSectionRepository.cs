namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ILanguageAssessmentSectionRepository
    {
        ILanguageAssessmentSection NewLanguageAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}