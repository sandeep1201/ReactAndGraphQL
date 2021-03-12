using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("NoTransferForTMJ")]
    public class NoTransferForTMJ : Rule
    {
        // US1475
        public override void Define()
        {
            EnrolledProgramContract transferContract       = null;
            MessageCodeLevelContext messageCodeLevelResult = null;

            // When we have TMJ program transfering do not allow. 
            When()
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc     != null && tc.IsTmj)
                .Match(() => messageCodeLevelResult,                    mCLR => mCLR != null);

            Then()
                // Error of code TRANPRONO - The selected program cannot be transferred.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TRANPRONO;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }

}
