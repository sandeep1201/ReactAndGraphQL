using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("CoEnrollmentOpenActivitiesWarning")]
    public class CoEnrollmentOpenActivitiesWarning : Rule
    {
        // US1459
        public override void Define()
        {
            ISP_PreCheckDisenrollment_Result  Db2Precheck            = null;
            List<IParticipantEnrolledProgram> peps                   = null;
            IParticipantEnrolledProgram       disEnrollPep           = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            // When our DB2 call returns a ActivityOpen as true.
            When()
                .Match<ISP_PreCheckDisenrollment_Result>(() => Db2Precheck, d => d.ActivityOpen.HasValue && d.ActivityOpen == true)

                 // When is a co-enrolled case with  CF / TMJ / TJ.
                .Match<List<IParticipantEnrolledProgram>>(() => peps, ps => peps.Count(p => p.IsEnrolled && (p.IsCF || p.IsTmj || p.IsTJ)) > 1)

                // When disenrolling CF / TMJ / TJ.
                .Match<IParticipantEnrolledProgram>(() => disEnrollPep, pep => (pep.IsCF || pep.IsTmj || pep.IsTJ) && pep.IsEnrolled)

                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                // Warning of code DPAO.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPAO;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
