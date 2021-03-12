using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISP_CWWChildCareEligibiltyStatus_Result
    {
        Decimal? CaseNumber        { get; set; }
        String   ProgramCode       { get; set; }
        String   SubProgramCode    { get; set; }
        String   EligibilityStatus { get; set; }
        String   ReasonCode        { get; set; }
        String   DescriptionText   { get; set; }
        String   ReasonCode1       { get; set; }
        String   DescriptionText1  { get; set; }
        String   ReasonCode2       { get; set; }
        String   DescriptionText2  { get; set; }
    }
}
