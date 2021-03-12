using System.Collections.Generic;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class GetRecoupResponse
    {
        #region Properties

        public List<string>                   MessageCodes                 { get; }
        public int                            RequestCount                 { get; set; }
        public int                            ResponseCount                { get; set; }
        public List<WWCaseCalcRecoupmentInfo> WWCaseCalcRecoupmentInfoList { get; set; }

        public GetRecoupResponse(List<string> messageCodes)
        {
            MessageCodes = messageCodes;
        }

        #endregion

        #region Methods

        #endregion
    }

    public class WWCaseCalcRecoupmentInfo
    {
        public long       CaseNumber       { get; set; }
        public int        Identifier       { get; set; }
        public List<long> LiablePINLIst    { get; set; }
        public decimal    RecoupmentAmount { get; set; }
    }
}
