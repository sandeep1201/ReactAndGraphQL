using System;
using System.Collections.Generic;

namespace DCF.Timelimits.Rules.Domain
{
    public class ExecutionResult
    {
        public String Pin { get; set; }
        private ClockTypes ClockType { get; set; }
        public Boolean Placement { get; set; }
        public Boolean ShouldContinue { get; set; } = true;
    }
}
