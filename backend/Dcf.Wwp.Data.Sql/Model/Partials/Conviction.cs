using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class Conviction : BaseEntity, IConviction, IEquatable<Conviction>
    {
        ILegalIssuesSection IConviction.LegalIssuesSection
        {
            get { return LegalIssuesSection; }
            set { LegalIssuesSection = (LegalIssuesSection) value; }
        }

        IConvictionType IConviction.ConvictionType
        {
            get { return ConvictionType; }
            set { ConvictionType = (ConvictionType) value; }
        }

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }
            set { DeleteReason = (DeleteReason) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var c = new Conviction();

            c.Id               = this.Id;
            c.LegalSectionId   = this.LegalSectionId;
            c.ConvictionTypeID = this.ConvictionTypeID;
            c.IsUnknown        = this.IsUnknown;
            c.Details          = this.Details;
            c.DateConvicted    = this.DateConvicted;
            c.DeleteReasonId   = this.DeleteReasonId;

            c.ConvictionType = (ConvictionType) this.ConvictionType?.Clone();

            // NOTE: We don't clone references to "parent" objects such as Legal Issue Section

            return c;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Conviction;
            return obj != null && Equals(obj);
        }

        public bool Equals(Conviction other)
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
            if (!AdvEqual(IsUnknown, other.IsUnknown))
                return false;
            if (!AdvEqual(DateConvicted, other.DateConvicted))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(ConvictionType, other.ConvictionType))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
