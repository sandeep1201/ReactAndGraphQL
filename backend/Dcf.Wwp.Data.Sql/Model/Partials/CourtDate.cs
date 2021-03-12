using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class CourtDate : BaseCommonModel, ICourtDate, IEquatable<CourtDate>
    {
        ILegalIssuesSection ICourtDate.LegalIssuesSection
        {
            get { return LegalIssuesSection; }
            set { LegalIssuesSection = (LegalIssuesSection) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new CourtDate
                        {
                            Id             = this.Id,
                            LegalSectionId = this.LegalSectionId,
                            Location       = this.Location,
                            Date           = this.Date,
                            IsUnknown      = this.IsUnknown,
                            Details        = this.Details,
                            IsDeleted      = this.IsDeleted
                        };

            // NOTE: We don't clone references to "parent" objects such as Legal Issue Section

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as CourtDate;
            return obj != null && Equals(obj);
        }

        public bool Equals(CourtDate other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(LegalSectionId, other.LegalSectionId))
                return false;
            if (!AdvEqual(Location, other.Location))
                return false;
            if (!AdvEqual(Date, other.Date))
                return false;
            if (!AdvEqual(IsUnknown, other.IsUnknown))
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
