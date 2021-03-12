using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Model.Interface.Constants;
using SmartFormat;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentPOPClaimAdd)]
    [Tag(RuleReason.JobRetentionPOPClaimAdd)]
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Name("CanAddPOPClaimWithOverHoursOrEarnings")]
    public class CanAddPOPClaimWithOverHoursOrEarnings : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext                 codeLevelMessageContext  = null;
            List<POPClaimEmploymentContract>        employments              = null;
            POPClaimDomain.HoursAndEarningsContract hoursAndEarningsContract = null;


            When()
                .Match(() => hoursAndEarningsContract, he => he != null)
                .Match(() => employments, c => c.Sum(i => i.Earnings)    < hoursAndEarningsContract.MinEarnings &&
                                               c.Sum(i => i.HoursWorked) < hoursAndEarningsContract.MinHours)
                .Match(() => codeLevelMessageContext, c => c != null);

            Then()
                .Do(ctx => CreateMsg(codeLevelMessageContext, hoursAndEarningsContract));
        }


        private void CreateMsg(CodeLevelMessageContext codeLevelMessageContext, POPClaimDomain.HoursAndEarningsContract hoursAndEarningsContract)
        {
            const string code         = RuleReason.POPMHE;
            var          placeHolders = new object[] { hoursAndEarningsContract.MinHours, hoursAndEarningsContract.MinEarnings };

            var msg = Smart.Format(codeLevelMessageContext.GetMessageByCode(code), placeHolders);
            codeLevelMessageContext.AddMessageCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
