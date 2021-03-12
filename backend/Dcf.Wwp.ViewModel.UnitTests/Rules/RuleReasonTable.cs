using System.Collections.Generic;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.ViewModel.UnitTests.Rules
{
    public class RuleReasonTable
    {
        public RuleReasonTable()
        {
            RuleReasons = CreateRulesList();
        }

        public List<IRuleReason> RuleReasons { get; set; }

        private List<IRuleReason> CreateRulesList()
        {
            var rr = new RuleReason
                     {
                         Code = "TMJEW2",
                         Name = "Individual is enrolled for TMJ. TMJ and W-2 cannot be co-enrolled."
                     };

            var rr1 = new RuleReason
                      {
                          Code = "TMJRW2",
                          Name = "Individual is referred for TMJ. TMJ and W-2 cannot be co-enrolled."
                      };

            var rr2 = new RuleReason
                      {
                          Code = "TJRW2",
                          Name = "Individual is referred for TJ. TJ and W-2 cannot be co-enrolled."
                      };

            var rr22 = new RuleReason
                       {
                           Code = "TJEW2",
                           Name = "Individual is enrolled for TJ. TJ and W-2 cannot be co-enrolled."
                       };


            var rr3 = new RuleReason
                      {
                          Code = "DPCA",
                          Name = "Review the participant's open accommodations, disenrollment will not auto end these"
                      };

            var rr4 = new RuleReason
                      {
                          Code = "DPCB",
                          Name = "Review the participant's open barriers, disenrollment will not auto close these barriers"
                      };


            var rr5 = new RuleReason
                      {
                          Code = "DPCEA",
                          Name = "Open activities/components exist for the participant. These must be ended prior to disenrollment."
                      };
            var rr6 = new RuleReason
                      {
                          Code = "DPCEP",
                          Name = "Open placements exist. These must be ended prior to disenrollment."
                      };

            var rr7 = new RuleReason
                      {
                          Code = "DPCET",
                          Name = "Transactions exists. These must be closed prior to disenrollment."
                      };

            var rr8 = new RuleReason
                      {
                          Code = "DPCWB",
                          Name = "There are one or more open barriers for this participant. They will be ended upon disenrollment."
                      };


            var rr9 = new RuleReason
                      {
                          Code = "DPAO",
                          Name = "There are open activities for this participant. Close program specific activities prior to disenrollment."
                      };

            var rr10 = new RuleReason
                       {
                           Code = "DPCFE",
                           Name = "Children First must be disenrolled with W-2. Please inform Children First Case Manager ({0} {1} - {2}) that they will need to re-enroll participant."
                       };

            var rr11 = new RuleReason
                       {
                           Code = "DPCFR",
                           Name = "Children First must be enrolled and then disenrolled to disenroll from W-2. Please inform Children First Case Manager ({0} {1} - {2}) that they will need to create a new RFA for this individual."
                       };

            var rr12 = new RuleReason
                       {
                           Code = "DPCWA",
                           Name = "There are one or more open accommodations for this participant.  They will be ended upon disenrollment."
            };



            var rr13 = new RuleReason
                       {
                           Code = "RTMJCFOFF",
                           Name = "This individual has been enrolled in WP Office [County Name - Office Number] for TMJ. Co-enrolled individuals must be in the same WP Office for both programs."
            };

            var rr14 = new RuleReason
                       {
                           Code = "TJCFDEN",
                           Name = "Children First must be disenrolled before transfer can be completed. Please contact CF Case Manager ({0} - {1})"
            };




            var rules = new List<IRuleReason>
                        {
                            rr,
                            rr1,
                            rr2,
                            rr3,
                            rr4,
                            rr5,
                            rr6,
                            rr7,
                            rr8,
                            rr22,
                            rr9,
                            rr10,
                            rr11,
                            rr12,
                            rr13,
                            rr14
                        };

            return rules;
        }
    }
}
