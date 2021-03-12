using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.DataAccess.Models;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreSavingActivity
{
    [Tag("ACT")]
    [Name("CanSaveUFActivity")]
    public class CanSaveUFActivity : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext          messageCodeLevelResult = null;
            USP_CheckDB2OpenPlacement_Result pre                    = null;

            // Check if participant has any current TA/TE statuses
            When()
                .Match(() => pre,                    p => p != null && p.HasOpenPlacement == true)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code ACTPSTATE
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.ACTUF;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
