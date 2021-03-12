using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class T0459_IN_W2_LIMITS
    {
        #region Properties (Columns)

        public int      Id                 { get; set; }
        public decimal  PIN_NUM            { get; set; }
        public decimal  BENEFIT_MM         { get; set; }
        public short    HISTORY_SEQ_NUM    { get; set; }
        public string   CLOCK_TYPE_CD      { get; set; }
        public string   CRE_TRAN_CD        { get; set; }
        public string   FED_CLOCK_IND      { get; set; }
        public short    FED_CMP_MTH_NUM    { get; set; }
        public short    FED_MAX_MTH_NUM    { get; set; }
        public short    HISTORY_CD         { get; set; }
        public short    OT_CMP_MTH_NUM     { get; set; }
        public string   OVERRIDE_REASON_CD { get; set; }
        public short    TOT_CMP_MTH_NUM    { get; set; }
        public short    TOT_MAX_MTH_NUM    { get; set; }
        public DateTime UPDATED_DT         { get; set; }
        public string   USER_ID            { get; set; }
        public short    WW_CMP_MTH_NUM     { get; set; }
        public short    WW_MAX_MTH_NUM     { get; set; }
        public string   COMMENT_TXT        { get; set; }

        #endregion
    }
}
