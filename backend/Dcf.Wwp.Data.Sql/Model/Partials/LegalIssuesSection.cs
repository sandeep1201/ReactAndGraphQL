using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class LegalIssuesSection : BaseCommonModel, ILegalIssuesSection, IEquatable<LegalIssuesSection>
    {
        ICollection<IConviction> ILegalIssuesSection.Convictions
        {
            get { return Convictions.Where(i => i.DeleteReasonId == null).Cast<IConviction>().ToList(); }

            set { Convictions = value.Cast<Conviction>().ToList(); }
        }

        ICollection<IConviction> ILegalIssuesSection.AllConvictions
        {
            get { return Convictions.Cast<IConviction>().ToList(); }

            set { Convictions = value.Cast<Conviction>().ToList(); }
        }

        ICollection<ICourtDate> ILegalIssuesSection.CourtDates
        {
            get { return (from x in CourtDates where x.IsDeleted == false select x).Cast<ICourtDate>().ToList(); }

            set { CourtDates = value.Cast<CourtDate>().ToList(); }
        }

        ICollection<ICourtDate> ILegalIssuesSection.AllCourtDates
        {
            get { return CourtDates.Cast<ICourtDate>().ToList(); }

            set { CourtDates = value.Cast<CourtDate>().ToList(); }
        }

        ICollection<IPendingCharge> ILegalIssuesSection.PendingCharges
        {
            get { return (from x in PendingCharges where x.IsDeleted == false select x).Cast<IPendingCharge>().ToList(); }

            set { PendingCharges = value.Cast<PendingCharge>().ToList(); }
        }

        ICollection<IPendingCharge> ILegalIssuesSection.AllPendingCharges
        {
            get { return PendingCharges.Cast<IPendingCharge>().ToList(); }

            set { PendingCharges = value.Cast<PendingCharge>().ToList(); }
        }

        IContact ILegalIssuesSection.Contact
        {
            get { return Contact; }
            set { Contact = (Contact) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var lis = new LegalIssuesSection();

            lis.Id                             = this.Id;
            lis.IsConvictedOfCrime             = this.IsConvictedOfCrime;
            lis.IsUnderCommunitySupervision    = this.IsUnderCommunitySupervision;
            lis.CommunitySupervisonDetails     = this.CommunitySupervisonDetails;
            lis.CommunitySupervisonContactId   = this.CommunitySupervisonContactId;
            lis.HasPendingCharges              = this.HasPendingCharges;
            lis.HasFamilyLegalIssues           = this.HasFamilyLegalIssues;
            lis.FamilyLegalIssueNotes          = this.FamilyLegalIssueNotes;
            lis.OrderedToPayChildSupport       = this.OrderedToPayChildSupport;
            lis.OweAnyChildSupportBack         = this.OweAnyChildSupportBack;
            lis.MonthlyAmount                  = this.MonthlyAmount;
            lis.IsUnknown                      = this.IsUnknown;
            lis.ChildSupportDetails            = this.ChildSupportDetails;
            lis.HasCourtDates                  = this.HasCourtDates;
            lis.ActionNeededDetails            = this.ActionNeededDetails;
            lis.Notes                          = this.Notes;
            lis.HasRestrainingOrders           = this.HasRestrainingOrders;
            lis.RestrainingOrderNotes          = this.RestrainingOrderNotes;
            lis.HasRestrainingOrderToPrevent   = this.HasRestrainingOrderToPrevent;
            lis.RestrainingOrderToPreventNotes = this.RestrainingOrderToPreventNotes;
            lis.Convictions                    = this.Convictions.Select(x => (Conviction) x.Clone()).ToList();
            lis.PendingCharges                 = this.PendingCharges.Select(y => (PendingCharge) y.Clone()).ToList();
            lis.CourtDates                     = this.CourtDates.Select(z => (CourtDate) z.Clone()).ToList();

            return lis;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as LegalIssuesSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(LegalIssuesSection other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(IsConvictedOfCrime, other.IsConvictedOfCrime))
                return false;
            if (!AdvEqual(IsUnderCommunitySupervision, other.IsUnderCommunitySupervision))
                return false;
            if (!AdvEqual(CommunitySupervisonDetails, other.CommunitySupervisonDetails))
                return false;
            if (!AdvEqual(CommunitySupervisonContactId, other.CommunitySupervisonContactId))
                return false;
            if (!AdvEqual(HasPendingCharges, other.HasPendingCharges))
                return false;
            if (!AdvEqual(HasFamilyLegalIssues, other.HasFamilyLegalIssues))
                return false;
            if (!AdvEqual(FamilyLegalIssueNotes, other.FamilyLegalIssueNotes))
                return false;
            if (!AdvEqual(HasCourtDates, other.HasCourtDates))
                return false;
            if (!AdvEqual(OrderedToPayChildSupport, other.OrderedToPayChildSupport))
                return false;
            if (!AdvEqual(OweAnyChildSupportBack, other.OweAnyChildSupportBack))
                return false;
            if (!AdvEqual(MonthlyAmount, other.MonthlyAmount))
                return false;
            if (!AdvEqual(IsUnknown, other.IsUnknown))
                return false;
            if (!AdvEqual(ChildSupportDetails, other.ChildSupportDetails))
                return false;
            if (!AdvEqual(ActionNeededDetails, other.ActionNeededDetails))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(HasRestrainingOrders, other.HasRestrainingOrders))
                return false;
            if (!AdvEqual(RestrainingOrderNotes, other.RestrainingOrderNotes))
                return false;
            if (!AdvEqual(HasRestrainingOrderToPrevent, other.HasRestrainingOrderToPrevent))
                return false;
            if (!AdvEqual(RestrainingOrderToPreventNotes, other.RestrainingOrderToPreventNotes))
                return false;
            if (AreBothNotNull(Convictions, other.Convictions) && !Convictions.OrderBy(x => x.Id).SequenceEqual(other.Convictions.OrderBy(x => x.Id)))
                return false; // this checks if the Conviction lists are the same
            if (AreBothNotNull(PendingCharges, other.PendingCharges) && !PendingCharges.OrderBy(y => y.Id).SequenceEqual(other.PendingCharges.OrderBy(y => y.Id)))
                return false;
            // this checks if the Pendingcharges lists are the same
            if (AreBothNotNull(CourtDates, other.CourtDates) && !CourtDates.OrderBy(z => z.Id).SequenceEqual(other.CourtDates.OrderBy(z => z.Id)))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
