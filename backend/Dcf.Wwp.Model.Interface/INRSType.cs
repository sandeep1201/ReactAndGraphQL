using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface INRSType : ICommonModel, ICloneable
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        String Rating { get; set; }
    }
}