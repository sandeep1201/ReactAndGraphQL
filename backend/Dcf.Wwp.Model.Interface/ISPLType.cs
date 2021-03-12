using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISPLType : ICommonModel, ICloneable
    {
        String Name { get; set; }
        String Rating { get; set; }
        Int32? SortOrder { get; set; }
    }
}