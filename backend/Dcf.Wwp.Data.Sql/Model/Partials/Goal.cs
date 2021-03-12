using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Goal : BaseCommonModel, IGoal, IEquatable<Goal>
    {
        // IEmployabilityPlan IGoal.EmployabilityPlan
        // {
        //     get { return EmployabilityPlan; }
        //     set { EmployabilityPlan = (EmployabilityPlan)value; }
        // }

        IGoalEndReason IGoal.GoalEndReason
        {
            get { return GoalEndReason; }
            set { GoalEndReason = (GoalEndReason) value; }
        }

        IGoalType IGoal.GoalType
        {
            get { return GoalType; }
            set { GoalType = (GoalType) value; }
        }

        ICollection<IGoalStep> IGoal.GoalSteps
        {
            get { return GoalSteps.Cast<IGoalStep>().ToList(); }
            set { GoalSteps = (ICollection<GoalStep>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var g = new Goal();

            g.Id = this.Id;
            //g.EmployabilityPlanId = this.EmployabilityPlanId;
            g.GoalTypeId       = this.GoalTypeId;
            g.BeginDate        = this.BeginDate;
            g.Name             = this.Name;
            g.Details          = this.Details;
            g.IsGoalEnded      = this.IsGoalEnded;
            g.GoalEndReasonId  = this.GoalEndReasonId;
            g.EndReasonDetails = this.EndReasonDetails;
            g.IsDeleted        = this.IsDeleted;
            return g;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Goal;
            return obj != null && Equals(obj);
        }

        public bool Equals(Goal other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            //if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
            //    return false;
            if (!AdvEqual(GoalTypeId, other.GoalTypeId))
                return false;
            if (!AdvEqual(BeginDate, other.BeginDate))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(IsGoalEnded, other.IsGoalEnded))
                return false;
            if (!AdvEqual(GoalEndReasonId, other.GoalEndReasonId))
                return false;
            if (!AdvEqual(EndReasonDetails, other.EndReasonDetails))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
