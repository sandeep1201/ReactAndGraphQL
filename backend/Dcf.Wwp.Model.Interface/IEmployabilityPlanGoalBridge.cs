using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmployabilityPlanGoalBridge :  ICloneable
    {
        int       Id                  { get; set; }
        int       EmployabilityPlanId { get; set; }
        int       GoalId              { get; set; }
        bool      IsDeleted           { get; set; }
        string    ModifiedBy          { get; set; }
        DateTime  ModifiedDate        { get; set; }
        byte[]    RowVersion          { get; set; }

        IEmployabilityPlan EmployabilityPlan { get; set; }
        IGoal              Goal              { get; set; }
    }
}
