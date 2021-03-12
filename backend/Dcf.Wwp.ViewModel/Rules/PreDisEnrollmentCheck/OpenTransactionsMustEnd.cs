using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;


namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenTransactionsMustEnd")]
    public class OpenTransactionsMustEnd : Rule
    {
        public override void Define()
        {
            ISP_PreCheckDisenrollment_Result  Db2Precheck            = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;
            List<IParticipantEnrolledProgram> peps                   = null;

            // When our DB2 call returns a TransactionExist as true.
            When()
                .Match<ISP_PreCheckDisenrollment_Result>(() => Db2Precheck, d => d.TransactionExist.HasValue && d.TransactionExist == true)
                .Match<List<IParticipantEnrolledProgram>>(() => peps,       p => p.Any(pep => pep.IsEnrolled))
                .Match(() => messageCodeLevelResult,                        c => c != null);

            Then()
                // Error of code DPCET
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCET;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
