using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WwpEntities : DbContext
    {
        public WwpEntities()
            : base("name=WwpEntities")
        {
        }

        public virtual DbSet<Absence>                                          Absences                                          { get; set; }
        public virtual DbSet<AbsenceReason>                                    AbsenceReasons                                    { get; set; }
        public virtual DbSet<Accommodation>                                    Accommodations                                    { get; set; }
        public virtual DbSet<SP_CaseAddressReturnType>                         SP_CaseAddressReturnType                          { get; set; }
        public virtual DbSet<SP_DeleteContactReturnType>                       SP_DeleteContactReturnType                        { get; set; }
        public virtual DbSet<SP_FSETStatusReturnType>                          SP_FSETStatusReturnType                           { get; set; }
        public virtual DbSet<SP_LearnfareStatusReturnType>                     SP_LearnfareStatusReturnType                      { get; set; }
        public virtual DbSet<SP_RentPaidReturnType>                            SP_RentPaidReturnType                             { get; set; }
        public virtual DbSet<SP_SocialSecurityStatusReturnType>                SP_SocialSecurityStatusReturnType                 { get; set; }
        public virtual DbSet<SP_SubsidizedHousingReturnType>                   SP_SubsidizedHousingReturnType                    { get; set; }
        public virtual DbSet<KnownLanguage>                                    KnownLanguages                                    { get; set; }
        public virtual DbSet<Language>                                         Languages                                         { get; set; }
        public virtual DbSet<City>                                             Cities                                            { get; set; }
        public virtual DbSet<Country>                                          Countries                                         { get; set; }
        public virtual DbSet<State>                                            States                                            { get; set; }
        public virtual DbSet<AgeCategory>                                      AgeCategories                                     { get; set; }
        public virtual DbSet<Child>                                            Children                                          { get; set; }
        public virtual DbSet<ChildYouthSection>                                ChildYouthSections                                { get; set; }
        public virtual DbSet<ChildYouthSectionChild>                           ChildYouthSectionChilds                           { get; set; }
        public virtual DbSet<ParticipantChildRelationshipBridge>               ParticipantChildRelationshipBridges               { get; set; }
        public virtual DbSet<Relationship>                                     Relationships                                     { get; set; }
        public virtual DbSet<AssessmentType>                                   AssessmentTypes                                   { get; set; }
        public virtual DbSet<Contact>                                          Contacts                                          { get; set; }
        public virtual DbSet<InformalAssessment>                               InformalAssessments                               { get; set; }
        public virtual DbSet<Participant>                                      Participants                                      { get; set; }
        public virtual DbSet<CertificateIssuingAuthority>                      CertificateIssuingAuthorities                     { get; set; }
        public virtual DbSet<EducationSection>                                 EducationSections                                 { get; set; }
        public virtual DbSet<SchoolCollegeEstablishment>                       SchoolCollegeEstablishments                       { get; set; }
        public virtual DbSet<SchoolGradeLevel>                                 SchoolGradeLevels                                 { get; set; }
        public virtual DbSet<SchoolGraduationStatus>                           SchoolGraduationStatuses                          { get; set; }
        public virtual DbSet<ApplicationStatusType>                            ApplicationStatusTypes                            { get; set; }
        public virtual DbSet<FamilyBarriersAssessmentSection>                  FamilyBarriersAssessmentSections                  { get; set; }
        public virtual DbSet<FamilyBarriersDetail>                             FamilyBarriersDetails                             { get; set; }
        public virtual DbSet<FamilyMember>                                     FamilyMembers                                     { get; set; }
        public virtual DbSet<ConvictionType>                                   ConvictionTypes                                   { get; set; }
        public virtual DbSet<CourtDate>                                        CourtDates                                        { get; set; }
        public virtual DbSet<LegalIssuesSection>                               LegalIssuesSections                               { get; set; }
        public virtual DbSet<PendingCharge>                                    PendingCharges                                    { get; set; }
        public virtual DbSet<MilitaryBranch>                                   MilitaryBranches                                  { get; set; }
        public virtual DbSet<MilitaryDischargeType>                            MilitaryDischargeTypes                            { get; set; }
        public virtual DbSet<MilitaryRank>                                     MilitaryRanks                                     { get; set; }
        public virtual DbSet<MilitaryTrainingSection>                          MilitaryTrainingSections                          { get; set; }
        public virtual DbSet<ContactInterval>                                  ContactIntervals                                  { get; set; }
        public virtual DbSet<NonCustodialCaretaker>                            NonCustodialCaretakers                            { get; set; }
        public virtual DbSet<NonCustodialChild>                                NonCustodialChilds                                { get; set; }
        public virtual DbSet<NonCustodialParentRelationship>                   NonCustodialParentRelationships                   { get; set; }
        public virtual DbSet<NonCustodialParentsAssessmentSection>             NonCustodialParentsAssessmentSections             { get; set; }
        public virtual DbSet<NonCustodialParentsSection>                       NonCustodialParentsSections                       { get; set; }
        public virtual DbSet<PolarLookup>                                      PolarLookups                                      { get; set; }
        public virtual DbSet<NonCustodialParentsReferralSection>               NonCustodialParentsReferralSections               { get; set; }
        public virtual DbSet<NonCustodialReferralChild>                        NonCustodialReferralChilds                        { get; set; }
        public virtual DbSet<NonCustodialReferralParent>                       NonCustodialReferralParents                       { get; set; }
        public virtual DbSet<ReferralContactInterval>                          ReferralContactIntervals                          { get; set; }
        public virtual DbSet<YesNoSkipLookup>                                  YesNoSkipLookups                                  { get; set; }
        public virtual DbSet<YesNoUnknownLookup>                               YesNoUnknownLookups                               { get; set; }
        public virtual DbSet<BarrierAccommodation>                             BarrierAccommodations                             { get; set; }
        public virtual DbSet<BarrierAssessmentSection>                         BarrierAssessmentSections                         { get; set; }
        public virtual DbSet<BarrierDetail>                                    BarrierDetails                                    { get; set; }
        public virtual DbSet<BarrierDetailContactBridge>                       BarrierDetailContactBridges                       { get; set; }
        public virtual DbSet<BarrierSection>                                   BarrierSections                                   { get; set; }
        public virtual DbSet<BarrierSubtype>                                   BarrierSubtypes                                   { get; set; }
        public virtual DbSet<BarrierType>                                      BarrierTypes                                      { get; set; }
        public virtual DbSet<BarrierTypeBarrierSubTypeBridge>                  BarrierTypeBarrierSubTypeBridges                  { get; set; }
        public virtual DbSet<FormalAssessment>                                 FormalAssessments                                 { get; set; }
        public virtual DbSet<Symptom>                                          Symptoms                                          { get; set; }
        public virtual DbSet<DegreeType>                                       DegreeTypes                                       { get; set; }
        public virtual DbSet<LicenseType>                                      LicenseTypes                                      { get; set; }
        public virtual DbSet<PostSecondaryCollege>                             PostSecondaryColleges                             { get; set; }
        public virtual DbSet<PostSecondaryDegree>                              PostSecondaryDegrees                              { get; set; }
        public virtual DbSet<PostSecondaryEducationAssessmentSection>          PostSecondaryEducationAssessmentSections          { get; set; }
        public virtual DbSet<PostSecondaryEducationSection>                    PostSecondaryEducationSections                    { get; set; }
        public virtual DbSet<PostSecondaryLicense>                             PostSecondaryLicenses                             { get; set; }
        public virtual DbSet<Authorization>                                    Authorizations                                    { get; set; }
        public virtual DbSet<Role>                                             Roles                                             { get; set; }
        public virtual DbSet<RoleAuthorization>                                RoleAuthorizations                                { get; set; }
        public virtual DbSet<DriverLicense>                                    DriverLicenses                                    { get; set; }
        public virtual DbSet<DriverLicenseType>                                DriverLicenseTypes                                { get; set; }
        public virtual DbSet<DriversLicenseInvalidReasonType>                  DriversLicenseInvalidReasonTypes                  { get; set; }
        public virtual DbSet<TransportationAssessmentSection>                  TransportationAssessmentSections                  { get; set; }
        public virtual DbSet<TransportationSection>                            TransportationSections                            { get; set; }
        public virtual DbSet<TransportationSectionMethodBridge>                TransportationSectionMethodBridges                { get; set; }
        public virtual DbSet<TransportationType>                               TransportationTypes                               { get; set; }
        public virtual DbSet<HousingHistory>                                   HousingHistories                                  { get; set; }
        public virtual DbSet<HousingSection>                                   HousingSections                                   { get; set; }
        public virtual DbSet<HousingSituation>                                 HousingSituations                                 { get; set; }
        public virtual DbSet<BenefitsOfferedType>                              BenefitsOfferedTypes                              { get; set; }
        public virtual DbSet<EmploymentInformation>                            EmploymentInformations                            { get; set; }
        public virtual DbSet<EmploymentInformationBenefitsOfferedTypeBridge>   EmploymentInformationBenefitsOfferedTypeBridges   { get; set; }
        public virtual DbSet<EmploymentInformationJobDutiesDetailsBridge>      EmploymentInformationJobDutiesDetailsBridges      { get; set; }
        public virtual DbSet<EmploymentProgramType>                            EmploymentProgramTypes                            { get; set; }
        public virtual DbSet<EmploymentStatusType>                             EmploymentStatusTypes                             { get; set; }
        public virtual DbSet<IntervalType>                                     IntervalTypes                                     { get; set; }
        public virtual DbSet<JobFoundMethod>                                   JobFoundMethods                                   { get; set; }
        public virtual DbSet<JobSector>                                        JobSectors                                        { get; set; }
        public virtual DbSet<LeavingReason>                                    LeavingReasons                                    { get; set; }
        public virtual DbSet<OtherJobInformation>                              OtherJobInformations                              { get; set; }
        public virtual DbSet<WageHour>                                         WageHours                                         { get; set; }
        public virtual DbSet<WageHourHistory>                                  WageHourHistories                                 { get; set; }
        public virtual DbSet<WageHourHistoryWageTypeBridge>                    WageHourHistoryWageTypeBridges                    { get; set; }
        public virtual DbSet<WageHourWageTypeBridge>                           WageHourWageTypeBridges                           { get; set; }
        public virtual DbSet<WageType>                                         WageTypes                                         { get; set; }
        public virtual DbSet<InvolvedWorkProgram>                              InvolvedWorkPrograms                              { get; set; }
        public virtual DbSet<WorkProgram>                                      WorkPrograms                                      { get; set; }
        public virtual DbSet<WorkProgramSection>                               WorkProgramSections                               { get; set; }
        public virtual DbSet<WorkProgramStatus>                                WorkProgramStatuses                               { get; set; }
        public virtual DbSet<ApprovalReason>                                   ApprovalReasons                                   { get; set; }
        public virtual DbSet<ChangeReason>                                     ChangeReasons                                     { get; set; }
        public virtual DbSet<DeleteReason>                                     DeleteReasons                                     { get; set; }
        public virtual DbSet<DenialReason>                                     DenialReasons                                     { get; set; }
        public virtual DbSet<EnrolledProgram>                                  EnrolledPrograms                                  { get; set; }
        public virtual DbSet<ExtensionDecision>                                ExtensionDecisions                                { get; set; }
        public virtual DbSet<TimeLimit>                                        TimeLimits                                        { get; set; }
        public virtual DbSet<TimeLimitExtension>                               TimeLimitExtensions                               { get; set; }
        public virtual DbSet<TimeLimitState>                                   TimeLimitStates                                   { get; set; }
        public virtual DbSet<TimeLimitSummary>                                 TimeLimitSummaries                                { get; set; }
        public virtual DbSet<TimeLimitType>                                    TimeLimitTypes                                    { get; set; }
        public virtual DbSet<T0459_IN_W2_LIMITS>                               T0459_IN_W2_LIMITS                                { get; set; }
        public virtual DbSet<T0460_IN_W2_EXT>                                  T0460_IN_W2_EXT                                   { get; set; }
        public virtual DbSet<ChildCareArrangement>                             ChildCareArrangements                             { get; set; }
        public virtual DbSet<ChildYouthSupportsAssessmentSection>              ChildYouthSupportsAssessmentSections              { get; set; }
        public virtual DbSet<CommentType>                                      CommentTypes                                      { get; set; }
        public virtual DbSet<ContactTitleType>                                 ContactTitleTypes                                 { get; set; }
        public virtual DbSet<JobDutiesDetail>                                  JobDutiesDetails                                  { get; set; }
        public virtual DbSet<JobType>                                          JobTypes                                          { get; set; }
        public virtual DbSet<JobTypeLeavingReasonBridge>                       JobTypeLeavingBridges                             { get; set; }
        public virtual DbSet<WorkHistorySection>                               WorkHistorySections                               { get; set; }
        public virtual DbSet<AuxiliaryPayment>                                 AuxiliaryPayments                                 { get; set; }
        public virtual DbSet<DriversLicenseState>                              DriversLicenseStates                              { get; set; }
        public virtual DbSet<EducationExam>                                    EducationExams                                    { get; set; }
        public virtual DbSet<ExamEquivalencyType>                              ExamEquivalencyTypes                              { get; set; }
        public virtual DbSet<ExamLevelType>                                    ExamLevelTypes                                    { get; set; }
        public virtual DbSet<ExamPassType>                                     ExamPassTypes                                     { get; set; }
        public virtual DbSet<ExamSubjectMaxScoreType>                          ExamSubjectMaxScoreTypes                          { get; set; }
        public virtual DbSet<ExamSubjectType>                                  ExamSubjectTypes                                  { get; set; }
        public virtual DbSet<ExamType>                                         ExamTypes                                         { get; set; }
        public virtual DbSet<NRSType>                                          NRSTypes                                          { get; set; }
        public virtual DbSet<SPLType>                                          SPLTypes                                          { get; set; }
        public virtual DbSet<DeleteReasonByRepeater>                           DeleteReasonByRepeaters                           { get; set; }
        public virtual DbSet<Conviction>                                       Convictions                                       { get; set; }
        public virtual DbSet<ExamResult>                                       ExamResults                                       { get; set; }
        public virtual DbSet<ExamSubjectTypeBridge>                            ExamSubjectTypeBridges                            { get; set; }
        public virtual DbSet<LanguageAssessmentSection>                        LanguageAssessmentSections                        { get; set; }
        public virtual DbSet<JobQueue>                                         JobQueues                                         { get; set; }
        public virtual DbSet<JobQueueItem>                                     JobQueueItems                                     { get; set; }
        public virtual DbSet<JobQueueItemHistory>                              JobQueueItemHistories                             { get; set; }
        public virtual DbSet<T0459_IN_W2_LIMITS_COMPARE>                       T0459_IN_W2_LIMITS_COMPARE                        { get; set; }
        public virtual DbSet<TimelimitClosureLog>                              TimelimitClosureLogs                              { get; set; }
        public virtual DbSet<LanguageSection>                                  LanguageSections                                  { get; set; }
        public virtual DbSet<ActionAssignee>                                   ActionAssignees                                   { get; set; }
        public virtual DbSet<ActionItem>                                       ActionItems                                       { get; set; }
        public virtual DbSet<ActionNeeded>                                     ActionNeededs                                     { get; set; }
        public virtual DbSet<ActionNeededPage>                                 ActionNeededPages                                 { get; set; }
        public virtual DbSet<ActionNeededPageActionItemBridge>                 ActionNeededPageActionItemBridges                 { get; set; }
        public virtual DbSet<ActionNeededTask>                                 ActionNeededTasks                                 { get; set; }
        public virtual DbSet<ActionPriority>                                   ActionPriorities                                  { get; set; }
        public virtual DbSet<WorkHistoryAssessmentSection>                     WorkHistoryAssessmentSections                     { get; set; }
        public virtual DbSet<EmploymentPreventionType>                         EmploymentPreventionTypes                         { get; set; }
        public virtual DbSet<WorkHistorySectionEmploymentPreventionTypeBridge> WorkHistorySectionEmploymentPreventionTypeBridges { get; set; }
        public virtual DbSet<WorkProgramAssessmentSection>                     WorkProgramAssessmentSections                     { get; set; }
        public virtual DbSet<EducationAssessmentSection>                       EducationAssessmentSections                       { get; set; }
        public virtual DbSet<MilitaryTrainingAssessmentSection>                MilitaryTrainingAssessmentSections                { get; set; }
        public virtual DbSet<HousingAssessmentSection>                         HousingAssessmentSections                         { get; set; }
        public virtual DbSet<LegalIssuesAssessmentSection>                     LegalIssuesAssessmentSections                     { get; set; }
        public virtual DbSet<RecentParticipant>                                RecentParticipants                                { get; set; }
        public virtual DbSet<SP_ParticpantsChildrenReturnType>                 SP_ParticpantsChildrenReturnType                  { get; set; }
        public virtual DbSet<ContentModule>                                    ContentModules                                    { get; set; }
        public virtual DbSet<ContentModuleMeta>                                ContentModuleMetas                                { get; set; }
        public virtual DbSet<ContentPage>                                      ContentPages                                      { get; set; }
        public virtual DbSet<EnrolledProgramStatusCode>                        EnrolledProgramStatusCodes                        { get; set; }
        public virtual DbSet<ParticipantEnrolledProgram>                       ParticipantEnrolledPrograms                       { get; set; }
        public virtual DbSet<ParticipantEnrolledProgramCutOverBridge>          ParticipantEnrolledProgramCutOverBridges          { get; set; }
        public virtual DbSet<NonCustodialParentsReferralAssessmentSection>     NonCustodialParentsReferralAssessmentSections     { get; set; }
        public virtual DbSet<YesNoRefused>                                     YesNoRefuseds                                     { get; set; }
        public virtual DbSet<FamilyBarriersSection>                            FamilyBarriersSections                            { get; set; }
        public virtual DbSet<OfficeTransfer>                                   OfficeTransfers                                   { get; set; }
        public virtual DbSet<DisabledPopulationType>                           DisabledPopulationTypes                           { get; set; }
        public virtual DbSet<PopulationType>                                   PopulationTypes                                   { get; set; }
        public virtual DbSet<RequestForAssistanceStatus>                       RequestForAssistanceStatuses                      { get; set; }
        public virtual DbSet<CountyAndTribe>                                   CountyAndTribes                                   { get; set; }
        public virtual DbSet<RequestForAssistance>                             RequestsForAssistance                             { get; set; }
        public virtual DbSet<RequestForAssistanceChild>                        RequestForAssistanceChilds                        { get; set; }
        public virtual DbSet<GenderType>                                       GenderTypes                                       { get; set; }
        public virtual DbSet<CompletionReason>                                 CompletionReasons                                 { get; set; }
        public virtual DbSet<EligibilityByFPL>                                 EligibilityByFPLs                                 { get; set; }
        public virtual DbSet<FamilyBarriersActionBridge>                       FamilyBarriersActionBridges                       { get; set; }
        public virtual DbSet<HolidayLookUp>                                    HolidayLookUps                                    { get; set; }
        public virtual DbSet<PhysicalHealthBarrierBridge>                      PhysicalHealthBarrierBridges                      { get; set; }
        public virtual DbSet<RequestForAssistancePopulationTypeBridge>         RequestForAssistancePopulationTypeBridges         { get; set; }
        public virtual DbSet<WageAction>                                       WageActions                                       { get; set; }
        public virtual DbSet<WorkerParticipantBridge>                          WorkerParticipantBridges                          { get; set; }
        public virtual DbSet<RuleReason>                                       RuleReasons                                       { get; set; }
        public virtual DbSet<SP_CwwReferredParticipantReturnType>              SP_CwwReferredParticipantReturnType               { get; set; }
        public virtual DbSet<ContractArea>                                     ContractAreas                                     { get; set; }
        public virtual DbSet<Organization>                                     Organizations                                     { get; set; }
        public virtual DbSet<Office>                                           Offices                                           { get; set; }
        public virtual DbSet<RequestForAssistanceRuleReason>                   RequestForAssistanceRuleReasons                   { get; set; }
        public virtual DbSet<EnrolledProgramOrganizationPopulationTypeBridge>  EnrolledProgramOrganizationPopulationTypeBridges  { get; set; }
        public virtual DbSet<EmployerOfRecordInformation>                      EmployerOfRecordInformations                      { get; set; }
        public virtual DbSet<EmployerOfRecordType>                             EmployerOfRecordTypes                             { get; set; }
        public virtual DbSet<SuffixType>                                       SuffixTypes                                       { get; set; }
        public virtual DbSet<AliasType>                                        AliasTypes                                        { get; set; }
        public virtual DbSet<AKA>                                              AKAs                                              { get; set; }
        public virtual DbSet<OtherDemographic>                                 OtherDemographics                                 { get; set; }
        public virtual DbSet<ElevatedAccess>                                   ElevatedAccesses                                  { get; set; }
        public virtual DbSet<ElevatedAccessReason>                             ElevatedAccessReasons                             { get; set; }
        public virtual DbSet<ConfidentialPinInformation>                       ConfidentialPinInformations                       { get; set; }
        public virtual DbSet<ParticipantContactInfo>                           ParticipantContactInfoes                          { get; set; }
        public virtual DbSet<AlternateMailingAddress>                          AlternateMailingAddresses                         { get; set; }
        public virtual DbSet<SSNType>                                          SSNTypes                                          { get; set; }
        public virtual DbSet<SP_ParticipantDetailsReturnType>                  SP_ParticipantDetailsReturnType                   { get; set; }
        public virtual DbSet<Worker>                                           Workers                                           { get; set; }
        public virtual DbSet<FeatureURL>                                       FeatureURLs                                       { get; set; }
        public virtual DbSet<Activity>                                         Activities                                        { get; set; }
        public virtual DbSet<ActivityLocation>                                 ActivityLocations                                 { get; set; }
        public virtual DbSet<ActivitySchedule>                                 ActivitySchedules                                 { get; set; }
        public virtual DbSet<ActivityScheduleFrequencyBridge>                  ActivityScheduleFrequencyBridges                  { get; set; }
        public virtual DbSet<ActivityType>                                     ActivityTypes                                     { get; set; }
        public virtual DbSet<EmployabilityPlan>                                EmployabilityPlans                                { get; set; }
        public virtual DbSet<Frequency>                                        Frequencies                                       { get; set; }
        public virtual DbSet<FrequencyType>                                    FrequencyTypes                                    { get; set; }
        public virtual DbSet<Goal>                                             Goals                                             { get; set; }
        public virtual DbSet<GoalEndReason>                                    GoalEndReasons                                    { get; set; }
        public virtual DbSet<GoalStep>                                         GoalSteps                                         { get; set; }
        public virtual DbSet<GoalType>                                         GoalTypes                                         { get; set; }
        public virtual DbSet<SupportiveService>                                SupportiveServices                                { get; set; }
        public virtual DbSet<SupportiveServiceType>                            SupportiveServiceTypes                            { get; set; }
        public virtual DbSet<ActivityContactBridge>                            ActivityContactBridges                            { get; set; }
        public virtual DbSet<EnrolledProgramEPActivityTypeBridge>              EnrolledProgramEPActivityTypeBridges              { get; set; }
        public virtual DbSet<EmployabilityPlanActivityBridge>                  EmployabilityPlanActivityBridges                  { get; set; }
        public virtual DbSet<EmployabilityPlanGoalBridge>                      EmployabilityPlanGoalBridges                      { get; set; }
        public virtual DbSet<PEPOtherInformation>                              PEPOtherInformations                              { get; set; }
        public virtual DbSet<NonSelfDirectedActivity>                          NonSelfDirectedActivities                         { get; set; }
        public virtual DbSet<ParticipationStatu>                               ParticipationStatus                               { get; set; }
        public virtual DbSet<ParticipationStatusType>                          ParticipationStatusTypes                          { get; set; }
        public virtual DbSet<CFRfaDetail>                                      CFRfaDetails                                      { get; set; }
        public virtual DbSet<FCDPRfaDetail>                                    FCDPRfaDetails                                    { get; set; }
        public virtual DbSet<TJTMJRfaDetail>                                   TJTMJRfaDetails                                   { get; set; }
        public virtual DbSet<SpecialInitiative>                                SpecialInitiatives                                { get; set; }
        public virtual DbSet<EmployabilityPlanStatusType>                      EmployabilityPlanStatusTypes                      { get; set; }
        public virtual DbSet<EPEIBridge>                                       EPEIBridges                                       { get; set; }
        public virtual DbSet<AssociatedOrganization>                           AssociatedOrganizations                           { get; set; }
        public virtual DbSet<AddressVerificationTypeLookup>                    AddressVerificationTypeLookups                    { get; set; }
        public virtual DbSet<EARequestParticipantBridge>                       EaRequestParticipantBridges                       { get; set; }
        public virtual DbSet<Transaction>                                      Transactions                                      { get; set; }
        public virtual DbSet<WorkerTaskCategory>                               WorkerTaskCategories                              { get; set; }
        public virtual DbSet<WorkerTaskList>                                   WorkerTaskLists                                   { get; set; }
        public virtual DbSet<WorkerTaskStatus>                                 WorkerTaskStatuses                                { get; set; }
        public virtual DbSet<EmploymentVerification>                           EmploymentVerifications                           { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("wwp");

            // global conventions for the WWP model
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Load all the entity *Config.cs files for the model (by convention they all end in 'Config'.cs)
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public virtual IList<SP_CaseAddressReturnType> SP_CaseAddress(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_CaseAddressReturnType>("wwp.SP_CaseAddress", parms);

            return rs;
        }

        public virtual IList<SP_CWWChildCareEligibiltyStatus_Result> SP_CWWChildCareEligibiltyStatus(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_CWWChildCareEligibiltyStatus_Result>("wwp.SP_CWWChildCareEligibiltyStatus", parms);

            return rs;
        }

        public virtual IList<SP_DeleteContactReturnType> SP_DeleteContact(int? contactId, string modifiedBy)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["ContactId"]  = contactId  ?? (object) DBNull.Value,
                            ["ModifiedBy"] = modifiedBy ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_DeleteContactReturnType>("wwp.SP_DeleteContact", parms);

            return rs;
        }

        public virtual IList<SP_FSETStatusReturnType> SP_FSETStatus(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_FSETStatusReturnType>("wwp.SP_FSETStatus", parms);

            return rs;
        }

        public virtual IList<SP_LearnfareStatusReturnType> SP_LearnFareStatus(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_LearnfareStatusReturnType>("wwp.SP_LearnFareStatus", parms);

            return rs;
        }

        public virtual IList<SP_OtherParticipant_Result> SP_OtherParticipant(string pinNumber, DateTime? beginDate, DateTime? endDate, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["BeginDate"]  = beginDate  ?? (object) DBNull.Value,
                            ["EndDate"]    = endDate    ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_OtherParticipant_Result>("wwp.SP_OtherParticipant", parms);

            return rs;
        }

        public virtual IList<SP_RentPaidReturnType> SP_RentPaid(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_RentPaidReturnType>("wwp.SP_RentPaid", parms);

            return rs;
        }

        public virtual IList<SP_SocialSecurityStatusReturnType> SP_SocialSecurityStatus(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_SocialSecurityStatusReturnType>("wwp.SP_SocialSecurityStatus", parms);

            return rs;
        }

        public virtual IList<SP_SubsidizedHousingReturnType> SP_SubsidizedHousing(string pinNumber, string yYYYMM, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["YYYYMM"]     = yYYYMM     ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_SubsidizedHousingReturnType>("wwp.SP_SubsidizedHousing", parms);

            return rs;
        }

        public virtual IList<Participant> SP_TimeLimitParticipant(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<Participant>("wwp.SP_TimeLimitParticipant", parms);

            return rs;
        }

        public virtual void DB2_T0460_Insert(string pIN_NUM, string cLOCK_TYPE_CD, string eXT_SEQ_NUM, string hISTORY_SEQ_NUM, string aGY_DCSN_CD, string aGY_DCSN_DT, string bENEFIT_MM, string dELETE_REASON_CD, string eXT_BEG_MM, string eXT_END_MM, string eXT_REQ_PRC_DT, string hISTORY_CD, string sTA_DCSN_CD, string uPDATED_DT, string uSER_ID)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PIN_NUM"]          = pIN_NUM          ?? (object) DBNull.Value,
                            ["CLOCK_TYPE_CD"]    = cLOCK_TYPE_CD    ?? (object) DBNull.Value,
                            ["EXT_SEQ_NUM"]      = eXT_SEQ_NUM      ?? (object) DBNull.Value,
                            ["HISTORY_SEQ_NUM"]  = hISTORY_SEQ_NUM  ?? (object) DBNull.Value,
                            ["AGY_DCSN_CD"]      = aGY_DCSN_CD      ?? (object) DBNull.Value,
                            ["AGY_DCSN_DT"]      = aGY_DCSN_DT      ?? (object) DBNull.Value,
                            ["BENEFIT_MM"]       = bENEFIT_MM       ?? (object) DBNull.Value,
                            ["DELETE_REASON_CD"] = dELETE_REASON_CD ?? (object) DBNull.Value,
                            ["EXT_BEG_MM"]       = eXT_BEG_MM       ?? (object) DBNull.Value,
                            ["EXT_END_MM"]       = eXT_END_MM       ?? (object) DBNull.Value,
                            ["EXT_REQ_PRC_DT"]   = eXT_REQ_PRC_DT   ?? (object) DBNull.Value,
                            ["HISTORY_CD"]       = hISTORY_CD       ?? (object) DBNull.Value,
                            ["STA_DCSN_CD"]      = sTA_DCSN_CD      ?? (object) DBNull.Value,
                            ["UPDATED_DT"]       = uPDATED_DT       ?? (object) DBNull.Value,
                            ["USER_ID"]          = uSER_ID          ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.DB2_T0460_Insert", parms);
        }

        public virtual void DB2_T0460_Update(string pIN_NUM, string cLOCK_TYPE_CD, string eXT_SEQ_NUM, string hISTORY_SEQ_NUM, string aGY_DCSN_CD, string aGY_DCSN_DT, string bENEFIT_MM, string dELETE_REASON_CD, string eXT_BEG_MM, string eXT_END_MM, string eXT_REQ_PRC_DT, string hISTORY_CD, string sTA_DCSN_CD, string uPDATED_DT, string uSER_ID)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PIN_NUM"]          = pIN_NUM          ?? (object) DBNull.Value,
                            ["CLOCK_TYPE_CD"]    = cLOCK_TYPE_CD    ?? (object) DBNull.Value,
                            ["EXT_SEQ_NUM"]      = eXT_SEQ_NUM      ?? (object) DBNull.Value,
                            ["HISTORY_SEQ_NUM"]  = hISTORY_SEQ_NUM  ?? (object) DBNull.Value,
                            ["AGY_DCSN_CD"]      = aGY_DCSN_CD      ?? (object) DBNull.Value,
                            ["AGY_DCSN_DT"]      = aGY_DCSN_DT      ?? (object) DBNull.Value,
                            ["BENEFIT_MM"]       = bENEFIT_MM       ?? (object) DBNull.Value,
                            ["DELETE_REASON_CD"] = dELETE_REASON_CD ?? (object) DBNull.Value,
                            ["EXT_BEG_MM"]       = eXT_BEG_MM       ?? (object) DBNull.Value,
                            ["EXT_END_MM"]       = eXT_END_MM       ?? (object) DBNull.Value,
                            ["EXT_REQ_PRC_DT"]   = eXT_REQ_PRC_DT   ?? (object) DBNull.Value,
                            ["HISTORY_CD"]       = hISTORY_CD       ?? (object) DBNull.Value,
                            ["STA_DCSN_CD"]      = sTA_DCSN_CD      ?? (object) DBNull.Value,
                            ["UPDATED_DT"]       = uPDATED_DT       ?? (object) DBNull.Value,
                            ["USER_ID"]          = uSER_ID          ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.DB2_T0460_Update", parms);
        }

        public virtual void SP_ReadCDCandHistoryData(string tableName, string identityNumber)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["TableName"]      = tableName      ?? (object) DBNull.Value,
                            ["IdentityNumber"] = identityNumber ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_ReadCDCandHistoryData", parms);
        }

        public virtual void SP_ReadCDCHistory(string tableName, string identityNumber)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["TableName"]      = tableName      ?? (object) DBNull.Value,
                            ["IdentityNumber"] = identityNumber ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_ReadCDCHistory", parms);
        }

        public virtual void DB2_T0754_Insert(string cS_RFA_PRV_PIN_NUM, string cS_RFA_PRV_PIN_IND, string dEPT_ID, string pROGRAM_CD, string sUBPROGRAM_CD, string aG_SEQ_NUM, string rQST_TMS, string cNTY_NUM, string cRE_IND, string dOC_CD, string lTR_MO, string oFC_NUM, string pROC_DT, string pRVD_LOC_NUM, string sEC_RCPT_ID, string sPRS_USER_ID, string uSER_ID, string lTR_TXT)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["CS_RFA_PRV_PIN_NUM"] = cS_RFA_PRV_PIN_NUM ?? (object) DBNull.Value,
                            ["CS_RFA_PRV_PIN_IND"] = cS_RFA_PRV_PIN_IND ?? (object) DBNull.Value,
                            ["DEPT_ID"]            = dEPT_ID            ?? (object) DBNull.Value,
                            ["PROGRAM_CD"]         = pROGRAM_CD         ?? (object) DBNull.Value,
                            ["SUBPROGRAM_CD"]      = sUBPROGRAM_CD      ?? (object) DBNull.Value,
                            ["AG_SEQ_NUM"]         = aG_SEQ_NUM         ?? (object) DBNull.Value,
                            ["RQST_TMS"]           = rQST_TMS           ?? (object) DBNull.Value,
                            ["CNTY_NUM"]           = cNTY_NUM           ?? (object) DBNull.Value,
                            ["CRE_IND"]            = cRE_IND            ?? (object) DBNull.Value,
                            ["DOC_CD"]             = dOC_CD             ?? (object) DBNull.Value,
                            ["LTR_MO"]             = lTR_MO             ?? (object) DBNull.Value,
                            ["OFC_NUM"]            = oFC_NUM            ?? (object) DBNull.Value,
                            ["PROC_DT"]            = pROC_DT            ?? (object) DBNull.Value,
                            ["PRVD_LOC_NUM"]       = pRVD_LOC_NUM       ?? (object) DBNull.Value,
                            ["SEC_RCPT_ID"]        = sEC_RCPT_ID        ?? (object) DBNull.Value,
                            ["SPRS_USER_ID"]       = sPRS_USER_ID       ?? (object) DBNull.Value,
                            ["USER_ID"]            = uSER_ID            ?? (object) DBNull.Value,
                            ["LTR_TXT"]            = lTR_TXT            ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.DB2_T0754_Insert", parms);
        }

        public virtual void DB2_T0459_Insert(decimal? pIN_NUM, decimal? bENEFIT_MM, short? hISTORY_SEQ_NUM, string cLOCK_TYPE_CD, string cRE_TRAN_CD, string fED_CLOCK_IND, short? fED_CMP_MTH_NUM, short? fED_MAX_MTH_NUM, short? hISTORY_CD, short? oT_CMP_MTH_NUM, string oVERRIDE_REASON_CD, short? tOT_CMP_MTH_NUM, short? tOT_MAX_MTH_NUM, DateTime? uPDATED_DT, string uSER_ID, short? wW_CMP_MTH_NUM, short? wW_MAX_MTH_NUM, string cOMMENT_TXT)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PIN_NUM"]            = pIN_NUM            ?? (object) DBNull.Value,
                            ["BENEFIT_MM"]         = bENEFIT_MM         ?? (object) DBNull.Value,
                            ["HISTORY_SEQ_NUM"]    = hISTORY_SEQ_NUM    ?? (object) DBNull.Value,
                            ["CLOCK_TYPE_CD"]      = cLOCK_TYPE_CD      ?? (object) DBNull.Value,
                            ["CRE_TRAN_CD"]        = cRE_TRAN_CD        ?? (object) DBNull.Value,
                            ["FED_CLOCK_IND"]      = fED_CLOCK_IND      ?? (object) DBNull.Value,
                            ["FED_CMP_MTH_NUM"]    = fED_CMP_MTH_NUM    ?? (object) DBNull.Value,
                            ["FED_MAX_MTH_NUM"]    = fED_MAX_MTH_NUM    ?? (object) DBNull.Value,
                            ["HISTORY_CD"]         = hISTORY_CD         ?? (object) DBNull.Value,
                            ["OT_CMP_MTH_NUM"]     = oT_CMP_MTH_NUM     ?? (object) DBNull.Value,
                            ["OVERRIDE_REASON_CD"] = oVERRIDE_REASON_CD ?? (object) DBNull.Value,
                            ["TOT_CMP_MTH_NUM"]    = tOT_CMP_MTH_NUM    ?? (object) DBNull.Value,
                            ["TOT_MAX_MTH_NUM"]    = tOT_MAX_MTH_NUM    ?? (object) DBNull.Value,
                            ["UPDATED_DT"]         = uPDATED_DT         ?? (object) DBNull.Value,
                            ["USER_ID"]            = uSER_ID            ?? (object) DBNull.Value,
                            ["WW_CMP_MTH_NUM"]     = wW_CMP_MTH_NUM     ?? (object) DBNull.Value,
                            ["WW_MAX_MTH_NUM"]     = wW_MAX_MTH_NUM     ?? (object) DBNull.Value,
                            ["COMMENT_TXT"]        = cOMMENT_TXT        ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.DB2_T0459_Insert", parms);
        }

        public virtual IList<DB2_T0459_Update_Result> DB2_T0459_Update(decimal? pIN_NUM, decimal? bENEFIT_MM, short? hISTORY_SEQ_NUM, string cLOCK_TYPE_CD, string cRE_TRAN_CD, string fED_CLOCK_IND, short? fED_CMP_MTH_NUM, short? fED_MAX_MTH_NUM, short? hISTORY_CD, short? oT_CMP_MTH_NUM, string oVERRIDE_REASON_CD, short? tOT_CMP_MTH_NUM, short? tOT_MAX_MTH_NUM, DateTime? uPDATED_DT, string uSER_ID, short? wW_CMP_MTH_NUM, short? wW_MAX_MTH_NUM, string cOMMENT_TXT)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PIN_NUM"]            = pIN_NUM            ?? (object) DBNull.Value,
                            ["BENEFIT_MM"]         = bENEFIT_MM         ?? (object) DBNull.Value,
                            ["HISTORY_SEQ_NUM"]    = hISTORY_SEQ_NUM    ?? (object) DBNull.Value,
                            ["CLOCK_TYPE_CD"]      = cLOCK_TYPE_CD      ?? (object) DBNull.Value,
                            ["CRE_TRAN_CD"]        = cRE_TRAN_CD        ?? (object) DBNull.Value,
                            ["FED_CLOCK_IND"]      = fED_CLOCK_IND      ?? (object) DBNull.Value,
                            ["FED_CMP_MTH_NUM"]    = fED_CMP_MTH_NUM    ?? (object) DBNull.Value,
                            ["FED_MAX_MTH_NUM"]    = fED_MAX_MTH_NUM    ?? (object) DBNull.Value,
                            ["HISTORY_CD"]         = hISTORY_CD         ?? (object) DBNull.Value,
                            ["OT_CMP_MTH_NUM"]     = oT_CMP_MTH_NUM     ?? (object) DBNull.Value,
                            ["OVERRIDE_REASON_CD"] = oVERRIDE_REASON_CD ?? (object) DBNull.Value,
                            ["TOT_CMP_MTH_NUM"]    = tOT_CMP_MTH_NUM    ?? (object) DBNull.Value,
                            ["TOT_MAX_MTH_NUM"]    = tOT_MAX_MTH_NUM    ?? (object) DBNull.Value,
                            ["UPDATED_DT"]         = uPDATED_DT         ?? (object) DBNull.Value,
                            ["USER_ID"]            = uSER_ID            ?? (object) DBNull.Value,
                            ["WW_CMP_MTH_NUM"]     = wW_CMP_MTH_NUM     ?? (object) DBNull.Value,
                            ["WW_MAX_MTH_NUM"]     = wW_MAX_MTH_NUM     ?? (object) DBNull.Value,
                            ["COMMENT_TXT"]        = cOMMENT_TXT        ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<DB2_T0459_Update_Result>("wwp.DB2_T0459_Update", parms);

            return rs;
        }

        public virtual IList<SP_ParticpantsChildrenReturnType> SP_ParticpantsChildrenFromCARES(string pinnumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinnumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_ParticpantsChildrenReturnType>("wwp.SP_ParticpantsChildrenFromCARES", parms);

            return rs;
        }

        public virtual IList<SP_CwwReferredParticipantReturnType> SP_CWWReferredParticipant(string entSecOrgCode, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["EntSecOrgCode"] = entSecOrgCode ?? (object) DBNull.Value,
                            ["SchemaName"]    = schemaName    ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_CwwReferredParticipantReturnType>("wwp.SP_CWWReferredParticipant", parms);

            return rs;
        }

        public virtual void DB2_T0018_Update(string pIN_NUM, string wP_SYSTEM_STS_CD, string dISENROLLMENT_DT)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["WP_SYSTEM_STS_CD"] = wP_SYSTEM_STS_CD ?? (object) DBNull.Value,
                            ["DISENROLLMENT_DT"] = dISENROLLMENT_DT ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.DB2_T0018_Update", parms);
        }

        public virtual void SP_DB2_Disenrollment_Update(decimal? pinNumber, DateTime? effectiveDate, short? countyNumber, short? officeNumber, string mFWorkerId, string mFUserId, string programCode, string subProgramCode, string registrationCode, string completionReason, string anyOtherProgramOpen, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]           = pinNumber           ?? (object) DBNull.Value,
                            ["EffectiveDate"]       = effectiveDate       ?? (object) DBNull.Value,
                            ["CountyNumber"]        = countyNumber        ?? (object) DBNull.Value,
                            ["OfficeNumber"]        = officeNumber        ?? (object) DBNull.Value,
                            ["MFWorkerId"]          = mFWorkerId          ?? (object) DBNull.Value,
                            ["MFUserId"]            = mFUserId            ?? (object) DBNull.Value,
                            ["ProgramCode"]         = programCode         ?? (object) DBNull.Value,
                            ["SubProgramCode"]      = subProgramCode      ?? (object) DBNull.Value,
                            ["RegistrationCode"]    = registrationCode    ?? (object) DBNull.Value,
                            ["CompletionReason"]    = completionReason    ?? (object) DBNull.Value,
                            ["AnyOtherProgramOpen"] = anyOtherProgramOpen ?? (object) DBNull.Value,
                            ["SchemaName"]          = schemaName          ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_DB2_Disenrollment_Update", parms);
        }

        public virtual IList<SP_PreCheckDisenrollment_Result> SP_PreCheckDisenrollment(decimal? pinNumber, decimal? caseNumber, string schemaName, int? pEPId)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["CaseNumber"] = caseNumber ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value,
                            ["PEPId"]      = pEPId      ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_PreCheckDisenrollment_Result>("wwp.SP_PreCheckDisenrollment", parms);

            return rs;
        }

        public virtual void SP_DB2_Enrollment_Update(decimal? pinNumber, DateTime? effectiveDate, short? countyNumber, short? officeNumber, string mFWorkerId, string mFUserId, string programCode, string subProgramCode, string currentRegCode, string schemaName, decimal? rFANumber)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]      = pinNumber      ?? (object) DBNull.Value,
                            ["EffectiveDate"]  = effectiveDate  ?? (object) DBNull.Value,
                            ["CountyNumber"]   = countyNumber   ?? (object) DBNull.Value,
                            ["OfficeNumber"]   = officeNumber   ?? (object) DBNull.Value,
                            ["MFWorkerId"]     = mFWorkerId     ?? (object) DBNull.Value,
                            ["MFUserId"]       = mFUserId       ?? (object) DBNull.Value,
                            ["ProgramCode"]    = programCode    ?? (object) DBNull.Value,
                            ["SubProgramCode"] = subProgramCode ?? (object) DBNull.Value,
                            ["CurrentRegCode"] = currentRegCode ?? (object) DBNull.Value,
                            ["SchemaName"]     = schemaName     ?? (object) DBNull.Value,
                            ["RFANumber"]      = rFANumber      ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_DB2_Enrollment_Update", parms);
        }

        public virtual IList<Participant> SP_RefreshParticipant(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<Participant>("wwp.SP_RefreshParticipant", parms);

            return rs;
        }

        public virtual IList<SP_ParticipantDetailsReturnType> SP_RefreshParticipantDetails(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_ParticipantDetailsReturnType>("wwp.SP_RefreshParticipantDetails", parms);

            return rs;
        }

        public virtual void SP_DB2_Transfer_Update(decimal? pinNumber, DateTime? effectiveDate, short? outGoingCountyNumber, short? outGoingOfficeNumber, short? incomingCountyNumber, short? incomingOfficeNumber, string mFWorkerId, string mFUserId, string programCode, string subProgramCode, string registrationCode, string schemaName, string fEPId)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]            = pinNumber            ?? (object) DBNull.Value,
                            ["EffectiveDate"]        = effectiveDate        ?? (object) DBNull.Value,
                            ["OutGoingCountyNumber"] = outGoingCountyNumber ?? (object) DBNull.Value,
                            ["OutGoingOfficeNumber"] = outGoingOfficeNumber ?? (object) DBNull.Value,
                            ["IncomingCountyNumber"] = incomingCountyNumber ?? (object) DBNull.Value,
                            ["IncomingOfficeNumber"] = incomingOfficeNumber ?? (object) DBNull.Value,
                            ["MFWorkerId"]           = mFWorkerId           ?? (object) DBNull.Value,
                            ["MFUserId"]             = mFUserId             ?? (object) DBNull.Value,
                            ["ProgramCode"]          = programCode          ?? (object) DBNull.Value,
                            ["SubProgramCode"]       = subProgramCode       ?? (object) DBNull.Value,
                            ["RegistrationCode"]     = registrationCode     ?? (object) DBNull.Value,
                            ["SchemaName"]           = schemaName           ?? (object) DBNull.Value,
                            ["FEPId"]                = fEPId                ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_DB2_Transfer_Update", parms);
        }

        public virtual IList<DateTime?> USP_GetComputedBusniessDays(DateTime? startDate, int? noofDays)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["StartDate"] = startDate ?? (object) DBNull.Value,
                            ["NoofDays"]  = noofDays  ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<DateTime?>("wwp.USP_GetComputedBusniessDays", parms);

            return rs;
        }

        public virtual IList<ParticipantEnrolledProgram> USP_ProgramStatus(decimal? pinNumber, string schemaName, bool? checkT0018, int? t0018EnrolledProgramId, ObjectParameter recentStatus, ObjectParameter referralDate, ObjectParameter enrollmentDate, ObjectParameter disEnrollmemtDate, ObjectParameter enrolledProgramId)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]              = pinNumber              ?? (object) DBNull.Value,
                            ["SchemaName"]             = schemaName             ?? (object) DBNull.Value,
                            ["CheckT0018"]             = checkT0018             ?? (object) DBNull.Value,
                            ["T0018EnrolledProgramId"] = t0018EnrolledProgramId ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<ParticipantEnrolledProgram>("wwp.USP_ProgramStatus", parms);

            return rs;
        }

        public virtual void SP_DB2_Referral_Update(decimal? pinNumber, decimal? rFANumber, DateTime? effectiveDate, short? countyNumber, short? officeNumber, short? courtOrderedCounty, DateTime? courtOrderedDate, string mFWorkerId, string programCode, string subProgramCode, string referralRegCode, string schemaName, DateTime? appDate)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]          = pinNumber          ?? (object) DBNull.Value,
                            ["RFANumber"]          = rFANumber          ?? (object) DBNull.Value,
                            ["EffectiveDate"]      = effectiveDate      ?? (object) DBNull.Value,
                            ["CountyNumber"]       = countyNumber       ?? (object) DBNull.Value,
                            ["OfficeNumber"]       = officeNumber       ?? (object) DBNull.Value,
                            ["CourtOrderedCounty"] = courtOrderedCounty ?? (object) DBNull.Value,
                            ["CourtOrderedDate"]   = courtOrderedDate   ?? (object) DBNull.Value,
                            ["MFWorkerId"]         = mFWorkerId         ?? (object) DBNull.Value,
                            ["ProgramCode"]        = programCode        ?? (object) DBNull.Value,
                            ["SubProgramCode"]     = subProgramCode     ?? (object) DBNull.Value,
                            ["ReferralRegCode"]    = referralRegCode    ?? (object) DBNull.Value,
                            ["SchemaName"]         = schemaName         ?? (object) DBNull.Value,
                            ["AppDate"]            = appDate            ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_DB2_Referral_Update", parms);
        }

        public virtual IList<SP_DB2_CreateRFA_Result> SP_DB2_CreateRFA(string schemaName, string pinNumber, string rFAProgramType, string createOrUpdateMode, string firstName, string lastName, string middleName, string suffixName, string languageIndicator, string countyNumber, string courtOrderCountyNumber, string courtOrderedEffectiveDate, string rFATimeStamp, string streetNumber, string streetName, string addressLine2, string cityAddress, string stateAddress, string zipAddress, string phoneNumber, string appStatusReasonCode, string rFAStatusChangeDate, string mfWorkerId, string inputRfaNumber)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["SchemaName"]                = schemaName                ?? (object) DBNull.Value,
                            ["PinNumber"]                 = pinNumber                 ?? (object) DBNull.Value,
                            ["RFAProgramType"]            = rFAProgramType            ?? (object) DBNull.Value,
                            ["CreateOrUpdateMode"]        = createOrUpdateMode        ?? (object) DBNull.Value,
                            ["FirstName"]                 = firstName                 ?? (object) DBNull.Value,
                            ["LastName"]                  = lastName                  ?? (object) DBNull.Value,
                            ["MiddleName"]                = middleName                ?? (object) DBNull.Value,
                            ["SuffixName"]                = suffixName                ?? (object) DBNull.Value,
                            ["LanguageIndicator"]         = languageIndicator         ?? (object) DBNull.Value,
                            ["CountyNumber"]              = countyNumber              ?? (object) DBNull.Value,
                            ["CourtOrderCountyNumber"]    = courtOrderCountyNumber    ?? (object) DBNull.Value,
                            ["CourtOrderedEffectiveDate"] = courtOrderedEffectiveDate ?? (object) DBNull.Value,
                            ["RFATimeStamp"]              = rFATimeStamp              ?? (object) DBNull.Value,
                            ["StreetNumber"]              = streetNumber              ?? (object) DBNull.Value,
                            ["StreetName"]                = streetName                ?? (object) DBNull.Value,
                            ["AddressLine2"]              = addressLine2              ?? (object) DBNull.Value,
                            ["CityAddress"]               = cityAddress               ?? (object) DBNull.Value,
                            ["StateAddress"]              = stateAddress              ?? (object) DBNull.Value,
                            ["ZipAddress"]                = zipAddress                ?? (object) DBNull.Value,
                            ["PhoneNumber"]               = phoneNumber               ?? (object) DBNull.Value,
                            ["AppStatusReasonCode"]       = appStatusReasonCode       ?? (object) DBNull.Value,
                            ["RFAStatusChangeDate"]       = rFAStatusChangeDate       ?? (object) DBNull.Value,
                            ["MfWorkerId"]                = mfWorkerId                ?? (object) DBNull.Value,
                            ["InputRfaNumber"]            = inputRfaNumber            ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_DB2_CreateRFA_Result>("wwp.SP_DB2_CreateRFA", parms);

            return rs;
        }

        public virtual IList<USP_RecentlyAccessed_ProgramStatus_Result> USP_RecentlyAccessed_ProgramStatus(string wAMSId)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["WAMSId"] = wAMSId ?? (object) DBNull.Value
                        };
            var rs = ExecStoredProc<USP_RecentlyAccessed_ProgramStatus_Result>("wwp.USP_RecentlyAccessed_ProgramStatus", parms);

            return rs;
        }

        public virtual IList<USP_RecentlyAccessed_ProgramStatus_Result> USP_ParticipantSearch_ProgramStatus(string firstName, string lastName, string middleName, string gender, DateTime? dob)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["FirstName"]     = firstName  ?? (object) DBNull.Value,
                            ["LastName"]      = lastName   ?? (object) DBNull.Value,
                            ["MiddleInitial"] = middleName ?? (object) DBNull.Value,
                            ["Gender"]        = gender     ?? (object) DBNull.Value,
                            ["Dob"]           = dob        ?? (object) DBNull.Value
                        };
            var rs = ExecStoredProc<USP_RecentlyAccessed_ProgramStatus_Result>("wwp.USP_ParticipantSearch_ProgramStatus", parms);

            return rs;
        }

        public virtual void SP_Barrier_Accommodation_Update(int? pEPId, DateTime? disenrollmentDate, bool? isBatch, string userId)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PEPId"]             = pEPId             ?? (object) DBNull.Value,
                            ["DisenrollmentDate"] = disenrollmentDate ?? (object) DBNull.Value,
                            ["IsBatch"]           = isBatch           ?? (object) DBNull.Value,
                            ["UserId"]            = userId            ?? (object) DBNull.Value
                        };
            ExecStoredProc("wwp.SP_Barrier_Accommodation_Update", parms);
        }

        public virtual IList<USP_ReferralsAndTransfers_Result> USP_ReferralsAndTransfers(string wAMSId, string schemaName, string entsecAgencyCode, string authorizations)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["WAMSId"]           = wAMSId           ?? (object) DBNull.Value,
                            ["SchemaName"]       = schemaName       ?? (object) DBNull.Value,
                            ["EntsecAgencyCode"] = entsecAgencyCode ?? (object) DBNull.Value,
                            ["Authorizations"]   = authorizations   ?? (object) DBNull.Value
                        };
            var rs = ExecStoredProc<USP_ReferralsAndTransfers_Result>("wwp.USP_ReferralsAndTransfers", parms);

            return rs;
        }

        public virtual void SP_DB2_InformalAssessment_Update(decimal? pinNumber, DateTime? effectiveDate, string mFWorkerId, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]     = pinNumber     ?? (object) DBNull.Value,
                            ["EffectiveDate"] = effectiveDate ?? (object) DBNull.Value,
                            ["MFWorkerId"]    = mFWorkerId    ?? (object) DBNull.Value,
                            ["SchemaName"]    = schemaName    ?? (object) DBNull.Value
                        };
            ExecStoredProc("wwp.SP_DB2_InformalAssessment_Update", parms);
        }

        public virtual void SP_DB2_ReassignLFCaseManager(decimal? pinNumber, string mFUserId, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["MFUserId"]   = mFUserId   ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };
            ExecStoredProc("wwp.SP_DB2_ReassignLFCaseManager", parms);
        }

        public virtual IList<USP_ParticipantbyWorker_ProgramStatus_Result> USP_ParticipantbyWorker_ProgramStatus(string wAMSId, string entsecAgencyCode, string program)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["WAMSId"]           = wAMSId           ?? (object) DBNull.Value,
                            ["EntsecAgencyCode"] = entsecAgencyCode ?? (object) DBNull.Value,
                            ["Program"]          = program          ?? (object) DBNull.Value
                        };
            var rs = ExecStoredProc<USP_ParticipantbyWorker_ProgramStatus_Result>("wwp.USP_ParticipantbyWorker_ProgramStatus", parms);

            return rs;
        }

        public virtual void SP_Work_History_WriteBack(string schemaName, int? participantId, short? employmentSequenceNumber, string mFUserId, bool? isDeletedEmployment, bool? isNewEmployment, string computedDB2WageRateValue)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["SchemaName"]               = schemaName               ?? (object) DBNull.Value,
                            ["ParticipantId"]            = participantId            ?? (object) DBNull.Value,
                            ["EmploymentSequenceNumber"] = employmentSequenceNumber ?? (object) DBNull.Value,
                            ["MFUserId"]                 = mFUserId                 ?? (object) DBNull.Value,
                            ["IsDeletedEmployment"]      = isDeletedEmployment      ?? (object) DBNull.Value,
                            ["IsNewEmployment"]          = isNewEmployment          ?? (object) DBNull.Value,
                            ["ComputedDB2WageRateValue"] = computedDB2WageRateValue ?? (object) DBNull.Value,
                            ["Debug"]                    = false
                        };
            ExecStoredProc("wwp.SP_Work_History_WriteBack", parms);
        }

        public virtual IList<USP_ProgramStatus_Recent_Result> USP_ProgramStatus_Recent(decimal? pinNumber, string schemaName, bool? checkT0018, int? t0018EnrolledProgramId, ObjectParameter recentStatus, ObjectParameter referralDate, ObjectParameter enrollmentDate, ObjectParameter disEnrollmemtDate, ObjectParameter enrolledProgramId)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]              = pinNumber              ?? (object) DBNull.Value,
                            ["SchemaName"]             = schemaName             ?? (object) DBNull.Value,
                            ["CheckT0018"]             = checkT0018             ?? (object) DBNull.Value,
                            ["T0018EnrolledProgramId"] = t0018EnrolledProgramId ?? (object) DBNull.Value
                        };
            var rs = ExecStoredProc<USP_ProgramStatus_Recent_Result>("wwp.USP_ProgramStatus", parms);

            return rs;
        }

        public virtual IList<SP_DB2_RFAs_Result> SP_DB2_RFAs(decimal? pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };
            var rs = ExecStoredProc<SP_DB2_RFAs_Result>("wwp.SP_DB2_RFAs", parms);

            return rs;
        }

        public virtual IList<SP_MostRecentFEPFromDB2_Result> SP_MostRecentFEPFromDB2(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_MostRecentFEPFromDB2_Result>("wwp.SP_MostRecentFEPFromDB2", parms);

            return rs;
        }

        public virtual void SP_DB2_T0532_Update(decimal? pinNumber, string mFWorkerId, string programCode, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]   = pinNumber   ?? (object) DBNull.Value,
                            ["MFWorkerId"]  = mFWorkerId  ?? (object) DBNull.Value,
                            ["ProgramCode"] = programCode ?? (object) DBNull.Value,
                            ["SchemaName"]  = schemaName  ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_DB2_T0532_Update", parms);
        }

        public virtual IList<USP_MostRecentPrograms_Result> USP_MostRecentPrograms(decimal? pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<USP_MostRecentPrograms_Result>("wwp.USP_MostRecentPrograms", parms);

            return rs;
        }

        public virtual IList<SP_GetCARESCaseNumber_Result> SP_GetCARESCaseNumber(string pinNumber, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber  ?? (object) DBNull.Value,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_GetCARESCaseNumber_Result>("wwp.SP_GetCARESCaseNumber", parms);

            return rs;
        }

        public virtual IList<SP_DB2_PreCheck_POP_Claim_Result> SP_DB2_PreCheck_POP_Claim(string schemaName, string pinNumber, string employmentSequenceNumber)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value,

                            ["PinNumber"]                = pinNumber                ?? (object) DBNull.Value,
                            ["EmploymentSequenceNumber"] = employmentSequenceNumber ?? (object) DBNull.Value
                        };

            var rs = ExecStoredProc<SP_DB2_PreCheck_POP_Claim_Result>("wwp.SP_DB2_PreCheck_POP_Claim", parms);

            return rs;
        }

        public virtual void SP_DB2_ReassignW2CaseManager(decimal? pinNumber, string fEPid, string schemaName)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pinNumber ?? (object) DBNull.Value,
                            ["FEPid"]      = fEPid,
                            ["SchemaName"] = schemaName ?? (object) DBNull.Value
                        };

            ExecStoredProc("wwp.SP_DB2_ReassignW2CaseManager", parms);
        }

        public virtual int USP_EP_CutOver(string pinNumber, int participantId, int enrolledProgramId, string enrollmentDate, string mfUserId, string wiuid)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]         = pinNumber,
                            ["ParticipantId"]     = participantId,
                            ["EnrolledProgramId"] = enrolledProgramId,
                            ["BeginDate"]         = enrollmentDate,
                            ["MFUserId"]          = mfUserId,
                            ["ModifiedBy"]        = wiuid
                        };

            return ExecStoredProc<int>("wwp.USP_EP_CutOver", parms).FirstOrDefault();
        }
    }
}
