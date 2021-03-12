namespace Dcf.Wwp.Model.Interface
{
    public interface IRequestForAssistanceRuleReason : ICommonModelFinal
    {
        int                   RequestForAssistanceId { get; set; }
        IRequestForAssistance RequestForAssistance   { get; set; }

        int                   RuleReasonId           { get; set; }
        IRuleReason           RuleReason             { get; set; }
    }
}
