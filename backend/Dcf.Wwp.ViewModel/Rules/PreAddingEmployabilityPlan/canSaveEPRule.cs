using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingEmployabilityPlan
{
    [Tag("EPSave")]
    [Name("CanSaveEP")]
    public class CanSaveEPRule : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext messageCodeLevelResult = null;
            EmployabilityPlanContract contract = null;
            List<Activity> activities = null;

            // Check for Employment type, participant Enrolled program and the Participation Status Type Selected
            When()
                .Match<List<Activity>>(() => activities, ac => ac.Any(a => (!a.IsDeleted)))
                .Match<EmployabilityPlanContract>(() => contract, c => (c.EmployabilityPlanStatusTypeName == "In Progress"))
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code PS
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.EP;
            var msg = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
