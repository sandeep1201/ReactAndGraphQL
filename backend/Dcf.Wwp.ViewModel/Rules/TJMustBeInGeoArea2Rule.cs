using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Tag("TJ")]
    [Name("TJMustBeInGeoArea2Rule")]
    public class TJMustBeInGeoArea2Rule : Rule
    {
        public override void Define()
        {
            RequestForAssistanceContract contract            = null;
            RFARulesContext              context             = null;
            List<ICountyAndTribe>        counties            = null;
            ICountyAndTribe              countyOfEligibility = null;
            //var tjCountyNumbers = new[] { 2, 4, 19, 21, 26, 34, 72, 51, 53, 54, 57, 60 };

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId)
                .Match<List<ICountyAndTribe>>(() => counties)
                .Match<RFARulesContext>(() => context)
                .Let(() => countyOfEligibility, () => counties.FirstOrDefault(i => i.Id == contract.CountyOfResidenceId))
                .Having(() => !context.TJCounties.Contains((int) countyOfEligibility.CountyNumber));

            Then()
                .Do(ctx => contract.AddAlert("GEO")); // "Does not reside in geographical area of service."
        }
    }
}
