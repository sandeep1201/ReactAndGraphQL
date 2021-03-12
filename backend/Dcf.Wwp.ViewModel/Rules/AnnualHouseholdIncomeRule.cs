using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("AnnualHouseholdIncomeRule")]
    public class AnnualHouseholdIncomeRule : Rule
    {
        /// <summary>
        /// Eligibility Rule are per US1387.
        /// </summary>
        public override void Define()
        {
            //RFARulesContext              rfaCtx      = null;
            EligibilityByFPL             fpl      = null;
            RequestForAssistanceContract contract = null;
            
            When()
                .Match<EligibilityByFPL>(() => fpl)
                .Match<RequestForAssistanceContract>(() => contract, c => c.AnnualHouseholdIncome.Value >= (fpl.Pct150PerMonth.Value * 12m));

            Then()
                .Do(ctx => contract.AddAlert("FPL")); // "Annual household income must be below the 150% of the Federal Poverty Level (FPL) for the household size."
        }
    }
}
