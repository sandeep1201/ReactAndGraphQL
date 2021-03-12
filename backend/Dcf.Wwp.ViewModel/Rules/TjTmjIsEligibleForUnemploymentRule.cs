using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("TjTmjIsEligibleForUnemploymentRule")]
    public class TjTmjIsEligibleForUnemploymentRule : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "Yes" for the question "Eligible for Unemployment
        // Insurance benefits?" the following failure reason will be displayed and stored:
        // "Eligible to receive Unemployment Insurance benefits"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.TjTmjIsEligibleForUnemployment.HasValue && c.TjTmjIsEligibleForUnemployment.Value);

            Then()
                .Do(ctx => contract.AddAlert("UIB")); // "Eligible to receive Unemployment Insurance benefits."
        }
    }
}
