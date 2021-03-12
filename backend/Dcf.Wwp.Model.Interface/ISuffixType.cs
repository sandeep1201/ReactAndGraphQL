using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISuffixType : ICommonDelModel
    {
        String Name      { get; set; }
        String Code { get; set; }
        Int32? SortOrder { get; set; }
    }
}
