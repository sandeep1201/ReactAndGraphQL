using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class GoalStep : BaseCommonModel, IGoalStep, IEquatable<GoalStep>
    {
        IGoal IGoalStep.Goal
        {
            get { return Goal; }
            set { Goal = (Goal) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var gs = new GoalStep();

            gs.Id                  = this.Id;
            gs.GoalId              = this.GoalId;
            gs.Details             = this.Details;
            gs.IsGoalStepCompleted = this.IsGoalStepCompleted;
            gs.IsDeleted           = this.IsDeleted;
            return gs;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as GoalStep;
            return obj != null && Equals(obj);
        }

        public bool Equals(GoalStep other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(GoalId, other.GoalId))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(IsGoalStepCompleted, other.IsGoalStepCompleted))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
