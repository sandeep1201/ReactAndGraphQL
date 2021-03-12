using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IHousingHistory : ICommonDelModel
    {
        int? HousingSectionId { get; set; }
        int? HousingSituationId { get; set; }
        DateTime? BeginDate { get; set; }
        DateTime? EndDate { get; set; }
        bool? HasEvicted { get; set; }
        bool? IsAmountUnknown { get; set; }
        decimal? MonthlyAmount { get; set; }
        string Details { get; set; }
        int? SortOrder { get; set; }

        IHousingSection HousingSection { get; set; }
        IHousingSituation HousingSituation { get; set; }
    }
}