using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("NoTransferToSameOffice")]
    public class NoTransferToSameOffice : Rule
    {
        public override void Define()
        {
            EnrolledProgramContract     transferContract       = null;
            IParticipantEnrolledProgram pep                    = null;
            MessageCodeLevelContext     messageCodeLevelResult = null;

            // When we have program transfering to the same office.
            // Not showing for TMJ transfers.
            When()
                .Match<IParticipantEnrolledProgram>(() => pep,          p => p       != null)
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc     != null && tc.OfficeNumber == pep.Office.OfficeNumber && !tc.IsTmj)
                .Match(() => messageCodeLevelResult,                    mCLR => mCLR != null);

            Then()
                // Error of code TRANOFFSME - Participant cannot transfer into the same office.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TRANOFFSME;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }

}
