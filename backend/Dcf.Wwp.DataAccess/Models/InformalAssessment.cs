using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class InformalAssessment : BaseEntity
    {
        #region Properties

        public int?      AssessmentTypeId                               { get; set; }
        public int       ParticipantId                                  { get; set; }
        public int?      LanguageAssessmentSectionId                    { get; set; }
        public int?      WorkHistoryAssessmentSectionId                 { get; set; }
        public int?      WorkProgramAssessmentSectionId                 { get; set; }
        public int?      PostSecondaryEducationAssessmentSectionId      { get; set; }
        public int?      MilitaryTrainingAssessmentSectionId            { get; set; }
        public int?      HousingAssessmentSectionId                     { get; set; }
        public int?      TransportationAssessmentSectionId              { get; set; }
        public int?      LegalIssuesAssessmentSectionId                 { get; set; }
        public int?      BarriersAssessmentSectionId                    { get; set; }
        public int?      ChildYouthSupportsAssessmentSectionId          { get; set; }
        public int?      FamilyBarriersAssessmentSectionId              { get; set; }
        public int?      NonCustodialParentsAssessmentSectionId         { get; set; }
        public int?      NonCustodialParentsReferralAssessmentSectionId { get; set; }
        public int?      WorkHistorySectionId                           { get; set; }
        public int?      EducationAssessmentSectionId                   { get; set; }
        public DateTime? EndDate                                        { get; set; }
        public DateTime  CreatedDate                                    { get; set; }
        public bool      IsDeleted                                      { get; set; }
        public string    ModifiedBy                                     { get; set; }
        public DateTime? ModifiedDate                                   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion
    }
}
