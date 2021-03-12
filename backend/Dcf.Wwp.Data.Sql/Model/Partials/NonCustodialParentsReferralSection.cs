using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Model.Interface;


// ReSharper disable once CheckNamespace
namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialParentsReferralSection : BaseCommonModel, INonCustodialParentsReferralSection, IEquatable<NonCustodialParentsReferralSection>
    {
        ICollection<INonCustodialReferralParent> INonCustodialParentsReferralSection.AllNonCustodialReferralParents
        {
            get { return NonCustodialReferralParents.Cast<INonCustodialReferralParent>().ToList(); }
            set { NonCustodialReferralParents = value.Cast<NonCustodialReferralParent>().ToList(); }
        }

        ICollection<INonCustodialReferralParent> INonCustodialParentsReferralSection.NonCustodialReferralParents
        {
            get { return (from x in NonCustodialReferralParents where x.DeleteReasonId == null select x).Cast<INonCustodialReferralParent>().ToList(); }
            set { NonCustodialReferralParents = value.Cast<NonCustodialReferralParent>().ToList(); }
        }

        IYesNoSkipLookup INonCustodialParentsReferralSection.YesNoSkipLookup
        {
            get { return YesNoSkipLookup; }
            set { YesNoSkipLookup = (YesNoSkipLookup) value; }
        }


        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialParentsReferralSection()
                        {
                            Id            = Id,
                            ParticipantId = ParticipantId,
                            HasChildrenId = HasChildrenId,
                            Notes         = Notes,
                            IsDeleted     = IsDeleted,

                            NonCustodialReferralParents = NonCustodialReferralParents.Select(x => (NonCustodialReferralParent) x.Clone()).ToList()
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonCustodialParentsReferralSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialParentsReferralSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(HasChildrenId, other.HasChildrenId))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            if (AreBothNotNull(NonCustodialReferralParents, other.NonCustodialReferralParents) && !(NonCustodialReferralParents.OrderBy(x => x.Id).SequenceEqual(other.NonCustodialReferralParents.OrderBy(x => x.Id))))
                return false;

            return true;
        }

        #endregion IEquatable<T>

        #region IComplexModel

        public void SetModifiedOnComplexProperties<T>(T cloned, string user, DateTime modDate)
            where T : class, ICloneable, ICommonModel
        {
            // We don't need to set modified on null objects.
            if (cloned == null) return;

            Debug.Assert(cloned is INonCustodialParentsReferralSection, "cloned is not INonCustodialParentsReferralSection");

            var clone = (INonCustodialParentsReferralSection) cloned;

            if (AreBothNotNull(NonCustodialReferralParents, clone.NonCustodialReferralParents))
            {
                var first  = NonCustodialReferralParents.OrderBy(x => x.Id).ToList();
                var second = clone.NonCustodialReferralParents.OrderBy(x => x.Id).ToList();

                int i = 0;
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
    }
}
