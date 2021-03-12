using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IConvictionType : ICommonModel, ICloneable
    {
         Int32? SortOrder { get; set; }
         String Name { get; set; }
         ICollection<IConviction> Convictions { get; set; }
         ICollection<IPendingCharge> PendingCharges { get; set; }
    }
}