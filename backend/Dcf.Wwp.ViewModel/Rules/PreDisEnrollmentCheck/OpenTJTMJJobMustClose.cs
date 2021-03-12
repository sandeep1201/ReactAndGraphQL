using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenTJTMJJobMustClose")]
    public class OpenTJTMJJobMustClose : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext      messageCodeLevelResult = null;
            List<IEmploymentInformation> employmentInformation  = null;


            When()
                .Match(() => employmentInformation, i => i.Any(j => j.JobType != null
                                                                    && (j.JobType.Name == JobType.TJSubsidized || j.JobType.Name == JobType.TMJSubsidized)
                                                                    && j.IsCurrentlyEmployed == true))
                .Match(() => messageCodeLevelResult, i => i != null);

            Then()
                // Error of code DPOTJ
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = RuleReason.DPOTJ;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
