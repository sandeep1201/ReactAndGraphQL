using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class USP_MostRecentPrograms_Result
    {
        public string    ProgramName      { get; set; }
        public string    RecentStatus     { get; set; }
        public DateTime? RecentStatusDate { get; set; }
        public string    AssignedWorker   { get; set; }
        public string    WIUID            { get; set; }
    }
}
