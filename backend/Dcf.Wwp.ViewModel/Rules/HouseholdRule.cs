using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("HouseholdRule")]
    public class HouseholdRule : Rule
    {
        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => !contract.HouseholdSize.HasValue && !contract.AnnualHouseholdIncome.HasValue);

            Then()
                .Do(ctx => contract.AddAlert("FPLERR3")); // "Annual household income rules cannot be verified."
        }
    }
}
