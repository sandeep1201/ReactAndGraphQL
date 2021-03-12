using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public  class SP_FSETStatusReturnType
    {
        public long      ID                      { get; set; }
        public string    CURRENT_STATUS_CD       { get; set; }
        public DateTime? ENROLLMENT_DATE         { get; set; }
        public DateTime? DISENROLLMENT_DATE      { get; set; }
        public string    DISENROLLMENT_REASON_CD { get; set; }
    }
}
