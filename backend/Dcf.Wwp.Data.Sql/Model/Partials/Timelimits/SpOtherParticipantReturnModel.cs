namespace Dcf.Wwp.Data.Sql.Model
{
    public class SpOtherParticipantReturnModel
    {
        public System.Decimal?  OTHER_PARTICIPANT            { get; set; }
        public System.Decimal?  PARTICIPANT                  { get; set; }
        public System.String    FIRST_NAME                   { get; set; }
        public System.String    LAST_NAME                    { get; set; }
        public System.String    MIDDLE_INITIAL_NAME          { get; set; }
        public System.DateTime? BIRTH_DATE                   { get; set; }
        public System.DateTime? DEATH_DATE                   { get; set; }
        public System.String    GENDER                       { get; set; }
        public System.String    RELATIONSHIP                 { get; set; }
        public System.String    AGE                          { get; set; }
        public System.String    ELIGIBILITY_PART_STATUS_CODE { get; set; }
        public System.String    ISINPLACEMENTPLACED          { get; set; }
    }
}
