using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ContactReportContract
    {
        public int    ActivityId {get;set;}
        public string ContactTitleTypeName { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactExt { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }
        public string ContactEmail { get; set; }
    }
}
