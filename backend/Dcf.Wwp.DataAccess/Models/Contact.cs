using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Contact : BaseEntity
    {
        #region Properties

        public int?      ParticipantId          { get; set; }
        public int?      TitleId                { get; set; }
        public string    CustomTitle            { get; set; }
        public string    Name                   { get; set; }
        public string    Email                  { get; set; }
        public string    Phone                  { get; set; }
        public string    ExtensionNo            { get; set; }
        public string    FaxNo                  { get; set; }
        public DateTime? ReleaseInformationDate { get; set; }
        public string    Address                { get; set; }
        public int?      LegalIssuesSectionId   { get; set; }
        public string    Notes                  { get; set; }
        public bool?     IsDeleted              { get; set; }
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                              Participant                  { get; set; }
        public virtual ContactTitleType                         Title                        { get; set; }
        //public virtual ICollection<BarrierDetailContactBridge>  BarrierDetailContactBridges  { get; set; }
        //public virtual ICollection<ChildYouthSection>           ChildYouthSections           { get; set; }
        //public virtual ICollection<EmployerOfRecordInformation> EmployersOfRecordInformation { get; set; }
        //public virtual ICollection<EmploymentInformation>       EmploymentInfo               { get; set; }
        //public virtual ICollection<FamilyBarriersSection>       FamilyBarriersSections       { get; set; }
        //public virtual ICollection<InvolvedWorkProgram>         InvolvedWorkPrograms         { get; set; }
        //public virtual ICollection<LegalIssuesSection>          LegalIssuesSections          { get; set; }
        //public virtual ICollection<NonCustodialReferralParent>  NonCustodialReferralParents  { get; set; }
        public virtual ICollection<ActivityContactBridge> ActivityContactBridges { get; set; } = new List<ActivityContactBridge>();

        #endregion
    }
}
