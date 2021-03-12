namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SP_CWWChildCareEligibiltyStatus_Result
    {
        public decimal? CaseNumber        { get; set; }
        public string   ProgramCode       { get; set; }
        public string   SubProgramCode    { get; set; }
        public string   EligibilityStatus { get; set; }
        public string   ReasonCode        { get; set; }
        public string   DescriptionText   { get; set; }
        public string   ReasonCode1       { get; set; }
        public string   DescriptionText1  { get; set; }
        public string   ReasonCode2       { get; set; }
        public string   DescriptionText2  { get; set; }
    }
}
