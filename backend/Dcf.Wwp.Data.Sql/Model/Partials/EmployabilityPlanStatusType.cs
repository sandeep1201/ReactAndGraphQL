using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlanStatusType : BaseCommonModel, IEmployabilityPlanStatusType, IEquatable<EmployabilityPlanStatusType>
    {
        ICollection<IEmployabilityPlan> IEmployabilityPlanStatusType.EmployabilityPlans
        {
            get { return EmployabilityPlans.Cast<IEmployabilityPlan>().ToList(); }
            set { EmployabilityPlans = (ICollection<EmployabilityPlan>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var at = new ActivityType();

            at.Id        = this.Id;
            at.Name      = this.Name;
            at.SortOrder = this.SortOrder;
            at.IsDeleted = this.IsDeleted;
            return at;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmployabilityPlanStatusType;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmployabilityPlanStatusType other)
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
