using System;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("NoMultipleTransferPerDay")]
    public class NoMultipleTransferPerDay : Rule
    {
        public override void Define()
        {
            EnrolledProgramContract     transferContract       = null;
            IParticipantEnrolledProgram pep                    = null;
            MessageCodeLevelContext     messageCodeLevelResult = null;
            bool                        isTransfer             = false;

            // When we have transfer happening multiple times on the same day.
            When()
                .Match<IParticipantEnrolledProgram>(() => pep,          p => p       != null)
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc     != null)
                .Match<bool>(() => isTransfer,        ta => ta     == true)
                .Match(() => messageCodeLevelResult,                    mCLR => mCLR != null);

            Then()
                // Error of code TRANDTMUL - Participant cannot transfer more than once on the same day.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TRANDTMUL;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }

}
