using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenBarriersWillBeClosedWarning")]
    public class OpenBarriersWillBeClosedWarning : Rule
    {
        public override void Define()
        {
            List <IBarrierDetail>                barrierDetails         = null;
            MessageCodeLevelContext              messageCodeLevelResult = null;
            List < IParticipantEnrolledProgram > peps                   = null;

            When()
                .Match<List<IBarrierDetail>>(() => barrierDetails, bds => bds.Any(bd => bd.IsDeleted != true && bd.EndDate == null))
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.Where(c => c.IsEnrolled).ToList().Count() == 1)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                 // Warning of code DPCWB
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCWB;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
