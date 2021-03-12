using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Model.Interface.Constants;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentWithHighWage)]
    [Name("CanAddPOPClaimWithStartingWageRule")]
    public class CanAddPOPClaimWithStartingWageRule : Rule
    {
        public override void Define()
        {
            POPClaimDomain.StartingWageHourDetails startingWageHourDetails = null;
            CodeLevelMessageContext                codeLevelMessageContext = null;


            When()
                .Match(() => startingWageHourDetails, stw => stw.selectedEmploymentsStartingWageHourUnit == "Hour" && stw.selectedEmploymentsStartingWageHourValue < stw.startingWage )
                .Match(() => codeLevelMessageContext, c => c                                             != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(codeLevelMessageContext, RuleReason.POPPEW, CodeLevel.Error));
        }
    }
}
