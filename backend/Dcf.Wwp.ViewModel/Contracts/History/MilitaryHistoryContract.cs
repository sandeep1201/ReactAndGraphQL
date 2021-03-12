using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class MilitaryHistoryContract
    {
        public List<HistoryValueContract> DoesHaveTraining { get; set; }
        public List<HistoryValueContract> MilitaryRankId { get; set; }
        public List<HistoryValueContract> MilitaryBranchId { get; set; }
        public List<HistoryValueContract> Rate { get; set; }
        public List<HistoryValueContract> EnlistmentDate { get; set; }
        public List<HistoryValueContract> DischargeDate { get; set; }
        public List<HistoryValueContract> IsCurrentlyEnlisted { get; set; }
        public List<HistoryValueContract> MilitaryDischargeTypeId { get; set; }
        public List<HistoryValueContract> SkillsAndTraining { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
    }
}