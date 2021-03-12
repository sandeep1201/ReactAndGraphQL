using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    partial class NonCustodialChild : BaseEntity, INonCustodialChild, IEquatable<NonCustodialChild>
    {
        IContactInterval INonCustodialChild.ContactInterval
        {
            get { return ContactInterval; }
            set { ContactInterval = (ContactInterval) value; }
        }

        INonCustodialCaretaker INonCustodialChild.NonCustodialCaretaker
        {
            get { return NonCustodialCaretaker; }
            set { NonCustodialCaretaker = (NonCustodialCaretaker) value; }
        }

        IYesNoUnknownLookup INonCustodialChild.HasOtherAdultsYesNoUnknownLookup
        {
            get { return HasOtherAdultsYesNoUnknownLookup; }
            set { HasOtherAdultsYesNoUnknownLookup = (YesNoUnknownLookup) value; }
        }

        IYesNoUnknownLookup INonCustodialChild.IsNeedOfServicesYesNoUnknownLookup
        {
            get { return IsNeedOfServicesYesNoUnknownLookup; }
            set { IsNeedOfServicesYesNoUnknownLookup = (YesNoUnknownLookup) value; }
        }

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }

            set { DeleteReason = (DeleteReason) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialChild()
                        {
                            Id                                   = this.Id,
                            FirstName                            = this.FirstName,
                            LastName                             = this.LastName,
                            DateOfBirth                          = this.DateOfBirth,
                            HasChildSupportOrder                 = this.HasChildSupportOrder,
                            HasNameOnChildBirthRecord            = this.HasNameOnChildBirthRecord,
                            ChildSupportOrderDetails             = this.ChildSupportOrderDetails,
                            ContactIntervalId                    = this.ContactIntervalId,
                            ContactIntervalDetails               = this.ContactIntervalDetails,
                            HasOtherAdultsYesNoUnknownLookupId   = this.HasOtherAdultsYesNoUnknownLookupId,
                            OtherAdultsDetails                   = this.OtherAdultsDetails,
                            IsRelationshipChangeRequested        = this.IsRelationshipChangeRequested,
                            IsNeedOfServicesYesNoUnknownLookupId = this.IsNeedOfServicesYesNoUnknownLookupId,
                            NeedOfServicesDetails                = this.NeedOfServicesDetails,
                            DeleteReasonId                       = this.DeleteReasonId,

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

            var obj = other as NonCustodialChild;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialChild other)
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

            if (!AdvEqual(DateOfBirth, other.DateOfBirth))
                return false;
            if (!AdvEqual(HasChildSupportOrder, other.HasChildSupportOrder))
                return false;

            if (!AdvEqual(ChildSupportOrderDetails, other.ChildSupportOrderDetails))
                return false;

            if (!AdvEqual(HasNameOnChildBirthRecord, other.HasNameOnChildBirthRecord))
                return false;

            if (!AdvEqual(ContactIntervalId, other.ContactIntervalId))
                return false;

            if (!AdvEqual(ContactIntervalDetails, other.ContactIntervalDetails))
                return false;

            if (!AdvEqual(HasOtherAdultsYesNoUnknownLookupId, other.HasOtherAdultsYesNoUnknownLookupId))
                return false;

            if (!AdvEqual(OtherAdultsDetails, other.OtherAdultsDetails))
                return false;

            if (!AdvEqual(IsRelationshipChangeRequested, other.IsRelationshipChangeRequested))
                return false;
            if (!AdvEqual(RelationshipChangeRequestedDetails, other.RelationshipChangeRequestedDetails))
                return false;
            if (!AdvEqual(IsNeedOfServicesYesNoUnknownLookupId, other.IsNeedOfServicesYesNoUnknownLookupId))
                return false;
            if (!AdvEqual(NeedOfServicesDetails, other.NeedOfServicesDetails))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;
            //if (!AdvEqual(AgeCategory, other.AgeCategory))
            //    return false;
            //if (!AdvEqual(Child, other.Child))
            //    return false;
            //if (!AdvEqual(ChildCareArrangement, other.ChildCareArrangement))
            //    return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
