using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenPlacementsMustEnd")]
    public class OpenPlacementsMustEnd: Rule
    {
        public override void Define()
        {
            ISP_PreCheckDisenrollment_Result  Db2Precheck            = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;
            List<IParticipantEnrolledProgram> peps                   = null;

            // When our DB2 call returns a PlacementOpen as true.
            When()
                .Match<ISP_PreCheckDisenrollment_Result>(() => Db2Precheck, d => d.PlacementOpen.HasValue && d.PlacementOpen == true)
                .Match<List<IParticipantEnrolledProgram>>(() => peps, p => p.Count(pep => pep.IsEnrolled) == 1)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Error of code DPCEP
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCEP;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}

