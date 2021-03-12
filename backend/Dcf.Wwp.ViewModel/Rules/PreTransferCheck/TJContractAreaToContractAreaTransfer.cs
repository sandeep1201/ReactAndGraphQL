using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("TJContractAreaToContractAreaTransfer")]
    public class TJContractAreaToContractAreaTransfer : Rule
    {
        // US1406
        public override void Define()
        {
            EnrolledProgramContract     transferContract       = null;
            IParticipantEnrolledProgram pep                    = null;
            MessageCodeLevelContext     messageCodeLevelResult = null;

            // When we have TJ program transfering. 
            When()
                .Match(() => pep,                    p => p   != null && p.IsTJ)
                .Match(() => transferContract,       tc => tc != null && tc.IsTJ && !pep.CanTransferContractAreas(pep.Office.ContractArea.ContractAreaName, transferContract.ContractorName))
                .Match(() => messageCodeLevelResult, mCLR => mCLR != null);

            Then()
                // Error of code TJTP - Participant cannot transfer into this Office.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TJTP;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
