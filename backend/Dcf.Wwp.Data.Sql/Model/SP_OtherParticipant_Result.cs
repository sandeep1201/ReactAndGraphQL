using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SP_OtherParticipant_Result
    {
        public decimal?  OTHER_PARTICIPANT            { get; set; }
        public decimal?  PARTICIPANT                  { get; set; }
        public string    FIRST_NAME                   { get; set; }
        public string    LAST_NAME                    { get; set; }
        public string    MIDDLE_INITIAL_NAME          { get; set; }
        public DateTime? BIRTH_DATE                   { get; set; }
        public DateTime? DEATH_DATE                   { get; set; }
        public string    GENDER                       { get; set; }
        public string    RELATIONSHIP                 { get; set; }
        public string    AGE                          { get; set; }
        public string    ELIGIBILITY_PART_STATUS_CODE { get; set; }
        public string    ISINPLACEMENTPLACED          { get; set; }
    }
}
