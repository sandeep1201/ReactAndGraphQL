using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class WorkHistorySectionContract : BaseInformalAssessmentContract
    {
        public int?   EmploymentStatusTypeId   { get; set; }
        public string EmploymentStatusTypeName { get; set; }

        public List<int>    PreventionFactorIds   { get; set; }
        public List<string> PreventionFactorNames { get; set; }

        public bool?  HasVolunteered           { get; set; }
        public string NonFullTimeDetails       { get; set; }
        public string Notes                    { get; set; }
        public int?   HasCareerAssessment      { get; set; }
        public string HasCareerAssessmentName  { get; set; }
        public string HasCareerAssessmentNotes { get; set; }

        public WorkHistorySectionContract()
        {
            // Initialize the contract with an empty list of PreventionFactors
            // to make it easier for consumers of the API to handle an
            // initial state.
            PreventionFactorIds   = new List<int>();
            PreventionFactorNames = new List<string>();
        }

        public void AddPreventionFactor(int id, string name)
        {
            PreventionFactorIds.Add(id);
            PreventionFactorNames.Add(name);
        }
    }

    public class PreventionFactorContract
    {
        public int    Id   { get; set; }
        public string Name { get; set; }
    }
}
