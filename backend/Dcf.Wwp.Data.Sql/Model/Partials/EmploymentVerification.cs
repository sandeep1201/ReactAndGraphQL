using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentVerification : BaseCommonModel, IEmploymentVerification, IEquatable<EmploymentVerification>
    {
        IEmploymentInformation IEmploymentVerification.EmploymentInformation
        {
            get => EmploymentInformation;
            set => EmploymentInformation = (EmploymentInformation) value;
        }

        #region ICloneable

        public object Clone()
        {
            var a = new EmploymentVerification
                    {
                        Id                         = Id,
                        EmploymentInformationId    = EmploymentInformationId,
                        IsVerified                 = IsVerified,
                        CreatedDate                = CreatedDate,
                        IsDeleted                  = IsDeleted,
                        ModifiedBy                 = ModifiedBy,
                        ModifiedDate               = ModifiedDate,
                        NumberOfDaysAtVerification = NumberOfDaysAtVerification
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

            return other is EmploymentVerification obj && Equals(obj);
        }

        public bool Equals(EmploymentVerification other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmploymentInformationId, other.EmploymentInformationId))
                return false;
            if (!AdvEqual(IsVerified, other.IsVerified))
                return false;
            if (!AdvEqual(CreatedDate, other.CreatedDate))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(ModifiedBy, other.ModifiedBy))
                return false;
            if (!AdvEqual(ModifiedDate, other.ModifiedDate))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
