using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class Contact : BaseCommonModel, IContact
    {
        IContactTitleType IContact.ContactTitleType
        {
            get { return ContactTitleType; }
            set { ContactTitleType = (ContactTitleType) value; }
        }


        ICollection<IInvolvedWorkProgram> IContact.InvolvedWorkPrograms
        {
            get { return InvolvedWorkPrograms.Cast<IInvolvedWorkProgram>().ToList(); }

            set { InvolvedWorkPrograms = value.Cast<InvolvedWorkProgram>().ToList(); }
        }


        ICollection<ILegalIssuesSection> IContact.LegalIssuesSections
        {
            get { return LegalIssuesSections.Cast<ILegalIssuesSection>().ToList(); }

            set { LegalIssuesSections = value.Cast<LegalIssuesSection>().ToList(); }
        }

        ICollection<IEmploymentInformation> IContact.EmploymentInformations
        {
            get { return EmploymentInformations.Cast<IEmploymentInformation>().ToList(); }
            set { EmploymentInformations = value.Cast<EmploymentInformation>().ToList(); }
        }

        IParticipant IContact.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        ICollection<IBarrierDetailContactBridge> IContact.BarrierDetailContactBridges
        {
            get { return BarrierDetailContactBridges.Cast<IBarrierDetailContactBridge>().ToList(); }
            set { BarrierDetailContactBridges = value.Cast<BarrierDetailContactBridge>().ToList(); }
        }

        ICollection<IActivityContactBridge> IContact.ActivityContactBridges
        {
            get { return ActivityContactBridges.Cast<IActivityContactBridge>().ToList(); }
            set { ActivityContactBridges = value.Cast<ActivityContactBridge>().ToList(); }
        }

        ICollection<INonCustodialParentsSection> IContact.NonCustodialParentsSections
        {
            get { return NonCustodialParentsSections.Cast<INonCustodialParentsSection>().ToList(); }
            set { NonCustodialParentsSections = value.Cast<NonCustodialParentsSection>().ToList(); }
        }


        #region ICloneable

        public object Clone()
        {
            var c = new Contact();

            c.Id                     = this.Id;
            c.ParticipantId          = this.ParticipantId;
            c.TitleId                = this.TitleId;
            c.CustomTitle            = this.CustomTitle;
            c.Name                   = this.Name;
            c.Email                  = this.Email;
            c.Phone                  = this.Phone;
            c.ExtensionNo            = this.ExtensionNo;
            c.FaxNo                  = this.FaxNo;
            c.Address                = this.Address;
            c.Notes                  = this.Notes;
            c.ReleaseInformationDate = this.ReleaseInformationDate;
            c.IsDeleted              = this.IsDeleted;
            c.ContactTitleType       = (ContactTitleType) this.ContactTitleType?.Clone();

            return c;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Contact;
            return obj != null && Equals(obj);
        }

        public bool Equals(Contact other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;


            //Check whether the products' properties are equal.


            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(TitleId, other.TitleId))
                return false;
            if (!AdvEqual(CustomTitle, other.CustomTitle))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(Email, other.Email))
                return false;
            if (!AdvEqual(Phone, other.Phone))
                return false;
            if (!AdvEqual(ExtensionNo, other.ExtensionNo))
                return false;
            if (!AdvEqual(FaxNo, other.FaxNo))
                return false;
            if (!AdvEqual(Address, other.Address))
                return false;
            if (!AdvEqual(Notes, other.Email))
                return false;
            if (!AdvEqual(IsDeleted, other.Phone))
                return false;
            if (!AdvEqual(ReleaseInformationDate, other.ReleaseInformationDate))
                return false;
            if (!AdvEqual(ContactTitleType, other.ContactTitleType))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
