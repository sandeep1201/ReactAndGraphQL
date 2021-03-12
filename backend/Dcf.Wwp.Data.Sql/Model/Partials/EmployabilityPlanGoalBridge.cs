using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlanGoalBridge : BaseCommonModel, IEmployabilityPlanGoalBridge, IEquatable<EmployabilityPlanGoalBridge>
    {
        IGoal IEmployabilityPlanGoalBridge.Goal
        {
            get { return Goal; }
            set { Goal = (Goal) value; }
        }

        IEmployabilityPlan IEmployabilityPlanGoalBridge.EmployabilityPlan
        {
            get { return EmployabilityPlan; }
            set { EmployabilityPlan = (EmployabilityPlan) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new EmployabilityPlanGoalBridge()
                        {
                            GoalId              = this.GoalId,
                            EmployabilityPlanId = this.EmployabilityPlanId
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmployabilityPlanGoalBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmployabilityPlanGoalBridge other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.           
            if (!AdvEqual(GoalId, other.GoalId))
                return false;
            if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
                return false;
            return true;
        }

        #endregion IEquatable<T>

        DateTime IEmployabilityPlanGoalBridge.ModifiedDate
        {
            get => ModifiedDate;
            set => ModifiedDate = value;
        }
    }
}
