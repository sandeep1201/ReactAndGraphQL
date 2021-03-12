using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDeletingWorkHistory
{
    [Tag("WHDelete")]
    [Name("CanDeleteWHWithPOPRule")]
    public class CanDeleteWHWithPOPRule : Rule
    {
        public override void Define()
        {
            ISP_DB2_PreCheck_POP_Claim_Result Db2Precheck            = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            // When our DB2 call returns a Open POP Claim as true.
            When()
                .Match<ISP_DB2_PreCheck_POP_Claim_Result>(() => Db2Precheck, d => d.PopClaim.HasValue && d.PopClaim == true)
                .Match(() => messageCodeLevelResult,                         c => c                                 != null && c.PossibleRuleReasons.Any(i => i.Code == RuleReason.POP));

            Then()
                // Error of code POP
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = RuleReason.POP;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
