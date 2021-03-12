using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    partial class NonCustodialCaretaker : BaseEntity, INonCustodialCaretaker, IEquatable<NonCustodialCaretaker>
    {

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }
            set { DeleteReason = (DeleteReason) value; }
        }

        IContactInterval INonCustodialCaretaker.ContactInterval
        {
            get { return ContactInterval; }
            set { ContactInterval = (ContactInterval) value; }
        }

        ICollection<INonCustodialChild> INonCustodialCaretaker.NonCustodialChilds
        {
            get { return NonCustodialChilds.Cast<INonCustodialChild>().ToList(); }
            set { NonCustodialChilds = value.Cast<NonCustodialChild>().ToList(); }
        }

        INonCustodialParentRelationship INonCustodialCaretaker.NonCustodialParentRelationship
        {
            get { return NonCustodialParentRelationship; }
            set { NonCustodialParentRelationship = (NonCustodialParentRelationship) value; }
        }

        INonCustodialParentsSection INonCustodialCaretaker.NonCustodialParentsSection
        {
            get { return NonCustodialParentsSection; }
            set { NonCustodialParentsSection = (NonCustodialParentsSection) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialCaretaker()
                        {
                            Id                                      = this.Id,
                            FirstName                               = this.FirstName,
                            IsFirstNameUnknown                      = this.IsFirstNameUnknown,
                            LastName                                = this.LastName,
                            IsLastNameUnknown                       = this.IsLastNameUnknown,
                            NonCustodialParentRelationshipId        = this.NonCustodialParentRelationshipId,
                            RelationshipDetails                     = this.RelationshipDetails,
                            ContactIntervalId                       = this.ContactIntervalId,
                            ContactIntervalDetails                  = this.ContactIntervalDetails,
                            IsRelationshipChangeRequested           = this.IsRelationshipChangeRequested,
                            RelationshipChangeRequestedDetails      = this.RelationshipChangeRequestedDetails,
                            IsInterestedInRelationshipReferral      = this.IsInterestedInRelationshipReferral,
                            InterestedInRelationshipReferralDetails = this.InterestedInRelationshipReferralDetails,
                            DeleteReasonId                          = this.DeleteReasonId,

                            //AgeCategory = (AgeCategory)this.AgeCategory?.Clone(),
                            //Child = (Child)this.Child?.Clone(),
                            //ChildCareArrangement = (ChildCareArrangement)this.ChildCareArrangement?.Clone(),

                            // Don't clone parent object -- qill cause recursive calls.
                            //ChildYouthSection = (ChildYouthSection)this.ChildYouthSection?.Clone(),
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonCustodialCaretaker;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialCaretaker other)
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
            if (!AdvEqual(IsFirstNameUnknown, other.IsFirstNameUnknown))
                return false;
            if (!AdvEqual(LastName, other.LastName))
                return false;
            if (!AdvEqual(IsLastNameUnknown, other.IsLastNameUnknown))
                return false;
            if (!AdvEqual(NonCustodialParentRelationshipId, other.NonCustodialParentRelationshipId))
                return false;
            if (!AdvEqual(RelationshipDetails, other.RelationshipDetails))
                return false;
            if (!AdvEqual(ContactIntervalId, other.ContactIntervalId))
                return false;
            if (!AdvEqual(ContactIntervalDetails, other.ContactIntervalDetails))
                return false;
            if (!AdvEqual(IsRelationshipChangeRequested, other.IsRelationshipChangeRequested))
                return false;
            if (!AdvEqual(RelationshipChangeRequestedDetails, other.RelationshipChangeRequestedDetails))
                return false;
            if (!AdvEqual(IsInterestedInRelationshipReferral, other.IsInterestedInRelationshipReferral))
                return false;
            if (!AdvEqual(InterestedInRelationshipReferralDetails, other.InterestedInRelationshipReferralDetails))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;
            //if (!AdvEqual(AgeCategory, other.AgeCategory))
            //    return false;
            if (!AdvEqual(NonCustodialChilds, other.NonCustodialChilds))
                return false;
            //if (!AdvEqual(ChildCareArrangement, other.ChildCareArrangement))
            //    return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
