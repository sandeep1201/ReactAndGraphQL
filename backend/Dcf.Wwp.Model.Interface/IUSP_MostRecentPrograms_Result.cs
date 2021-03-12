using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IUSP_MostRecentPrograms_Result
    {
        string    ProgramName      { get; set; }
        string    RecentStatus     { get; set; }
        DateTime? RecentStatusDate { get; set; }
        string    AssignedWorker   { get; set; }
        string    WIUID            { get; set; }
    }
}
