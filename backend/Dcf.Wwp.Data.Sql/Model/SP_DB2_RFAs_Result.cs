using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SP_DB2_RFAs_Result
    {
        public decimal?  PinNumber       { get; set; }
        public string    ProgramName     { get; set; }
        public decimal?  RfaNumber       { get; set; }
        public string    RfaStatus       { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? Program_End_Dt  { get; set; }
        public DateTime? CourtOrderDate  { get; set; }
        public short?    CountyNumber    { get; set; }
        public string    CountyName      { get; set; }
    }
}
