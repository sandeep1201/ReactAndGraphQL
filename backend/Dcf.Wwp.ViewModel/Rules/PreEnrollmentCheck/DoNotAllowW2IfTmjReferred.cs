using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Rules.PreEnrollmentCheck
{
  
    [Tag("PreEnrollmentCheck")]
    [Name("DoNotAllowW2IfTmjReferred")]
    public class DoNotAllowW2IfTmjReferred : Rule
    {
        public override void Define()
        {
            IParticipantEnrolledProgram       pep  = null;
            List<IParticipantEnrolledProgram> peps = null;
            DisenrollCheckContract contract = null;      

            When()
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.EnrolledProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId
                                                                                               && c.EnrolledProgramStatusCodeId == Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId) != null)
                .Match<IParticipantEnrolledProgram>(() => pep, p => p.IsW2)
                .Match<DisenrollCheckContract>(() => contract, c => c != null);

            Then()
                .Do(ctx => contract.AddErrorCodes("TMJRW2"));

        }

    }
}
