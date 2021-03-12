using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Models;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentPOPClaimAdd)]
    [Tag(RuleReason.JobRetentionPOPClaimAdd)]
    [Tag(RuleReason.LongTermParticipantJobAttainment)]
    [Tag(RuleReason.JobAttainmentWithHighWage)]
    [Name("CanAddPOPClaimWithoutEnrollment")]
    public class CanAddPOPClaimWithoutEnrollment : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext          codeLevelMessageContext     = null;
            POPClaimContract                 contract                    = null;
            List<ParticipantEnrolledProgram> participantEnrolledPrograms = null;

            When()
                .Match(() => contract, c => c != null)
                .Match(() => participantEnrolledPrograms, peps => peps.All(pep => pep.EnrollmentDate >= contract.POPClaimEmployments
                                                                                                                .FirstOrDefault(i => i.IsPrimary)
                                                                                                                .JobBeginDate.ToDateMonthDayYear()))
                .Match(() => codeLevelMessageContext, c => c != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(codeLevelMessageContext, RuleReason.POPAEN, CodeLevel.Error));
        }
    }
}
