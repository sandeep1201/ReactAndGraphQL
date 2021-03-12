using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmployabilityPlanStatusType : ICommonModelFinal, ICloneable
    {
        string                          Name               { get; set; }
        int                             SortOrder          { get; set; }
        ICollection<IEmployabilityPlan> EmployabilityPlans { get; set; }
    }
}
