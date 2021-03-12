using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("TjTmjHasEligibilityBeenVerified")]
    public class TjTmjHasEligibilityBeenVerified : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "No" for the question "Has eligibility information
        // been verified?" the following failure reason will be displayed and stored:
        // "Failed to verify eligibility information"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.TjTmjHasEligibilityBeenVerified.HasValue && !c.TjTmjHasEligibilityBeenVerified.Value);

            Then()
                .Do(ctx => contract.AddAlert("VER")); // "Failed to verify eligibility information."
        }
    }
}
