using System;

namespace Dcf.Wwp.Model.Interface
{
    public  interface ISP_DB2_RFAs_Result
    {
        decimal? PinNumber { get; set; }
         string ProgramName { get; set; }
        decimal? RfaNumber { get; set; }
         string RfaStatus { get; set; }
        DateTime? ApplicationDate { get; set; }
        DateTime? Program_End_Dt { get; set; }
        DateTime? CourtOrderDate { get; set; }
        short? CountyNumber { get; set; }
         string CountyName { get; set; }
    }
}
