using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierAccommodation : ICommonModel, IHasDeleteReason, ICloneable
    {
        Int32? BarrierDetailsId { get; set; }
        Int32? AccommodationId { get; set; }
        DateTime? BeginDate { get; set; }
        DateTime? EndDate { get; set; }
        String Details { get; set; }

        bool IsOpen { get; }

        IAccommodation Accommodation { get; set; }
        IBarrierDetail BarrierDetail { get; set; }
    }
}