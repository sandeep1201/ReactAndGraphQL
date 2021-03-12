using System.Collections.Generic;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class RecoupmentCaseInfoList
    {
        public List<CaseInfoList> WWRecoupmentCaseInfoList { get; set; }
    }

    public class CaseInfoList
    {
        public long       CaseNumber      { get; set; }
        public List<long> EligiblePINList { get; set; }
        public decimal    AllotmentAmount { get; set; }
        public int        Identifier      { get; set; }
    }
}
