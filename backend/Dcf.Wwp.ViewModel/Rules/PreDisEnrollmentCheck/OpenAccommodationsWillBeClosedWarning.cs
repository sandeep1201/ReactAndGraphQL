using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenAccommodationsWillBeClosedWarning")]
    public class OpenAccommodationsWillBeClosedWarning: Rule
    {
        // This rule is for single enrollment that displays a accommodation will end messeg.
        public override void Define()
        {
            List<IBarrierDetail>              barrierDetails         = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;
            List<IParticipantEnrolledProgram> peps                   = null;

            When()
                // When we have a open barrier with open accommodations.
                .Match<List<IBarrierDetail>>(() => barrierDetails, bds => bds.Any(bd => bd.IsOpen && bd.IsAccommodationNeeded == true && bd.BarrierAccommodations.Any(ba => ba.IsOpen)))
                // When we are just enrolled into one program.
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.Where(c => c.IsEnrolled).ToList().Count() == 1)
                .Match(() => messageCodeLevelResult,                  c => c                                             != null);

            Then()
                // Warning of DPCWA.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCWA;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }


      
    }
}
