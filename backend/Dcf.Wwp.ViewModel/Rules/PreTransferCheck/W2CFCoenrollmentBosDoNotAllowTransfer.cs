using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("W2CFCoenrollmentBosDoNotAllowTransfer")]
    public class W2CFCoenrollmentBosDoNotAllowTransfer : Rule
    {
        //US1471
        public override void Define()
        {
            EnrolledProgramContract transferContract = null;
            List<IParticipantEnrolledProgram> peps = null;
            MessageCodeLevelContext messageCodeLevelResult = null;

            // When we are transferring W-2 that is co-enrolled(W-2 and CF) in BOS. 
            When()
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc != null && (tc.IsW2))
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.IsCF && c.IsEnrolled && c.IsInBalanceOfState) != null)
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.IsW2 && c.IsEnrolled && c.IsInBalanceOfState) != null)
                .Match(() => messageCodeLevelResult, mCLR => mCLR != null);

            Then()
                // Error of code TRANW2CF - Children First must be disenrolled before transfer can be completed. Please contact CF Case Manager.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TRANW2CF;
            var msg = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }

}
