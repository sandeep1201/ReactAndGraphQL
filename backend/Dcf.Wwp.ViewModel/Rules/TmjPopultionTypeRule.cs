using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Data.Sql.Model;
using System;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Tag("TMJ")]
    [Name("TMJPOPULATIONTYPE")]
    public class TmjPopultionTypeRule : Rule
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
                //.Match<EligibilityByFPL>(() => fpl
                .Match<RequestForAssistanceContract>(() => contract, c =>
                c.PopulationTypesIds != null && c.PopulationTypesIds.Length != 0 && c.PopulationTypesIds[0] == Wwp.Model.Interface.Constants.PopulationType.DNMPTC);
                
            Then()
                .Do(ctx => contract.AddAlert("DNMPTC")); // "Does not reside in geographical area of service."

        }

    }
}
