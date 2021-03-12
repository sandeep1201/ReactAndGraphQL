using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierSubtype : ICommonDelModel, ICloneable
    {
        String Name { get; set; }
        Boolean? DisablesOthersFlag { get; set; }
        Int32? BarrierTypeId { get; set; }
        Int32? SortOrder { get; set; }
        IBarrierType BarrierType { get; set; }
        ICollection<IBarrierTypeBarrierSubTypeBridge> BarrierTypeBarrierSubTypeBridges { get; set; }

    }
}