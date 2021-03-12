using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public class TimeLimitWSSummary
    {
        public string               PinNumber        { get; set; }
        public bool                 IsDataFound      { get; set; }
        public List<TimeLimitTicks> TimeLimitSummary { get; set; }
    }
}
