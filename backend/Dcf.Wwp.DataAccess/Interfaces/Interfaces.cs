using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Model;

namespace Dcf.Wwp.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        int          Commit();
        CommitStatus CommitWithStatus();
        Task<int>    CommitAsync();
        void         RollBack();
        void         SetEntityModified<T>(int id) where T : class;
    }

    // Listed alphabetically in the descendants

    public interface IAbsenceRepository : IRepository<Absence>
    {
    }

    public interface IActionAssigneeRepository : IRepository<ActionAssignee>
    {
    }

    public interface IActionItemRepository : IRepository<ActionItem>
    {
    }

    public interface IActionNeededRepository : IRepository<ActionNeeded>
    {
    }

    public interface IActionNeededPageRepository : IRepository<ActionNeededPage>
    {
    }

    public interface IActionNeededTaskRepository : IRepository<ActionNeededTask>
    {
    }

    public interface IActionPriorityRepository : IRepository<ActionPriority>
    {
    }

    public interface IActivityRepository : IRepository<Activity>
    {
    }

    public interface IActivityCompletionReasonRepository : IRepository<ActivityCompletionReason>
    {
    }

    public interface IActivityContactBridgeRepository : IRepository<ActivityContactBridge>
    {
    }

    public interface IActivityLocationRepository : IRepository<ActivityLocation>
    {
    }

    public interface IActivityScheduleRepository : IRepository<ActivitySchedule>
    {
    }

    public interface IActivityScheduleFrequencyBridgeRepository : IRepository<ActivityScheduleFrequencyBridge>
    {
    }

    public interface IActivityTypeRepository : IRepository<ActivityType>
    {
    }

    public interface IAfterPullDownWeeklyBatchDetailsRepository : IRepository<AfterPullDownWeeklyBatchDetails>
    {
    }

    public interface IAuxiliaryRepository : IRepository<Auxiliary>
    {
    }

    public interface IAuxiliaryReasonRepository : IRepository<AuxiliaryReason>
    {
    }

    public interface IAuxiliaryStatusRepository : IRepository<AuxiliaryStatus>
    {
    }

    public interface IAuxiliaryStatusTypeRepository : IRepository<AuxiliaryStatusType>
    {
    }

    public interface ICareerAssessmentElementBridgeRepository : IRepository<CareerAssessmentElementBridge>
    {
    }

    public interface ICareerAssessmentRepository : IRepository<CareerAssessment>
    {
    }

    public interface ICFParticipationEntryRepository : IRepository<CFParticipationEntry>
    {
    }

    public interface ICityRepository : IRepository<City>
    {
    }

    public interface IContactRepository : IRepository<Contact>
    {
    }

    public interface IContactTitleTypeRepository : IRepository<ContactTitleType>
    {
    }


    public interface IContractAreaRepository : IRepository<ContractArea>
    {
    }

    public interface ICountryRepository : IRepository<Country>
    {
    }

    public interface ICountyAndTribeRepository : IRepository<CountyAndTribe>
    {
    }

    public interface IDrugScreeningRepository : IRepository<DrugScreening>
    {
    }

    public interface IDrugScreeningStatusRepository : IRepository<DrugScreeningStatus>
    {
    }

    public interface IDrugScreeningStatusTypeRepository : IRepository<DrugScreeningStatusType>
    {
    }

    public interface IDocumentRepository : IRepository<Document>
    {
    }

    public interface IEAAlternateMailingAddressRepository : IRepository<EAAlternateMailingAddress>
    {
    }

    public interface IEAAssetsRepository : IRepository<EAAssets>
    {
    }

    public interface IEACommentRepository : IRepository<EAComment>
    {
    }

    public interface IEACommentTypeRepository : IRepository<EACommentType>
    {
    }

    public interface IEACommentTypeBridgeRepository : IRepository<EACommentTypeBridge>
    {
    }

    public interface IEAEmergencyTypeRepository : IRepository<EAEmergencyType>
    {
    }

    public interface IEAEmergencyTypeReasonBridgeRepository : IRepository<EAEmergencyTypeReasonBridge>
    {
    }

    public interface IEAEmergencyTypeReasonRepository : IRepository<EAEmergencyTypeReason>
    {
    }

    public interface IEAEnergyCrisisRepository : IRepository<EAEnergyCrisis>
    {
    }

    public interface IEAFinancialNeedRepository : IRepository<EAFinancialNeed>
    {
    }

    public interface IEAHomelessnessRepository : IRepository<EAHomelessness>
    {
    }

    public interface IEAHouseHoldIncomeRepository : IRepository<EAHouseHoldIncome>
    {
    }

    public interface IEAImpendingHomelessnessRepository : IRepository<EAImpendingHomelessness>
    {
    }

    public interface IEAIndividualTypeRepository : IRepository<EAIndividualType>
    {
    }

    public interface IEAIPVRepository : IRepository<EAIPV>
    {
    }

    public interface IEAIPVOccurrenceRepository : IRepository<EAIPVOccurrence>
    {
    }

    public interface IEAIPVReasonRepository : IRepository<EAIPVReason>
    {
    }

    public interface IEAIPVReasonBridgeRepository : IRepository<EAIPVReasonBridge>
    {
    }

    public interface IEAIPVStatusRepository : IRepository<EAIPVStatus>
    {
    }

    public interface IEAPaymentRepository : IRepository<EAPayment>
    {
    }

    public interface IEAPaymentAmountRepository : IRepository<EAPaymentAmount>
    {
    }

    public interface IEARelationshipTypeRepository : IRepository<EARelationshipType>
    {
    }

    public interface IEARequestContactInfoRepository : IRepository<EARequestContactInfo>
    {
    }

    public interface IEARequestEmergencyTypeBridgeRepository : IRepository<EARequestEmergencyTypeBridge>
    {
    }

    public interface IEARequestParticipantBridgeRepository : IRepository<EARequestParticipantBridge>
    {
    }

    public interface IEARequestRepository : IRepository<EARequest>
    {
    }

    public interface IEARequestStatusRepository : IRepository<EARequestStatus>
    {
    }

    public interface IEARequestStatusReasonRepository : IRepository<EARequestStatusReason>
    {
    }

    public interface IEASSNExemptTypeRepository : IRepository<EASSNExemptType>
    {
    }

    public interface IEAStatusRepository : IRepository<EAStatus>
    {
    }

    public interface IEAStatusReasonRepository : IRepository<EAStatusReason>
    {
    }

    public interface IEAVehiclesRepository : IRepository<EAVehicles>
    {
    }

    public interface IEAVerificationTypeBridgeRepository : IRepository<EAVerificationTypeBridge>
    {
    }

    public interface IEAVerificationTypeRepository : IRepository<EAVerificationType>
    {
    }

    public interface IElementRepository : IRepository<Element>
    {
    }

    public interface IEligibilityByFPLRepository : IRepository<EligibilityByFPL>
    {
    }

    public interface IEmployabilityPlanRepository : IRepository<EmployabilityPlan>
    {
    }

    public interface IEmployabilityPlanActivityBridgeRepository : IRepository<EmployabilityPlanActivityBridge>
    {
    }

    public interface IEmployabilityPlanGoalBridgeRepository : IRepository<EmployabilityPlanGoalBridge>
    {
    }

    public interface IEmployabilityPlanEmploymentInfoBridgeRepository : IRepository<EmployabilityPlanEmploymentInfoBridge>
    {
    }

    public interface IEmploymentInformationRepository : IRepository<EmploymentInformation>
    {
    }

    public interface IEmploymentVerificationRepository : IRepository<EmploymentVerification>
    {
    }

    public interface IEmployabilityPlanStatusTypeRepository : IRepository<EmployabilityPlanStatusType>
    {
    }

    public interface IEnrolledProgramRepository : IRepository<EnrolledProgram>
    {
    }

    public interface IEnrolledProgramValidityRepository : IRepository<EnrolledProgramValidity>
    {
    }

    public interface IEnrolledProgramActivityCompletionReasonBridgeRepository : IRepository<EnrolledProgramActivityCompletionReasonBridge>
    {
    }

    public interface IEnrolledProgramEPActivityTypeBridgeRepository : IRepository<EnrolledProgramEPActivityTypeBridge>
    {
    }

    public interface IEnrolledProgramJobTypeBridgeRepository : IRepository<EnrolledProgramJobTypeBridge>
    {
    }

    public interface IEnrolledProgramParticipationStatusTypeBridgeRepository : IRepository<EnrolledProgramParticipationStatusTypeBridge>
    {
    }

    public interface IEnrolledProgramPinCommentTypeBridgeRepository : IRepository<EnrolledProgramPinCommentTypeBridge>
    {
    }

    public interface IFrequencyRepository : IRepository<Frequency>
    {
    }

    public interface IFrequencyTypeRepository : IRepository<FrequencyType>
    {
    }

    public interface IGoalRepository : IRepository<Goal>
    {
    }

    public interface IGoalEndReasonRepository : IRepository<GoalEndReason>
    {
    }

    public interface IGoalStepRepository : IRepository<GoalStep>
    {
    }

    public interface IGoalTypeRepository : IRepository<GoalType>
    {
    }

    public interface IGoodCauseDeniedReasonRepository : IRepository<GoodCauseDeniedReason>
    {
    }

    public interface IGoodCauseGrantedReasonRepository : IRepository<GoodCauseGrantedReason>
    {
    }

    public interface IJobReadinessRepository : IRepository<JobReadiness>
    {
    }

    public interface IJobTypeRepository : IRepository<JobType>
    {
    }

    public interface IJRApplicationInfoRepository : IRepository<JRApplicationInfo>
    {
    }

    public interface IJRContactInfoRepository : IRepository<JRContactInfo>
    {
    }

    public interface IJRHistoryInfoRepository : IRepository<JRHistoryInfo>
    {
    }

    public interface IJRInterviewInfoRepository : IRepository<JRInterviewInfo>
    {
    }

    public interface IJRWorkPreferencesRepository : IRepository<JRWorkPreferences>
    {
    }

    public interface IJRWorkPreferenceShiftBridgeRepository : IRepository<JRWorkPreferenceShiftBridge>
    {
    }

    public interface IJRWorkShiftRepository : IRepository<JRWorkShift>
    {
    }

    public interface INonParticipationReasonRepository : IRepository<NonParticipationReason>
    {
    }

    public interface INonSelfDirectedActivityRepository : IRepository<NonSelfDirectedActivity>
    {
    }

    public interface IOfficeRepository : IRepository<Office>
    {
    }

    public interface IOrganizationRepository : IRepository<Organization>
    {
    }

    public interface IOrganizationInformationRepository : IRepository<OrganizationInformation>
    {
    }

    public interface IOrganizationLocationRepository : IRepository<OrganizationLocation>
    {
    }

    public interface IOverPaymentRepository : IRepository<OverPayment>
    {
    }

    public interface IParticipantRepository : IRepository<Participant>
    {
    }

    public interface IParticipantEnrolledProgramRepository : IRepository<ParticipantEnrolledProgram>
    {
    }

    public interface IParticipantEnrolledProgramCutOverBridgeRepository : IRepository<ParticipantEnrolledProgramCutOverBridge>
    {
    }

    public interface IParticipantPlacementRepository : IRepository<ParticipantPlacement>
    {
    }

    public interface IParticipationEntryRepository : IRepository<ParticipationEntry>
    {
    }

    public interface IParticipationEntryHistoryRepository : IRepository<ParticipationEntryHistory>
    {
    }

    public interface IParticipationMakeUpEntryRepository : IRepository<ParticipationMakeUpEntry>
    {
    }

    public interface IParticipantPaymentHistoryRepository : IRepository<ParticipantPaymentHistory>
    {
    }

    public interface IParticipationPeriodLookUpRepository : IRepository<ParticipationPeriodLookUp>
    {
    }

    public interface IParticipationPeriodSummaryRepository : IRepository<ParticipationPeriodSummary>
    {
    }

    public interface IParticipationStatusRepository : IRepository<ParticipationStatus>
    {
    }

    public interface IStatusRepository : IRepository<ParticipationStatusType>
    {
    }

    public interface IPCCTBridgeRepository : IRepository<PCCTBridge>
    {
    }

    public interface IPinCommentRepository : IRepository<PinComment>
    {
    }

    public interface IPinCommentTypeRepository : IRepository<PinCommentType>
    {
    }

    public interface IPlacementTypeRepository : IRepository<PlacementType>
    {
    }

    public interface IPlanRepository : IRepository<Plan>
    {
    }

    public interface IPlanSectionRepository : IRepository<PlanSection>
    {
    }

    public interface IPlanSectionResourceRepository : IRepository<PlanSectionResource>
    {
    }

    public interface IPlanSectionTypeRepository : IRepository<PlanSectionType>
    {
    }

    public interface IPlanStatusTypeRepository : IRepository<PlanStatusType>
    {
    }

    public interface IPlanTypeRepository : IRepository<PlanType>
    {
    }

    public interface IPOPClaimActivityBridgeRepository : IRepository<POPClaimActivityBridge>
    {
    }

    public interface IPOPClaimRepository : IRepository<POPClaim>
    {
    }

    public interface IPOPClaimEmploymentBridgeRepository : IRepository<POPClaimEmploymentBridge>
    {
    }

    public interface IPOPClaimHighWageRepository : IRepository<POPClaimHighWage>
    {
    }

    public interface IPOPClaimStatusRepository : IRepository<POPClaimStatus>
    {
    }

    public interface IPOPClaimStatusTypeRepository : IRepository<POPClaimStatusType>
    {
    }

    public interface IPOPClaimTypeRepository : IRepository<POPClaimType>
    {
    }

    public interface IPullDownDateRepository : IRepository<PullDownDate>
    {
    }

    public interface IRuleReasonRepository : IRepository<RuleReason>
    {
    }

    public interface ISimulatedDateRepository : IRepository<SimulatedDate>
    {
    }

    public interface ISpecialInitiativeRepository : IRepository<SpecialInitiative>
    {
    }

    public interface IStateRepository : IRepository<State>
    {
    }

    public interface ISupportiveServiceRepository : IRepository<SupportiveService>
    {
    }

    public interface ISupportiveServiceTypeRepository : IRepository<SupportiveServiceType>
    {
    }

    public interface ITimeLimitRepository : IRepository<TimeLimit>
    {
    }

    public interface ITransactionRepository : IRepository<Transaction>
    {
    }

    public interface ITransactionTypeRepository : IRepository<TransactionType>
    {
    }

    public interface IWageHourRepository : IRepository<WageHour>
    {
    }

    public interface IWeeklyHoursWorkedRepository : IRepository<WeeklyHoursWorked>
    {
    }

    public interface IWorkerRepository : IRepository<Worker>
    {
    }

    public interface IWorkerContactInfoRepository : IRepository<WorkerContactInfo>
    {
    }

    public interface IWorkerTaskCategoryRepository : IRepository<WorkerTaskCategory>
    {
    }

    public interface IWorkerTaskListRepository : IRepository<WorkerTaskList>
    {
    }

    public interface IWorkerTaskStatusRepository : IRepository<WorkerTaskStatus>
    {
    }
}
