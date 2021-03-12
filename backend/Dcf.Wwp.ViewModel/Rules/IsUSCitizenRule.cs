using System;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("IsUSCitizenRule")]
    public class IsUSCitizenRule : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "No" for the question "Is applicant a U.S. citizen or
        // qualified non-citizen?" the following failure reason will be displayed and stored:
        // "Not a U.S. citizen or qualified non-citizen"

        public override void Define()
        {
            RFARulesContext              context  = null;
            RequestForAssistanceContract contract = null;

            When()
                .Match<RFARulesContext>(() => context)
                .Match<RequestForAssistanceContract>(() => contract, c => c.IsUSCitizen.HasValue, c => !c.IsUSCitizen.Value);

            Then()
                .Do(ctx => contract.AddAlert("NUS"))    // "Not a U.S. citizen or qualified non-citizen."
                .Do(ctx => context.AddReason("NUS"))    //TODO: pick one or the other (take the context, not the contract?)
                .Do(ctx => UpdateContext(context,false));
        }

        private void UpdateContext(RFARulesContext context, Boolean val)
        {
            context.IsEligible = false;
        }
    }
}
