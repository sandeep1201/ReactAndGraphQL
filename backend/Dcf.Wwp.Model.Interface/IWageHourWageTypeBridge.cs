using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWageHourWageTypeBridge : ICommonDelModel
    {
        Int32? WageHourId { get; set; }
        Int32? SortOrder { get; set; }
        Int32? WageTypeId { get; set; }

        IWageHour WageHour { get; set; }
        IWageType WageType { get; set; }
    }
}