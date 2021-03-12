using System;
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
    [Name("CoEnrollmentOpenWorkerTasks")]
    public class CoEnrollmentOpenWorkerTasksWarning : Rule
    {
        public override void Define()
        {
            List<IParticipantEnrolledProgram> peps                   = null;
            IParticipantEnrolledProgram       pep                    = null;
            DateTime?                         workerTaskDate         = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            var today = DateTime.Today;

            When()
                .Match(() => pep, p => p != null)
                .Match(() => workerTaskDate, w => w != null && w <= today)
                .Match(() => peps, p => p.Count(i => i.EnrolledProgramId != pep.EnrolledProgramId &&
                                                     (i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.ReferredId ||
                                                      i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.EnrolledId)) > 0)
                .Match(() => messageCodeLevelResult, c => c != null);

            Then()
                .Do(ctx => CreateMessage.CreateMsg(messageCodeLevelResult, RuleReason.DPOWT, CodeLevel.Warning));
        }
    }
}
