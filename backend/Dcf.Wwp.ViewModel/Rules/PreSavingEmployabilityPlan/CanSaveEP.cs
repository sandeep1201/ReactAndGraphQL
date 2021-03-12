using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Rules.PreSavingEmployabilityPlan
{
    [Tag("EP")]
    [Name("CanSaveEP")]
    public class CanSaveEP : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext               messageCodeLevelResult = null;
            List<EmployabilityPlanActivityBridge> epab                   = null;
            EmployabilityPlanContract             contract               = null;

            // Check for CanHaveActivities and then check if there is any activities
            When()
                .Match<EmployabilityPlanContract>(() => contract,         e => (e.CanSaveWithoutActivity != null && e.CanSaveWithoutActivity == true))
                .Match<List<EmployabilityPlanActivityBridge>>(() => epab, a => a.Any())
                .Match(() => messageCodeLevelResult,                      c => c != null);

            Then()
                // Error of code EP
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.EP;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
