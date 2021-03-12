using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenActivitiesMustEnd")]
    public class OpenActivitiesMustEnd : Rule
    {
        public override void Define()
        {
            ISP_PreCheckDisenrollment_Result  Db2Precheck            = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;
            List<IParticipantEnrolledProgram> peps                   = null;
            IEmployabilityPlan                ep                     = null;

            // When our DB2 call returns a ActivityOpen as true.
            When()
                .Match<IEmployabilityPlan>(() => ep,                        i => i.Id == 0)
                .Match<ISP_PreCheckDisenrollment_Result>(() => Db2Precheck, d => d.ActivityOpen.HasValue && d.ActivityOpen == true)
                .Match<List<IParticipantEnrolledProgram>>(() => peps,       p => p.Count(pep => pep.IsEnrolled) == 1)
                .Match(() => messageCodeLevelResult,                        c => c                              != null);

            Then()
                // Error of code DPCEA
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCEA;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
