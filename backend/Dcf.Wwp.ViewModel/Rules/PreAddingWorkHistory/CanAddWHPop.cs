using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingWorkHistory
{
    [Tag("WH")]
    [Name("PreAddingWorkHistoryPOPCheck")]
    public class PreAddingWorkHistoryPOPCheck : Rule
    {
        public override void Define()
        {
            ISP_DB2_PreCheck_POP_Claim_Result Db2Precheck            = null;
            var                               IsHD                   = false;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            // Check if there is any POP Claim type, if not show the error
            When()
                .Match<ISP_DB2_PreCheck_POP_Claim_Result>(() => Db2Precheck, d => !d.PopClaim.HasValue || d.PopClaim == false)
                .Match(() => IsHD,                                           i => i)
                .Match(() => messageCodeLevelResult,                         c => c != null && c.PossibleRuleReasons.Any(i => i.Code == RuleReason.NOPOPADD));

            Then()
                // Error of code NOPOPADD
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = RuleReason.NOPOPADD;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
