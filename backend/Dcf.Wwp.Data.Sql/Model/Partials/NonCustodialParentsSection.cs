using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    partial class NonCustodialParentsSection : BaseCommonModel, INonCustodialParentsSection, IEquatable<NonCustodialParentsSection>
    {
        ICollection<INonCustodialCaretaker> INonCustodialParentsSection.AllNonCustodialCaretakers
        {
            get => NonCustodialCaretakers.Cast<INonCustodialCaretaker>().ToList();
            set => NonCustodialCaretakers = value.Cast<NonCustodialCaretaker>().ToList();
        }

        ICollection<INonCustodialCaretaker> INonCustodialParentsSection.NonCustodialCaretakers
        {
            get => (from x in NonCustodialCaretakers where x.DeleteReasonId == null select x).Cast<INonCustodialCaretaker>().ToList();
            set => NonCustodialCaretakers = value.Cast<NonCustodialCaretaker>().ToList();
        }

        IParticipant INonCustodialParentsSection.Participant
        {
            get => Participant;
            set => Participant = (Participant) value;
        }

        IContact INonCustodialParentsSection.ChildSupportContact
        {
            get => ChildSupportContact;
            set => ChildSupportContact = (Contact) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialParentsSection
                        {
                            Id                             = Id,
                            ParticipantId                  = ParticipantId,
                            HasChildren                    = HasChildren,
                            ChildSupportPayment            = ChildSupportPayment,
                            HasOwedChildSupport            = HasOwedChildSupport,
                            HasInterestInChildServices     = HasInterestInChildServices,
                            IsInterestedInReferralServices = IsInterestedInReferralServices,
                            Notes                          = Notes,
                            IsDeleted                      = IsDeleted,
                            ChildSupportContactId          = ChildSupportContactId,
                            NonCustodialCaretakers         = NonCustodialCaretakers.Select(x => (NonCustodialCaretaker) x.Clone()).ToList()
                        };

            return clone;
        }

        #endregion ICloneable


        #region IComplexModel

        public void SetModifiedOnComplexProperties<T>(T cloned, string user, DateTime modDate)
            where T : class, ICloneable, ICommonModel
        {
            // We don't need to set modified on null objects.
            if (cloned == null) return;

            Debug.Assert(cloned is INonCustodialParentsSection, "cloned is not INonCustodialParentsSection");

            var clone = (INonCustodialParentsSection) cloned;

            if (AreBothNotNull(NonCustodialCaretakers, clone.NonCustodialCaretakers))
            {
                var first  = NonCustodialCaretakers.OrderBy(x => x.Id).ToList();
                var second = clone.NonCustodialCaretakers.OrderBy(x => x.Id).ToList();

                var i = 0;
                foreach (var ncc1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (ncc1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (i < second.Count)
                        {
                            var ncc2 = second[i];

                            if (!ncc1.Equals(ncc2))
                            {
                                ncc1.ModifiedBy   = user;
                                ncc1.ModifiedDate = modDate;
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            ncc1.ModifiedBy   = user;
                            ncc1.ModifiedDate = modDate;
                        }
                    }

                    i++;
                }
            }
        }

        #endregion IComplexModel

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonCustodialParentsSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialParentsSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(HasChildren, other.HasChildren))
                return false;
            if (!AdvEqual(ChildSupportPayment, other.ChildSupportPayment))
                return false;
            if (!AdvEqual(HasOwedChildSupport, other.HasOwedChildSupport))
                return false;
            if (!AdvEqual(HasInterestInChildServices, other.HasInterestInChildServices))
                return false;
            if (!AdvEqual(IsInterestedInReferralServices, other.IsInterestedInReferralServices))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(ChildSupportContactId, other.ChildSupportContactId))
                return false;

            if (AreBothNotNull(NonCustodialCaretakers, other.NonCustodialCaretakers) && !NonCustodialCaretakers.OrderBy(x => x.Id).SequenceEqual(other.NonCustodialCaretakers.OrderBy(x => x.Id)))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
