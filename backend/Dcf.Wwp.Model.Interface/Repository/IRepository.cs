using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRepository : IDisposable,
                                   IAbsenceReasonRepository,
                                   IAbsenceRepository,
                                   IAccommodationRepository,
                                   IActionNeededRepository,
                                   IActivityRepository,
                                   IActivityLocationTypeRepository,
                                   IActivityTypeRepository,
                                   IAgeCategoryRepository,
                                   IApplicationStatusTypeRepository,
                                   IBarrierSubtypeRepository,
                                   IBarrierTypeRepository,
                                   IBarrierSectionRepository,
                                   IBarrierAssessmentSectionRepository,
                                   IBarrierDetailRepository,
                                   IBarrierAccommodationRepository,
                                   IBarrierTypeBarrierSubTypeBridgeRepository,
                                   IBarrierDetailContactBridgeRepository,
                                   IBenefitsOfferedTypeRepository,
                                   ICertificateIssuingAuthorityRepository,
                                   IChildCareArrangementRepository,
                                   IChildRepository,
                                   IChildYouthSectionChildRepository,
                                   IChildYouthSectionRepository,
                                   IAssistanceGroupRepository,
                                   IChildYouthSupportsAssessmentSectionRepository,
                                   ICityRepository,
                                   ICompletionReasonRepository,
                                   IContactRepository,
                                   IContactIntervalRepository,
                                   IContactTitleTypeRepository,
                                   IConvictionRepository,
                                   IConvictionTypeRepository,
                                   ICountryRepository,
                                   ICourtDateRepository,
                                   IContractorRepository,
                                   ICwwRepository,
                                   ICountyAndTribeRepository,
                                   IDegreeTypeRepository,
                                   IDeleteReasonRepository,
                                   IDeleteReasonByRepeaterRepository,
                                   IDisabledPopulationTypeRepository,
                                   IEARequestParticipantBridgeRepository,
                                   IEducationExamRepository,
                                   IEducationAssessmentSectionRepository,
                                   IEducationSectionRepository,
                                   IEligibilityRepository,
                                   IEmployerOfRecordInformationRepository,
                                   IEmployerOfRecordTypeRepository,
                                   IEmploymentProgramTypeRepository,
                                   IEmploymentInformationJobDutiesDetailsBridgeRepository,
                                   IEmploymentInformationBenefitsOfferedTypeBridgeRepository,
                                   IEmploymentPreventionTypeRepository,
                                   IEmploymentRepository,
                                   IEmploymentStatusTypeRepository,
                                   IEnrolledProgramRepository,
                                   IExamPassTypeRepository,
                                   IExamResultRepository,
                                   IExamSubjectTypeRepository,
                                   IExamTypeRepository,
                                   IElevatedAccessReasonRepository,
                                   IElevatedAccessRepository,
                                   IFamilyBarriersAssessmentSectionRepository,
                                   IFamilyBarriersDetailRepository,
                                   IFamilyBarriersSectionRepository,
                                   IFamilyMemberRepository,
                                   IFeatureURLRepository,
                                   IFieldDataRepository,
                                   IFormalAssessmentRepository,
                                   IGoalRepository,
                                   IGoalTypeRepository,
                                   IGenderTypeRepository,
                                   IHistoryRepository,
                                   IHousingAssessmentSectionRepository,
                                   IHousingHistoryRepository,
                                   IHousingSectionRepository,
                                   IHousingSituationRepository,
                                   IInformalAssessmentRepository,
                                   IIntervalTypeRepository,
                                   IInvolvedWorkProgramRepository,
                                   IJobDutyDetailRepository,
                                   IJobFoundMethodRepository,
                                   IJobTypeRepository,
                                   IKnownLanguageRepository,
                                   ILanguageAssessmentSectionRepository,
                                   ILanguageRepository,
                                   ILanguageSectionRepository,
                                   ILeavingReasonRepository,
                                   ILegalIssuesAssessmentSectionRepository,
                                   ILegalIssuesSectionRepository,
                                   ILicenseTypeRepository,
                                   IMilitaryBranchRepository,
                                   IMilitaryDischargeTypeRepository,
                                   IMilitaryRankRepository,
                                   IMilitaryTrainingAssessmentSectionRepository,
                                   IMilitaryTrainingSectionRepository,
                                   INonCustodialCaretakerRepository,
                                   INonCustodialChildRepository,
                                   INonCustodialParentRelationshipRepository,
                                   INonCustodialParentsAssessmentSectionRepository,
                                   INonCustodialParentReferralSectionRepository,
                                   INonCustodialParentsReferralAssessmentSectionRepository,
                                   INonCustodialParentSectionRepository,
                                   INonCustodialReferralChildRepository,
                                   INonCustodialReferralParentRepository,
                                   IOfficeRepository,
                                   IOrganizationRepository,
                                   IOfficeTransferRepository,
                                   IOtherJobInformationRepository,
                                   IParticipantRepository,
                                   IParticipantEnrolledProgramRepository,
                                   IPendingChargeRepository,
                                   IPolarLookupRepository,
                                   IPopulationTypeRepository,
                                   IPostSecondaryCollegeRepository,
                                   IPostSecondaryDegreeRepository,
                                   IPostSecondaryEducationRepository,
                                   IPostSecondaryLicenseRepository,
                                   IReferralContactIntervalRepository,
                                   IRelationshipRepository,
                                   IRequestForAssistanceRepository,
                                   IRequestForAssistanceRuleReasonRepository,
                                   IRequestForAssistanceStatusRepository,
                                   IRequestForAssistancePopulationTypeBridgeRepository,
                                   IRuleReasonRepository,
                                   ISchoolCollegeEstablishmentRepository,
                                   ISchoolGradeLevelRepository,
                                   ISchoolGraduationStatusRepository,
                                   ISecurityRepository,
                                   IStateRepository,
                                   ITransactionRepository,
                                   ITransportationAssessmentSectionRepository,
                                   ITransportationSectionMethodBridgeRepository,
                                   ITransportationSectionRepository,
                                   ITransportationTypeRepository,
                                   IWageHourRepository,
                                   IWageHourWageTypeBridgeRepository,
                                   IWageHourHistoryRepository,
                                   IWorkHistorySectionEmploymentPreventionTypeBridgeRepository,
                                   IWageHourHistoryWageTypeBridgeRepository,
                                   IWorkerTaskListRepository,
                                   IJobSectorRepository,
                                   IWageTypeRepository,
                                   ISymptomRepository,
                                   IPostSecondaryEducationAssessmentSectionRepository,
                                   ITimelimitRepository,
                                   IExtensionRepository,
                                   IWorkHistoryAssessmentSectionRepository,
                                   IWorkingHistorySectionRepository,
                                   IWorkProgramRepository,
                                   IWorkProgramAssessmentSectionRepository,
                                   IWorkProgramSectionRepository,
                                   IWorkProgramStatusRepository,
                                   IT0459_IN_W2_LIMITSRepository,
                                   IT0460_IN_W2_EXTRepository,
                                   IT0754_LTR_RQST_Repository,
                                   IWorkerRepository,
                                   IWpGeoAreaRepository,
                                   ISplTypeRepository,
                                   INrsTypeRepository,
                                   IContractAreaRepository,
                                   ISuffixTypeRepository,
                                   IAliasTypeRepository,
                                   ISSNTypeRepository,
                                   IOtherDemographicRepository,
                                   IParticipantContactInfoRepository,
                                   IAkaRepository,
                                   IConfidentialPinInformationRepository,
                                   IAlternateMailingAddressRepository,
                                   ISupportiveServiceTypeRepository

    {
        // DevOps
        string Server   { get; }
        string Database { get; }
        string UserId   { get; }
        string Pass     { get; }

        bool IsRowVersionStillCurrent<T>(T model, byte[] usersRowVersion) where T : class, ICloneable, ICommonModel;
        bool HasChanged<T>(T               model) where T : class, ICloneable, ICommonModel;
        void ResetContext();
        bool SaveIfChanged<T>(T       model, string user) where T : class, ICloneable, ICommonModel;
        bool SaveIfChanged<T>(T       model, string user, DateTime modifiedDate) where T : class, ICloneable, ICommonModel;
        void StartChangeTracking<T>(T model) where T : class, ICloneable, ICommonModel;
        void ResetChangeTracking<T>(T model) where T : class, ICloneable, ICommonModel;
        T    GetClonedOriginal<T>(T   model) where T : class, ICloneable, ICommonModel;

        ISP_GetCARESCaseNumber_Result GetCARESCaseNumber(string pin);
        //IQueryable<T> GetAsQueryable<T>() where T : class;
        //IQueryable<T> GetAsQueryableAsNoTracking<T>() where T : class;
    }

    public interface ICommonRepo
    {
        Boolean          Attach<T>(T                 model) where T : class;
        Boolean          Dettach<T>(T                model) where T : class;
        DbEntityEntry<T> GetEntityEntry<T>(T         model) where T : class;
        void             Save(bool                   refreshOnConcurrencyException = false);
        Task<Int32>      SaveAsync(CancellationToken token                         = default(CancellationToken));
    }
}
