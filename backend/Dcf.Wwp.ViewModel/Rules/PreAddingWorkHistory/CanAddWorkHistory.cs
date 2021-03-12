using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingWorkHistory
{
    [Tag("WH")]
    [Name("PreAddingWorkHistoryCheck")]
    public class PreAddingWorkHistoryCheck: Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext messageCodeLevelResult = null;
            EmploymentInfoContract contract = null;
            List<IParticipationStatu> pss = null;

            // Check for Employment type, participant Enrolled program and the Participation Status Type Selected
            When()
                .Match<EmploymentInfoContract>(() => contract, e => (e.JobTypeName == Wwp.Model.Interface.Constants.JobType.TJSubsidized || e.JobTypeName == Wwp.Model.Interface.Constants.JobType.TMJSubsidized || e.JobTypeName == Wwp.Model.Interface.Constants.JobType.TJUnSubsidized || e.JobTypeName == Wwp.Model.Interface.Constants.JobType.TMJUnSubsidized) && e.IsCurrentlyEmployed == true)
                .Match<List<IParticipationStatu>>(() => pss, ps => ps.Any(p => (p.StatusId == Wwp.Model.Interface.Constants.ParticipationStatus.TA || p.StatusId == Wwp.Model.Interface.Constants.ParticipationStatus.TE) && p.IsCurrent == true))
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code PS
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TATE;
            var msg = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }

    }
}
