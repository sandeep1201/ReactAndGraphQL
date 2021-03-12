using System.Collections.Generic;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class PostRecoupResponse
    {
        #region Properties

        public List<InvalidCaseList> InvalidCaseList { get; set; }
        public int                   RequestCount    { get; set; }
        public int                   ResponseCount   { get; set; }
        public List<string>          MessageCodes    { get; }

        public PostRecoupResponse(List<string> messageCodes)
        {
            MessageCodes = messageCodes;
        }

        #endregion

        #region Methods

        #endregion
    }

    public class InvalidCaseList
    {
        public long    CaseNumber                { get; set; }
        public string  InvalidMessageCode        { get; set; }
        public decimal RequestedRecoupmentAmount { get; set; }
        public decimal PartialRecoupedAmount     { get; set; }
    }
}
