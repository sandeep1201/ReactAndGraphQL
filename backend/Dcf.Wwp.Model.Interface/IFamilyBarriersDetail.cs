using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFamilyBarriersDetail : ICommonDelModel, ICloneable
    {
        String Details { get; set; }
    }
}