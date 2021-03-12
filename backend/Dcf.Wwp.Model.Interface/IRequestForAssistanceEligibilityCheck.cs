namespace Dcf.Wwp.Model.Interface
{
    public interface IRequestForAssistanceEligibilityCheck : ICommonDelModel
    {
        int                   RequestForAssistanceId { get; set; }
        bool                  IsEligible             { get; set; }
        string                ResultCodes            { get; set; }

        IRequestForAssistance RequestForAssistance   { get; set; }
    }
}
