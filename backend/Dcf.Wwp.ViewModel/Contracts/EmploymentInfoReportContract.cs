using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EmploymentInfoReportContract
    {
        public string EmploymentJobTypeName { get; set; }
        public string EmploymentJobPosition { get; set; }
        public string EmploymentCompanyName { get; set; }
        public string EmploymentLocation { get; set; }
        public string EmploymentJobBeginDate { get; set; }
        public string EmploymentCurrentAverageWeeklyHours { get; set; }
        public string EmploymentPosition { get; set; }
    }
}
