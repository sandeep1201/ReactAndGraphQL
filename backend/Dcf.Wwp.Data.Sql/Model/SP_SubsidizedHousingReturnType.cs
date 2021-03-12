namespace Dcf.Wwp.Data.Sql.Model
{
    public  class SP_SubsidizedHousingReturnType
    {
        public long     ID             { get; set; }
        public decimal  CASE_NUM       { get; set; }
        public string   SUBSD_HSE_TEXT { get; set; }
        public string   SUBSD_HSE_CD   { get; set; }
        public decimal? EFF_BEGIN_MM   { get; set; }
        public decimal? EFF_END_MM     { get; set; }
    }
}
