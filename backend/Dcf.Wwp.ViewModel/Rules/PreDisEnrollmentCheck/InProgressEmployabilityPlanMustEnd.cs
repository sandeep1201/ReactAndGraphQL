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
    [Name("InProgressEmployabilityPlanMustEnd")]
    public class InProgressEmployabilityPlanMustEnd : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext messageCodeLevelResult = null;
            IEmployabilityPlan      ep                     = null;

            // When we have an in-progress EP.
            When()
                .Match<IEmployabilityPlan>(() => ep, i => i != null && i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code DPEPIP
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = RuleReason.DPEPIP;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
