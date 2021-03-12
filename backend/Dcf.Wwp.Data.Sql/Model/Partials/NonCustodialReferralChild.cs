using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialReferralChild : BaseEntity, INonCustodialReferralChild, IEquatable<NonCustodialReferralChild>
    {
        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }

            set { DeleteReason = (DeleteReason) value; }
        }

        IReferralContactInterval INonCustodialReferralChild.ReferralContactInterval
        {
            get { return ReferralContactInterval; }

            set { ReferralContactInterval = (ReferralContactInterval) value; }
        }

        INonCustodialReferralParent INonCustodialReferralChild.NonCustodialReferralParent
        {
            get { return NonCustodialReferralParent; }

            set { NonCustodialReferralParent = (NonCustodialReferralParent) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialReferralChild()
                        {
                            Id                        = this.Id,
                            FirstName                 = this.FirstName,
                            LastName                  = this.LastName,
                            HasChildSupportOrder      = this.HasChildSupportOrder,
                            ChildSupportOrderDetails  = this.ChildSupportOrderDetails,
                            ReferralContactIntervalId = this.ReferralContactIntervalId,
                            ContactIntervalDetails    = this.ContactIntervalDetails,
                            DeleteReasonId            = this.DeleteReasonId,

                            // Don't clone parent object -- will cause recursive calls.
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonCustodialReferralChild;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialReferralChild other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(FirstName, other.FirstName))
                return false;
            if (!AdvEqual(LastName, other.LastName))
                return false;
            if (!AdvEqual(HasChildSupportOrder, other.HasChildSupportOrder))
                return false;
            if (!AdvEqual(ChildSupportOrderDetails, other.ChildSupportOrderDetails))
                return false;
            if (!AdvEqual(ReferralContactIntervalId, other.ReferralContactIntervalId))
                return false;
            if (!AdvEqual(ContactIntervalDetails, other.ContactIntervalDetails))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
