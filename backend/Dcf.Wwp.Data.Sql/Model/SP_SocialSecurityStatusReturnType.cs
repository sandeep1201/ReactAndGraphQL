using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public  class SP_SocialSecurityStatusReturnType
    {
        public long      ID                 { get; set; }
        public decimal?  PARTICIPANT        { get; set; }
        public string    FIRST_NAM          { get; set; }
        public string    MIDDLE_INITIAL_NAM { get; set; }
        public string    LAST_NAM           { get; set; }
        public DateTime? DOB_DT             { get; set; }
        public string    REL_CD             { get; set; }
        public string    AGE                { get; set; }
        public string    FED_SSI            { get; set; }
        public string    STATE_SSI          { get; set; }
        public string    SSA                { get; set; }
    }
}
