// ReSharper disable ImplicitlyCapturedClosure
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using NRules.Fluent.Dsl;


namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("OpenParticipationStatusWillEnd")]
    public class OpenParticipationStatusWillEnd : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext     messageCodeLevelResult = null;
            List<IParticipationStatu>   ps                     = null;
            IParticipantEnrolledProgram pep                    = null;

            // When we have a Current PS.
            When()
                .Match<List<IParticipationStatu>>(() => ps)
                .Match<IParticipantEnrolledProgram>(() => pep, i => ps.Any(p => !p.IsDeleted && p.IsCurrent == true && p.EnrolledProgramId == i.EnrolledProgramId))
                .Match(() => messageCodeLevelResult,        c => c != null);

            Then()
                // Error of code DPPSO
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = RuleReason.DPPSO;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
