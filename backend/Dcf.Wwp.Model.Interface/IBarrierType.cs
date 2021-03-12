using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierType:ICommonDelModel,ICloneable
    {
         String Name { get; set; }
         Boolean? IsRequired { get; set; }
         Int32? SortOrder { get; set; }

        ICollection<IBarrierDetail> BarrierDetails { get; set; }
        ICollection<IBarrierSubtype> BarrierSubtypes { get; set; }
        
    }
}