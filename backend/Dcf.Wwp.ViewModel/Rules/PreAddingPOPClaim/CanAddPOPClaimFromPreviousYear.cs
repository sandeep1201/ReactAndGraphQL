using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using SmartFormat;
using Dcf.Wwp.Model.Interface.Constants;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentPOPClaimAdd)]
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Tag(RuleReason.JobAttainmentWithHighWage)]
    [Name("CanAddPOPClaimFromPreviousYear")]
    public class CanAddPOPClaimFromPreviousYear : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext codeLevelMessageContext = null;
            POPClaimContract        contract                = null;
            int?                    year                    = null;

            When()
                .Match(() => year, y => y != null)
                .Match(() => contract, c => c.ClaimPeriodBeginDate != null && (c.ClaimPeriodBeginDate.GetValueOrDefault().Year != year || c.POPClaimEmployments
                                                                                                                                           .FirstOrDefault(i => i.IsPrimary)
                                                                                                                                           .JobBeginDate.ToDateMonthDayYear().Year != year))
                .Match(() => codeLevelMessageContext, c => c != null);

            Then()
                .Do(ctx => CreateMsg(codeLevelMessageContext, RuleReason.POPPEBD, year))
                .Do(ctx => CreateMsg(codeLevelMessageContext, RuleReason.POPCPBD, year));
        }

        private void CreateMsg(CodeLevelMessageContext codeLevelMessageContext, string code, int? year)
        {
            var msg = Smart.Format(codeLevelMessageContext.GetMessageByCode(code), new object[] { year });

            codeLevelMessageContext.AddMessageCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
