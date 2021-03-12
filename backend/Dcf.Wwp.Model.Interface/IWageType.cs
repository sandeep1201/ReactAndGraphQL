using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWageType : ICommonDelModel
    {
        String Name { get; set; }
        Boolean? DisablesOthersFlag { get; set; }
        Int32? SortOrder { get; set; }

        // Don't need the other nav properties
    }
}