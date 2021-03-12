using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class WorkProgramHistoryContract
    {
        public List<InvolvedWorkProgramHistoryContract> WorkPrograms { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
        public List<HistoryValueContract> IsInOtherPrograms { get; set; }

        public WorkProgramHistoryContract()
        {
            WorkPrograms = new List<InvolvedWorkProgramHistoryContract>();
        }
    }

    public class InvolvedWorkProgramHistoryContract
    {
        public List<HistoryValueContract> WorkProgramStatusId { get; set; }
        public List<HistoryValueContract> WorkProgramId { get; set; }
        public List<HistoryValueContract> CityId { get; set; }
        public List<HistoryValueContract> StartMonth { get; set; }
        public List<HistoryValueContract> EndMonth { get; set; }
        public List<HistoryValueContract> ContactId { get; set; }
        public List<HistoryValueContract> Details { get; set; }
    }
}
