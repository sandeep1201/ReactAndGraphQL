using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public  class SP_ParticpantsChildrenReturnType
    {
        public long      ID                 { get; set; }
        public decimal   SOURCE_PIN_NUM     { get; set; }
        public string    FIRST_NAM          { get; set; }
        public string    LAST_NAM           { get; set; }
        public string    MIDDLE_INITIAL_NAM { get; set; }
        public DateTime? DOB_DT             { get; set; }
        public DateTime? DEATH_DT           { get; set; }
        public string    GENDER             { get; set; }
        public string    RELATIONSHIP       { get; set; }
        public int?      AGE                { get; set; }
    }
}
