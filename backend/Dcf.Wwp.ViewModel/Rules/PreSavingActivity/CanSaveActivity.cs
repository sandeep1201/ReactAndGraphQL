using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Rules.PreSavingActivity
{
    [Tag("ACT")]
    [Name("CanSaveActivity")]
    public class CanSaveActivity : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext   messageCodeLevelResult = null;
            List<ParticipationStatus> ps                     = null;
            EmployabilityPlan         ep                     = null;

            // Check if participant has any current TA/TE statuses
            When()
                .Match(() => ep, a => a.CanSaveWithoutActivity != true)
                .Match(() => ps, a => a.Any(i => (i.Status.Code == "TA" && i.isCurrent == true) || (i.Status.Code == "TE" && i.isCurrent == true)))
                .Match(() => messageCodeLevelResult,        c => c != null);

            Then()
                // Error of code ACTPSTATE
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.ACTPSTATE;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
