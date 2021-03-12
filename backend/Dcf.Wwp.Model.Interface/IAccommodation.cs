using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAccommodation:ICommonDelModel,ICloneable
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        ICollection<IBarrierAccommodation> BarrierAccommodations { get; set; }
    }
}