using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Contact
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
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ChildYouthSection>           ChildYouthSections           { get; set; }
        public virtual Participant                              Participant                  { get; set; }
        public virtual ICollection<LegalIssuesSection>          LegalIssuesSections          { get; set; }
        public virtual ICollection<NonCustodialReferralParent>  NonCustodialReferralParents  { get; set; }
        public virtual ICollection<BarrierDetailContactBridge>  BarrierDetailContactBridges  { get; set; }
        public virtual ICollection<EmploymentInformation>       EmploymentInformations       { get; set; }
        public virtual ICollection<InvolvedWorkProgram>         InvolvedWorkPrograms         { get; set; }
        public virtual ContactTitleType                         ContactTitleType             { get; set; }
        public virtual ICollection<FamilyBarriersSection>       FamilyBarriersSections       { get; set; }
        public virtual ICollection<EmployerOfRecordInformation> EmployerOfRecordInformations { get; set; }
        public virtual ICollection<ActivityContactBridge>       ActivityContactBridges       { get; set; }
        public virtual ICollection<NonCustodialParentsSection>  NonCustodialParentsSections  { get; set; }

        #endregion
    }
}
