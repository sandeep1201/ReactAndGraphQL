using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWageHourHistoryWageTypeBridge : ICommonDelModel, ICloneable
    {
        Int32? WageHourHistoryId { get; set; }
        Int32? WageTypeId { get; set; }
        Int32? SortOrder { get; set; }

        IWageHourHistory WageHourHistory { get; set; }
        IWageType WageType { get; set; }
    }
}