using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.DataAccess.Models;
using NRules.Fluent.Dsl;
using SmartFormat;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Name("CanAddPOPClaimWIthTimeLimitRule")]
    public class CanAddPOPClaimWIthTimeLimitRule : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext    codeLevelMessageContext = null;
            Participant                participant             = null;
            POPClaimEmploymentContract primaryJob              = null;

            When()
                .Match(() => primaryJob, pj => pj != null)
                .Match(() => participant, p => p.TimeLimits != null && p.TimeLimits
                                                                        .Count(i => i.EffectiveMonth    <= primaryJob.JobBeginDate.ToDateMonthDayYear()
                                                                                    && i.StateTimelimit == true) < 24)
                .Match(() => codeLevelMessageContext, c => c != null);

            Then()
                .Do(ctx => CreateMsg(codeLevelMessageContext, primaryJob));
        }

        private void CreateMsg(CodeLevelMessageContext codeLevelMessageContext, POPClaimEmploymentContract primaryJob)
        {
            const string code         = RuleReason.POPTL;
            var          placeholder  = primaryJob.JobBeginDate;
            var          placeHolders = new object[] { placeholder };

            var msg = Smart.Format(codeLevelMessageContext.GetMessageByCode(code), placeHolders);
            codeLevelMessageContext.AddMessageCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
