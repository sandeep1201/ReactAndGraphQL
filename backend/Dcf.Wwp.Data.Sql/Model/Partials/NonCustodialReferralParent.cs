using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;


// ReSharper disable once CheckNamespace
namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialReferralParent : BaseEntity, INonCustodialReferralParent, IEquatable<NonCustodialReferralParent>
    {
        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }

            set { DeleteReason = (DeleteReason) value; }
        }

        ICollection<INonCustodialReferralChild> INonCustodialReferralParent.NonCustodialReferralChilds
        {
            get { return (from x in NonCustodialReferralChilds where x.DeleteReasonId == null select x).Cast<INonCustodialReferralChild>().ToList(); }
            set { NonCustodialReferralChilds = value.Cast<NonCustodialReferralChild>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialReferralParent()
                        {
                            Id                             = this.Id,
                            FirstName                      = this.FirstName,
                            LastName                       = this.LastName,
                            IsAvailableOrWorking           = this.IsAvailableOrWorking,
                            AvailableOrWorkingDetails      = this.AvailableOrWorkingDetails,
                            IsInterestedInWorkProgram      = this.IsInterestedInWorkProgram,
                            InterestedInWorkProgramDetails = this.InterestedInWorkProgramDetails,
                            IsContactKnownWithParent       = this.IsContactKnownWithParent,
                            ContactId                      = this.ContactId,
                            DeleteReasonId                 = this.DeleteReasonId,

                            NonCustodialReferralChilds = NonCustodialReferralChilds.Select(x => (NonCustodialReferralChild) x.Clone()).ToList()

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

            var obj = other as NonCustodialReferralParent;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialReferralParent other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(FirstName, other.FirstName))
                return false;
            if (!AdvEqual(LastName, other.LastName))
                return false;
            if (!AdvEqual(IsAvailableOrWorking, other.IsAvailableOrWorking))
                return false;
            if (!AdvEqual(AvailableOrWorkingDetails, other.AvailableOrWorkingDetails))
                return false;
            if (!AdvEqual(IsInterestedInWorkProgram, other.IsInterestedInWorkProgram))
                return false;
            if (!AdvEqual(InterestedInWorkProgramDetails, other.InterestedInWorkProgramDetails))
                return false;
            if (!AdvEqual(IsContactKnownWithParent, other.IsContactKnownWithParent))
                return false;
            if (!AdvEqual(ContactId, other.ContactId))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;

            if (AreBothNotNull(NonCustodialReferralChilds, other.NonCustodialReferralChilds) && !NonCustodialReferralChilds.OrderBy(x => x.Id).SequenceEqual(other.NonCustodialReferralChilds.OrderBy(x => x.Id)))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
