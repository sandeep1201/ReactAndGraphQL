using System.Data.Entity;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Contexts
{
    /// <summary>
    ///     DbContext for Employability Plans
    /// </summary>
    public class EPContext : BaseDbContext
    {
        #region Properties

        public DbSet<Activity>                        Activities                       { get; set; }
        public DbSet<ActivityContactBridge>           ActivityContactBridges           { get; set; }
        public DbSet<ActivityLocation>                ActivityLocations                { get; set; }
        public DbSet<ActivitySchedule>                ActivitySchedules                { get; set; }
        public DbSet<ActivityScheduleFrequencyBridge> ActivityScheduleFrequencyBridges { get; set; }
        public DbSet<ActivityType>                    ActivityTypes                    { get; set; }
        public DbSet<City>                            Cities                           { get; set; }
        public DbSet<Contact>                         Contacts                         { get; set; }
        public DbSet<ContactTitleType>                ContactTitleTypes                { get; set; }
        public DbSet<ContractArea>                    ContractAreas                    { get; set; }
        public DbSet<Country>                         Countries                        { get; set; }
        public DbSet<EmployabilityPlan>               EmployabilityPlans               { get; set; }
        public DbSet<EmployabilityPlanActivityBridge> EmployabilityPlanActivityBridges { get; set; }
        public DbSet<EmployabilityPlanGoalBridge>     EmployabilityPlanGoalBridges     { get; set; }
        public DbSet<EnrolledProgram>                 EnrolledPrograms                 { get; set; }
        public DbSet<Frequency>                       Frequencies                      { get; set; }
        public DbSet<FrequencyType>                   FrequencyTypes                   { get; set; }
        public DbSet<Goal>                            Goals                            { get; set; }
        public DbSet<GoalStep>                        GoalSteps                        { get; set; }
        public DbSet<GoalType>                        GoalTypes                        { get; set; }
        public DbSet<Organization>                    Organizations                    { get; set; }
        public DbSet<Participant>                     Participants                     { get; set; }
        public DbSet<ParticipationStatus>             ParticipationStatuses            { get; set; }
        public DbSet<NonSelfDirectedActivity>         NonSelfDirectedActivities        { get; set; }
        public DbSet<State>                           States                           { get; set; }
        public DbSet<SimulatedDate>                   SimulatedDates                   { get; set; }
        public DbSet<SupportiveService>               SupportiveServices               { get; set; }
        public DbSet<SupportiveServiceType>           SupportiveServiceTypes           { get; set; }
        public DbSet<Worker>                          Workers                          { get; set; }

        //Reference Data Context

        public DbSet<ActivityCompletionReason>                      ActivityCompletionReasons                      { get; set; }
        public DbSet<AuxiliaryReason>                               AuxiliaryReasons                               { get; set; }
        public DbSet<AuxiliaryStatusType>                           AuxiliaryStatusTypes                           { get; set; }
        public DbSet<DrugScreeningStatusType>                       DrugScreeningStatusTypes                       { get; set; }
        public DbSet<EACommentType>                                 EaCommentTypes                                 { get; set; }
        public DbSet<EAEmergencyType>                               EaEmergencyTypes                               { get; set; }
        public DbSet<EAEmergencyTypeReasonBridge>                   EaEmergencyTypeReasonBridges                   { get; set; }
        public DbSet<EAIndividualType>                              EaIndividualTypes                              { get; set; }
        public DbSet<EAApplicationInitiationMethodLookUp>           EaApplicationInitiationMethodLookUps           { get; set; }
        public DbSet<EAFinancialNeedType>                           EaFinancialNeedTypes                           { get; set; }
        public DbSet<EAIPVOccurrence>                               EaIpvOccurrences                               { get; set; }
        public DbSet<EAIPVReason>                                   EaIpvReasons                                   { get; set; }
        public DbSet<EAIPVStatus>                                   EaIpvStatuses                                  { get; set; }
        public DbSet<EARelationshipTypeBridge>                      EaRelationshipTypeBridges                      { get; set; }
        public DbSet<EASSNExemptType>                               EaSsnExemptTypes                               { get; set; }
        public DbSet<EAStatus>                                      EaStatuses                                     { get; set; }
        public DbSet<EAStatusReason>                                EaStatusReasons                                { get; set; }
        public DbSet<EAVerificationTypeBridge>                      EaVerificationTypeBridges                      { get; set; }
        public DbSet<Element>                                       Elements                                       { get; set; }
        public DbSet<EmployabilityPlanStatusType>                   EmployabilityPlanStatusTypes                   { get; set; }
        public DbSet<EnrolledProgramActivityCompletionReasonBridge> EnrolledProgramActivityCompletionReasonBridges { get; set; }
        public DbSet<EnrolledProgramGCDReasonBridge>                EnrolledProgramGcdReasonBridges                { get; set; }
        public DbSet<EnrolledProgramGCGReasonBridge>                EnrolledProgramGcgReasonBridges                { get; set; }
        public DbSet<EnrolledProgramNPReasonBridge>                 EnrolledProgramNpReasonBridges                 { get; set; }
        public DbSet<EnrolledProgramParticipationStatusTypeBridge>  EnrolledProgramParticipationStatusTypeBridges  { get; set; }
        public DbSet<EnrolledProgramPinCommentTypeBridge>           EnrolledProgramPinCommentTypeBridges           { get; set; }
        public DbSet<EnrolledProgramValidity>                       EnrolledProgramValidities                      { get; set; }
        public DbSet<GoalEndReason>                                 GoalEndReasons                                 { get; set; }
        public DbSet<GoodCauseDeniedReason>                         GoodCauseDeniedReason                          { get; set; }
        public DbSet<GoodCauseGrantedReason>                        GoodCauseGrantedReason                         { get; set; }
        public DbSet<JRWorkShift>                                   JrWorkShifts                                   { get; set; }
        public DbSet<NonParticipationReason>                        NonParticipationReasons                        { get; set; }
        public DbSet<ParticipationPeriodLookUp>                     ParticipationPeriodLookUps                     { get; set; }
        public DbSet<ParticipationStatusType>                       ParticipationStatusType                        { get; set; }
        public DbSet<PlanSectionType>                               PlanSectionTypes                               { get; set; }
        public DbSet<PlanStatusType>                                PlanStatusTypes                                { get; set; }
        public DbSet<PlanType>                                      PlanTypes                                      { get; set; }
        public DbSet<POPClaimHighWage>                              POPClaimHighWages                              { get; set; }
        public DbSet<POPClaimStatusType>                            POPClaimStatusTypes                            { get; set; }
        public DbSet<POPClaimType>                                  POPClaimTypes                                  { get; set; }
        public DbSet<PullDownDate>                                  PullDownDates                                  { get; set; }
        public DbSet<WorkerTaskCategory>                            WorkerTaskCategories                           { get; set; }
        public DbSet<WorkerTaskStatus>                              WorkerTaskStatuses                             { get; set; }

        #endregion

        #region Methods

        public EPContext(string connString) : base(connString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _log.Debug($"{GetType().Name}.OnModelCreating()");

            // ancestor contains all of the common logic
            // so just go up the food chain...

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
