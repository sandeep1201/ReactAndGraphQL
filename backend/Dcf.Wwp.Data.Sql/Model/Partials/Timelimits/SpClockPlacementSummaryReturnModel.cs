using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class SpClockPlacementSummaryReturnModel
    {
        public DateTime PLACEMENT_BEGIN_DATE { get; }
        public DateTime PLACEMENT_END_MONTH  { get; }
        public String   PLACEMENT_TYPE       { get; set; }
    }
}
