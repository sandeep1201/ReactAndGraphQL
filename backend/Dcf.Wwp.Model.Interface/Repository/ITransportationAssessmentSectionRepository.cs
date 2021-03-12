namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ITransportationAssessmentSectionRepository
    {
        ITransportationAssessmentSection NewTransportationAssessmentSection(IInformalAssessment parentAssessment, string user);
    }
}