using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDeletingWorkHistory
{
    [Tag("WHDelete")]
    [Name("CanDeleteWHWithWeeklyHoursRule")]
    public class CanDeleteWHWithWeeklyHoursRule : Rule
    {
        public override void Define()
        {
            IEmploymentInformation  employmentInfo         = null;
            MessageCodeLevelContext messageCodeLevelResult = null;

            // When EmploymentInfo has weeklyHours attached to it.
            When()
                .Match(() => messageCodeLevelResult, c => c != null)
                .Match(() => employmentInfo, e => (e.JobType.Name    == JobType.TJSubsidized
                                                   || e.JobType.Name == JobType.TMJSubsidized)
                                                  && e.WeeklyHoursWorkedEntries.Any(j => !j.IsDeleted));
            Then()
                // Error of code POP
                .Do(ctx => CreateMessage.CreateMsg(messageCodeLevelResult, RuleReason.TJTMJWH, CodeLevel.Error));
        }
    }
}
