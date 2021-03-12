using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmployabilityPlanActivityBridge :  ICloneable
    {
        int       Id                  { get; set; }
        int       EmployabilityPlanId { get; set; }
        int       ActivityId          { get; set; }
        string    ModifiedBy          { get; set; }
        DateTime  ModifiedDate        { get; set; }
        byte[]    RowVersion          { get; set; }

        IEmployabilityPlan EmployabilityPlan { get; set; }
        IActivity          Activity          { get; set; }
    }
}
