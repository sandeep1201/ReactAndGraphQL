using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class T0460_IN_W2_EXT
    {
        #region Properties (Columns)

        public decimal  PIN_NUM          { get; set; }
        public string   CLOCK_TYPE_CD    { get; set; }
        public short    EXT_SEQ_NUM      { get; set; }
        public short    HISTORY_SEQ_NUM  { get; set; }
        public string   AGY_DCSN_CD      { get; set; }
        public DateTime AGY_DCSN_DT      { get; set; }
        public decimal  BENEFIT_MM       { get; set; }
        public string   DELETE_REASON_CD { get; set; }
        public decimal  EXT_BEG_MM       { get; set; }
        public decimal  EXT_END_MM       { get; set; }
        public DateTime EXT_REQ_PRC_DT   { get; set; }
        public short    HISTORY_CD       { get; set; }
        public string   STA_DCSN_CD      { get; set; }
        public DateTime UPDATED_DT       { get; set; }
        public string   USER_ID          { get; set; }
        public int      Id               { get; set; }

        #endregion
    }
}
