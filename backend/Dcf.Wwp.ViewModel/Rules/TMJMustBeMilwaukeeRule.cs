using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Tag("TMJ")]
    [Name("TMJMustBeMilwaukeeRule")]
    public class TMJMustBeMilwaukeeRule : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "TMJ" for "Program" and the "County of Residence" is not "Milwaukee"
        // the following failure reason will be displayed and stored:
        //
        // "Does not reside in geographical area of service"

        // or in the affirmative:

        // If the Agency Worker selects "TMJ" for "Program", the "County of Residence" must be "Milwaukee"
        //
        // "Does not reside in geographical area of service"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                //.Match<EligibilityByFPL>(() => fpl)
                .Match<RequestForAssistanceContract>(() => contract, c => c.ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId)
                .Match<RequestForAssistanceContract>(() => contract, c => c.CountyOfResidenceId != 40);

            Then()
                .Do(ctx => contract.AddAlert("GEO")); // "Does not reside in geographical area of service."
        }
    }
}
