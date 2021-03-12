using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using SmartFormat;

namespace Dcf.Wwp.Api.Library.Rules.RFA.PreCheck
{
    [Tag("RFA-PreCheck")]
    [Name("NoRfaForNonClientRegParticipants")]
    public class NoRfaForNonClientRegParticipants : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext messageCodeLevelResult = null;
            IParticipant            participant            = null;

            When()
                .Match<MessageCodeLevelContext>(() => messageCodeLevelResult, c => c != null)
                .Match<IParticipant>(() => participant,                       p => p != null && p.HasBeenThroughClientReg != true);
            Then()
                // "Client registration has not been completed for this individual. You must complete client registration before creating an RFA.".
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }


        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.CRPASS;
            var msg = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
