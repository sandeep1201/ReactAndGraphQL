using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockRuleReasonRepository : MockRepositoryBase<RuleReason>, IRuleReasonRepository
    {
        #region Properties

        public bool IsPOPClaim = false;

        #endregion

        #region Methods

        public new IEnumerable<RuleReason> GetMany(Expression<Func<RuleReason, bool>> clause)
        {
            var ruleReasons = new List<RuleReason>();

            if (IsPOPClaim)
            {
                var ruleReason1 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPJA12",
                                      Name        = "The Agency has received a {0} payment for this participant within the past 12 months."
                                  };

                var ruleReason2 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPAEN",
                                      Name        = "Participant must have been \"Enrolled\" in your agency one day prior to the Primary Employment's Begin Date."
                                  };


                var ruleReason3 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPUPPL",
                                      Name        = "Participant must have been assigned an Up-Front Activity -or- placed in a W-2 Placement prior to the Primary Employment''s Begin Date."
                                  };

                var ruleReason4 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPPEBD",
                                      Name        = "The selected Work History entries must have a Primary Employment Begin Date on or after 01/01/{0}."
                                  };

                var ruleReason5 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPCPBD",
                                      Name        = "The Claim Period Begin Date must be on or after 01/01/{0}."
                                  };

                var ruleReason6 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPJB31",
                                      Name        = "The selected Primary Employment must have lasted at least 31 days."
                                  };

                var ruleReason7 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPMHE",
                                      Name        = "Total of Hours Worked must be equal to or greater than 110 hours -or- Total of Earnings must be equal to or greater than $870."
                                  };
                var ruleReason8 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPEPSPEBD",
                                      Name        = "Participant must have had an EP with an activity submitted at least one day prior to the Primary Employment's Begin Date."
                                  };

                var ruleReason9 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPE9314",
                                      Name        = "Employment(s) must be at least 93 days which includes a consecutive 14 day interruption (not including weekends or holidays)."
                                  };
                var ruleReason10 = new RuleReason
                                   {
                                       Category    = "POPClaim",
                                       SubCategory = "PreCheck",
                                       Code        = "POPTL",
                                       Name        = "The participant must have used 24 or more months of their State Time Limit as of the Primary Employment's Begin Date {0}."
                                   };
                var ruleReason11 = new RuleReason
                                   {
                                       Category    = "POPClaim",
                                       SubCategory = "PreCheck",
                                       Code        = "POPPEW",
                                       Name        = "The Primary Employment's starting wage does not meet or exceed the minimum starting wage for your agency."
                                   };
                var ruleReason12 = new RuleReason
                                  {
                                      Category    = "POPClaim",
                                      SubCategory = "PreCheck",
                                      Code        = "POPLPJA",
                                      Name        = "The Agency has previously received payment for a Long-Term Participant Job Attainment claim."
                };

                ruleReasons.Add(ruleReason1);
                ruleReasons.Add(ruleReason2);
                ruleReasons.Add(ruleReason3);
                ruleReasons.Add(ruleReason4);
                ruleReasons.Add(ruleReason5);
                ruleReasons.Add(ruleReason6);
                ruleReasons.Add(ruleReason7);
                ruleReasons.Add(ruleReason8);
                ruleReasons.Add(ruleReason9);
                ruleReasons.Add(ruleReason10);
                ruleReasons.Add(ruleReason11);
                ruleReasons.Add(ruleReason12);
            }

            return ruleReasons;
        }

        #endregion
    }
}
