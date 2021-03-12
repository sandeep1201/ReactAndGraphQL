using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EPEIBridge : BaseCommonModel, IEPEIBridge, IEquatable<EPEIBridge>
    {
        IEmployabilityPlan IEPEIBridge.EmployabilityPlan
        {
            get { return EmployabilityPlan; }
            set { EmployabilityPlan = (EmployabilityPlan)value; }
        }

        IEmploymentInformation IEPEIBridge.EmploymentInformation
        {
            get { return EmploymentInformation; }
            set { EmploymentInformation = (EmploymentInformation)value; }
        }

        #region ICloneable

        public object Clone()
        {
            var a = new EPEIBridge
            {
                Id = Id,
                EmployabilityPlanId = EmployabilityPlanId,
                EmploymentInformationId = EmploymentInformationId,
                IsDeleted = IsDeleted
            };

            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            return other is EPEIBridge obj && Equals(obj);
        }

        public bool Equals(EPEIBridge other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
                return false;
            if (!AdvEqual(EmploymentInformationId, other.EmploymentInformationId))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
