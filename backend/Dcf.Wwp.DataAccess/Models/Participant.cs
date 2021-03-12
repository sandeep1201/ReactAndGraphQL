using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Participant : BaseEntity
    {
        #region Properties

        public decimal?  PinNumber                { get; set; }
        public string    FirstName                { get; set; }
        public string    MiddleInitial            { get; set; }
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
        public decimal?  MciId                    { get; set; }
        public string    TribalMemberIndicator    { get; set; }
        public bool?     TimeLimitStatus          { get; set; }
        public string    ConversionProjectDetails { get; set; }
        public DateTime? ConversionDate           { get; set; }
        public bool      IsDeleted                { get; set; }
        public DateTime? CreatedDate              { get; set; }
        public string    ModifiedBy               { get; set; }
        public DateTime? ModifiedDate             { get; set; }
        public bool?     HasBeenThroughClientReg  { get; set; }
        public DateTime? TotalLifetimeHoursDate   { get; set; }
        public bool?     Is60DaysVerified         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Auxiliary>                               Auxiliaries                              { get; set; } = new List<Auxiliary>();
        public virtual ICollection<CFParticipationEntry>                    CFParticipationEntries                   { get; set; } = new List<CFParticipationEntry>();
        public virtual ICollection<Contact>                                 Contacts                                 { get; set; }
        public virtual ICollection<EmployabilityPlan>                       EmployabilityPlans                       { get; set; } = new List<EmployabilityPlan>();
        public virtual ICollection<EmploymentInformation>                   EmploymentInformations                   { get; set; } = new List<EmploymentInformation>();
        public virtual ICollection<ParticipationStatus>                     ParticipationStatuses                    { get; set; } = new List<ParticipationStatus>();
        public virtual ICollection<ParticipantEnrolledProgram>              ParticipantEnrolledPrograms              { get; set; } = new List<ParticipantEnrolledProgram>();
        public virtual ICollection<ParticipationEntry>                      ParticipationEntries                     { get; set; } = new List<ParticipationEntry>();
        public virtual ICollection<ParticipationPeriodSummary>              ParParticipationPeriodSummaries          { get; set; } = new List<ParticipationPeriodSummary>();
        public virtual ICollection<PinComment>                              PinComments                              { get; set; } = new List<PinComment>();
        public virtual ICollection<EAAssets>                                EaAssetses                               { get; set; } = new List<EAAssets>();
        public virtual ICollection<EAVehicles>                              EaVehicleses                             { get; set; } = new List<EAVehicles>();
        public virtual ICollection<EAHouseHoldIncome>                       EaHouseHoldIncomes                       { get; set; } = new List<EAHouseHoldIncome>();
        public virtual ICollection<EARequestParticipantBridge>              EARequestParticipantBridges              { get; set; } = new List<EARequestParticipantBridge>();
        public virtual ICollection<OverPayment>                             OverPayments                             { get; set; } = new List<OverPayment>();
        public virtual ICollection<DrugScreening>                           DrugScreenings                           { get; set; } = new List<DrugScreening>();
        public virtual ICollection<ParticipantPlacement>                    ParticipantPlacements                    { get; set; } = new List<ParticipantPlacement>();
        public virtual ICollection<EAIPV>                                   EaIpvs                                   { get; set; } = new List<EAIPV>();
        public virtual ICollection<Plan>                                    Plans                                    { get; set; } = new List<Plan>();
        public virtual ICollection<ParticipantEnrolledProgramCutOverBridge> ParticipantEnrolledProgramCutOverBridges { get; set; } = new List<ParticipantEnrolledProgramCutOverBridge>();
        public virtual ICollection<POPClaim>                                POPClaims                                { get; set; } = new List<POPClaim>();
        public virtual ICollection<Transaction>                             Transactions                             { get; set; } = new List<Transaction>();
        public virtual ICollection<TimeLimit>                               TimeLimits                               { get; set; } = new List<TimeLimit>();
        public virtual ICollection<CareerAssessment>                        CareerAssessments                        { get; set; }
        public virtual ICollection<JobReadiness>                            JobReadinesses                           { get; set; }
        public virtual ICollection<EducationExam>                           EducationExams                           { get; set; }
        public virtual ICollection<WorkerTaskList>                          WorkerTaskLists                          { get; set; }
        public virtual ICollection<InformalAssessment>                      InformalAssessments                      { get; set; }

        // public virtual ICollection<ActionNeeded>                       ActionsNeeded                       { get; set; }
        // public virtual ICollection<ActionNeeded>                       ActionsNeeded                       { get; set; }
        // public virtual ICollection<Aka>                                Akas                                { get; set; }
        // public virtual ICollection<BarrierDetail>                      BarrierDetails                      { get; set; }
        // public virtual ICollection<BarrierSection>                     BarrierSections                     { get; set; }
        // public virtual ICollection<ChildYouthSection>                  ChildYouthSections                  { get; set; }
        // public virtual ICollection<ConfidentialPinInformation>         ConfidentialPinInformation          { get; set; }
        // public virtual ICollection<Contact>                            Contacts                            { get; set; }
        // public virtual ICollection<EducationSection>                   EducationSections                   { get; set; }
        // public virtual ICollection<ElevatedAccess>                     ElevatedAccesses                    { get; set; }
        // public virtual ICollection<EmploymentInformation>              EmploymentInformation               { get; set; }
        // public virtual ICollection<FamilyBarriersSection>              FamilyBarriersSections              { get; set; }
        // public virtual ICollection<HousingSection>                     HousingSections                     { get; set; }
        // public virtual ICollection<LanguageSection>                    LanguageSections                    { get; set; }
        // public virtual ICollection<LegalIssuesSection>                 LegalIssuesSections                 { get; set; }
        // public virtual ICollection<MilitaryTrainingSection>            MilitaryTrainingSections            { get; set; }
        // public virtual ICollection<NonCustodialParentsReferralSection> NonCustodialParentsReferralSections { get; set; }
        // public virtual ICollection<NonCustodialParentsSection>         NonCustodialParentsSections         { get; set; }
        // public virtual ICollection<OfficeTransfer>                     OfficeTransfers                     { get; set; }
        // public virtual ICollection<OtherDemographics>                  OtherDemographics                   { get; set; }
        // public virtual ICollection<ParticipantChildRelationshipBridge> ParticipantChildRelationshipBridges { get; set; }
        // public virtual ICollection<ParticipantContactInfo>             ParticipantContactInfo              { get; set; }
        // public virtual ICollection<PostSecondaryEducationSection>      PostSecondaryEducationSections      { get; set; }
        // public virtual ICollection<RecentParticipant>                  RecentParticipants                  { get; set; }
        // public virtual ICollection<RequestForAssistance>               RequestsForAssistance               { get; set; }
        // public virtual ICollection<TimeLimit>                          TimeLimits                          { get; set; }
        // public virtual ICollection<TimeLimitExtension>                 TimeLimitExtensions                 { get; set; }
        // public virtual ICollection<TimeLimitSummary>                   TimeLimitSummaries                  { get; set; }
        // public virtual ICollection<TransportationSection>              TransportationSections              { get; set; }
        // public virtual ICollection<WorkHistorySection>                 WorkHistorySections                 { get; set; }
        // public virtual ICollection<WorkProgramSection>                 WorkProgramSections                 { get; set; }
        // public virtual ICollection<WorkerParticipantBridge>            WorkerParticipantBridges            { get; set; }

        #endregion

        #region NotMappedProp

        [NotMapped]
        public string DisplayName =>
            ($"{FirstName.Trim()} " + $"{(string.IsNullOrWhiteSpace(MiddleInitial) ? string.Empty : $"{MiddleInitial.Trim()}. ")}" +
             $"{LastName.Trim()} "  + $"{(string.IsNullOrWhiteSpace(SuffixName) ? string.Empty : $"{SuffixName.Trim()}")}").Trim().ToUpper();

        [NotMapped]
        public IEnumerable<ParticipantEnrolledProgram> EnrolledParticipantEnrolledPrograms =>
            ParticipantEnrolledPrograms.Where(i => i.EnrolledProgramStatusCodeId == Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId)
                                       .ToList();

        #endregion
    }
}
