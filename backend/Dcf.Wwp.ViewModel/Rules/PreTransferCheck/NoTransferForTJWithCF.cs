using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using SmartFormat;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("NoTransferForTJWithCF")]
    public class NoTransferForTJWithCF : Rule
    {
        // US1473.
        public override void Define()
        {
            EnrolledProgramContract transferContract       = null;
            List<IParticipantEnrolledProgram> peps = null;
            MessageCodeLevelContext messageCodeLevelResult = null;

            // When we have TJ program that is coenrolled with a CF do not allow. 
            When()
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc     != null && tc.IsTJ)
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.IsCF && c.IsEnrolled) != null)
                .Match(() => messageCodeLevelResult,                    mCLR => mCLR != null);

            Then()
                // Error of code TJCFDEN - Children First must be disenrolled before transfer can be completed. Please contact the CF Case Manager.
                .Do(ctx => CreateMsg(messageCodeLevelResult, peps));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, List<IParticipantEnrolledProgram> peps)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TJCFDEN;

            var ruleForcingPep = peps.FirstOrDefault(x => x.IsCF && x.IsEnrolled);


            var msg = (messageCodeLevelResult.GetMessegeByCode(code));

            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }

}
