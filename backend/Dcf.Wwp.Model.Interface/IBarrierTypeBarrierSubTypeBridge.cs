using System;


namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierTypeBarrierSubTypeBridge : ICommonDelModel, ICloneable
    {
        Int32? BarrierSubTypeId { get; set; }
        IBarrierDetail BarrierDetail { get; set; }
        IBarrierSubtype BarrierSubtype { get; set; }
    }
}
