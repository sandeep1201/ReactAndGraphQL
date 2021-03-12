using System;

namespace Dcf.Wwp.Api.Library.Model
{
    public class MostRecentProgram
    {
        #region Properties

        public string    ProgramName      { get; set; }
        public string    RecentStatus     { get; set; }
        public DateTime? RecentStatusDate { get; set; }
        public string    AssignedWorker   { get; set; }
        public string    WIUID            { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
