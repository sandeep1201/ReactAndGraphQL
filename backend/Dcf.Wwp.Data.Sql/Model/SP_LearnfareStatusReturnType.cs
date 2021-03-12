using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public  class SP_LearnfareStatusReturnType
    {
        public long      ID                 { get; set; }
        public string    FIRST_NAM          { get; set; }
        public string    LAST_NAM           { get; set; }
        public string    MIDDLE_INITIAL_NAM { get; set; }
        public DateTime? DOB_DT             { get; set; }
        public string    LEARN_FARE_STATUS  { get; set; }
    }
}
