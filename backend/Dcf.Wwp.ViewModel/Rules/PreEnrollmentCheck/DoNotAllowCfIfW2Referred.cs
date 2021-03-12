using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.PreEnrollmentCheck
{
    [Tag("PreEnrollmentCheck")]
    [Name("DoNotAllowCfIfW2Referred")]
    public class DoNotAllowCfIfW2Referred : Rule
    {
        public override void Define()
        {
            IParticipantEnrolledProgram       pep      = null;
            List<IParticipantEnrolledProgram> peps     = null;
            DisenrollCheckContract            contract = null;


            When()
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.IsW2
                                                                                                   && (c.EnrolledProgramStatusCodeId == Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId)) != null)
                .Match<IParticipantEnrolledProgram>(() => pep, p => p.IsCF)
                .Match<DisenrollCheckContract>(() => contract,        c => c != null);

            Then()
                .Do(ctx => contract.AddErrorCodes("CFRW2"));
        }
    }
}
