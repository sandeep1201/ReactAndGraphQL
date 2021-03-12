using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface IHousingSection : ICommonDelModel, ICloneable
    {
        int? HousingSituationId { get; set; }
        string CurrentHousingDetails { get; set; }
        DateTime? CurrentHousingBeginDate { get; set; }
        DateTime? CurrentHousingEndDate { get; set; }
        decimal? CurrentMonthlyAmount { get; set; }
        bool? IsCurrentAmountUnknown { get; set; }
        bool? HasCurrentEvictionRisk { get; set; }
        bool? HasBeenEvicted { get; set; }
        bool? IsCurrentMovingToHistory { get; set; }
        bool? HasUtilityDisconnectionRisk { get; set; }
        string UtilityDisconnectionRiskNotes { get; set; }
        bool? HasDifficultyWorking { get; set; }
        string DifficultyWorkingNotes { get; set; }
        string Notes { get; set; }
        ICollection<IHousingHistory> HousingHistories { get; set; }
        ICollection<IHousingHistory> AllHousingHistories { get; set; }
        IHousingSituation HousingSituation { get; set; }
    }
}
