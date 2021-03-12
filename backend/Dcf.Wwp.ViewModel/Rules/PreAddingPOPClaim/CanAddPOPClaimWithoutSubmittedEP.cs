using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentPOPClaimAdd)]
    [Tag(RuleReason.JobRetentionPOPClaimAdd)]
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Tag(RuleReason.JobAttainmentWithHighWage)]
    [Name("CanAddPOPClaimWithoutSubmittedEP")]
    public class CanAddPOPClaimWithoutSubmittedEP : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext                               codeLevelMessageContext                = null;
            POPClaimDomain.ActivityPlacementEPContract            activityPlacementEp                    = null;
            POPClaimDomain.CutOverDateAndFeatureToggleDateDetails cutOverDateAndFeatureToggleDateDetails = null;


            When()
                .Match(() => cutOverDateAndFeatureToggleDateDetails, ctd => ctd != null  && (!ctd.IsTodayWithinSixMonthsFromFeatureToggleDate || (ctd.IsTodayWithinSixMonthsFromFeatureToggleDate && ctd.CutOverDate != null)))
                .Match(() => activityPlacementEp,                    a => !a.HasActivity && a.HasNoEP)
                .Match(() => codeLevelMessageContext,                c => c != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(codeLevelMessageContext, RuleReason.POPEPSPEBD, CodeLevel.Error));
        }
    }
}
