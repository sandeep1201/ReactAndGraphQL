using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class LicenseType : BaseCommonModel, ILicenseType, IEquatable<LicenseType>
    {
        ICollection<IPostSecondaryLicense> ILicenseType.PostSecondaryLicenses
        {
            get { return PostSecondaryLicenses.Cast<IPostSecondaryLicense>().ToList(); }

            set { PostSecondaryLicenses = value.Cast<PostSecondaryLicense>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var lt = new LicenseType();

            lt.Id        = this.Id;
            lt.SortOrder = this.SortOrder;
            lt.Name      = this.Name;
            return lt;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as LicenseType;
            return obj != null && Equals(obj);
        }

        public bool Equals(LicenseType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)               &&
                   SortOrder.Equals(other.SortOrder) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
