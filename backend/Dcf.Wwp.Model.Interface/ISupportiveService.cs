using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISupportiveService : ICommonModelFinal, ICloneable
    {
        int                    Id                      { get; set; }
        int                    EmployabilityPlanId     { get; set; }
        int                    SupportiveServiceTypeId { get; set; }
        string                 Details                 { get; set; }
        IEmployabilityPlan     EmployabilityPlan       { get; set; }
        ISupportiveServiceType SupportiveServiceType   { get; set; }
    }
}
