using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class SpAlienStatusResult
    {
        public Decimal ParticipantId { get; set; }
        public String  ALIEN_STS_CD  { get; set; }

        public String   AlienStatusCodeDescriptionText { get; set; }
        public Decimal  EffectiveBeginMonth            { get; set; }
        public Decimal? EffectiveEndMonth              { get; set; }
        public DateTime ArrivalDate                    { get; set; }
        public String   CountryOfOrign                 { get; set; }
    }
}
