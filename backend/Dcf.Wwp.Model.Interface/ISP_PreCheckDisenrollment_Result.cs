using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISP_PreCheckDisenrollment_Result
    {
        decimal?  PinNumber        { get; set; }
        bool?     ActivityOpen     { get; set; }
        DateTime? ActivityEndDate  { get; set; }
        bool?     PlacementOpen    { get; set; }
        bool?     TransactionExist { get; set; }
    }
}
