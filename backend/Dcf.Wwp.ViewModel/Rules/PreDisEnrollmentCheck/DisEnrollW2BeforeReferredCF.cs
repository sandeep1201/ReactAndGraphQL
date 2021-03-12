using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using SmartFormat;

namespace Dcf.Wwp.Api.Library.Rules.PreDisEnrollmentCheck
{
    [Tag("Disenrollment")]
    [Name("DisEnrollW2BeforeReferredCF")]
    public class DisEnrollW2BeforeReferredCF : Rule
    {
        // US1460.
        public override void Define()
        {
            List<IParticipantEnrolledProgram> peps                   = null;
            IParticipantEnrolledProgram       disEnrollPep           = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            When()
                // When CF case is referred
                .Match<List<IParticipantEnrolledProgram>>(() => peps, ps => peps.FirstOrDefault(p => p.IsReferred && p.IsCF) != null)

                // When dis-enrolling W2.
                .Match<IParticipantEnrolledProgram>(() => disEnrollPep, pep => pep.IsW2 && pep.IsEnrolled)

                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                 // Error of code DPCFR.
                .Do(ctx => CreateMsg(messageCodeLevelResult, peps));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, List<IParticipantEnrolledProgram> peps)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.DPCFR;

            var worker = peps.FirstOrDefault(p => p.IsReferred && p.IsCF)?.Worker;

            var placeHolders = new object[] { worker?.FirstName, worker?.LastName, worker?.MFUserId };

            var msg = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code), placeHolders);

            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
