namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IHousingAssessmentSectionRepository
    {
        IHousingAssessmentSection NewHousingAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}