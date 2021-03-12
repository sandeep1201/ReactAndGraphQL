using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("CoEnrollmentAutoCloseBarriers")]
    public class CoEnrollmentAutoCloseBarriersWarning : Rule
    {
        public override void Define()
        {
            List<IParticipantEnrolledProgram> peps                   = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;
            List<IBarrierDetail>              barrierDetails         = null;

            // When there are more than 1 enrolled PEPs and there are open Barriers, we trigger our rule.
            When()
                .Match<List<IBarrierDetail>>(() => barrierDetails, bds => bds.Any(bd => bd.IsDeleted != true && bd.EndDate == null))
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.Where(c => c.IsEnrolled).ToList().Count > 1)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                  // Warning of code DPCB - Review the participant's open barriers, disenrollment will not auto close these barriers
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCB;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
