using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public class TimeLimitWebService
    {
        public List<TimeLimitWSSummary> Participants { get; set; }
        public string                   MessageCode  { get; set; }
    }
}
