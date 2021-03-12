using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IDegreeType : ICommonModel
    {
        String Code { get; set; }
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        ICollection<IPostSecondaryDegree> PostSecondaryDegrees { get; set; }
    }
}