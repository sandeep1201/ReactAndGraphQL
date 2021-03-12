using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SupportiveService : BaseCommonModel, ISupportiveService, IEquatable<SupportiveService>
    {
        IEmployabilityPlan ISupportiveService.EmployabilityPlan
        {
            get { return EmployabilityPlan; }
            set { EmployabilityPlan = (EmployabilityPlan) value; }
        }

        ISupportiveServiceType ISupportiveService.SupportiveServiceType
        {
            get { return SupportiveServiceType; }
            set { SupportiveServiceType = (SupportiveServiceType) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var ss = new SupportiveService();

            ss.Id                      = this.Id;
            ss.EmployabilityPlanId     = this.EmployabilityPlanId;
            ss.SupportiveServiceTypeId = this.SupportiveServiceTypeId;
            ss.Details                 = this.Details;
            ss.IsDeleted               = this.IsDeleted;
            return ss;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as SupportiveService;
            return obj != null && Equals(obj);
        }

        public bool Equals(SupportiveService other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
                return false;
            if (!AdvEqual(SupportiveServiceTypeId, other.SupportiveServiceTypeId))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
