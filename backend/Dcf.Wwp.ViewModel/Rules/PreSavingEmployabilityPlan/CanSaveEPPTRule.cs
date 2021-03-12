using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Rules.PreSavingEmployabilityPlan
{
    [Tag("EP")]
    [Name("CanSaveEPPT")]
    public class CanSaveEPPTRule : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext  messageCodeLevelResult = null;
            List<ParticipationEntry> pts                    = null;

            // Check for any Participation Tracking from the EP Begin Date for the Participant
            When()
                .Match<List<ParticipationEntry>>(() => pts, pt => pt.Any())
                .Match(() => messageCodeLevelResult,        c => c != null);

            Then()
                // Error of code EPPT
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.EPPT;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
