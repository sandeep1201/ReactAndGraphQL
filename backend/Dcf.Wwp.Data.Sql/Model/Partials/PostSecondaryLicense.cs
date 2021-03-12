using Dcf.Wwp.Model.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class PostSecondaryLicense : BaseCommonModel, IPostSecondaryLicense, IEquatable<PostSecondaryLicense>
    {
        IPostSecondaryEducationSection IPostSecondaryLicense.PostSecondaryEducationSection
        {
            get { return PostSecondaryEducationSection; }
            set { PostSecondaryEducationSection = (PostSecondaryEducationSection) value; }
        }

        ILicenseType IPostSecondaryLicense.LicenseType
        {
            get { return LicenseType; }
            set { LicenseType = (LicenseType) value; }
        }

        IPolarLookup IPostSecondaryLicense.PolarLookup
        {
            get { return PolarLookup; }
            set { PolarLookup = (PolarLookup) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var psl = new PostSecondaryLicense();

            psl.Id                     = this.Id;
            psl.Name                   = this.Name;
            psl.Issuer                 = this.Issuer;
            psl.AttainedDate           = this.AttainedDate;
            psl.ExpiredDate            = this.ExpiredDate;
            psl.IsInProgress           = this.IsInProgress;
            psl.DoesNotExpire          = this.DoesNotExpire;
            psl.LicenseTypeId          = this.LicenseTypeId;
            psl.ValidInWIPolarLookupId = this.ValidInWIPolarLookupId;
            psl.IsDeleted              = this.IsDeleted;

            // NOTE: We don't clone references to "parent" objects such as PostSecondaryEducationSection

            return psl;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as PostSecondaryLicense;
            return obj != null && Equals(obj);
        }

        public bool Equals(PostSecondaryLicense other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            //Check whether the products' properties are equal.
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(Issuer, other.Issuer))
                return false;
            if (!AdvEqual(AttainedDate, other.AttainedDate))
                return false;
            if (!AdvEqual(ExpiredDate, other.ExpiredDate))
                return false;
            if (!AdvEqual(IsInProgress, other.IsInProgress))
                return false;
            if (!AdvEqual(DoesNotExpire, other.DoesNotExpire))
                return false;
            if (!AdvEqual(LicenseTypeId, other.LicenseTypeId))
                return false;
            if (!AdvEqual(ValidInWIPolarLookupId, other.ValidInWIPolarLookupId))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
