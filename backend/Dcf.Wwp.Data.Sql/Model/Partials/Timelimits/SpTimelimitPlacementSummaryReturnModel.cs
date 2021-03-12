using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class SpTimelimitPlacementSummaryReturnModel
    {
        public Decimal?  CASE_NUMBER               { get; set; }
        public Int16?    PLACEMENT_SEQUENCE_NUMBER { get; set; }
        public Int16?    HISTORY_SEQUENCE_NUMBER   { get; set; }
        public Int16?    HISTORY_CD                { get; set; }
        public string    MFWorkerId                { get; set; }
        public DateTime? W2_EPISODE_BEGIN_DATE     { get; set; }
        public DateTime? W2_EPISODE_END_DATE       { get; set; }
        public DateTime  PLACEMENT_BEGIN_DATE      { get; set; }
        public DateTime  PLACEMENT_END_MONTH       { get; set; }
        public String    PLACEMENT_TYPE            { get; set; }
        public Decimal   PARTICIPANT               { get; set; }
    }
}
