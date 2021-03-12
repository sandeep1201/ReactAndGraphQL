using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.Delegates;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.UnitTest.Infrastructure
{
    public class MockPhase1Repository : IRepository
    {
        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IAbsenceReason> AbsenceReasons()
        {
            throw new NotImplementedException();
        }

        public virtual IAbsence NewAbsence(IEmploymentInformation parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IAccommodation> Accommodations()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActionNeeded> ActionNeededsByParticipantId(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeededTask NewActionNeededTask(IActionNeeded actionNeeded, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeededTask ActionNeededTaskById(int taskId)
        {
            throw new NotImplementedException();
        }

        public virtual void ResetNoActionNeededs(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeeded ActionNeededById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeeded ActionNeededByParticipantIdAndPageCode(int participantId, string pageCode)
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeeded NewActionNeeded(int participantId, int pageId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActionAssignee> ActionAssignees()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActionPriority> ActionPriorities(bool orderDesc = false)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActionNeededPage> ActionNeededPages()
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeededPage ActionNeededPageById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IActionNeededPage ActionNeededPageByCode(string code)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActionItem> ActionItemsForPage(string pageCode)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActionNeededPageActionItemBridge> ActionNeededPageActionItems()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActivity> WhereActivites(Expression<Func<IActivity, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActivity> AllActiviesByEP(int epId)
        {
            throw new NotImplementedException();
        }

        public virtual IActivity NewActivity(int epId)
        {
            throw new NotImplementedException();
        }

        public virtual INonSelfDirectedActivity NewSelfDirectedActivity(IActivity activity, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteActivity(int id)
        {
            throw new NotImplementedException();
        }

        public virtual string GetActivityLocationName(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IActivity GetActivity(int id)
        {
            throw new NotImplementedException();
        }

        public virtual INonSelfDirectedActivity GetSelfDirectedActivity(int activityId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActivitySchedule> WhereActivitySchedules(Expression<Func<IActivitySchedule, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActivityScheduleFrequencyBridge> WhereActivityScheduleFrequencies(Expression<Func<IActivityScheduleFrequencyBridge, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IActivitySchedule NewActivitySchedule(int activityId)
        {
            throw new NotImplementedException();
        }

        public virtual IActivitySchedule GetActivitySchedule(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IActivityScheduleFrequencyBridge NewActivityScheduleFrequencyBridge(IActivitySchedule schedule)
        {
            throw new NotImplementedException();
        }

        public virtual IActivityScheduleFrequencyBridge GetActivityScheduleFrequencyBridge(int id)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteActivitySchedules(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteActivityScheduleFrequencies(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteSelfDirectedActivity(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IFrequencyType> FrequencyTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IFrequency> WeeklyFrequencies()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IFrequency> MonthlyFrequencies()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IActivityLocation> ActivityLocationTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEnrolledProgramEPActivityTypeBridge> ActivityTypes(string options)
        {
            throw new NotImplementedException();
        }

        public virtual int AgeCategoryByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IAgeCategory> AllAgeCategories()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IApplicationStatusType> ApplicationStatusTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IApplicationStatusType ApplicationStatusTypeById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IBarrierSubtype> BarrierSubtypeByBarriertype(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IBarrierType> BarrierTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierType BarrierTypeById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierSection NewBarrierSection(int participantId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierAssessmentSection NewBarrierAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierDetail BarrierDetailById(int? barrierId)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierDetail NewBarrierDetailInfo(IParticipant participant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IBarrierDetail> BarrierDetailsByParticipantId(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierAccommodation NewBarrierAccommodation(IBarrierDetail parentObject)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierTypeBarrierSubTypeBridge NewBarrierTypeBarrierSubTypeBridge(IBarrierDetail parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IBarrierDetailContactBridge NewBarrierDetailContactBridge(IBarrierDetail parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IBenefitsOfferedType> BenefitsOfferedTypes()
        {
            throw new NotImplementedException();
        }

        public virtual ICertificateIssuingAuthority CertificateIssuerById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICertificateIssuingAuthority> CertificateIssuersIssuingAuthorities()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICertificateIssuingAuthority> AllCertificateIssuersIssuingAuthorities()
        {
            throw new NotImplementedException();
        }

        public virtual IChildCareArrangement ChildCareArrangementById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IChildCareArrangement> ChildCareArrangements()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IChildCareArrangement> AllChildCareArrangements()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IChild> AllChildrenForParticipant(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IChild NewChild()
        {
            throw new NotImplementedException();
        }

        public virtual IChildYouthSectionChild NewChildYouthSectionChild(IChildYouthSection childYouthSection)
        {
            throw new NotImplementedException();
        }

        public virtual IChildYouthSection NewChildYouthSection(int participantId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<AssistanceGroupMember> ParticipantAssistanceGroupByPin(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IChildYouthSupportsAssessmentSection NewChildYouthSupportsAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICity> GetCities()
        {
            throw new NotImplementedException();
        }

        public virtual ICity CityByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual ICity CityByGooglePlaceId(string placeId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICity> GetCitiesByIds(IEnumerable<int> cityIds)
        {
            throw new NotImplementedException();
        }

        public virtual ICity NewCity(IEmploymentInformation employment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ICity NewCity(ISchoolCollegeEstablishment parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ICity GetOrCreateCity(IGoogleLocation googleLocation = null, DetailsProvider getDetails = null, LatLongProvider getLatLong = null, string user = null, IFinalistAddress finalistAddress = null, bool isClientReg = false)
        {
            throw new NotImplementedException();
        }

        public virtual string CityDisplayName(ICity city)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICompletionReason> GetCompletionReasonsforEnrolledProgram(string programCd)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICompletionReason> GetCompletionReasonsforEnrolledProgram(Expression<Func<ICompletionReason, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IContact ContactById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IContact> AllContactsByParticipant(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IContact NewContact(IParticipant participant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual bool DeleteContact(int id, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IContactInterval> ContactIntervals()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IContactTitleType> ContactTitleTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IContactTitleType ContactTitleById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IConviction NewConviction(ILegalIssuesSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteConviction(IConviction Conviction)
        {
            throw new NotImplementedException();
        }

        public virtual IConvictionType ConvictionTypeById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IConvictionType> ConvictionTypes()
        {
            throw new NotImplementedException();
        }

        public virtual ICountry CountryByName(string countryName)
        {
            throw new NotImplementedException();
        }

        public virtual ICountry NewCountry(IState parentobject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ICountry NewCountry(ICity parentobject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICountry> Countries()
        {
            throw new NotImplementedException();
        }

        public virtual ICourtDate NewCourtDate(ILegalIssuesSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteCourtDate(ICourtDate courtDate)
        {
            throw new NotImplementedException();
        }

        public virtual List<ICurrentChild> CwwCurrentChildren(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual ISP_CWWChildCareEligibiltyStatus_Result CwwChildCareEligibiltyStatus(string caseNum)
        {
            throw new NotImplementedException();
        }

        public virtual List<ILearnfare> CwwLearnfare(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual List<ISocialSecurityStatus> CwwSocialSecurityStatus(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICountyAndTribe> GetCountyAndTribes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICountyAndTribe> GetTribes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICountyAndTribe> GetCounties()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ICountyAndTribe> WhereCountyAndTribe(Expression<Func<ICountyAndTribe, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual ICountyAndTribe GetCountyOrTribe(Expression<Func<ICountyAndTribe, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual ICountyAndTribe GetCountyOrTribeById(long id)
        {
            throw new NotImplementedException();
        }

        public virtual IDegreeType DegreeByCode(int? degreeType)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IDegreeType> DegreeTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IDeleteReason DeleteReasonByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IDeleteReason DeleteReasonById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IDeleteReasonByRepeater> DeleteReasonsByRepeater(string repeater)
        {
            throw new NotImplementedException();
        }

        public virtual IEARequestParticipantBridge NewEaRequestParticipantBridge(IParticipant participant)
        {
            throw new NotImplementedException();
        }

        public virtual IEducationExam NewEducationExam(IParticipant participant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEducationAssessmentSection NewEducationAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEducationSection NewEducationSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasEducationSectionChanged(IEducationSection educationSection)
        {
            throw new NotImplementedException();
        }

        public virtual IEligibilityByFPL EligibilityByFPL(int householdSize)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteEoRInfo(int eorId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEmployerOfRecordType> EmployerOfRecordTypes()
        {
            throw new NotImplementedException();
        }

        public virtual int EmploymentProgramTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentInformationJobDutiesDetailsBridge NewEmploymentInformationJobDutiesDetailsBridge(IEmploymentInformation parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentInformationBenefitsOfferedTypeBridge NewJobBenefitsOfferedActionBridge(IEmploymentInformation parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEmploymentPreventionType> EmploymentPreventionTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEmploymentPreventionType> AllEmploymentPreventionTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentPreventionType EmploymentPreventionTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentInformation EmploymentByIdAsNoTracking(int? employmentId)
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentInformation EmploymentById(int? employmentId)
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentInformation NewEmploymentInfo(IParticipant participant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void SP_Work_History_WriteBack(int? participantId, short? eSeqNo, string mFUserId, bool isDeletedEmployment, bool isNewEmployment, string computedDB2WageRateValue)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteOnFailure(IEmploymentInformation employmentInformation)
        {
            throw new NotImplementedException();
        }

        public virtual bool EmploymentInfoTransactionalSave(IEmploymentInformation employmentInformation, string user, string mFUserId, string computedDB2WageRateUnit, string computedDB2WageRateValue)
        {
            throw new NotImplementedException();
        }

        public virtual void EmploymentInfoTransactionalDelete(IEmploymentInformation employmentInfo, string mFUserId, IEPEIBridge epei)
        {
            throw new NotImplementedException();
        }

        public virtual ISP_DB2_PreCheck_POP_Claim_Result PreCheckPop(string pin, short? seqNo)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IUSP_ProgramStatus_Recent_Result> GetRecentPEPForPin(decimal? pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEmploymentStatusType> EmploymentStatusTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEmploymentStatusType> AllEmploymentStatusTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEmploymentStatusType EmploymentStatusTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEnrolledProgram> EnrolledPrograms()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEnrolledProgram> NonEligibiltyEnrolledPrograms()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IEnrolledProgram> WhereEnrolledPrograms(Expression<Func<IEnrolledProgram, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IExamPassType> ExamPassTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IExamResult ExamResultById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IExamResult NewExamResult(IEducationExam parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IExamSubjectType> ExamSubjectsByExamType(string examType)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IExamSubjectType> ExamSubjectTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IExamType> ExamTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IElevatedAccessReason> ElevatedAccessReasons()
        {
            throw new NotImplementedException();
        }

        public virtual IElevatedAccess NewElevatedAccess(string user, int workerId, int participantId, int? earId, string details)
        {
            throw new NotImplementedException();
        }

        public virtual IFamilyBarriersAssessmentSection NewFamilyBarriersAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IFamilyBarriersDetail NewFamilyBarriersDetail(IFamilyBarriersSection parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IFamilyBarriersDetail FamilBarrierDetailsByDetailId(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IFamilyBarriersSection NewFamilyBarriersSection(IParticipant participant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IFamilyMember NewFamilyMember(IFamilyBarriersSection familyBarriersSection)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<string> GetFeatureUrl(string feature)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IYesNoSkipLookup> YesNoSkipLookups()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IYesNoUnknownLookup> YesNoUnknownLookups()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IYesNoRefused> AllYesNoRefusedLookups()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IYesNoRefused> YesNoRefusedLookups()
        {
            throw new NotImplementedException();
        }

        public virtual IFormalAssessment NewFormalAssessment(IBarrierDetail barrierDetail)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteFormalAssements(Expression<Func<IFormalAssessment, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IGoal> AllGoalsByEP(int epId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IGoalType> GoalTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IGenderType> GenderTypes()
        {
            throw new NotImplementedException();
        }

        public virtual string SectionHistory(string storedProcedureName, string tableName, string pin, int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IHousingAssessmentSection NewHousingAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IHousingHistory NewHousingHistory(IHousingSection parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ICurrentAddressDetails CwwCurrentAddressDetails(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IHousingSection NewHousingSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IHousingSituation HousingSituationById(int? housingSituation)
        {
            throw new NotImplementedException();
        }

        public virtual IHousingSituation OtherHousingSituation(int? housingSituation)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IHousingSituation> HousingSituations()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IHousingSituation> AllHousingSituations()
        {
            throw new NotImplementedException();
        }

        public virtual IInformalAssessment NewInformalAssessment(int participantId, bool isSubsequent, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IInformalAssessment InformalAssessmentById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual void SP_DB2_InformalAssessment_Update(decimal? pinNumber, string MFWorkerId)
        {
            throw new NotImplementedException();
        }

        public virtual IInformalAssessment GetMostRecentAssessment(IParticipant part)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IIntervalType> IntervalTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IInvolvedWorkProgram NewInvolvedWorkProgram(IWorkProgramSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteWorkProgram(IInvolvedWorkProgram workProgram)
        {
            throw new NotImplementedException();
        }

        public virtual IJobDutiesDetail JobDutyById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IJobDutiesDetail NewJobDuty(string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IJobFoundMethod> JobFoundMethods()
        {
            throw new NotImplementedException();
        }

        public virtual IJobFoundMethod JobFoundMethodByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IJobType> JobTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IJobType JobTypeById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IJobType JobTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<IJobType> GetJobTypes(Expression<Func<IJobType, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IKnownLanguage NewKnownLanguage(ILanguageSection parentSection)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteKnownLanguageById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual ILanguageAssessmentSection NewLanguageAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ILanguage> Languages()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ILanguage> AllLanguages()
        {
            throw new NotImplementedException();
        }

        public virtual ILanguageSection NewLanguageSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IJobTypeLeavingReasonBridge> LeavingReasons()
        {
            throw new NotImplementedException();
        }

        public virtual ILegalIssuesAssessmentSection NewLegalIssuesAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ILegalIssuesSection NewLegalIssuesSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ILicenseType> LicenseTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IMilitaryBranch MilitaryBranchById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IMilitaryBranch> MilitaryBranches()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IMilitaryBranch> AllMilitaryBranches()
        {
            throw new NotImplementedException();
        }

        public virtual IMilitaryDischargeType DischargeTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IMilitaryDischargeType> DischargeTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IMilitaryDischargeType> AllDischargeTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IMilitaryRank MilitaryRankById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IMilitaryRank> MilitaryRanks()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IMilitaryRank> AllMilitaryRanks()
        {
            throw new NotImplementedException();
        }

        public virtual IMilitaryTrainingAssessmentSection NewMilitaryTrainingAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IMilitaryTrainingSection NewMilitaryTrainingSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialCaretaker NewNonCustodialCaretaker(INonCustodialParentsSection section, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialChild NewNonCustodialChild(INonCustodialCaretaker caretaker, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialChild GetNonCustodialChild(int id)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialChild GetNonCustodialChild(Expression<Func<INonCustodialChild, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<INonCustodialParentRelationship> NonCustodialParentRelationships()
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialParentsAssessmentSection NewNonCustodialParentsAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialParentsReferralSection NewNonCustodialParentsReferralSection(int participantId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialParentsReferralAssessmentSection NewNonCustodialParentsReferralAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialParentsSection NewNonCustodialParentsSection(int participantId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialReferralChild NewNonCustodialReferralChild(INonCustodialReferralParent parent, string user)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialReferralChild GetNonCustodialReferralChildById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialReferralChild GetNonCustodialReferralChild(int id)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialReferralChild GetNonCustodialReferralChild(Expression<Func<INonCustodialReferralChild, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual INonCustodialReferralParent NewNonCustodialReferralParent(INonCustodialParentsReferralSection section, string user)
        {
            throw new NotImplementedException();
        }

        public virtual List<IOffice> GetOffices()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOffice> MilwaukeeOffices()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOffice> MilwaukeeOfficesByProgramCode(string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOffice> OfficesByOrganizationCode(string code)
        {
            throw new NotImplementedException();
        }

        public virtual IOffice GetOfficeByNumberAndProgram(string number, int programId)
        {
            throw new NotImplementedException();
        }

        public virtual IOffice GetOfficeByNumberAndProgramCode(int number, string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOffice> GetOfficesByCountyAndProgramCode(int countyandTribeId, string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOffice> GetOfficesByContractAreaAndProgramCode(int contractAreaId, string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual IOffice GetOfficeById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IOffice GetOfficeByNumber(int officeNumber)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOrganization> GetOrganizations()
        {
            throw new NotImplementedException();
        }

        public virtual IOrganization GetOrganizationByOfficeNumber(short? officeNumber)
        {
            throw new NotImplementedException();
        }

        public virtual IOrganization GetOrganizationByCode(string orgCode)
        {
            throw new NotImplementedException();
        }

        public virtual IAssociatedOrganization GetAssociatedOrganization(IOrganization org)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOrganization> GetOrganizationsForProgram(string programName)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IOrganization> GetOrganizationsByProgramId(string programId)
        {
            throw new NotImplementedException();
        }

        public virtual IOfficeTransfer NewOfficeTransfer(IParticipantEnrolledProgram participantEnrolledProgram, IOffice sourceOffice, IOffice destinationOffice, int? workerId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual bool hasOfficeTransfer(int? participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IOtherJobInformation OtherJobInformationById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IOtherJobInformation NewOtherJobInformation(string user)
        {
            throw new NotImplementedException();
        }

        public virtual IParticipant GetParticipant(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IParticipant GetRefreshedParticipant(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual ISP_ParticipantDetailsReturnType GetParticipantDetails(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IParticipant> GetAllParticipants()
        {
            throw new NotImplementedException();
        }

        public virtual IParticipant GetParticipantById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IParticipant GetParticipantByMciId(decimal mciId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IParticipant> GetRecentParticipantsByUser(string userId, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IParticipantEnrolledProgram> GetNonEligibilityReferrals(IWorker worker)
        {
            throw new NotImplementedException();
        }

        public virtual IParticipantEnrolledProgram GetParticantEnrollment(int pepId)
        {
            throw new NotImplementedException();
        }

        public virtual ISP_PreCheckDisenrollment_Result PreDisenrollmentErrors(decimal? pinNumer, decimal? caseNumber, int? pepId)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateDisenrollment(IParticipantEnrolledProgram pep, int? workerLoginId, string mFUserId, string authWorker, string completionReasonDetails = null, DateTime? disenrollmentDate = null, int? completionReasonId = null)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IParticipant> PariticipantsBeingTransferred(IWorker worker)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IParticipantEnrolledProgram> GetPepRecordsForPin(decimal pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IUSP_ReferralsAndTransfers_Result> GetReferralsAndTransfersResults(IWorker worker, bool refreshInd, string agencyCode, string roles)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IUSP_GetLastWWOrLFInstance> GetLastWWOrLFInstance(decimal pin)
        {
            throw new NotImplementedException();
        }

        public virtual string GetEnrolledProgramStatus(int? enrolledProgStatusId)
        {
            throw new NotImplementedException();
        }

        public virtual string GetEnrolledProgramCd(string enrolledProgramName)
        {
            throw new NotImplementedException();
        }

        public virtual void EnrollPep(int? pepId, int? workerLoginId, string userId)
        {
        }

        public virtual void TransferPariticipant(IParticipantEnrolledProgram pep, IOffice sourceOffice, IOffice destOffice, IWorker sourceWorker, IWorker destWorker, string userId, string t2536Fep)
        {
            throw new NotImplementedException();
        }

        public virtual void UpsertRecentParticipant(string userId, int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual void UpsertParticipantEnrollment(int? pepId, int? workerLoginId, string action, string userId, string worker, string completionReasonDetails = null, DateTime? disenrollmentDate = null, int? completionReasonId = null)
        {
            throw new NotImplementedException();
        }

        public virtual void ReassignLFCaseManagerInDB2(decimal? pinNumber, string mFUserId)
        {
            throw new NotImplementedException();
        }

        public virtual void ReassignW2CaseManagerInDB2(decimal? pinNumber, string FepId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IConfidentialPinInformation> GetConfidentialPinInfo(decimal pin)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateT0532(decimal? pin, string mfUserId, string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual DataTable GetMostRecentPrograms(decimal pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISpecialInitiative> GetFeatureValue(string featureName)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkerTaskStatus GetWorkerTaskStatus(string code)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkerTaskCategory GetWorkerTaskCategory(string code)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IUSP_RecentlyAccessed_ProgramStatus_Result> GetRecentParticipants(string wamsId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IUSP_RecentlyAccessed_ProgramStatus_Result> GetParticipantsBySearch(string firstName, string lastName, string middleName, string gender, DateTime? dob)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IUSP_ParticipantbyWorker_ProgramStatus_Result> GetParticipantsForWorker(string wamsId, string agencyCode, string program)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<IParticipant> GetParticipantsAsQueryable()
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<IRecentParticipant> GetRecentParticipantsAsQueryable()
        {
            throw new NotImplementedException();
        }

        public virtual IParticipantEnrolledProgram NewPep(IRequestForAssistance rfa, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IParticipantEnrolledProgram GetPepById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual ISP_MostRecentFEPFromDB2_Result GetMostRecentFepDetails(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IPendingCharge NewPendingCharge(ILegalIssuesSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeletePendingCharge(IPendingCharge PendingCharge)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IPolarLookup> PolarLookups()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IPopulationType> PopulationTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IPopulationTypeDto> PopulationTypesFor(string programName, string agencyName = null)
        {
            throw new NotImplementedException();
        }

        public virtual IPostSecondaryCollege NewCollege(IPostSecondaryEducationSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IPostSecondaryDegree NewDegree(IPostSecondaryEducationSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IPostSecondaryEducationSection NewPostSecondaryEducationSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IPostSecondaryLicense NewLicense(IPostSecondaryEducationSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IReferralContactInterval> ReferralContactIntervals()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRelationship> RelationshipTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IRequestForAssistance GetRfa(string pin, int id)
        {
            throw new NotImplementedException();
        }

        public virtual ICFRfaDetail GetCfRfaDetail(int rfaId)
        {
            throw new NotImplementedException();
        }

        public virtual ITJTMJRfaDetail GetTjTmjRfaDetail(int rfaId)
        {
            throw new NotImplementedException();
        }

        public virtual IFCDPRfaDetail GetFcdpRfaDetail(int rfaId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRequestForAssistance> GetRfasForPin(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISP_DB2_RFAs_Result> GetOldRfasForPin(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IRequestForAssistance NewRfa(IParticipant participant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ICFRfaDetail NewCfRfaDetail(IRequestForAssistance rfa, string user, DateTime updateDate)
        {
            throw new NotImplementedException();
        }

        public virtual ITJTMJRfaDetail NewTjTmjRfaDetail(IRequestForAssistance rfa, string user, DateTime updateDate)
        {
            throw new NotImplementedException();
        }

        public virtual IFCDPRfaDetail NewFcdpRfaDetail(IRequestForAssistance rfa, string user, DateTime updateDate)
        {
            throw new NotImplementedException();
        }

        public virtual IRequestForAssistanceChild NewRequestForAssistanceChild(IRequestForAssistance parentRfa, DateTime date, string user)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasAnyRfasInStatus(decimal pin, string programCode, string[] rfaStatuses)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasAnyActiveProgramRfas(decimal pin, string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual DateTime? AddBusinessDays(DateTime? fromDate, int daysForward = 10)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteBackReferralToDb2(IRequestForAssistance rfa, DateTime effectiveDate, string userId)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GenerateRFANumberFromDB2(IRequestForAssistance rfa, string userId)
        {
            throw new NotImplementedException();
        }

        public virtual void DenyRFAInDB2(IRequestForAssistance rfa, string mainframeUserId)
        {
            throw new NotImplementedException();
        }

        public virtual string GetReferralRegCode(decimal? pin, string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual IRequestForAssistanceRuleReason NewRfaEligibility(IRequestForAssistance rfa, string eligibilityCode, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAllRfaEligibilityRows(int rfaId)
        {
            throw new NotImplementedException();
        }

        public virtual IRequestForAssistanceStatus GetRequestForAssistanceStatus(string statusName)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRequestForAssistanceStatus> GetRequestForAssistanceStatusesWhere(Expression<Func<IRequestForAssistanceStatus, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IRequestForAssistancePopulationTypeBridge NewRfaPopulationTypeBridge(IRequestForAssistance rfa, int populationTypeId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAllRfaPopulationTypeBridgeRows(int rfaId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRuleReason> GetRuleReasonsAll()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRuleReason> GetRuleReasonsWhere(Expression<Func<IRuleReason, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolCollegeEstablishment SchoolByNameStreetCityStateCodeCountry(string name, string street, string city, string stateCode, string country)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolCollegeEstablishment SchoolByNameStreetCityCountry(string name, string street, string city, string country)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolCollegeEstablishment NewSchoolByEducationSection(IEducationSection parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolCollegeEstablishment NewSchoolByPostSecondaryEducation(IPostSecondaryCollege parentSection, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolCollegeEstablishment SchoolByNameStreet(string name, string street)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolCollegeEstablishment SchoolById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISchoolCollegeEstablishment> AllSchools()
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolGradeLevel SchoolGradeLevelByGrade(int grade)
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolGradeLevel SchoolGradeLevelById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISchoolGradeLevel> SchoolGradeLevels()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISchoolGradeLevel> AllSchoolGradeLevels()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISchoolGraduationStatus> SchoolGraduationStatuses()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISchoolGraduationStatus> AllSchoolGraduationStatuses()
        {
            throw new NotImplementedException();
        }

        public virtual ISchoolGraduationStatus SchoolGraduationStatusByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual List<string> AuthorizationsForRoles(IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRole> AuthorizationRoles()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IRole> AuthorizationRoles(string[] roleCodes)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<string> GetWorkerUsernames()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IState> States()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IState> USStates()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IState> AllStates()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IState> DriversLicenseStates()
        {
            throw new NotImplementedException();
        }

        public virtual IState StateByCode(string stateCode)
        {
            throw new NotImplementedException();
        }

        public virtual IState StateByCodeAndCountryId(string stateCode, int? countryId)
        {
            throw new NotImplementedException();
        }

        public virtual IState StateById(int? stateId)
        {
            throw new NotImplementedException();
        }

        public virtual IState NewState(ICity city, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ITransportationAssessmentSection NewTransportationAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ITransportationSectionMethodBridge NewTransportationSectionMethodBridge(ITransportationSection parent, string user)
        {
            throw new NotImplementedException();
        }

        public virtual ITransportationSection NewTransportationSection(int participantId, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IDriversLicenseInvalidReasonType> DriversLicenseInvalidReasonTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IDriversLicenseInvalidReasonType> AllDriversLicenseInvalidReasonTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITransportationType> TransportationTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITransportationType> GetTransportationTypes()
        {
            throw new NotImplementedException();
        }

        public virtual List<ITransportationType> GetTransportationTypesWhere(Expression<Func<ITransportationType, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public virtual IWageHour WageHourById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IWageHour NewWageHour(string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWageHourWageTypeBridge NewWageHourWageTypeBridge(IWageHour parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWageHourHistory NewWageHourHistory(IWageHour parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkHistorySectionEmploymentPreventionTypeBridge NewWorkHistorySectionEmploymentPreventionTypeBridge(IWorkHistorySection parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWageHourHistoryWageTypeBridge NewWageHourHistoryWageTypeBridge(IWageHourHistory parentObject, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IJobSector> JobSectors()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWageType> WageTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISymptom> Symptoms()
        {
            throw new NotImplementedException();
        }

        public virtual IPostSecondaryEducationAssessmentSection NewPostSecondaryEducationAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimit> TimeLimitsByPin(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimit TimeLimitById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimit> TimeLimitsByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimit TimeLimitByDate(string pin, DateTime date, bool includedDeleted)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimit> TimeLimitsHistory(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimit> TimeLimitsByDates(string pin, List<DateTime> dates)
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimit NewTimeLimit()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimitState> TimeLimitStates(bool excludeWisconsin = true)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IChangeReason> ChangeReasons()
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimitSummary NewTimeLimitSummary()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimitExtension> GetExtensionSequenceExtensionsByExtById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ITimeLimitExtension> GetExtensionsByPin(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimitExtension GetExtensionsById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimitExtension GetCurrentExtensionByType(int timelimitTypeId, string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IApprovalReason> GetExtensionApprovalReasons()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IDenialReason> GetExtensionDenialReasons()
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimitExtension NewTimeLimitExtension()
        {
            throw new NotImplementedException();
        }

        public virtual ITimeLimitExtension GetExensionByDateRange(int participantId, int timelimitTypeId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public virtual string GetTimeLimitType(int? timelimitTypeId)
        {
            throw new NotImplementedException();
        }

        public virtual string GetExtensionDecision(int? extensionDecisionId)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<ITimeLimit> GetTimeLimit(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkHistoryAssessmentSection NewWorkHistoryAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkHistorySection NewWorkHistorySection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkProgram WorkProgramById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkProgram OtherProgram(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWorkProgram> WorkPrograms()
        {
            throw new NotImplementedException();
        }

        public virtual IWorkProgramAssessmentSection NewWorkProgramAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkProgramSection NewWorkProgramSection(IParticipant parentParticipant, string user)
        {
            throw new NotImplementedException();
        }

        public virtual IFsetStatus CwwFsetStatus(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual IWorkProgramStatus WorkProgramStatusByOrder(int? sortOrder)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWorkProgramStatus> WorkProgramStatuses()
        {
            throw new NotImplementedException();
        }

        public virtual IT0459_IN_W2_LIMITS NewT0459_IN_W2_LIMITS(bool isTracked)
        {
            throw new NotImplementedException();
        }

        public virtual List<IT0459_IN_W2_LIMITS> GetLatestW2LimitsMonthsForEachClockType(decimal pinNum)
        {
            throw new NotImplementedException();
        }

        public virtual IT0459_IN_W2_LIMITS GetW2LimitByMonth(decimal effectiveMonth, decimal pinNum)
        {
            throw new NotImplementedException();
        }

        public virtual List<IT0459_IN_W2_LIMITS> GetW2LimitsByPin(decimal pinNum)
        {
            throw new NotImplementedException();
        }

        public virtual List<IT0459_IN_W2_LIMITS> GetSubsequentW2Limits(decimal pinNum, DateTime timelineMonthDate)
        {
            throw new NotImplementedException();
        }

        public virtual void DB2_T0459_Update(IT0459_IN_W2_LIMITS db2Record)
        {
            throw new NotImplementedException();
        }

        public virtual IT0459_IN_W2_LIMITS GetLatestW2LimitsByClockType(decimal pinNum, ClockTypes clockType)
        {
            throw new NotImplementedException();
        }

        public virtual bool Attach<T>(T model) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual bool Dettach<T>(T model) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual DbEntityEntry<T> GetEntityEntry<T>(T model) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual void Save(bool refreshOnConcurrencyException = false)
        {
        }

        public virtual Task<int> SaveAsync(CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual IT0460_IN_W2_EXT NewT0460InW2Ext(bool isTracked)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IT0460_IN_W2_EXT> GetW2Extensions(decimal pinNum)
        {
            throw new NotImplementedException();
        }

        public virtual int? GetCurrentExtensionSequenceNumber(int participantId, int timelimitTypeId)
        {
            throw new NotImplementedException();
        }

        public virtual IT0460_IN_W2_EXT GetW2ExtensionByClockType(decimal pinNum, ClockTypes timelimitType, int sequenceNumber)
        {
            throw new NotImplementedException();
        }

        public virtual void SpDB2_T0754_Insert(IT0754_LTR_RQST letterRequest)
        {
            throw new NotImplementedException();
        }

        public virtual IWorker WorkerByWamsId(string wamsId)
        {
            throw new NotImplementedException();
        }

        public virtual IWorker WorkerByMainframeId(string mfUserId)
        {
            throw new NotImplementedException();
        }

        public virtual IWorker WorkerByWIUID(string wiuid)
        {
            throw new NotImplementedException();
        }

        public virtual IWorker WorkerById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWorker> WorkersByMainframeIds(List<string> mfUserIdList)
        {
            throw new NotImplementedException();
        }

        public virtual IWorker GetOrCreateWorkerLogin(string wamsId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWorker> GetWorkersByAgency(string agencyCode)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWorker> GetWorkersByOrganization(string orgCode)
        {
            throw new NotImplementedException();
        }

        public virtual List<IWorker> GetWorkerInfosByWamsId(List<string> wamsIds)
        {
            throw new NotImplementedException();
        }

        public virtual string GetFnMFId()
        {
            throw new NotImplementedException();
        }

        public virtual List<IWorker> GetWorkersByAuthToken(string agencyCode, string programId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IWorker> GetWorkersByOrganizationByRole(string orgCode, string roleCode)
        {
            throw new NotImplementedException();
        }

        public virtual string GetWorkerNameByWamsId(string wamsId)
        {
            return "";
        }

        public virtual string GetWorkerNameByWIUId(string wiuid)
        {
            throw new NotImplementedException();
        }

        public virtual GeoArea WpGeoAreaByOfficeNumber(short officeNumber, string programCode = "WW")
        {
            throw new NotImplementedException();
        }

        public virtual GeoArea WpGeoAreaByPin(decimal pin)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISPLType> SplTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<INRSType> NrsTypes()
        {
            throw new NotImplementedException();
        }

        public virtual List<IContractArea> GetContractAreasByProgramCode(string programCode)
        {
            throw new NotImplementedException();
        }

        public virtual List<IContractArea> GetContractAreasByProgramCodeAndOrganizationId(string programCode, int orgId)
        {
            throw new NotImplementedException();
        }

        public virtual List<IContractArea> GetContractArea(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISuffixType> SuffixTypes()
        {
            throw new NotImplementedException();
        }

        public virtual ISuffixType GetSuffixTypeById(int? id)
        {
            throw new NotImplementedException();
        }

        public virtual ISuffixType GetSuffixTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IAliasType> AliasTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISSNType> SSNTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IOtherDemographic NewOtherDemographic(IParticipant parent)
        {
            throw new NotImplementedException();
        }

        public virtual IParticipantContactInfo NewParticipantContactInfo(IParticipant parent)
        {
            throw new NotImplementedException();
        }

        public virtual IAKA NewAKA(IParticipant parent)
        {
            throw new NotImplementedException();
        }

        public virtual IConfidentialPinInformation NewConfidentialPinInformation(IParticipant participant)
        {
            throw new NotImplementedException();
        }

        public virtual ICollection<IConfidentialPinInformation> GetConfidentialPinInformation(int participantId)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasConfidentialInfomation(decimal pin)
        {
            throw new NotImplementedException();
        }

        public virtual IAlternateMailingAddress NewAlternateMailingAddress(IParticipantContactInfo parent)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ISupportiveServiceType> SupportiveServiceTypes()
        {
            throw new NotImplementedException();
        }

        public virtual void NewTransaction(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public virtual string Server   { get; }
        public virtual string Database { get; }
        public virtual string UserId   { get; }
        public virtual string Pass     { get; }

        public virtual bool IsRowVersionStillCurrent<T>(T model, byte[] usersRowVersion) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual bool HasChanged<T>(T model) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual void ResetContext()
        {
            throw new NotImplementedException();
        }

        public virtual bool SaveIfChanged<T>(T model, string user) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual bool SaveIfChanged<T>(T model, string user, DateTime modifiedDate) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual void StartChangeTracking<T>(T model) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual void ResetChangeTracking<T>(T model) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual T GetClonedOriginal<T>(T model) where T : class, ICloneable, ICommonModel
        {
            throw new NotImplementedException();
        }

        public virtual ISP_GetCARESCaseNumber_Result GetCARESCaseNumber(string pin)
        {
            throw new NotImplementedException();
        }

        public virtual void NewWorkerTask(IWorkerTaskList workerTaskList)
        {
            throw new NotImplementedException();
        }
    }
}
