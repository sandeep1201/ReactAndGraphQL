using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAliasType : ICommonDelModel
    {
        String Name      { get; set; }
        String Code      { get; set; }
        Int32? SortOrder { get; set; }
    }
}
