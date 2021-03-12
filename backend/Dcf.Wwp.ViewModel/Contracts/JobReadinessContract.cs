using System;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class JobReadinessContract
    {
        public int                       Id                { get; set; }
        public JRApplicationInfoContract JrApplicationInfo { get; set; }
        public JRContactInfoContract     JrContactInfo     { get; set; }
        public JRHistoryInfoContract     JrHistoryInfo     { get; set; }
        public JRInterviewInfoContract   JrInterviewInfo   { get; set; }
        public JRWorkPreferencesContract JrWorkPreferences { get; set; }
        public ActionNeededContract      ActionNeeded      { get; set; }
        public DateTime                  CreatedDate       { get; set; }
        public string                    ModifiedBy        { get; set; }
        public DateTime                  ModifiedDate      { get; set; }
    }
}
