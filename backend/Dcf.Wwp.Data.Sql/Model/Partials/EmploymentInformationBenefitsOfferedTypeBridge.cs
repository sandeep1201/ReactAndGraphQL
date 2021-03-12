using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentInformationBenefitsOfferedTypeBridge : BaseCommonModel, IEmploymentInformationBenefitsOfferedTypeBridge, IEquatable<EmploymentInformationBenefitsOfferedTypeBridge>
    {
        IBenefitsOfferedType IEmploymentInformationBenefitsOfferedTypeBridge.BenefitsOfferedType
        {
            get { return BenefitsOfferedType; }
            set { BenefitsOfferedType = (BenefitsOfferedType) value; }
        }

        IEmploymentInformation IEmploymentInformationBenefitsOfferedTypeBridge.EmploymentInformation
        {
            get { return EmploymentInformation; }
            set { EmploymentInformation = (EmploymentInformation) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new EmploymentInformationBenefitsOfferedTypeBridge()
                        {
                            Id                      = this.Id,
                            EmploymentInformationId = this.EmploymentInformationId,
                            BenefitsOfferedTypeId   = this.BenefitsOfferedTypeId,
                            SortOrder               = this.SortOrder,
                            IsDeleted               = this.IsDeleted,
                            BenefitsOfferedType     = (BenefitsOfferedType) this.BenefitsOfferedType?.Clone()
                        };

            // NOTE: We don't clone references to "parent" objects or other reference collections.

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmploymentInformationBenefitsOfferedTypeBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmploymentInformationBenefitsOfferedTypeBridge other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmploymentInformationId, other.EmploymentInformationId))
                return false;
            if (!AdvEqual(BenefitsOfferedTypeId, other.BenefitsOfferedTypeId))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            // NOTE: We don't equals check "parent" or ohter reference objects.

            return true;
        }

        #endregion IEquatable<T>
    }
}
