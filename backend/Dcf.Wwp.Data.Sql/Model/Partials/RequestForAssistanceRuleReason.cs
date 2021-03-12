using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistanceRuleReason : BaseCommonModel, IRequestForAssistanceRuleReason
    {
        IRequestForAssistance IRequestForAssistanceRuleReason.RequestForAssistance
        {
            get { return RequestForAssistance; }
            set { RequestForAssistance = (RequestForAssistance) value; }
        }

        IRuleReason IRequestForAssistanceRuleReason.RuleReason
        {
            get { return RuleReason; }
            set { RuleReason = (RuleReason) value; }
        }
    }
}
