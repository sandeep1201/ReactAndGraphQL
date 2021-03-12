using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingParticipationStatusCheck
{
    [Tag("PSAdd")]
    [Name("PreAddingParticipationStatus")]
    public class PreAddingParticipationStatus : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext           messageCodeLevelResult = null;
            ParticipantStatusContract         contract               = null;
            List<IEmploymentInformation>      employments            = null;
            List<IParticipantEnrolledProgram> peps                   = null;

            // Check for Employment type, participant Enrolled program and the Participation Status Type Selected
            When()
                .Match<List<IEmploymentInformation>>(() => employments, es => es.Any(e => (e.JobType.Name == Wwp.Model.Interface.Constants.JobType.TJSubsidized || e.JobType.Name == Wwp.Model.Interface.Constants.JobType.TMJSubsidized || e.JobType.Name == Wwp.Model.Interface.Constants.JobType.TJUnSubsidized || e.JobType.Name == Wwp.Model.Interface.Constants.JobType.TMJUnSubsidized) && e.IsCurrentlyEmployed == true && e.DeleteReasonId == null))
                .Match<List<IParticipantEnrolledProgram>>(() => peps,   ps => ps.Any(p => p.IsEnrolled && (p.IsTJ || p.IsTmj)))
                .Match<ParticipantStatusContract>(() => contract,       c => (c.StatusId                                        == Wwp.Model.Interface.Constants.ParticipationStatus.TA || c.StatusId == Wwp.Model.Interface.Constants.ParticipationStatus.TE))
                .Match(() => messageCodeLevelResult,                    c => c                                                 != null);

            Then()
                // Error of code PS
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.PS;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
