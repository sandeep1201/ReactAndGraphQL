using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class T0164_WP_IN_WKR_HI
    {
        public int      Id                 { get; set; }
        public decimal  PIN_NUM            { get; set; }
        public short    EMPLOYER_SEQ_NUM   { get; set; }
        public short    HISTORY_SEQ_NUM    { get; set; }
        public short    AVG_WK_HRS         { get; set; }
        public DateTime CRE_TMS            { get; set; }
        public string   DEL_IND            { get; set; }
        public string   DURATION_EMP_IND   { get; set; }
        public string   EE_IND             { get; set; }
        public string   EMP_CITY_ADR       { get; set; }
        public string   EMP_LINE_1_ADR     { get; set; }
        public string   EMP_LINE_2_ADR     { get; set; }
        public string   EMP_STATE_ADR      { get; set; }
        public string   EMP_TYPE_CD        { get; set; }
        public string   EMP_ZIP_ADR        { get; set; }
        public string   EMPLOYER_NAM       { get; set; }
        public DateTime EMPLOYMENT_BEG_DT  { get; set; }
        public DateTime EMPLOYMENT_END_DT  { get; set; }
        public short    HISTORY_CD         { get; set; }
        public decimal  HOURLY_WAGE_AMT    { get; set; }
        public string   JOB_CD             { get; set; }
        public string   JOB_DUTIES_1_TXT   { get; set; }
        public string   JOB_DUTIES_2_TXT   { get; set; }
        public string   JOB_DUTIES_3_TXT   { get; set; }
        public string   MEDICAL_BEN_IND    { get; set; }
        public short    OFFICE_NUM         { get; set; }
        public string   OT_BEN_CD          { get; set; }
        public string   PAY_CD             { get; set; }
        public short    PROVIDER_ID        { get; set; }
        public string   STAFF_ID           { get; set; }
        public DateTime UPDT_TMS           { get; set; }
        public string   USER_ID            { get; set; }
        public string   WORK_LEFT_CD       { get; set; }
        public DateTime HRS_OR_WAGE_CHG_DT { get; set; }
        public string   JOB_TYP            { get; set; }
        public string   RES_MILW_ITIV_IND  { get; set; }
    }
}
