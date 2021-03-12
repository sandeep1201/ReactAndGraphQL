using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Api.Library.Rules.Infrastructure;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using DCF.Common.Extensions;
using NRules.Fluent;
using ActivityCompletionReason = Dcf.Wwp.Model.Interface.Constants.ActivityCompletionReason;
using Authorization = Dcf.Wwp.Model.Interface.Constants.Authorization;
using EmployabilityPlan = Dcf.Wwp.DataAccess.Models.EmployabilityPlan;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using POPClaimStatusType = Dcf.Wwp.Model.Interface.Constants.POPClaimStatusType;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using POPStatusToClaimMap = Dcf.Wwp.Model.Interface.Constants.POPStatusToClaimMap;
using RuleReason = Dcf.Wwp.Data.Sql.Model.RuleReason;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class POPClaimDomain : IPOPClaimDomain
    {
        #region Properties

        private readonly IUnitOfWork                           _unitOfWork;
        private readonly IAuthUser                             _authUser;
        private readonly IMapper                               _mapper;
        private readonly IPOPClaimRepository                   _popClaimRepository;
        private readonly IPOPClaimEmploymentBridgeRepository   _popClaimEmploymentBridgeRepository;
        private readonly IPOPClaimActivityBridgeRepository     _popClaimActivityBridgeRepository;
        private readonly IPOPClaimStatusTypeRepository         _popClaimStatusTypeRepository;
        private readonly IPOPClaimStatusRepository             _popClaimStatusRepository;
        private readonly IEmploymentInformationRepository      _employmentInformationRepository;
        private readonly IParticipantEnrolledProgramRepository _participantEnrolledProgramRepository;
        private readonly IOrganizationRepository               _organizationRepository;
        private readonly IEmployabilityPlanRepository          _employabilityPlanRepository;
        private readonly IParticipantPlacementRepository       _participantPlacementRepository;
        private readonly IRuleReasonRepository                 _ruleReasonRepository;
        private readonly IWWPRuleRepository                    _ruleRepository;
        private readonly IPOPClaimTypeRepository               _popClaimTypeRepository;
        private readonly IPOPClaimHighWageRepository           _popClaimHighWageRepository;
        private readonly IWorkerTaskListDomain                 _workerTaskListDomain;
        private readonly IWorkerTaskCategoryRepository         _workerTaskCategoryRepository;
        private readonly IParticipantRepository                _participantRepository;
        private readonly ISpecialInitiativeRepository          _featureToggleRepository;

        #endregion

        #region Methods

        public POPClaimDomain(IUnitOfWork                           unitOfWork,
                              IAuthUser                             authUser,
                              IMapper                               mapper,
                              IPOPClaimRepository                   popClaimRepository,
                              IPOPClaimEmploymentBridgeRepository   popClaimEmploymentBridgeRepository,
                              IPOPClaimActivityBridgeRepository     popClaimActivityBridgeRepository,
                              IPOPClaimStatusTypeRepository         popClaimStatusTypeRepository,
                              IPOPClaimStatusRepository             popClaimStatusRepository,
                              IEmploymentInformationRepository      employmentInformationRepository,
                              IParticipantEnrolledProgramRepository participantEnrolledProgramRepository,
                              IOrganizationRepository               organizationRepository,
                              IEmployabilityPlanRepository          employabilityPlanRepository,
                              IParticipantPlacementRepository       participantPlacementRepository,
                              IRuleReasonRepository                 ruleReasonRepository,
                              IWWPRuleRepository                    ruleRepository,
                              IPOPClaimTypeRepository               popClaimTypeRepository,
                              IPOPClaimHighWageRepository           popClaimHighWageRepository,
                              IWorkerTaskListDomain                 workerTaskListDomain,
                              IWorkerTaskCategoryRepository         workerTaskCategoryRepository,
                              IParticipantRepository                participantRepository,
                              ISpecialInitiativeRepository          featureToggleRepository
        )
        {
            _unitOfWork                           = unitOfWork;
            _authUser                             = authUser;
            _mapper                               = mapper;
            _popClaimRepository                   = popClaimRepository;
            _popClaimEmploymentBridgeRepository   = popClaimEmploymentBridgeRepository;
            _popClaimActivityBridgeRepository     = popClaimActivityBridgeRepository;
            _popClaimStatusTypeRepository         = popClaimStatusTypeRepository;
            _popClaimStatusRepository             = popClaimStatusRepository;
            _employmentInformationRepository      = employmentInformationRepository;
            _participantEnrolledProgramRepository = participantEnrolledProgramRepository;
            _organizationRepository               = organizationRepository;
            _employabilityPlanRepository          = employabilityPlanRepository;
            _participantPlacementRepository       = participantPlacementRepository;
            _ruleReasonRepository                 = ruleReasonRepository;
            _ruleRepository                       = ruleRepository;
            _popClaimTypeRepository               = popClaimTypeRepository;
            _popClaimHighWageRepository           = popClaimHighWageRepository;
            _workerTaskListDomain                 = workerTaskListDomain;
            _workerTaskCategoryRepository         = workerTaskCategoryRepository;
            _participantRepository                = participantRepository;
            _featureToggleRepository              = featureToggleRepository;
        }


        public List<POPClaimContract> GetPOPClaims(int participantId)
        {
            return _mapper.Map<List<POPClaimContract>>(_popClaimRepository.GetMany(i => i.ParticipantId == participantId && !i.IsDeleted)
                                                                          ?.OrderByDescending(i => i.ClaimPeriodBeginDate)
                                                                          .ThenByDescending(i => i.ModifiedDate)
                                                                          .ToList());
        }

        public List<POPClaimContract> GetPOPClaimsWithStatuses(List<string> statuses, string agencyCode = null)
        {
            var agencyCd                   = agencyCode;
            var popClaimStatusCodesToCheck = statuses;

            return _mapper.Map<List<POPClaimContract>>(_popClaimRepository.GetAsQueryable()
                                                                          .Where(i => !i.IsDeleted && i.Organization.EntsecAgencyCode.ToLower().Trim() == agencyCd)
                                                                          .SelectMany(i => i.POPClaimStatuses
                                                                                            .OrderByDescending(j => j.Id)
                                                                                            .Take(1))
                                                                          .Where(i => popClaimStatusCodesToCheck.Contains(i.POPClaimStatusType.Code))
                                                                          .Select(i => i.POPClaim)
                                                                          .OrderBy(i => i.ClaimPeriodBeginDate)
                                                                          .ThenByDescending(i => i.ModifiedDate).ToList());
        }

        public List<POPClaimContract> GetPOPClaimsByAgency(string agencyCode = null)
        {
            var agencyCd = agencyCode ?? _authUser.AgencyCode.ToLower().Trim();
            return _mapper.Map<List<POPClaimContract>>(_popClaimRepository.GetAsQueryable()
                                                                          .Where(i => !i.IsDeleted && (agencyCd == AgencyCode.DCF || i.Organization.EntsecAgencyCode.ToLower().Trim() == agencyCd))
                                                                          .SelectMany(i => i.POPClaimStatuses
                                                                                            .OrderByDescending(j => j.Id)
                                                                                            .Take(1))
                                                                          .Select(i => i.POPClaim)
                                                                          .OrderByDescending(i => i.ClaimPeriodBeginDate)
                                                                          .ThenByDescending(i => i.ModifiedDate)
                                                                          .ToList());
        }

        public POPClaimContract GetPOPClaim(int id)
        {
            return _mapper.Map<POPClaimContract>(_popClaimRepository.Get(i => i.Id == id));
        }

        public List<POPClaimEmploymentContract> GetEmploymentsForPOP(string pin, int popClaimId)
        {
            var decimalPin     = decimal.Parse(pin);
            var contracts      = new List<POPClaimEmploymentContract>();
            var employments    = _employmentInformationRepository.GetMany(i => i.Participant.PinNumber == decimalPin).ToList();
            var popEmployments = _popClaimEmploymentBridgeRepository.GetMany(i => i.POPClaimId         == popClaimId).ToList();


            employments.ForEach(employment =>
                                {
                                    var id           = popEmployments.FirstOrDefault(i => i.EmploymentInformationId == employment.Id)?.Id ?? 0;
                                    var employmentId = employments.FirstOrDefault(i => i.Id == employment.Id)?.Id;
                                    if (employmentId == null) return;
                                    var computedWageHourDetails = new List<ComputedWageHourDetails>
                                                                  {
                                                                      new ComputedWageHourDetails
                                                                      {
                                                                          ComputedWageRateUnit  = employment.WageHour.ComputedCurrentWageRateUnit,
                                                                          ComputedWageRateValue = employment.WageHour.ComputedCurrentWageRateValue,
                                                                          EffectiveDate         = employment.WageHour.CurrentEffectiveDate
                                                                      }
                                                                  };
                                    if (employment.WageHour.WageHourHistories != null)
                                    {
                                        computedWageHourDetails.AddRange(employment.WageHour.WageHourHistories.Select(wageHourHistory => new ComputedWageHourDetails
                                                                                                                                         {
                                                                                                                                             ComputedWageRateUnit  = wageHourHistory.ComputedWageRateUnit,
                                                                                                                                             ComputedWageRateValue = wageHourHistory.ComputedWageRateValue,
                                                                                                                                             EffectiveDate         = wageHourHistory.EffectiveDate
                                                                                                                                         }));
                                    }

                                    computedWageHourDetails = computedWageHourDetails.OrderBy(i => i.EffectiveDate).ToList();
                                    var contract = new POPClaimEmploymentContract
                                                   {
                                                       Id                      = id,
                                                       EmploymentInformationId = (int) employmentId,
                                                       JobTypeId               = employment.JobTypeId,
                                                       JobTypeName             = employment.JobType?.Name,
                                                       JobBeginDate            = employment.JobBeginDate?.ToString("MM/dd/yyyy"),
                                                       JobEndDate              = employment.JobEndDate?.ToString("MM/dd/yyyy"),
                                                       JobPosition             = employment.JobPosition,
                                                       CompanyName             = employment.CompanyName,
                                                       IsSelected              = id != 0,
                                                       DeletedReasonId         = employment.DeleteReasonId,
                                                       StartingWage            = computedWageHourDetails.FirstOrDefault()?.ComputedWageRateValue ?? employment.WageHour.ComputedCurrentWageRateValue,
                                                       StartingWageUnit        = computedWageHourDetails.FirstOrDefault()?.ComputedWageRateUnit  ?? employment.WageHour.ComputedCurrentWageRateUnit,
                                                   };
                                    contracts.Add(contract);
                                });
            return contracts;
        }

        public void UpsertPOPClaim(POPClaimContract contract, bool isSystemGenerated = false)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            var popClaim     = contract.Id == 0 ? _popClaimRepository.New() : _popClaimRepository.Get(i => i.Id == contract.Id && !i.IsDeleted);
            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = _authUser.CDODate ?? DateTime.Now;
            var anySubmittedHighWagePOPClaims = _popClaimRepository.GetMany(i => i.ParticipantId == contract.ParticipantId && !i.IsDeleted && i.POPClaimType.Code == POPClaimType.JobAttainmentWithHighWageCd)
                                                                   .SelectMany(i => i.POPClaimStatuses
                                                                                     .OrderByDescending(j => j.Id)
                                                                                     .Take(1))
                                                                   .Any(i => i.POPClaimStatusType.Code == POPClaimStatusType.SubmitCd);

            if (contract.Id == 0)
            {
                popClaim.ParticipantId        = contract.ParticipantId;
                popClaim.ClaimPeriodBeginDate = contract.ClaimPeriodBeginDate;
                popClaim.ClaimEffectiveDate   = contract.ClaimEffectiveDate;
                popClaim.POPClaimTypeId       = contract.POPClaimTypeId;
                popClaim.OrganizationId       = _organizationRepository.Get(i => i.EntsecAgencyCode.ToLower().Trim() == _authUser.AgencyCode.ToLower().Trim()).Id;
            }
            else
                if (popClaim.IsProcessed == true)
                    popClaim.IsProcessed = false;

            if (isSystemGenerated)
                InsertPOPActivities(contract, popClaim, modifiedBy, modifiedDate);
            else
            {
                UpsertPOPEmployments(contract, popClaim, modifiedBy, modifiedDate);

                if (contract.Id == 0 && !anySubmittedHighWagePOPClaims && contract.POPClaimTypeId == _popClaimTypeRepository.Get(i => i.Code == POPClaimType.JobAttainmentCd).Id)
                {
                    if (ValidatePreCheckForJobAttainmentWithHighWagePOPClaim(contract))
                    {
                        popClaim.POPClaimTypeId = _popClaimTypeRepository.Get(i => i.Code == POPClaimType.JobAttainmentWithHighWageCd).Id;
                    }
                    else
                    {
                        contract.POPClaimTypeId   = _popClaimTypeRepository.Get(i => i.Code == POPClaimType.JobAttainmentCd).Id;
                        contract.POPClaimTypeCode = _popClaimTypeRepository.Get(i => i.Code == POPClaimType.JobAttainmentCd).Code;
                    }
                }
            }

            CreateWorkerTaskItem(contract);

            UpsertPOPClaimStatus(contract, popClaim, modifiedBy, modifiedDate);

            popClaim.ModifiedBy   = modifiedBy;
            popClaim.ModifiedDate = modifiedDate;

            if (contract.Id == 0)
                _popClaimRepository.Add(popClaim);
            else
                _popClaimRepository.Update(popClaim);

            _unitOfWork.Commit();
        }

        public bool ValidatePreCheckForJobAttainmentWithHighWagePOPClaim(POPClaimContract contract)
        {
            contract.POPClaimTypeId   = _popClaimTypeRepository.Get(i => i.Code == POPClaimType.JobAttainmentWithHighWageCd).Id;
            contract.POPClaimTypeCode = _popClaimTypeRepository.Get(i => i.Code == POPClaimType.JobAttainmentWithHighWageCd).Code;

            return PreAddCheck(contract).CanAddPOPClaim;
        }

        public void UpsertPOPClaimStatus(POPClaimContract popClaimContract, POPClaim popClaim, string modifiedBy, DateTime modifiedDate)
        {
            var popClaimStatus = _popClaimStatusRepository.New();
            var popStatusType = popClaimContract.IsSubmit
                                    ? _popClaimStatusTypeRepository.Get(i => i.Code == POPClaimStatusType.SubmitCd)
                                    : popClaimContract.IsWithdraw
                                        ? _popClaimStatusTypeRepository.Get(i => i.Code == POPClaimStatusType.WithdrewCd)
                                        : _popClaimStatusTypeRepository.Get(i => i.Id   == popClaimContract.ClaimStatusTypeId);
            popClaimStatus.POPClaim             = popClaim;
            popClaimStatus.POPClaimStatusTypeId = popStatusType.Id;
            popClaimStatus.POPClaimStatusDate   = modifiedDate;
            popClaimStatus.Details              = popClaimContract.Details;
            popClaimStatus.ModifiedBy           = modifiedBy;
            popClaimStatus.ModifiedDate         = modifiedDate;

            _popClaimStatusRepository.Add(popClaimStatus);
        }

        public void InsertPOPActivities(POPClaimContract popClaimContract, POPClaim popClaim, string modifiedBy, DateTime modifiedDate)
        {
            if (popClaimContract.ActivityId == null)
                throw new ArgumentNullException(nameof(popClaimContract.ActivityId));

            var popActivitiesFromBridge = _popClaimActivityBridgeRepository.New();

            popActivitiesFromBridge.POPClaim     = popClaim;
            popActivitiesFromBridge.ActivityId   = popClaimContract.ActivityId.Value;
            popActivitiesFromBridge.ModifiedDate = modifiedDate;
            popActivitiesFromBridge.ModifiedBy   = modifiedBy;

            _popClaimActivityBridgeRepository.Add(popActivitiesFromBridge);
        }

        public void UpsertPOPEmployments(POPClaimContract popClaimContract, POPClaim popClaim, string modifiedBy, DateTime modifiedDate)
        {
            var popEmployments         = _popClaimEmploymentBridgeRepository.GetMany(i => i.POPClaimId == popClaimContract.Id).ToList();
            var allIds                 = popEmployments.Select(i => i.Id).ToList();
            var contractIds            = popClaimContract.POPClaimEmployments.Where(i => i.IsSelected).Select(i => i.Id).ToList();
            var idsToDelete            = allIds.Except(contractIds.AsNotNull()).ToList();
            var popEmploymentsToDelete = popEmployments.Where(i => idsToDelete.Contains(i.Id)).Select(i => i).ToList();
            var popEmploymentsToUpdate = popClaimContract.POPClaimEmployments.Where(i => i.Id != 0 && i.IsSelected).Select(i => i).ToList();
            var popEmploymentsToAdd    = popClaimContract.POPClaimEmployments.Where(i => i.Id == 0 && i.IsSelected).Select(i => i).ToList();

            _popClaimEmploymentBridgeRepository.DeleteRange(popEmploymentsToDelete);

            popEmploymentsToUpdate.AsNotNull().ForEach(popEmployment =>
                                                       {
                                                           var popEmploymentFromBridge = popEmployments.FirstOrDefault(i => i.Id == popEmployment.Id);

                                                           if (popEmploymentFromBridge == null)
                                                               return;

                                                           popEmploymentFromBridge.POPClaim                = popClaim;
                                                           popEmploymentFromBridge.EmploymentInformationId = popEmployment.EmploymentInformationId;
                                                           popEmploymentFromBridge.Earnings                = popEmployment.Earnings;
                                                           popEmploymentFromBridge.HoursWorked             = popEmployment.HoursWorked;
                                                           popEmploymentFromBridge.IsPrimary               = popEmployment.IsPrimary;
                                                           popEmploymentFromBridge.ModifiedBy              = modifiedBy;
                                                           popEmploymentFromBridge.ModifiedDate            = modifiedDate;
                                                       });

            popEmploymentsToAdd.AsNotNull().ForEach(popEmployment =>
                                                    {
                                                        var popEmploymentFromBridge = _popClaimEmploymentBridgeRepository.New();

                                                        popEmploymentFromBridge.POPClaim                = popClaim;
                                                        popEmploymentFromBridge.EmploymentInformationId = popEmployment.EmploymentInformationId;
                                                        popEmploymentFromBridge.Earnings                = popEmployment.Earnings;
                                                        popEmploymentFromBridge.HoursWorked             = popEmployment.HoursWorked;
                                                        popEmploymentFromBridge.ModifiedBy              = modifiedBy;
                                                        popEmploymentFromBridge.ModifiedDate            = modifiedDate;
                                                        popEmploymentFromBridge.IsPrimary               = popEmployment.IsPrimary;
                                                        _popClaimEmploymentBridgeRepository.Add(popEmploymentFromBridge);
                                                    });
        }

        public void CreateWorkerTaskItem(POPClaimContract contract)
        {
            if (contract.POPClaimTypeId == 0)
                throw new ArgumentNullException(nameof(contract));

            var claimTypeCode = _popClaimTypeRepository.Get(i => i.Id == contract.POPClaimTypeId).Code;
            var key           = new Tuple<string, string>(claimTypeCode, contract.ClaimStatusTypeCode);

            if (POPStatusToClaimMap.POPStatusToClaimMaps.ContainsKey(key))
            {
                var workerTaskList         = new WorkerTaskListContract();
                var workerTaskCategoryCode = POPStatusToClaimMap.POPStatusToClaimMaps[key];
                var workerCategory         = _workerTaskCategoryRepository.Get(i => i.Code == workerTaskCategoryCode);
                var participant            = _participantRepository.Get(i => i.Id          == contract.ParticipantId);
                var authsToCheck           = new List<string> { Authorization.canAccessAdjudicatorPOP_Edit , Authorization.canAccessApproverPOP_Edit };

                workerTaskList.CategoryId    = workerCategory.Id;
                workerTaskList.ParticipantId = contract.ParticipantId;
                workerTaskList.TaskDetails   = workerCategory.Description;
                workerTaskList.WorkerId      = ParticipantHelper.GetMostRecentEnrolledProgram(participant, _authUser, _authUser.Authorizations.Intersect(authsToCheck).Any(), true).Worker.Id;

                _workerTaskListDomain.UpsertWorkerTaskList(workerTaskList, true, false);
            }
        }

        public PreAddingPOPClaimContract PreAddCheck(POPClaimContract contract)
        {
            var popClaimStatusToConsider = (contract.POPClaimTypeCode == (POPClaimType.JobAttainmentWithHighWageCd ) || contract.POPClaimTypeCode == (POPClaimType.LongTermCd) || contract.POPClaimTypeCode == (POPClaimType.JobAttainmentCd)) ? new List<string> { POPClaimStatusType.ApproveCd, POPClaimStatusType.ValidateCd } :  new List<string> { POPClaimStatusType.ApproveCd };
            var possibleRuleReasons = _ruleReasonRepository.GetMany(i => i.Category       == Wwp.Model.Interface.Constants.RuleReason.POPClaim
                                                                         && i.SubCategory == Wwp.Model.Interface.Constants.RuleReason.PreCheckError)
                                                           .ToList();
            var organizationId          = _organizationRepository.Get(i => !i.IsDeleted && i.EntsecAgencyCode == _authUser.AgencyCode).Id;
            var primaryJob              = contract.POPClaimEmployments?.FirstOrDefault(j => j.IsPrimary);
            var startingWageHourDetails = new StartingWageHourDetails() ;
            var previousYearDate        = DateTime.Today.AddMonths(-12);
            var popClaimsInAgency = _popClaimRepository.GetMany(i => !i.IsDeleted && i.ParticipantId  == contract.ParticipantId
                                                                                  && i.OrganizationId == organizationId
                                                                                  && popClaimStatusToConsider.Contains(i.POPClaimStatuses
                                                                                                                        .OrderByDescending(j => j.Id)
                                                                                                                        .FirstOrDefault()
                                                                                                                        .POPClaimStatusType
                                                                                                                        .Code)
                                                                                  && (i.ClaimPeriodBeginDate
                                                                                      ?? i.POPClaimEmploymentBridges
                                                                                          .FirstOrDefault(j => j.IsPrimary)
                                                                                          .EmploymentInformation.JobBeginDate) >= previousYearDate)
                                                       .ToList();
            var peps = _participantEnrolledProgramRepository.GetMany(i => i.ParticipantId                         == contract.ParticipantId
                                                                          && i.Office.ContractArea.OrganizationId == organizationId);
            var activities = _employabilityPlanRepository.GetMany(i => !i.IsDeleted && i.ParticipantId == contract.ParticipantId)
                                                         .SelectMany(i => i.EmploybilityPlanActivityBridges)
                                                         .Select(i => i.Activity)
                                                         .ToList();
            var participant = _participantEnrolledProgramRepository.Get(i => i.ParticipantId == contract.ParticipantId).Participant;
            var latestEmployabilityPlan = _employabilityPlanRepository.GetMany(i => !i.IsDeleted && i.ParticipantId == contract.ParticipantId)
                                                                      .OrderByDescending(i => i.SubmitDate)
                                                                      .FirstOrDefault();
            var hasNoEP = latestEmployabilityPlan == null || latestEmployabilityPlan.SubmitDate != null
                          && DateTime.Compare((DateTime) latestEmployabilityPlan.SubmitDate,
                                              DateTime.Parse(contract.POPClaimEmployments
                                                                     ?.FirstOrDefault(i => i.IsPrimary)
                                                                     ?.JobBeginDate)) >= 0;
            var hasActivity = activities.Any(i => i.StartDate < primaryJob?.JobBeginDate.ToDateMonthDayYear());
            var hasUpfrontActivity = activities.Any(i => i.ActivityType
                                                          .EnrolledProgramEPActivityTypeBridges
                                                          .FirstOrDefault(j => j.EnrolledProgramId == EnrolledProgram.WW)
                                                          ?.IsUpfrontActivity == true
                                                         && i.StartDate < primaryJob?.JobBeginDate.ToDateMonthDayYear() && i.EndDate < primaryJob.JobBeginDate.ToDateMonthDayYear());
            var isValidEmployment = true;
            var hasPlacement = _participantPlacementRepository.GetMany(i => i.ParticipantId  == contract.ParticipantId)
                                                              .Any(i => i.PlacementStartDate < primaryJob?.JobBeginDate.ToDateMonthDayYear());
            var checkDB2ClaimTypExists = GetDB2ClaimTypExistsContract(participant.PinNumber.ToString(), contract);
            var activityPlacement = new ActivityPlacementEPContract
                                    {
                                        HasActivity        = hasActivity,
                                        HasUpfrontActivity = hasUpfrontActivity,
                                        HasPlacement       = hasPlacement,
                                        HasNoEP            = hasNoEP
                                    };
            var today                    = _authUser.CDODate ?? DateTime.Today;
            var hoursAndEarningsContract = new HoursAndEarningsContract();
            var cutOverDateAndFeatureToggleDateDetails = new CutOverDateAndFeatureToggleDateDetails
                                                         {
                                                             CutOverDate                                 = participant.ParticipantEnrolledProgramCutOverBridges.FirstOrDefault(i => i.EnrolledProgram?.ProgramCode.SafeTrim() == EnrolledProgram.W2ProgramCode)?.CutOverDate.ToString("MM/dd/yyyy"),
                                                             IsTodayWithinSixMonthsFromFeatureToggleDate = today.IsBefore(_featureToggleRepository.Get(i => i.ParameterName == SpecialInitiatives.EPCutOverValidationDate).ParameterValue.ToDateMonthDayYear().AddMonths(6))
                                                         };
            var codeLevelMessageContext = new CodeLevelMessageContext
                                          {
                                              PossibleRuleReasons = possibleRuleReasons
                                                                    .Select(possibleRuleReason => new RuleReason
                                                                                                  {
                                                                                                      Category    = possibleRuleReason.Category,
                                                                                                      SubCategory = possibleRuleReason.SubCategory,
                                                                                                      Name        = possibleRuleReason.Name,
                                                                                                      Code        = possibleRuleReason.Code
                                                                                                  })
                                                                    .ToList()
                                          };


            switch (_popClaimTypeRepository.Get(i => i.Id == contract.POPClaimTypeId).Code)
            {
                case POPClaimType.JobAttainmentCd:
                    hoursAndEarningsContract.MinEarnings = MinHoursEarning.MINEARNINGS;
                    hoursAndEarningsContract.MinHours    = MinHoursEarning.MINHOURS;
                    _ruleRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged(Wwp.Model.Interface.Constants.RuleReason.JobAttainmentPOPClaimAdd)));
                    break;
                case POPClaimType.JobRetentionCd:
                    hoursAndEarningsContract.MinEarnings = MinHoursEarning.MINEARNINGSFORJR;
                    hoursAndEarningsContract.MinHours    = MinHoursEarning.MINHOURSFORJR;
                    isValidEmployment                    = ValidateEmploymentDaysAndLeaveOfAbsence(contract.POPClaimEmployments, contract.PinNumber);
                    _ruleRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged(Wwp.Model.Interface.Constants.RuleReason.JobRetentionPOPClaimAdd)));
                    break;
                case POPClaimType.LongTermCd:
                    _ruleRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged(Wwp.Model.Interface.Constants.RuleReason.LongTermParticipantJobAttainment)));
                    break;
                case POPClaimType.JobAttainmentWithHighWageCd:
                    startingWageHourDetails = GetStartingWageHourData(primaryJob, organizationId);
                    _ruleRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged(Wwp.Model.Interface.Constants.RuleReason.JobAttainmentWithHighWage)));
                    break;
            }

            var session = _ruleRepository.CreateSession();

            session.InsertAll(new List<object>
                              {
                                  codeLevelMessageContext,
                                  contract,
                                  popClaimsInAgency,
                                  peps,
                                  activityPlacement,
                                  today.Year,
                                  primaryJob,
                                  contract.POPClaimEmployments?.Where(i => i.IsSelected).Select(i => i).ToList(),
                                  activities,
                                  hoursAndEarningsContract,
                                  isValidEmployment,
                                  participant,
                                  startingWageHourDetails,
                                  today,
                                  cutOverDateAndFeatureToggleDateDetails,
                                  checkDB2ClaimTypExists
                              });

            session.Fire();

            var preAddCheckContract = new PreAddingPOPClaimContract();

            codeLevelMessageContext.CodesAndMessagesByLevel.AsNotNull().ForEach(cml =>
                                                                                {
                                                                                    switch (cml.Level)
                                                                                    {
                                                                                        case CodeLevel.Error:
                                                                                            preAddCheckContract.Errors?.Add(cml.Message);
                                                                                            break;
                                                                                        case CodeLevel.Warning:
                                                                                            preAddCheckContract.Warnings?.Add(cml.Message);
                                                                                            break;
                                                                                        default:
                                                                                            throw new ArgumentOutOfRangeException();
                                                                                    }

                                                                                    preAddCheckContract.AddErrorCodes(cml.Code);
                                                                                });

            preAddCheckContract.CanAddPOPClaim = preAddCheckContract.Errors?.Count == 0;

            return preAddCheckContract;
        }

        private StartingWageHourDetails GetStartingWageHourData(POPClaimEmploymentContract primaryJob, int organizationId)
        {
            return new StartingWageHourDetails
                   {
                       selectedEmploymentsStartingWageHourValue = primaryJob.StartingWage,
                       selectedEmploymentsStartingWageHourUnit  = primaryJob.StartingWageUnit,
                       startingWage                             = _popClaimHighWageRepository.Get(i => i.OrganizationId == organizationId).StartingWage
                   };
        }

        public bool ValidateEmploymentDaysAndLeaveOfAbsence(List<POPClaimEmploymentContract> popClaimEmployments, decimal decimalPin)
        {
            const int minNumberOfEmploymentDays = 93;
            const int maxNumberOfLeaveDays      = 14;

            var isEmploymentValid     = false;
            var gapBetweenEmployments = default(int);
            var today                 = _authUser.CDODate ?? DateTime.Now;
            var selectedEmployments   = _employmentInformationRepository.GetMany(i => i.Participant.PinNumber == decimalPin).Where(emp => popClaimEmployments.Any(popEmp => popEmp.EmploymentInformationId == emp.Id)).ToList();
            var allDatesOfEmployment  = new List<DateTime>();

            if (selectedEmployments.Count == 1)
            {
                if (selectedEmployments[0].Absences.Count == 1)
                {
                    var beginDate = selectedEmployments[0].JobBeginDate ?? today;
                    var endDate   =  selectedEmployments[0].JobEndDate  ?? today;

                    allDatesOfEmployment = beginDate.GetDates(endDate).ToList();

                    isEmploymentValid = allDatesOfEmployment.Distinct().Count() >= minNumberOfEmploymentDays;
                }
            }
            else
            {
                DateTime? previousJobsEndDate = null;

                foreach (var employmentInformation in selectedEmployments)
                {
                    var beginDate = employmentInformation.JobBeginDate ?? today;
                    var endDate   = employmentInformation.JobEndDate   ?? today;

                    allDatesOfEmployment = beginDate.GetDates(endDate).ToList();

                    if (previousJobsEndDate != null)
                    {
                        var parms = new Dictionary<string, object>
                                    {
                                        ["DateFrom"]         = previousJobsEndDate,
                                        ["DateTo"]           = beginDate,
                                        ["IsFederalHoliday"] = 1
                                    };

                        gapBetweenEmployments = _popClaimRepository.ExecFunction<int>("UFN_GetNoOfBusinessDaysBetweenTwoDates", parms).FirstOrDefault();

                        if (gapBetweenEmployments > maxNumberOfLeaveDays)
                            break;
                    }

                    previousJobsEndDate = employmentInformation.JobEndDate ?? today;
                }

                isEmploymentValid = allDatesOfEmployment.Distinct().Count() >= minNumberOfEmploymentDays && gapBetweenEmployments <= maxNumberOfLeaveDays;
            }

            return isEmploymentValid;
        }

        public bool InsertSystemGeneratedPOPClaim(EmployabilityPlan employabilityPlan, string activityTypeCode, string activityCompletionReasonCode, DateTime? activityEndDate, int activityId, string popClaimType)
        {
            var canInsertPOPClaim = true;
            var activityCodes     = popClaimType == POPClaimType.VocationalTrainingCd ? new List<string> { ActivityCode.JS, ActivityCode.TC } : new List<string> { ActivityCode.GE, ActivityCode.HE, ActivityCode.RS };
            var popClaimStatuses  = new List<string> { POPClaimStatusType.ApproveCd, POPClaimStatusType.ValidateCd, POPClaimStatusType.SubmitCd };

            if (employabilityPlan.ParticipantEnrolledProgram.IsW2          &&
                activityCodes.Contains(activityTypeCode)                   &&
                activityCompletionReasonCode == ActivityCompletionReason.V &&
                !GetPOPClaimsWithStatuses(popClaimStatuses, employabilityPlan.Organization.EntsecAgencyCode.SafeTrim().ToLower()).Any(i => i.POPClaimTypeCode == popClaimType && i.ParticipantId == employabilityPlan.ParticipantId))
            {
                var participant = employabilityPlan.Participant;
                var popClaimContract = new POPClaimContract
                                       {
                                           ParticipantId        = participant.Id,
                                           POPClaimTypeId       = _popClaimTypeRepository.Get(i => i.Code == popClaimType).Id,
                                           ClaimPeriodBeginDate = activityEndDate,
                                           ClaimEffectiveDate   = activityEndDate,
                                           IsSubmit             = true,
                                           ClaimStatusTypeCode  = POPClaimStatusType.SubmitCd,
                                           ActivityId           = activityId
                                       };

                if (GetDB2ClaimTypExistsContract(participant.PinNumber.ToString(), popClaimContract).HasClaimTypExists == false)
                {
                    UpsertPOPClaim(popClaimContract, true);

                    canInsertPOPClaim = false;
                }
            }

            return canInsertPOPClaim;
        }

        private CheckDB2ClaimTypExists GetDB2ClaimTypExistsContract(string pinNumber, POPClaimContract contract)
        {
            CheckDB2ClaimTypExists dB2ClaimTypExistsContract;

            if (DateTime.Today.IsBefore(_featureToggleRepository.Get(i => i.ParameterName == SpecialInitiatives.POPClaims).ParameterValue.ToDateMonthDayYear().AddMonths(6)))
            {
                var parms = new Dictionary<string, object>
                            {
                                ["PinNumber"]      = pinNumber,
                                ["ClaimTypeCode"]  = contract.POPClaimTypeCode,
                                ["ClaimBeginDate"] = contract.ClaimPeriodBeginDate?.ToString("yyyy-MM-dd") ?? contract.POPClaimEmployments?.FirstOrDefault(i => i.IsPrimary)?.JobBeginDate.ToDateMonthDayYear().ToString("yyyy-MM-dd"),
                                ["EntSecAgencyCd"] = _authUser.AgencyCode
                            };

                dB2ClaimTypExistsContract = _popClaimRepository.ExecStoredProc<CheckDB2ClaimTypExists>("USP_CheckDB2ClaimTypExists", parms).FirstOrDefault();
            }
            else
                dB2ClaimTypExistsContract = new CheckDB2ClaimTypExists
                                            {
                                                PinNumber         = pinNumber,
                                                HasClaimTypExists = false
                                            };

            return dB2ClaimTypExistsContract;
        }

        #endregion

        public class ActivityPlacementEPContract
        {
            public bool HasActivity        { get; set; }
            public bool HasUpfrontActivity { get; set; }
            public bool HasPlacement       { get; set; }
            public bool HasNoEP            { get; set; }
        }

        public class HoursAndEarningsContract
        {
            public int MinHours    { get; set; } = 110;
            public int MinEarnings { get; set; } = 870;
        }

        public class StartingWageHourDetails
        {
            public decimal? selectedEmploymentsStartingWageHourValue { get; set; }
            public string   selectedEmploymentsStartingWageHourUnit  { get; set; }
            public decimal  startingWage                             { get; set; }
        }

        private class ComputedWageHourDetails
        {
            public decimal?  ComputedWageRateValue { get; set; }
            public string    ComputedWageRateUnit  { get; set; }
            public DateTime? EffectiveDate         { get; set; }
        }

        public class CutOverDateAndFeatureToggleDateDetails
        {
            public string CutOverDate                                 { get; set; }
            public bool   IsTodayWithinSixMonthsFromFeatureToggleDate { get; set; }
        }

        public class CheckDB2ClaimTypExists
        {
            public string PinNumber         { get; set; }
            public bool?  HasClaimTypExists { get; set; }
        }
    }
}
