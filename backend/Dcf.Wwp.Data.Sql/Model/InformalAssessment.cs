using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class InformalAssessment
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

        public virtual AssessmentType                               AssessmentType                               { get; set; }
        public virtual Participant                                  Participant                                  { get; set; }
        public virtual LanguageAssessmentSection                    LanguageAssessmentSection                    { get; set; }
        public virtual WorkHistoryAssessmentSection                 WorkHistoryAssessmentSection                 { get; set; }
        public virtual WorkProgramAssessmentSection                 WorkProgramAssessmentSection                 { get; set; }
        public virtual PostSecondaryEducationAssessmentSection      PostSecondaryEducationAssessmentSection      { get; set; }
        public virtual MilitaryTrainingAssessmentSection            MilitaryTrainingAssessmentSection            { get; set; }
        public virtual HousingAssessmentSection                     HousingAssessmentSection                     { get; set; }
        public virtual TransportationAssessmentSection              TransportationAssessmentSection              { get; set; }
        public virtual LegalIssuesAssessmentSection                 LegalIssuesAssessmentSection                 { get; set; }
        public virtual BarrierAssessmentSection                     BarrierAssessmentSection                     { get; set; }
        public virtual ChildYouthSupportsAssessmentSection          ChildYouthSupportsAssessmentSection          { get; set; }
        public virtual FamilyBarriersAssessmentSection              FamilyBarriersAssessmentSection              { get; set; }
        public virtual NonCustodialParentsAssessmentSection         NonCustodialParentsAssessmentSection         { get; set; }
        public virtual NonCustodialParentsReferralAssessmentSection NonCustodialParentsReferralAssessmentSection { get; set; }
        public virtual EducationAssessmentSection                   EducationAssessmentSection                   { get; set; }

        #endregion
    }
}
