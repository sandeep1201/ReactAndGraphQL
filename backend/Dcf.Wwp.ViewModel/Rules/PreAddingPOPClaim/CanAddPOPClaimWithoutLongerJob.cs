using System;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface.Constants;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentPOPClaimAdd)]
    [Tag(RuleReason.JobRetentionPOPClaimAdd)]
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Tag(RuleReason.JobAttainmentWithHighWage)]
    [Name("CanAddPOPClaimWithoutLongerJob")]
    public class CanAddPOPClaimWithoutLongerJob : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext    codeLevelMessageContext = null;
            POPClaimEmploymentContract primaryJob              = null;
            DateTime?                  cdoDateOrToday          = null;

            When()
                .Match(() => cdoDateOrToday, c => c != null)
                .Match(() => primaryJob, p =>  p.JobBeginDate.ToDateMonthDayYear().DateDiff(p.JobEndDate != null
                                                                                                ? p.JobEndDate.ToDateMonthDayYear()
                                                                                                : cdoDateOrToday.GetValueOrDefault()) < 31)
                .Match(() => codeLevelMessageContext, c => c != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(codeLevelMessageContext, RuleReason.POPJB31, CodeLevel.Error));
        }
    }
}
