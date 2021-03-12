using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EndEPContract
    {
        public int                    EPId       { get; set; }
        public List<GoalContract>     Goals      { get; set; }
        public List<ActivityContract> Activities { get; set; }
    }
}
