namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRequestForAssistanceRuleReasonRepository
    {
        IRequestForAssistanceRuleReason NewRfaEligibility(IRequestForAssistance rfa, string eligibilityCode, string user);
        void                            DeleteAllRfaEligibilityRows(int rfaId);
    }
}
