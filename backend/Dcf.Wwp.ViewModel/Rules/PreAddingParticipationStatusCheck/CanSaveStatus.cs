using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingParticipationStatusCheck
{
    [Tag("PSAdd")]
    [Name("CanSaveStatus")]
    public class CanSaveStatus : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext messageCodeLevelResult = null;
            List<Activity>          ac                     = null;

            // Check if participant has any current TMJ/TJ Activities
            When()
                .Match<List<Activity>>(() => ac,     a => a.Any(i => i.ActivityCompletionReasonId == null))
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code PSTATEACT
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.PSTATEACT;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
