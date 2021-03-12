using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("NoTransferForChildrenFirst")]
    public class NoTransferForChildrenFirst : Rule
    {
        // US1474
        public override void Define()
        {
            EnrolledProgramContract     transferContract       = null;
            MessageCodeLevelContext     messageCodeLevelResult = null;

            // When we have CF program transfering do not allow. 
            When()
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc     != null && tc.IsCF)
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
