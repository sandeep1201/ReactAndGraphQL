using System.Collections.Generic;


namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class WorkHistoryHistoryContract
    {
        public List<HistoryValueContract> EmploymentStatusTypeId { get; set; }
        public List<HistoryValueContract> HasVolunteered { get; set; }
        public List<HistoryValueContract> NonFullTimeDetails { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
        public List<HistoryValueContract> PreventionFactors { get; set; }
        //public List<List<HistoryValueContract>> PreventionFactorIds { get; set; }

        public WorkHistoryHistoryContract()
        {
            //PreventionFactorIds = new List<List<HistoryValueContract>>();
        }
    }
}