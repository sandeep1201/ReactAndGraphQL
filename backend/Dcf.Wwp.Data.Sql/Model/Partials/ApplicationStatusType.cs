using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ApplicationStatusType : BaseCommonModel, IApplicationStatusType
    {
        ICollection<IFamilyBarriersSection> IApplicationStatusType.FamilyBarriersSections
        {
            get { return FamilyBarriersSections.Cast<IFamilyBarriersSection>().ToList(); }
            set { FamilyBarriersSections = value.Cast<FamilyBarriersSection>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var cca = new ApplicationStatusType()
                      {
                          Id                    = this.Id,
                          ApplicationStatusName = this.ApplicationStatusName,
                          IsDeleted             = this.IsDeleted
                      };

            return cca;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ApplicationStatusType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ApplicationStatusType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(ApplicationStatusName, other.ApplicationStatusName))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
