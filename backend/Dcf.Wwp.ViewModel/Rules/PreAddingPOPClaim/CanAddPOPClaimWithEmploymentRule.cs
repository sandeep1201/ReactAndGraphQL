using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface.Constants;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobRetentionPOPClaimAdd)]
    [Name("CanAddPOPClaimWithEmploymentRule")]
    public class CanAddPOPClaimWithEmploymentRule : Rule
    {
        public override void Define()
        {
            bool?                   isValidEmployment       = null;
            CodeLevelMessageContext codeLevelMessageContext = null;

            When()
                .Match(() => isValidEmployment,       empValidity => empValidity == false)
                .Match(() => codeLevelMessageContext, c => c                     != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(codeLevelMessageContext, RuleReason.POPE9314, CodeLevel.Error));
        }
    }
}
