using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class GoalEndReason : BaseCommonModel, IGoalEndReason, IEquatable<GoalEndReason>
    {
        ICollection<IGoal> IGoalEndReason.Goals
        {
            get { return Goals.Cast<IGoal>().ToList(); }
            set { Goals = (ICollection<Goal>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var ger = new GoalEndReason();

            ger.Id        = this.Id;
            ger.Name      = this.Name;
            ger.SortOrder = this.SortOrder;
            ger.IsDeleted = this.IsDeleted;
            return ger;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as GoalEndReason;
            return obj != null && Equals(obj);
        }

        public bool Equals(GoalEndReason other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
