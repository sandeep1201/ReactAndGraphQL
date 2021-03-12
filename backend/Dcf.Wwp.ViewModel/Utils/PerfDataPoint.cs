using System;

namespace Dcf.Wwp.Api.Library.Utils
{
    public class PerfDataPoint
    {
        #region Properties

        public int      Id         { get; set; }
        public string   MethodName { get; set; }
        public DateTime StartTime  { get; set; }
        public DateTime StopTime   { get; set; }
        public TimeSpan Elapsed    => StopTime - StartTime;
        public int      Cached     { get; set; }
        public int      Web        { get; set; }
        public int      Retries    { get; set; }
        public int      Total      { get; set; }
        public string   UserId     { get; set; }

        #endregion

        #region Methods

        public PerfDataPoint (string methodName, string userId)
        {
            MethodName = methodName;
            UserId     = userId;
            Cached     = 0;
            Web        = 0;
            Retries    = 0;
            Total      = 0;
        }

        public void Start() => StartTime = DateTime.Now;
        public void Stop()  => StopTime  = DateTime.Now;

        #endregion
    }
}
