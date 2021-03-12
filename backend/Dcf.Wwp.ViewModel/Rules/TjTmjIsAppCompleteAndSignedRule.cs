using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("TjTmjIsAppCompleteAndSignedRule")]
    public class TjTmjIsAppCompleteAndSignedRule : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "No" for the question "Has application been completed
        // and signed?" the following failure reason will be displayed and stored:
        // "Failed to complete or sign application"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.TjTmjIsAppCompleteAndSigned.HasValue, c => !c.TjTmjIsAppCompleteAndSigned.Value);

            Then()
                .Do(ctx => contract.AddAlert("APP")); // "Failed to complete or sign application."
        }
    }
}
