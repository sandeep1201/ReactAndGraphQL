using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Participant
    {
        #region Properties

        public decimal?  PinNumber                { get; set; }
        public string    FirstName                { get; set; }
        public string    MiddleInitialName        { get; set; } // should have been simply 'MiddleInitialName'
        public string    LastName                 { get; set; }
        public string    SuffixName               { get; set; }
        public DateTime? DateOfBirth              { get; set; }
        public DateTime? DateOfDeath              { get; set; }
        public string    GenderIndicator          { get; set; }
        public string    AliasResponse            { get; set; }
        public string    LanguageCode             { get; set; }
        public short?    MaxHistorySequenceNumber { get; set; }
        public string    RaceCode                 { get; set; }
        public string    USCitizenSwitch          { get; set; }
        public string    AmericanIndianIndicator  { get; set; }
        public string    AsianIndicator           { get; set; }
        public string    BlackIndicator           { get; set; }
        public string    HispanicIndicator        { get; set; }
        public string    PacificIslanderIndicator { get; set; }
        public string    WhiteIndicator           { get; set; }
        public decimal?  MCI_ID                   { get; set; }
        public string    TribalMemberIndicator    { get; set; }
        public bool?     TimeLimitStatus          { get; set; }
        public bool?     HasBeenThroughClientReg  { get; set; }
        public string    ConversionProjectDetails { get; set; }
        public DateTime? ConversionDate           { get; set; }
        public DateTime? TotalLifetimeHoursDate   { get; set; }
        public bool      IsDeleted                { get; set; }
        public DateTime? CreatedDate              { get; set; }
        public string    ModifiedBy               { get; set; }
        public DateTime? ModifiedDate             { get; set; }
        public bool?     Is60DaysVerified         { get; set; }

        #endregion

        #region Navigation Properties

        //TODO: try to keep the number of NavProps to just what is required.
        // The following comment on applies if you use the smaller repos, not the monster Legacgy.IRepository.cs
        // EXAMPLE: if you're workign with Housing Sections, just using the HousingSection repo directly...
        // you don't need to get all convoluted by loading up a Participant AND THEN drilling down.
        //
        // You probably don't need to load everything - just inject the appropriate 
        // repos to get to whatever additional info you need in a given process/domain/function...
        // It'll also radically speed-up the model.

        public virtual ICollection<ActionNeeded>                            ActionsNeeded                            { get; set; }
        public virtual ICollection<AKA>                                     AKAs                                     { get; set; }
        public virtual ICollection<BarrierDetail>                           BarrierDetails                           { get; set; }
        public virtual ICollection<BarrierSection>                          BarrierSections                          { get; set; }
        public virtual ICollection<ChildYouthSection>                       ChildYouthSections                       { get; set; }
        public virtual ICollection<ConfidentialPinInformation>              ConfidentialPinInformations              { get; set; }
        public virtual ICollection<Contact>                                 Contacts                                 { get; set; }
        public virtual ICollection<EducationExam>                           EducationExams                           { get; set; }
        public virtual ICollection<EducationSection>                        EducationSections                        { get; set; }
        public virtual ICollection<EmploymentInformation>                   EmploymentInformations                   { get; set; }
        public virtual ICollection<EmployabilityPlan>                       EmployabilityPlans                       { get; set; }
        public virtual ICollection<FamilyBarriersSection>                   FamilyBarriersSections                   { get; set; }
        public virtual ICollection<HousingSection>                          HousingSections                          { get; set; }
        public virtual ICollection<InformalAssessment>                      InformalAssessments                      { get; set; }
        public virtual ICollection<LanguageSection>                         LanguageSections                         { get; set; }
        public virtual ICollection<LegalIssuesSection>                      LegalIssuesSections                      { get; set; }
        public virtual ICollection<MilitaryTrainingSection>                 MilitaryTrainingSections                 { get; set; }
        public virtual ICollection<NonCustodialParentsReferralSection>      NonCustodialParentsReferralSections      { get; set; }
        public virtual ICollection<NonCustodialParentsSection>              NonCustodialParentsSections              { get; set; }
        public virtual ICollection<OfficeTransfer>                          OfficeTransfers                          { get; set; }
        public virtual ICollection<OtherDemographic>                        OtherDemographics                        { get; set; }
        public virtual ICollection<ParticipantChildRelationshipBridge>      ParticipantChildRelationshipBridges      { get; set; }
        public virtual ICollection<ParticipantContactInfo>                  ParticipantContactInfoes                 { get; set; }
        public virtual ICollection<ParticipantEnrolledProgram>              ParticipantEnrolledPrograms              { get; set; }
        public virtual ICollection<PostSecondaryEducationSection>           PostSecondaryEducationSections           { get; set; }
        public virtual ICollection<RecentParticipant>                       RecentParticipants                       { get; set; }
        public virtual ICollection<RequestForAssistance>                    RequestsForAssistance                    { get; set; }
        public virtual ICollection<TimeLimit>                               TimeLimits                               { get; set; }
        public virtual ICollection<TimeLimitExtension>                      TimeLimitExtensions                      { get; set; }
        public virtual ICollection<TimeLimitSummary>                        TimeLimitSummaries                       { get; set; }
        public virtual ICollection<TransportationSection>                   TransportationSections                   { get; set; }
        public virtual ICollection<WorkHistorySection>                      WorkHistorySections                      { get; set; }
        public virtual ICollection<WorkProgramSection>                      WorkProgramSections                      { get; set; }
        public virtual ICollection<WorkerParticipantBridge>                 WorkerParticipantBridges                 { get; set; }
        public virtual ICollection<ParticipationStatu>                      ParticipationStatus                      { get; set; }
        public virtual ICollection<EARequestParticipantBridge>              EaRequestParticipantBridges              { get; set; }
        public virtual ICollection<ParticipantEnrolledProgramCutOverBridge> ParticipantEnrolledProgramCutOverBridges { get; set; }
        public virtual ICollection<Transaction>                             Transactions                             { get; set; }
        public virtual ICollection<WorkerTaskList>                          WorkerTaskLists                          { get; set; }

        #endregion
    }
}
