using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class SupportiveServiceContract
    {
        public int Id { get; set; }

        public int EmployabilityPlanId { get; set; }

        public string Details                   { get; set; }
        public int    SupportiveServiceTypeId   { get; set; }
        public string SupportiveServiceTypeName { get; set; }
    }
}
