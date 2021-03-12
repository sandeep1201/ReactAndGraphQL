using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class HousingSectionHistoryContract
    {
        public List<HistoryValueContract> HousingSituationId { get; set; }
        public List<HistoryValueContract> CurrentHousingDetails { get; set; }
        public List<HistoryValueContract> CurrentHousingBeginDate { get; set; }
        public List<HistoryValueContract> CurrentHousingEndDate { get; set; }
        public List<HistoryValueContract> CurrentMonthlyAmount { get; set; }
        public List<HistoryValueContract> IsCurrentAmountUnknown { get; set; }
        public List<HistoryValueContract> HasCurrentEvictionRisk { get; set; }
        public List<HistoryValueContract> HasBeenEvicted { get; set; }
        public List<HistoryValueContract> IsCurrentMovingToHistory { get; set; }
        public List<HistoryValueContract> HasUtilityDisconnectionRisk { get; set; }
        public List<HistoryValueContract> UtilityDisconnectionRiskNotes { get; set; }
        public List<HistoryValueContract> HasDifficultyWorking { get; set; }
        public List<HistoryValueContract> DifficultyWorkingNotes { get; set; }
        public List<HistoryValueContract> Notes { get; set; }

        public List<HousingHistoryHistoryContract> Histories { get; set; }

        public HousingSectionHistoryContract()
        {
            Histories = new List<HousingHistoryHistoryContract>();
        }
    }

    public class HousingHistoryHistoryContract
    {
        public List<HistoryValueContract> HousingSituationId { get; set; }
        public List<HistoryValueContract> BeginDate { get; set; }
        public List<HistoryValueContract> EndDate { get; set; }
        public List<HistoryValueContract> HasEvicted { get; set; }
        public List<HistoryValueContract> MonthlyAmount { get; set; }
        public List<HistoryValueContract> IsAmountUnknown { get; set; }
        public List<HistoryValueContract> Details { get; set; }
    }
}