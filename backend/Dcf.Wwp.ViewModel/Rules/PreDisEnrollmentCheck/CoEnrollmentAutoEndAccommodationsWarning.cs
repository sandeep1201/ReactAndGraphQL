using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("CoEnrollmentAutoEndAccommodations")]
    public class CoEnrollmentAutoEndAccommodationsWarning: Rule
    {
        public override void Define()
        {
            List<IParticipantEnrolledProgram> peps                   = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;
            List<IBarrierDetail>              barrierDetails         = null;

            // When there are more than 1 enrolled PEPs and there are open BarrierAccommodations, we trigger our rule. 
            When()
                .Match<List<IBarrierDetail>>(() => barrierDetails, bds => bds.Any(bd => bd.IsDeleted != true && bd.IsAccommodationNeeded == true && bd.BarrierAccommodations.Any(ba => !ba.EndDate.HasValue && ba.DeleteReasonId == null)))
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.Where(c => c.IsEnrolled).ToList().Count > 1)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                   // Warning of code DPCA
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCA;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
