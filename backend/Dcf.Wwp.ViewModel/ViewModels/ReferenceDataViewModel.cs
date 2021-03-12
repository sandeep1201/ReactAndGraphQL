using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Auth;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Contexts;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ReferenceDataViewModel : BaseViewModel
    {
        #region Properties

        private readonly IAuthUser _authUser;
        private readonly EPContext _referenceDataContext;

        #endregion

        #region Methods

        public ReferenceDataViewModel(IRepository repository, IAuthUser authUser, EPContext referenceDataContext) : base(repository, authUser)
        {
            _authUser             = authUser;
            _referenceDataContext = referenceDataContext;
        }

        ///     When this switch statement is modified, be sure to update the FieldDataService in the WWP Angular project. - legacy
        ///     comment
        public IList<IFieldData> GetData2(string referenceType, string options, string subOptions = null)
        {
            IEnumerable<IFieldData> q;
            var                     currentDate = _authUser.CDODate ?? DateTime.Today;

            switch (referenceType.ToLower()) // discussed on 04/08/2019 kabob-case going foward
            {
                case "absencereasons":
                    q = Repo.AbsenceReasons().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "accommodations":
                    q = Repo.Accommodations().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "action-needed":
                    q = GetActionNeededItems(options); //TODO: revisit this
                    break;

                case "action-needed-pages":
                    return GetActionNeededPages();

                case "action-needed-assignees":
                    return GetActionNeededAssignees();

                case "action-needed-priorities":
                    return GetActionNeededPriorities();

                case "alias-types":
                    q = Repo.AliasTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "activity-completion-reasons":
                    options = options ?? string.Empty;
                    List<int> ids;

                    var bv = _referenceDataContext.EnrolledProgramActivityCompletionReasonBridges.AsEnumerable();

                    var cr = _referenceDataContext.ActivityCompletionReasons.AsEnumerable();
                    switch (options.ToLower())
                    {
                        case "w-2":
                        case "w2":
                        case "ww":
                            ids = bv.Where(i => i.EnrolledProgramId == EnrolledProgram.WW).Select(j => j.ActivityCompletionReasonId).ToList();
                            cr  = cr.Where(i => ids.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "lf":
                            ids = bv.Where(i => i.EnrolledProgramId == EnrolledProgram.LearnFareId).Select(j => j.ActivityCompletionReasonId).ToList();
                            cr  = cr.Where(i => ids.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "cf":
                            ids = bv.Where(i => i.EnrolledProgramId == EnrolledProgram.ChildrenFirstId).Select(j => j.ActivityCompletionReasonId).ToList();
                            cr  = cr.Where(i => ids.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tmj":
                            ids = bv.Where(i => i.EnrolledProgramId == EnrolledProgram.TransformMilwaukeeJobsId).Select(j => j.ActivityCompletionReasonId).ToList();
                            cr  = cr.Where(i => ids.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tj":
                            ids = bv.Where(i => i.EnrolledProgramId == EnrolledProgram.TransitionalJobsId).Select(j => j.ActivityCompletionReasonId).ToList();
                            cr  = cr.Where(i => ids.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                    }

                    var crs = cr.OrderBy(i => i.SortOrder).Select(i => i).ToList();
                    q = crs.Select(i => ReferenceDataContract.Create(i.Id, $"{i.Code} - {i.Name}"));
                    break;

                case "activitylocationtypes":
                    q = Repo.ActivityLocationTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "activitytypes":
                    q = Repo.ActivityTypes(options.ToLower())
                            .Where(i => (i.ActivityType.EndDate == null || i.ActivityType.EndDate >= currentDate) && (i.ActivityType.EffectiveDate <= currentDate))
                            .OrderBy(i => i.ActivityType.SortOrder)
                            .Select(i => ReferenceActivityTypeContract.Create(i.ActivityType.Id, i.ActivityType.Code, i.ActivityType.Code + " - " + i.ActivityType.Name, i.IsSelfDirected))
                            .ToList();
                    break;

                case "all-rfa-programs":
                    q = Repo.NonEligibiltyEnrolledPrograms().Select(i => ReferenceDataCodeContract.Create(i.Id, i.DescriptionText, i.ProgramCode));
                    break;

                case "auxiliaryreasons":
                    q = _referenceDataContext.AuxiliaryReasons
                                             .Where(i => !i.IsDeleted && !i.IsSystemUseOnly && (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                             .OrderBy(i => i.SortOrder)
                                             .ToList()
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "auxiliarystatustypes":
                    q = _referenceDataContext.AuxiliaryStatusTypes
                                             .Where(i => !i.IsDeleted && !i.IsSystemUseOnly && !i.IsDisplayOnly && (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                             .OrderBy(i => i.SortOrder)
                                             .ToList()
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "barriertypes":
                    // this version gets everything in one call ;) - scott v.
                    var query = Repo.BarrierTypes();

                    var r = query.OrderBy(i => i.SortOrder)
                                 .Select(i =>
                                             new NestedReferenceDataContract
                                             {
                                                 Id   = i.Id,
                                                 Name = i.Name,
                                                 SubTypes = i.BarrierSubtypes
                                                             .OrderBy(j => j.SortOrder)
                                                             .Select(j =>
                                                                         new ReferenceDataContract
                                                                         {
                                                                             Id                 = j.Id,
                                                                             Name               = j.Name,
                                                                             RequireDetailsFlag = j.DisablesOthersFlag
                                                                         }).ToList()
                                             }
                                        );

                    q = r.AsEnumerable();
                    break;

                case "certificateissuers":
                    q = Repo.CertificateIssuersIssuingAuthorities().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "childcarearrangements":
                    q = Repo.ChildCareArrangements().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "completion-reasons":
                    q = Repo.GetCompletionReasonsforEnrolledProgram(options).Select(i => ReferenceContract.Create(i.Id, i.Name));
                    break;

                case "contactintervaltypes":
                    q = Repo.ContactIntervals().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "contacttitletypes":
                    q = Repo.ContactTitleTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "contacttypes":
                    q = Repo.ContactTitleTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "convictiontypes":
                    q = Repo.ConvictionTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "counties-tribes":
                    q = options == "numeric" ? Repo.GetCountyAndTribes().Select(i => ReferenceDataContract.Create(i.Id, $"{i.CountyName.ToTitleCase()} - {i.CountyNumber:D2}")) : Repo.GetCountyAndTribes().Select(i => ReferenceDataContract.Create(i.Id, i.CountyName.ToTitleCase()));
                    break;

                case "counties":
                    q = options == "numeric" ? Repo.GetCounties().Select(i => ReferenceDataContract.Create(i.Id, $"{i.CountyName.ToTitleCase()} - {i.CountyNumber:D2}")) : Repo.GetCounties().Select(i => ReferenceDataContract.Create(i.Id, i.CountyName.ToTitleCase()));
                    break;

                case "countries":
                    q = Repo.Countries().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "degreetypes":
                    q = Repo.DegreeTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "drivers-license-invalid-reasons":
                    q = Repo.DriversLicenseInvalidReasonTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "drug-screening-status-types":
                    q = _referenceDataContext.DrugScreeningStatusTypes
                                             .Where(i => !i.IsDeleted && !i.IsSystemUseOnly && (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                             .OrderBy(i => i.SortOrder)
                                             .ToList()
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "drivers-license-states":
                    q = Repo.DriversLicenseStates().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "ea-comment-types":
                    q = _referenceDataContext.EaCommentTypes
                                             .Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                             .ToList()
                                             .OrderBy(i => i.Name)
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-emergency-types":
                    q = _referenceDataContext.EaEmergencyTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-emergency-type-reasons":
                    q = _referenceDataContext.EaEmergencyTypeReasonBridges.Where(i => (i.EaEmergencyType.EndDate == null || i.EaEmergencyType.EndDate >= currentDate) && i.EaEmergencyType.EffectiveDate <= currentDate)
                                             .ToList()
                                             .OrderBy(i => i.EaEmergencyTypeReason.SortOrder)
                                             .Select(i => ReferenceDataCodeContract.Create(i.EaEmergencyTypeReason.Id, i.EaEmergencyTypeReason.Name, i.EaEmergencyTypeReason.Code, i.EaEmergencyType.Code));
                    break;

                case "ea-financial-types":
                    q = _referenceDataContext.EaFinancialNeedTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-individual-types":
                    q = _referenceDataContext.EaIndividualTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                             .ToList()
                                             .OrderBy(i => i.SortOrder)
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-initiation-methods":
                    q = _referenceDataContext.EaApplicationInitiationMethodLookUps.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-ipv-occurrences":
                    q = _referenceDataContext.EaIpvOccurrences.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-ipv-reasons":
                    q = _referenceDataContext.EaIpvReasons.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-ipv-statuses":
                    q = _referenceDataContext.EaIpvStatuses.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-relationship-types":
                    q = _referenceDataContext.EaRelationshipTypeBridges
                                             .Where(i => (i.EaRelationshipType.EndDate == null || i.EaRelationshipType.EndDate >= currentDate) && i.EaRelationshipType.EffectiveDate <= currentDate)
                                             .ToList()
                                             .Select(i => ReferenceDataCodeContract.Create(i.EaRelationshipType.Id, i.EaRelationshipType.Name, i.EaRelationshipType.Code, i.IsOnlyForCR ? "OnlyForCR" : "All"));
                    break;

                case "ea-statuses":
                    q = _referenceDataContext.EaStatuses.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "ea-ssn-exempts":
                    q = _referenceDataContext.EaSsnExemptTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code, i.Code));
                    break;

                case "ea-status-reasons":
                    q = _referenceDataContext.EaStatusReasons.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code, i.EaStatus.Code));
                    break;

                case "ea-verifications":
                    q = _referenceDataContext.EaVerificationTypeBridges
                                             .Where(i => (i.EaVerificationType.EndDate == null || i.EaVerificationType.EndDate >= currentDate) && i.EaVerificationType.EffectiveDate <= currentDate)
                                             .ToList()
                                             .OrderBy(i => i.EaVerificationType.SortOrder)
                                             .Select(i => ReferenceDataCodeContract.Create(i.EaVerificationType.Id, i.EaVerificationType.Name, i.EaVerificationType.Code, i.IsIncome, i.IsAsset, i.IsVehicle, i.IsVehicleValue));
                    break;

                case "educationresulttypes":
                    q = Repo.ExamSubjectTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name)); // this probably never gets called - the Linq query is empty...
                    break;

                case "education-test-types":
                    q = Repo.ExamTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "education-test-statuses":
                    q = Repo.ExamPassTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;
                case "element":
                    q = _referenceDataContext.Elements.Where((i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)).ToList().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "elevatedaccessreason":
                    q = Repo.ElevatedAccessReasons().Select(i => ReferenceDataContract.Create(i.Id, i.Reason));
                    break;

                case "employment-prevention-factors":
                    q = Repo.EmploymentPreventionTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;
                case "employerofrecordtypes":
                    q = Repo.EmployerOfRecordTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "employmentstatustypes":
                    q = Repo.EmploymentStatusTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;
                case "employability-plan-statustypes":
                    q = _referenceDataContext.EmployabilityPlanStatusTypes.OrderBy(i => i.SortOrder).ToList().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "exam-subjects":
                    return GetExamSubjectsByExamType(options);

                case "feature-value":
                    var value = GetFeatureValue(options);
                    return value;

                case "frequency":
                    q = options == "mr"
                            ? Repo.MonthlyFrequencies().Select(i => ReferenceDataContract.Create(i.Id, i.Name))
                            : Repo.WeeklyFrequencies().Select(i => ReferenceDataContract.Create(i.Id,  i.Name));
                    break;

                case "frequency-type":
                    q = Repo.FrequencyTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "genders":
                    q = Repo.GenderTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "goal-end-reasons":
                    q = _referenceDataContext.GoalEndReasons.OrderBy(i => i.SortOrder).ToList().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "goaltypes":
                    q = Repo.GoalTypes().Where(x => x.EnrolledProgramId == options.ToInt()).Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;
                case "good-cause-denied-reasons":
                    options = options ?? string.Empty;
                    List<int> gcdIds;

                    var epGcdR = _referenceDataContext.EnrolledProgramGcdReasonBridges.AsEnumerable();
                    var gcdR   = _referenceDataContext.GoodCauseDeniedReason.AsEnumerable();
                    switch (options.ToLower())
                    {
                        case "w-2":
                        case "w2":
                        case "ww":
                            gcdIds = epGcdR.Where(i => i.EnrolledProgramId == EnrolledProgram.WW).Select(j => j.GoodCauseDeniedReasonId).ToList();
                            gcdR   = gcdR.Where(i => gcdIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "lf":
                            gcdIds = epGcdR.Where(i => i.EnrolledProgramId == EnrolledProgram.LearnFareId).Select(j => j.GoodCauseDeniedReasonId).ToList();
                            gcdR   = gcdR.Where(i => gcdIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "cf":
                            gcdIds = epGcdR.Where(i => i.EnrolledProgramId == EnrolledProgram.ChildrenFirstId).Select(j => j.GoodCauseDeniedReasonId).ToList();
                            gcdR   = gcdR.Where(i => gcdIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tmj":
                            gcdIds = epGcdR.Where(i => i.EnrolledProgramId == EnrolledProgram.TransformMilwaukeeJobsId).Select(j => j.GoodCauseDeniedReasonId).ToList();
                            gcdR   = gcdR.Where(i => gcdIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tj":
                            gcdIds = epGcdR.Where(i => i.EnrolledProgramId == EnrolledProgram.TransitionalJobsId).Select(j => j.GoodCauseDeniedReasonId).ToList();
                            gcdR   = gcdR.Where(i => gcdIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                    }

                    var gcdRs = gcdR.OrderBy(i => i.SortOrder).Select(i => i).ToList();
                    q = gcdRs.Select(i => ReferenceDataCodeContract.Create(i.Id, $"{i.Code} - {i.Name}", i.Code));
                    break;
                case "good-cause-granted-reasons":
                    options = options ?? string.Empty;
                    List<int> gcgIds;

                    var epgcgR = _referenceDataContext.EnrolledProgramGcgReasonBridges.AsEnumerable();
                    var gcgR   = _referenceDataContext.GoodCauseGrantedReason.AsEnumerable();
                    switch (options.ToLower())
                    {
                        case "w-2":
                        case "w2":
                        case "ww":
                            gcgIds = epgcgR.Where(i => i.EnrolledProgramId == EnrolledProgram.WW).Select(j => j.GoodCauseGrantedReasonId).ToList();
                            gcgR   = gcgR.Where(i => gcgIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "lf":
                            gcgIds = epgcgR.Where(i => i.EnrolledProgramId == EnrolledProgram.LearnFareId).Select(j => j.GoodCauseGrantedReasonId).ToList();
                            gcgR   = gcgR.Where(i => gcgIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "cf":
                            gcgIds = epgcgR.Where(i => i.EnrolledProgramId == EnrolledProgram.ChildrenFirstId).Select(j => j.GoodCauseGrantedReasonId).ToList();
                            gcgR   = gcgR.Where(i => gcgIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tmj":
                            gcgIds = epgcgR.Where(i => i.EnrolledProgramId == EnrolledProgram.TransformMilwaukeeJobsId).Select(j => j.GoodCauseGrantedReasonId).ToList();
                            gcgR   = gcgR.Where(i => gcgIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tj":
                            gcgIds = epgcgR.Where(i => i.EnrolledProgramId == EnrolledProgram.TransitionalJobsId).Select(j => j.GoodCauseGrantedReasonId).ToList();
                            gcgR   = gcgR.Where(i => gcgIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                    }

                    var gcgRs = gcgR.OrderBy(i => i.SortOrder).Select(i => i).ToList();
                    q = gcgRs.Select(i => ReferenceDataCodeContract.Create(i.Id, $"{i.Code} - {i.Name}", i.Code));
                    break;

                case "housingsituations":
                    q = Repo.HousingSituations().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "intervaltypes":
                    q = Repo.IntervalTypes().OrderBy(i => i.SortOrder).Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "jobbenefitsoffered":
                    q = Repo.BenefitsOfferedTypes().Select(i => ReferenceMultiDataContract.Create(i.Id, i.Name, i.DisablesOthersFlag));
                    break;

                case "jobfoundmethods":
                    q = Repo.JobFoundMethods().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "jobsectors":
                    q = Repo.JobSectors().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "jobtypes":
                    q = Repo.JobTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "jr-work-shifts":
                    q = _referenceDataContext.JrWorkShifts.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "languages":
                    q = Repo.Languages().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "leavingreasons":
                    q = Repo.LeavingReasons().ToList().OrderBy(i => i.LeavingReason.SortOrder).Select(i => ReferenceDataCodeContract.Create(i.LeavingReason.Id, i.LeavingReason.Name, i.JobTypeId));
                    break;

                case "licensetypes":
                    q = Repo.LicenseTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "max-days":
                    // this code snippet is part of the EF Database to Code First transition...
                    var progCode = options ?? string.Empty;
                    var epv      = _referenceDataContext.EnrolledProgramValidities.Where(i => i.EnrolledProgram.ProgramCode == progCode && !i.IsDeleted).ToList();
                    q = epv.Select(i => ReferenceEPContract.Create(i.Id, i.EnrolledProgramId, i.MaxDaysCanBackDate, i.MaxDaysInProgressStatus, i.MaxDaysCanBackDatePS));
                    break;

                case "militarybranches":
                    q = Repo.MilitaryBranches().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "militarydischargetypes":
                    q = Repo.DischargeTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "militaryranks":
                    q = Repo.MilitaryRanks().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "ncprelationshiptypes":
                case "noncustodialrelationships":
                    q = Repo.NonCustodialParentRelationships().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "nrs-types":
                    q = Repo.NrsTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;
                case "non-participation-reasons":
                    options = options ?? string.Empty;
                    List<int> npIds;

                    var epNpR = _referenceDataContext.EnrolledProgramNpReasonBridges.AsEnumerable();
                    var npR   = _referenceDataContext.NonParticipationReasons.AsEnumerable();
                    switch (options.ToLower())
                    {
                        case "w-2":
                        case "w2":
                        case "ww":
                            npIds = epNpR?.Where(i => i.EnrolledProgramId == EnrolledProgram.WW).Select(j => j.NonParticipationReasonId).ToList();
                            npR   = npR.Where(i => npIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "lf":
                            npIds = epNpR?.Where(i => i.EnrolledProgramId == EnrolledProgram.LearnFareId).Select(j => j.NonParticipationReasonId).ToList();
                            npR   = npR.Where(i => npIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "cf":
                            npIds = epNpR.Where(i => i.EnrolledProgramId == EnrolledProgram.ChildrenFirstId).Select(j => j.NonParticipationReasonId).ToList();
                            npR   = npR.Where(i => npIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tmj":
                            npIds = epNpR.Where(i => i.EnrolledProgramId == EnrolledProgram.TransformMilwaukeeJobsId).Select(j => j.NonParticipationReasonId).ToList();
                            npR   = npR?.Where(i => npIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tj":
                            npIds = epNpR.Where(i => i.EnrolledProgramId == EnrolledProgram.TransitionalJobsId).Select(j => j.NonParticipationReasonId).ToList();
                            npR   = npR.Where(i => npIds.Contains(i.Id) && (i.IsSystemUseOnly == false) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                    }

                    var npRs = npR.OrderBy(i => i.SortOrder).Select(i => i).ToList();
                    q = npRs.Select(i => ReferenceDataCodeContract.Create(i.Id, $"{i.Code} - {i.Name}", i.Code));
                    break;

                case "participation-period":
                    var pps = _referenceDataContext.ParticipationPeriodLookUps
                                                   .Where(i => !i.IsDeleted && !i.IsSystemUseOnly && (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                                   .OrderBy(i => i.SortOrder)
                                                   .Select(i => i).ToList();
                    q = pps.Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "participation-statuses":
                    // this code snippet is part of the EF Databasebase to Code First transition...

                    options = options ?? string.Empty;
                    List<int> enrolledProgramIds;

                    var bpv = _referenceDataContext.EnrolledProgramParticipationStatusTypeBridges.AsEnumerable();

                    var ps = _referenceDataContext.ParticipationStatusType.AsEnumerable();
                    switch (options.ToLower())
                    {
                        case "w-2":
                        case "w2":
                        case "ww":
                            enrolledProgramIds = bpv.Where(i => i.EnrolledProgramId == EnrolledProgram.WW).Select(j => j.ParticipationStatusTypeId).ToList();
                            ps                 = ps.Where(i => enrolledProgramIds.Contains(i.Id) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "lf":
                            enrolledProgramIds = bpv.Where(i => i.EnrolledProgramId == EnrolledProgram.LearnFareId).Select(j => j.ParticipationStatusTypeId).ToList();
                            ps                 = ps.Where(i => enrolledProgramIds.Contains(i.Id) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "cf":
                            enrolledProgramIds = bpv.Where(i => i.EnrolledProgramId == EnrolledProgram.ChildrenFirstId).Select(j => j.ParticipationStatusTypeId).ToList();
                            ps                 = ps.Where(i => enrolledProgramIds.Contains(i.Id) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tmj":
                            enrolledProgramIds = bpv.Where(i => i.EnrolledProgramId == EnrolledProgram.TransformMilwaukeeJobsId).Select(j => j.ParticipationStatusTypeId).ToList();
                            ps                 = ps.Where(i => enrolledProgramIds.Contains(i.Id) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                        case "tj":
                            enrolledProgramIds = bpv.Where(i => i.EnrolledProgramId == EnrolledProgram.TransitionalJobsId).Select(j => j.ParticipationStatusTypeId).ToList();
                            ps                 = ps.Where(i => enrolledProgramIds.Contains(i.Id) && (i.EndDate == null || i.EndDate >= currentDate) && (i.EffectiveDate <= currentDate));
                            break;
                    }

                    var pss = ps.OrderBy(i => i.SortOrder).Select(i => i).ToList();
                    q = pss.Select(i => ReferenceDataCodeContract.Create(i.Id, $"{i.Code} - {i.Name}", i.Code));
                    break;

                case "pendingtypes":
                    q = Repo.ConvictionTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "pin-comment-types":
                    var checkString = "canAccessProgram_";
                    var auths       = _authUser.Authorizations?.Where(i => i.StartsWith(checkString)).Select(i => i.Remove(0, checkString.Length).Trim().ToLower()).ToList();
                    var commentType = _referenceDataContext.EnrolledProgramPinCommentTypeBridges.ToList()
                                                           .OrderBy(i => i.PinCommentType.Name)
                                                           .Where(i => auths              != null        && i.SystemUseOnly == false && auths.Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                                                       && i.EffectiveDate <= currentDate && (i.EndDate == null || i.EndDate >= currentDate))
                                                           .GroupBy(i => i.PinCommentTypeId)
                                                           .Select(i => i.FirstOrDefault())
                                                           .Where(i => i != null && i.PinCommentType.SystemUseOnly == false
                                                                                 && (i.PinCommentType.EndDate == null || i.PinCommentType.EndDate >= currentDate)
                                                                                 && i.PinCommentType.EffectiveDate <= currentDate)
                                                           .ToList();
                    q = commentType.Where(i => i.EnrolledProgramId != EnrolledProgram.FCDPId)
                                   .Concat(commentType.Where(i => i.EnrolledProgramId == EnrolledProgram.FCDPId))
                                   .Select(i => ReferenceDataContract.Create(i.PinCommentType.Id, i.PinCommentType.Name));
                    break;

                case "plan-sections":
                    q = _referenceDataContext.PlanSectionTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList()
                                             .OrderBy(i => i.SortOrder)
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "plan-statuses":
                    q = _referenceDataContext.PlanStatusTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList()
                                             .OrderBy(i => i.SortOrder)
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "plan-types":
                    q = _referenceDataContext.PlanTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList()
                                             .OrderBy(i => i.SortOrder)
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "polarinput":
                    q = Repo.PolarLookups().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "pop-claim-status-types":
                    q = _referenceDataContext.POPClaimStatusTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList()
                                             .OrderBy(i => i.SortOrder).Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "pop-claim-types":
                    q = _referenceDataContext.POPClaimTypes.Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate).ToList()
                                             .OrderBy(i => i.Description).Select(i => ReferenceDataCodeContract.Create(i.Id, i.Description, i.Code, i.IsSystemUseOnly));
                    break;

                case "population-types":
                    q = Repo.PopulationTypesFor(options, subOptions).Select(i => ReferenceMultiExclusiveDataContract.Create(i.Id, i.Name, i.DisablesOthers, i.DisabledIds));
                    break;

                case "programs":
                    q = _referenceDataContext.EnrolledPrograms
                                             .Where(i => i.SubProgramCode == null && i.ProgramCode != EnrolledProgram.LFProgramCode)
                                             .ToList()
                                             .OrderBy(i => i.DescriptionText)
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.DescriptionText, i.ProgramCode));
                    break;

                case "program-organizations":
                    q = Repo.GetOrganizationsByProgramId(options).OrderBy(i => i.AgencyName).Select(i => ReferenceDataCodeContract.Create(i.Id, i.AgencyName, i.EntsecAgencyCode));
                    break;

                case "pull-down-dates":
                    q = _referenceDataContext.PullDownDates.Where(i => !i.IsDeleted).ToList()
                                             .Select(i => ReferencePullDownContract.Create(i.Id, i.BenefitMonth, i.BenefitYear, i.PullDownDates));
                    break;

                case "referral-contact-interval-types":
                    q = Repo.ReferralContactIntervals().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "relationships":
                    q = Repo.RelationshipTypes().Select(i => ReferenceDataContract.Create(i.Id, i.RelationName));
                    break;

                case "rfa-contractors":
                    q = Repo.GetOrganizationsForProgram(options).Where(x => x.EntsecAgencyCode == AuthUser.AgencyCode).Select(i => ReferenceDataCodeContract.Create(i.Id, i.AgencyName, i.EntsecAgencyCode));
                    break;

                case "rfa-programs":
                    var programCodesUserHasAccessTo = AuthUser.EnrolledProgramCodes(Repo);
                    q = Repo.NonEligibiltyEnrolledPrograms().Where(x => programCodesUserHasAccessTo.Contains(x.ProgramCode)).Select(i => ReferenceDataCodeContract.Create(i.Id, i.DescriptionText, i.ProgramCode));
                    break;

                case "schoolgrades":
                    q = Repo.SchoolGradeLevels().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "schoolgraduationstatustypes":
                    q = Repo.SchoolGraduationStatuses().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "spl-types":
                    q = Repo.SplTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Rating));
                    break;

                case "ssi-application-statuses":
                    q = Repo.ApplicationStatusTypes().Select(i => ReferenceDataContract.Create(i.Id, i.ApplicationStatusName));
                    break;

                case "ssn-types":
                    q = Repo.SSNTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "states":
                    q = Repo.USStates().Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "suffix-types":
                    q = Repo.SuffixTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Code));
                    break;

                case "supportiveservicetypes":
                    q = Repo.SupportiveServiceTypes().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "symptoms":
                    q = Repo.Symptoms().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "transportation-types":
                    q = Repo.TransportationTypes().Select(i => ReferenceMultiDataContract.Create(i.Id, i.Name, i.DisablesOthersFlag));
                    break;

                case "tribes":
                    q = options == "numeric" ? Repo.GetTribes().Select(i => ReferenceDataContract.Create(i.Id, $"{i.CountyName.ToTitleCase()} - {i.CountyNumber:D2}")) : Repo.GetTribes().Select(i => ReferenceDataContract.Create(i.Id, i.CountyName.ToTitleCase()));
                    break;

                case "wagetypes":
                    q = Repo.WageTypes().Select(i => ReferenceMultiDataContract.Create(i.Id, i.Name, i.DisablesOthersFlag));
                    break;

                case "workprograms":
                    q = Repo.WorkPrograms().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "workprogramstatuses":
                    q = Repo.WorkProgramStatuses().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "worker-task-categories":
                    var workerTaskAuths = _authUser.Authorizations
                                                   .Where(i => i.StartsWith("canAccessProgram_") || i.StartsWith("canAccessEA_"))
                                                   .Select(j => j.Trim().ToLower().Split('_')[1].Replace(new List<string> { "view", "edit" }, EnrolledProgram.EACode).ToUpper())
                                                   .Distinct()
                                                   .ToList();

                    var workerTaskCategory = _referenceDataContext.WorkerTaskCategories
                                                                  .Where(i => (((workerTaskAuths.Contains(EnrolledProgram.W2ProgramCode) || workerTaskAuths.Contains(EnrolledProgram.LFProgramCode))  && i.IsWWLF)  ||
                                                                               (workerTaskAuths.Contains(EnrolledProgram.CFProgramCode)                                                               && i.IsCF)    ||
                                                                               ((workerTaskAuths.Contains(EnrolledProgram.TjProgramCode) || workerTaskAuths.Contains(EnrolledProgram.TmjProgramCode)) && i.IsTJTMJ) ||
                                                                               (workerTaskAuths.Contains(EnrolledProgram.FCDPProgramCode)                                                             && i.IsFCDP)  ||
                                                                               (workerTaskAuths.Contains(EnrolledProgram.EACode)                                                                      && i.IsEA)) &&
                                                                              !i.IsSystemUseOnly                                                                                                                  && !i.IsDeleted)
                                                                  .OrderBy(i => i.SortOrder)
                                                                  .Distinct()
                                                                  .ToList();

                    q = workerTaskCategory.Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "worker-task-priorities":
                    return GetActionNeededPriorities(true);

                case "worker-task-statuses":
                    q = _referenceDataContext.WorkerTaskStatuses
                                             .Where(i => (i.EndDate == null || i.EndDate >= currentDate) && i.EffectiveDate <= currentDate)
                                             .ToList()
                                             .Select(i => ReferenceDataCodeContract.Create(i.Id, i.Name, i.Code));
                    break;

                case "yes-no-refused-types":
                    q = Repo.YesNoRefusedLookups().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "yes-no-skip-types":
                    q = Repo.YesNoSkipLookups().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                case "yes-no-unknown-types":
                    q = Repo.YesNoUnknownLookups().Select(i => ReferenceDataContract.Create(i.Id, i.Name));
                    break;

                default:

                    throw new InvalidOperationException("Section is not recognized");
            }

            return q.ToList();
        }

        public List<IFieldData> GetActionNeededItems(string section)
        {
            if (section == "all")
            {
                // this gets you the same thing in one shot:
                //ActionNeededPages.Select(i => new
                //                              {
                //                                  i.Id,
                //                                  i.Name,
                //                                  Items = i.ActionNeededPageActionItemBridges.Where(j => !j.IsDeleted).Select(j => new { Id = j.Id, Name = j.ActionItem.Name })
                //                              });

                var list = new List<NestedReferenceDataContract>();

                // The call to ActionNeededPageActionItems returns all the records
                // from the bridge table, but we want to group them by the page
                // or section.
                var bridgeItems = Repo.ActionNeededPageActionItems().ToList();
                var pages       = Repo.ActionNeededPages();

                // Create a Lookup so we can get the bridge items by the page ID.
                var actionItemsByPageId = bridgeItems.ToLookup(b => b.ActionNeededPageId, b => b.ActionItem);

                foreach (var page in pages)
                {
                    var actionItems = (from x in actionItemsByPageId[page.Id] where !x.IsDeleted select x).ToList();

                    var nrdc = NestedReferenceDataContract.CreateContract(page.Id, page.Name);
                    nrdc.SubTypes = (from x in actionItems select ReferenceDataContract.CreateContract(x.Id, x.Name, false)).ToList();
                    list.Add(nrdc);
                }

                return list.Cast<IFieldData>().ToList();
            }

            return
                (from x in Repo.ActionItemsForPage(section)
                 select ReferenceDataContract.Create(x.Id, x.Name)).ToList();
        }

        private List<IFieldData> GetActionNeededPages() =>
            (from x in Repo.ActionNeededPages()
             select ReferenceDataContract.Create(x.Id, x.Name)).ToList();

        private List<IFieldData> GetActionNeededAssignees() =>
            (from x in Repo.ActionAssignees()
             select ReferenceDataContract.Create(x.Id, x.Name)).ToList();

        private List<IFieldData> GetActionNeededPriorities(bool orderDesc = false) =>
            (from x in Repo.ActionPriorities(orderDesc)
             select ReferenceDataContract.Create(x.Id, x.Name)).ToList();

        public List<IFieldData> GetDeleteReasons(string repeater)
        {
            if (string.IsNullOrWhiteSpace(repeater))
            {
                throw new InvalidOperationException("Repeater is not defined");
            }

            return
                (from x in Repo.DeleteReasonsByRepeater(repeater)
                 select ReferenceDataContract.Create(x.DeleteReason.Id, x.DeleteReason?.Name)).ToList();
        }

        public List<IFieldData> GetBarrierTypes()
        {
            //var list =
            //    (from x in Repo.BarrierTypes() select NestedReferenceDataContract.CreateContract(x.Id, x.Name)).ToList();
            //foreach (var barrierType in list)
            //{
            //    barrierType.SubTypes =
            //        (from x in Repo.BarrierSubtypeByBarriertype(barrierType.Id).OrderBy(x => x.SortOrder)
            //         select ReferenceDataContract.CreateContract(x.Id, x.Name, x.DisablesOthersFlag)).ToList();
            //}

            //return list.Cast<IFieldData>().ToList();

            // this version gets everything in *one* call - scott v.
            //var q = Repo.GetAsQueryableAsNoTracking<BarrierType>();
            var rs = Repo.BarrierTypes().ToList();

            var r = rs.OrderBy(i => i.SortOrder) // this is only *one* sql call ;)
                      .Select(i =>
                                  new NestedReferenceDataContract
                                  {
                                      Id   = i.Id,
                                      Name = i.Name,
                                      SubTypes = i.BarrierSubtypes.Select(j =>
                                                                              new ReferenceDataContract
                                                                              {
                                                                                  Id                 = j.Id,
                                                                                  Name               = j.Name,
                                                                                  RequireDetailsFlag = j.DisablesOthersFlag
                                                                              }).ToList()
                                  }
                             );

            return (r.ToList<IFieldData>());
        }

        private List<IFieldData> GetExamSubjectsByExamType(string referenceType)
        {
            switch (referenceType)
            {
                case "tabe910":

                    return
                        (from x in Repo.ExamSubjectsByExamType(@"TABE 9 & 10")
                         select ReferenceDataContract.Create(x.Id, x.Name)).ToList();
                case "ged":

                    return
                        (from x in Repo.ExamSubjectsByExamType(@"GED/HSED")
                         select ReferenceDataContract.Create(x.Id, x.Name)).ToList();
                case "best":

                    return
                        (from x in Repo.ExamSubjectsByExamType(@"BEST")
                         select ReferenceDataContract.Create(x.Id, x.Name)).ToList();
                case "tabeclase":

                    return
                        (from x in Repo.ExamSubjectsByExamType(@"TABE CLAS-E")
                         select ReferenceDataContract.Create(x.Id, x.Name)).ToList();

                case "casas":

                    return
                        (from x in Repo.ExamSubjectsByExamType(@"CASAS")
                         select ReferenceDataContract.Create(x.Id, x.Name)).ToList();

                default:

                    throw new InvalidOperationException("Exam is not recognized");
            }
        }

        private List<IFieldData> GetFeatureValue(string featureName)
        {
            var value = Repo.GetFeatureValue(featureName)
                            .Select(i => ReferenceDataContract.Create(i.Id, i.ParameterValue))
                            .ToList();

            return value;
        }

        // private List<IFieldData> GetActivityTypes(string progCode)
        // {
        //     var l = Repo.ActivityTypes(progCode.ToLower())
        //                 .Select(i => ReferenceDataContract.Create(i.Id, i.Code + " - " + i.Name))
        //                 .ToList();
        //
        //     return (l);
        // }

        #endregion
    }
}
