using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("ElapsedActivityInMke")]
    public class ElapsedActivityInMke : Rule
    {
        public override void Define()
        {
            int                     elapsedCount           = 0;
            MessageCodeLevelContext messageCodeLevelResult = null;

            // When there is elapsed activity in the EP.
            When()
                .Match<int>(() => elapsedCount,      i => i       > 0)
                .Match(() => messageCodeLevelResult, mCLR => mCLR != null);

            Then()
                // Error of code TRANMKE - Elapsed activities exist and must be ended before a transfer is allowed.
                // Go to the Activities section of the Submitted EP to enter a completion reason for all elapsed activities.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TRANMKE;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
