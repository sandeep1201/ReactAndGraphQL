using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.DataAccess.Models;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Name("CanAddPOPClaimWithLPJARule")]
    public class CanAddPOPClaimWithLPJARule : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext codeLevelMessageContext = null;
            List<POPClaim> popClaimsInAgency = null;

            When()
                .Match(() => popClaimsInAgency, pops => pops.Any(pop => pop.POPClaimType.Code == POPClaimType.LongTermCd))
                .Match(() => codeLevelMessageContext, c => c != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(codeLevelMessageContext, RuleReason.POPLPJA, CodeLevel.Error));
        }

       
    }
}
