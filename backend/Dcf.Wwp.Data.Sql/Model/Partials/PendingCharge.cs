using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class PendingCharge : BaseCommonModel, IPendingCharge, IEquatable<PendingCharge>
    {
        ILegalIssuesSection IPendingCharge.LegalIssuesSection
        {
            get { return LegalIssuesSection; }
            set { LegalIssuesSection = (LegalIssuesSection) value; }
        }

        IConvictionType IPendingCharge.ConvictionType
        {
            get { return ConvictionType; }
            set { ConvictionType = (ConvictionType) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var c = new PendingCharge();

            c.Id               = this.Id;
            c.LegalSectionId   = this.LegalSectionId;
            c.ConvictionTypeID = this.ConvictionTypeID;
            c.ChargeDate       = this.ChargeDate;
            c.IsUnknown        = this.IsUnknown;
            c.Details          = this.Details;
            c.IsDeleted        = this.IsDeleted;
            c.ConvictionType   = (ConvictionType) this.ConvictionType?.Clone();

            // NOTE: We don't clone references to "parent" objects such as Legal Issue Section

            return c;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as PendingCharge;
            return obj != null && Equals(obj);
        }

        public bool Equals(PendingCharge other)
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
            if (!AdvEqual(ConvictionTypeID, other.ConvictionTypeID))
                return false;
            if (!AdvEqual(ChargeDate, other.ChargeDate))
                return false;
            if (!AdvEqual(IsUnknown, other.IsUnknown))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(ConvictionType, other.ConvictionType))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
