using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA-Test")]
    [Tag("TJ-Test")]
    [Name("TJMustBeInGeoArea")]
    public class TJMustBeInGeoArea : Rule
    {
        public override void Define()
        {
            RequestForAssistanceContract contract        = null;
            int                          countyNum       = 0;
            var                          tjCountyNumbers = new[] { 2, 4, 19, 21, 26, 34, 72, 51, 53, 54, 57, 60 };

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId)
                .Match<int>(() => countyNum, cn => !tjCountyNumbers.Contains(cn));

            Then()
                .Do(ctx => contract.AddAlert("Does not reside in geographical area of service.")); // GEO - "Does not reside in geographical area of service."
        }
    }
}
