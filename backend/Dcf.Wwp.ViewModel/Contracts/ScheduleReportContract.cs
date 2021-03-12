using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ScheduleReportContract
    {
        public string Date { get; set; }
        public string TimeWorked { get; set; }
        public string ActivityTypeName { get; set; }
        public string Description { get; set; }
        public string Business { get; set; }
        public string Location { get; set; }

    }
}
