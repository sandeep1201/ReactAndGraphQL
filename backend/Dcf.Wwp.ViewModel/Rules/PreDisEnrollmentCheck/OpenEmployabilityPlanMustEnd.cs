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
    [Name("OpenEmployabilityPlanMustEnd")]
    public class OpenEmployabilityPlanMustEnd : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext messageCodeLevelResult = null;
            IEmployabilityPlan      ep                     = null;

            // When we have a submitted EP.
            When()
                .Match<IEmployabilityPlan>(() => ep, i => i != null && i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code DPEPS
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = RuleReason.DPEPS;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
