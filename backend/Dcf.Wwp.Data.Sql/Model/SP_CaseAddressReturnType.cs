namespace Dcf.Wwp.Data.Sql.Model
{
    public  class SP_CaseAddressReturnType
    {
        public long    ID             { get; set; }
        public decimal CASE_NUM       { get; set; }
        public string  LINE_1_ADDRESS { get; set; }
        public string  LINE_2_ADDRESS { get; set; }
        public string  CITY_ADR       { get; set; }
        public string  STATE_ADR      { get; set; }
        public string  ZIP_ADR        { get; set; }
        public short?  COUNTY_NUM     { get; set; }
    }
}
