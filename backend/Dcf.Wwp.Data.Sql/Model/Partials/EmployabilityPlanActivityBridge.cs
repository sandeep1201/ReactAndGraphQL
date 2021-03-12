using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlanActivityBridge : BaseCommonModel, IEmployabilityPlanActivityBridge, IEquatable<EmployabilityPlanActivityBridge>
    {
        IActivity IEmployabilityPlanActivityBridge.Activity
        {
            get { return Activity; }
            set { Activity = (Activity) value; }
        }

        IEmployabilityPlan IEmployabilityPlanActivityBridge.EmployabilityPlan
        {
            get { return EmployabilityPlan; }
            set { EmployabilityPlan = (EmployabilityPlan) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new EmployabilityPlanActivityBridge()
                        {
                            ActivityId          = this.ActivityId,
                            EmployabilityPlanId = this.EmployabilityPlanId,
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmployabilityPlanActivityBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmployabilityPlanActivityBridge other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.           
            if (!AdvEqual(ActivityId, other.ActivityId))
                return false;
            if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
                return false;
            return true;
        }

        #endregion IEquatable<T>

        DateTime IEmployabilityPlanActivityBridge.ModifiedDate
        {
            get => ModifiedDate;
            set => ModifiedDate = value;
        }
    }
}
