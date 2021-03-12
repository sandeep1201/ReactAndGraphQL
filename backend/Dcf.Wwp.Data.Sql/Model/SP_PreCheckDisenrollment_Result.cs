using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SP_PreCheckDisenrollment_Result
    {
        public decimal?  PinNumber        { get; set; }
        public bool?     ActivityOpen     { get; set; }
        public bool?     PlacementOpen    { get; set; }
        public bool?     TransactionExist { get; set; }
        public DateTime? ActivityEndDate  { get; set; }
    }
}
