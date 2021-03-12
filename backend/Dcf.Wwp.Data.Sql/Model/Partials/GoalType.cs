using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class GoalType : BaseCommonModel, IGoalType, IEquatable<GoalType>
    {
        IEnrolledProgram IGoalType.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        ICollection<IGoal> IGoalType.Goals
        {
            get { return Goals.Cast<IGoal>().ToList(); }
            set { Goals = (ICollection<Goal>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var gt = new GoalType();

            gt.Id                = this.Id;
            gt.Name              = this.Name;
            gt.EnrolledProgramId = this.EnrolledProgramId;
            gt.SortOrder         = this.SortOrder;
            gt.IsDeleted         = this.IsDeleted;
            return gt;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as GoalType;
            return obj != null && Equals(obj);
        }

        public bool Equals(GoalType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EnrolledProgramId, other.EnrolledProgramId))
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
