using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using NRules;
using NRules.Fluent;
using DatetimeExtensions = DCF.Common.Extensions.DatetimeExtensions;
using EmployerOfRecordType = Dcf.Wwp.Model.Interface.Constants.EmployerOfRecordType;
using JobType = Dcf.Wwp.Model.Interface.Constants.JobType;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;
using WorkerTaskStatus = Dcf.Wwp.Model.Interface.Constants.WorkerTaskStatus;

namespace Dcf.Wwp.Api.Library.ViewModels.WorkHistoryApp
{
    public class EmploymentHistoryViewModel : BasePinViewModel
    {
        #region Properties

        private readonly IGoogleApi               _googleApi;
        private readonly IWeeklyHoursWorkedDomain _weeklyHoursWorkedDomain;
        private readonly ITransactionDomain       _transactionDomain;

        #endregion

        #region Methods

        public EmploymentHistoryViewModel(IGoogleApi googleApi, IRepository repository, IAuthUser authUser, IWeeklyHoursWorkedDomain weeklyHoursWorkedDomain, ITransactionDomain transactionDomain) : base(repository, authUser)
        {
            _googleApi               = googleApi;
            _weeklyHoursWorkedDomain = weeklyHoursWorkedDomain;
            _transactionDomain       = transactionDomain;
        }

        public IEnumerable<EmploymentInfoContract> GetParticipantEmployments()
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var employmentList = new List<EmploymentInfoContract>();

            var employmentHistory = Participant.AllEmploymentInformations
                                               .OrderByDescending(x => x.JobBeginDate)
                                               .ThenByDescending(x => x.JobEndDate == null)
                                               .ToList();

            employmentHistory.ForEach(ei =>
                                      {
                                          employmentList.Add(
                                                             new EmploymentInfoContract
                                                             {
                                                                 Id                        = ei.Id,
                                                                 CompanyName               = ei.CompanyName.NullStringToBlank(),
                                                                 JobTypeId                 = ei.JobTypeId,
                                                                 JobPosition               = ei.JobPosition.NullStringToBlank(),
                                                                 JobTypeName               = ei.JobType?.Name.SafeTrim(),
                                                                 JobBeginDate              = ei.JobBeginDate?.ToString("MM/dd/yyyy"),
                                                                 LeavingReasonName         = ei.LeavingReason?.Name,
                                                                 RowVersion                = ei.RowVersion,
                                                                 IsCurrentlyEmployed       = ei.IsCurrentlyEmployed,
                                                                 TotalSubsidizedHours      = ei.TotalSubsidizedHours,
                                                                 EmployerOfRecordId        = ei.EmployerOfRecordTypeId,
                                                                 JobEndDate                = ei.IsCurrentlyEmployed.HasValue && ei.IsCurrentlyEmployed.Value ? null : ei.JobEndDate?.ToString("MM/dd/yyyy"),
                                                                 EmploymentProgramTypeName = ei.EmploymentProgramType?.Name,
                                                                 Location                  = LocationHelper.GetLocationInfo(ei, ei.City),
                                                                 WageHour                  = GetWageHourInfo(ei),
                                                                 Absences                  = GetAbsencesInfo(ei),
                                                                 JobDuties                 = GetJobDutiesInfo(ei),
                                                                 JobAction                 = new JobActionTypeContract { JobActionTypes = new List<int>() },
                                                                 IsConverted               = ei.IsConverted ?? false,
                                                                 DeleteReasonId            = ei.DeleteReasonId,
                                                                 DeleteReasonName          = ei.DeleteReason?.Name,
                                                                 HasEmploymentOnEp         = HasEmploymentOnEp(ei),
                                                                 StreetAddress             = ei.StreetAddress,
                                                                 ZipAddress                = ei.ZipAddress,
                                                                 ContactId                 = ei.ContactId,
                                                                 IsVerified                = ei.EmploymentVerifications.Any(x => x.IsVerified)
                                                             });
                                      });

            return (employmentList);
        }

        public EmploymentInfoContract GetParticipantEmploymentInfo(int eiId)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var ei = Participant.AllEmploymentInformations.FirstOrDefault(i => i.Id == eiId);

            if (ei == null)
            {
                return null;
            }

            return GetParticipantEmploymentContractFromEmploymentInformation(ei);
        }

        private EmploymentInfoContract GetParticipantEmploymentContractFromEmploymentInformation(IEmploymentInformation ei)
        {
            var employerOfRecordInfo = ei.EmployerOfRecordInformations.FirstOrDefault(); // it's a 1:1 relationship

            LocationContract eorLocation = new LocationContract();

            if (employerOfRecordInfo != null)
            {
                eorLocation = LocationHelper.GetLocationInfo(employerOfRecordInfo, employerOfRecordInfo.City);
            }

            var hasEmploymentOnEp = HasEmploymentOnEp(ei);

            var employmentInfoContract = new EmploymentInfoContract
                                         {
                                             Id                        = ei.Id,
                                             CompanyName               = ei.CompanyName.NullStringToBlank(),
                                             JobTypeId                 = ei.JobTypeId,
                                             JobPosition               = ei.JobPosition.NullStringToBlank(),
                                             JobTypeName               = ei.JobType?.Name.SafeTrim(),
                                             JobBeginDate              = ei.JobBeginDate?.ToString("MM/dd/yyyy"),
                                             TotalSubsidizedHours      = ei.TotalSubsidizedHours,
                                             EmploymentProgramTypeId   = ei.EmploymentProgramtypeId,
                                             EmploymentProgramTypeName = ei.EmploymentProgramType?.Name,
                                             Fein                      = ei.Fein,
                                             ExpectedScheduleDetails   = ei.OtherJobInformation?.ExpectedScheduleDetails,
                                             LeavingReasonId           = ei.LeavingReasonId,
                                             LeavingReasonName         = ei.LeavingReason?.Name,
                                             LeavingReasonDetails      = ei.LeavingReasonDetails,
                                             Notes                     = ei.Notes,
                                             ContactId                 = ei.ContactId,
                                             RowVersion                = ei.RowVersion,
                                             ModifiedDate              = ei.ModifiedDate,
                                             ModifiedBy                = ei.ModifiedBy,
                                             ModifiedByName            = Repo.GetWorkerNameByWamsId(ei.ModifiedBy),
                                             IsCurrentlyEmployed       = ei.IsCurrentlyEmployed,
                                             EmployerOfRecordId        = ei.EmployerOfRecordTypeId,
                                             EmployerOfRecordDetails = new EmployerOfRecordDetailContract
                                                                       {
                                                                           Id            = employerOfRecordInfo?.Id,
                                                                           CompanyName   = employerOfRecordInfo?.CompanyName,
                                                                           Fein          = employerOfRecordInfo?.Fein,
                                                                           JobSectorId   = employerOfRecordInfo?.JobSectorId,
                                                                           JobSectorName = employerOfRecordInfo?.JobSector?.Name,
                                                                           ContactId     = employerOfRecordInfo?.ContactId,
                                                                           Location      = eorLocation
                                                                       },
                                             IsConverted            = ei.IsConverted ?? false,
                                             DeleteReasonId         = ei.DeleteReasonId,
                                             DeleteReasonName       = ei.DeleteReason?.Name,
                                             IsCurrentJobAtCreation = ei.IsCurrentJobAtCreation,
                                             HasEmploymentOnEp      = hasEmploymentOnEp,
                                             IsVerified             = ei.EmploymentVerifications.Any(x => x.IsVerified)
            };

            if (ei.IsCurrentlyEmployed.HasValue && ei.IsCurrentlyEmployed.Value)
            {
                employmentInfoContract.JobEndDate = null;
            }
            else
            {
                employmentInfoContract.JobEndDate = ei.JobEndDate?.ToString("MM/dd/yyyy");
            }

            employmentInfoContract.Location = LocationHelper.GetLocationInfo(ei, ei.City);

            //Get Job Duties;
            employmentInfoContract.JobDuties = GetJobDutiesInfo(ei);

            // Benefits offered 
            employmentInfoContract.JobAction = GetJobActionTypeInfo(ei);

            // In program job information display.
            GetOtherJobInformationInfo(ei, employmentInfoContract);

            // Wage Histories
            var wageHour = GetWageHourInfo(ei);

            wageHour.WageHourAction = GetCurrentPayTypeInfo(ei);

            //Get wage hour histories based on EmploymentInfo IsDeleted flag
            if (!ei.IsDeleted)
            {
                if (ei.WageHour?.WageHourHistories != null)
                {
                    wageHour.WageHourHistories = GetWageHourHistoriesInfo(ei.WageHour?.WageHourHistories, wageHour, false);
                }

                employmentInfoContract.WageHour = wageHour;
            }
            else
            {
                if (ei.WageHour?.AllWageHourHistories != null)
                {
                    wageHour.WageHourHistories = GetWageHourHistoriesInfo(ei.WageHour?.AllWageHourHistories, wageHour, true);
                }

                employmentInfoContract.WageHour = wageHour;
            }

            //Get Leave of Absence based on the isDeleted flag on BarrierDetail
            employmentInfoContract.Absences = GetAbsencesInfo(ei);

            employmentInfoContract.Notes = ei.Notes;
            return employmentInfoContract;
        }

        public PreAddingWorkHistoryContract PreAddCheck(string pin, bool isHD, EmploymentInfoContract contract)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var jobType        = Repo.JobTypeById(contract.JobTypeId.GetValueOrDefault());
            var ps             = Participant.ParticipationStatus;
            var employmentInfo = Participant.EmploymentInformations.FirstOrDefault(i => i.Id == contract.Id);
            var popCheck       = Repo.PreCheckPop(pin, employmentInfo?.EmploymentSequenceNumber);

            contract.JobTypeName = jobType.Name;

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category       == RuleReason.WorkHistory
                                                                                                 && i.SubCategory == RuleReason.PreCheckError
                                                                                                 && i.Code        != RuleReason.POP)
                                                                       .ToList()
                                         };

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("WH")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(ps);
            session.Insert(contract);
            session.Insert(popCheck);
            session.Insert(isHD);
            session.Fire();

            var preAddingWorkHistoryContract = new PreAddingWorkHistoryContract();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        preAddingWorkHistoryContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        preAddingWorkHistoryContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow delete if there are any errors.
            preAddingWorkHistoryContract.CanAddWorkHistory = preAddingWorkHistoryContract.Errors?.Count == 0;

            return preAddingWorkHistoryContract;
        }

        public UpsertResponse<IEmploymentInformation> UpsertData(EmploymentInfoContract contract, int id, string pin, string user /*, bool isRecursiveCall = false*/)
        {
            var modifiedDate = DateTime.Now;
            var response     = new UpsertResponse<IEmploymentInformation>();
            var deletedWhh   = contract.WageHour?.WageHourHistories?.Where(i => i.IsDeletedFromCurrent).ToList();
            if (deletedWhh != null && deletedWhh.Count > 0)
                deletedWhh.ForEach(i => contract.WageHour.WageHourHistories.Remove(i));

            const string pastJob        = "pastJob";
            const string currentJob     = "currentJob";
            const string workerAssisted = "Worker Assisted";

            if (contract == null)
            {
                throw new InvalidOperationException("Employment data is missing.");
            }

            var inProgram    = Repo.EmploymentProgramTypeByName(EmployeeProgramType.InProgram);
            var outOfProgram = Repo.EmploymentProgramTypeByName(EmployeeProgramType.OutOfProgram);
            var jobType      = Repo.JobTypeById(contract.JobTypeId.GetValueOrDefault());

            var employmentInfo = ((id == 0) ? Repo.NewEmploymentInfo(Participant, user) : Repo.EmploymentById(id));

            if (employmentInfo != null)
            {
                var rowVersion = contract.RowVersion;

                Repo.StartChangeTracking(employmentInfo);

                employmentInfo.JobTypeId    = contract.JobTypeId;
                employmentInfo.JobBeginDate = contract.JobBeginDate.ToDateTimeMonthDayYear();


                // If user checks for currently employment indicator job end date is null
                employmentInfo.IsCurrentlyEmployed = contract.IsCurrentlyEmployed;

                if (contract.IsCurrentlyEmployed == true)
                {
                    employmentInfo.JobEndDate = null;
                    contract.JobEndDate       = null;
                }
                else
                {
                    employmentInfo.JobEndDate = contract.JobEndDate.ToDateTimeMonthDayYear();
                }

                var part = Repo.GetParticipant(pin);
                var peps = part.ParticipantEnrolledPrograms.ToList();

                var jobCategory = WhichJobCategory(employmentInfo.JobEndDate, contract.IsCurrentlyEmployed);

                var isCurrentJob = currentJob.Equals(jobCategory);
                var isPastJob    = pastJob.Equals(jobCategory);

                if (jobCategory != null)
                {
                    employmentInfo.EmploymentProgramtypeId = IsInProgramjob(peps, contract.JobBeginDate.ToDateTimeMonthDayYear()) ? inProgram : outOfProgram;
                }

                // Basic Information
                employmentInfo.JobPosition = contract.JobPosition;
                employmentInfo.CompanyName = contract.CompanyName;
                employmentInfo.Fein        = contract.Fein;
                employmentInfo.IsConverted = contract.IsConverted;

                ICity                city                = null;
                IState               state               = null;
                ICountry             country             = null;
                IOtherJobInformation otherJobInformation = null;

                // Save All world cities 
                employmentInfo.City          = Repo.GetOrCreateCity(contract.Location, _googleApi.GetPlaceDetails, _googleApi.GetLatLong, user);
                employmentInfo.StreetAddress = contract.Location?.FullAddress;
                employmentInfo.ZipAddress    = contract.Location?.ZipAddress;
                employmentInfo.ContactId     = contract.ContactId;

                // Job Duties 
                employmentInfo.EmploymentInformationJobDutiesDetailsBridges.ForEach(jdb => jdb.IsDeleted = true);

                if (contract.JobDuties != null)
                {
                    // Remove the empty records... they can be ignored.
                    var jobduties = contract.JobDuties.WithoutEmpties();

                    if (jobduties.Count == 0)
                    {
                        employmentInfo.AllEmploymentInformationJobDutiesDetailsBridges.ForEach(i => i.IsDeleted = true);
                    }
                    else
                    {
                        foreach (var duty in jobduties)
                        {
                            IEmploymentInformationJobDutiesDetailsBridge iojb = null;
                            IJobDutiesDetail                             jdd  = null;

                            jdd  = Repo.JobDutyById(duty.Id) ?? Repo.NewJobDuty(user);
                            iojb = employmentInfo.AllEmploymentInformationJobDutiesDetailsBridges?.FirstOrDefault(z => z.JobDutiesId == duty.Id);

                            if (iojb != null && duty.Id != 0)
                            {
                                iojb.ModifiedDate    = modifiedDate;
                                iojb.ModifiedBy      = user;
                                iojb.IsDeleted       = false;
                                jdd.Details          = duty.Details.SafeTrim();
                                iojb.JobDutiesDetail = jdd;
                            }
                            else
                            {
                                iojb                 = Repo.NewEmploymentInformationJobDutiesDetailsBridge(employmentInfo, user);
                                jdd.Details          = duty.Details.SafeTrim();
                                iojb.JobDutiesDetail = jdd;
                            }

                            employmentInfo.EmploymentInformationJobDutiesDetailsBridges.Add(iojb);
                        }
                    }
                }

                // Benefits Offered is only displayed for Current or In-Program jobs.
                if (isCurrentJob || contract.JobAction?.JobActionTypes != null)
                {
                    // TODO: Fix Sprint 15

                    employmentInfo.EmploymentInformationBenefitsOfferedTypeBridges.ForEach(eibo => eibo.IsDeleted = true);

                    //if (contract.JobAction?.JobActionTypes != null)
                    //{
                    var jobActions = contract.JobAction?.JobActionTypes;

                    if (jobActions.Count == 0)
                    {
                        // TODO: Fix Sprint 15
                        employmentInfo.AllEmploymentInformationBenefitsOfferedTypeBridges.ForEach(i => i.IsDeleted = true);
                    }
                    else
                    {
                        foreach (var x in jobActions)
                        {
                            // TODO: Look into Id.
                            // TODO: Fix Sprint 15

                            var restore = employmentInfo.AllEmploymentInformationBenefitsOfferedTypeBridges?.FirstOrDefault(z => z.BenefitsOfferedTypeId == x);

                            if (restore != null)
                            {
                                restore.ModifiedDate = modifiedDate;
                                restore.ModifiedBy   = user;
                                restore.IsDeleted    = false;
                            }
                            else
                            {
                                IEmploymentInformationBenefitsOfferedTypeBridge iojb = null;
                                iojb                       = Repo.NewJobBenefitsOfferedActionBridge(employmentInfo, user);
                                iojb.BenefitsOfferedTypeId = x;
                                employmentInfo.EmploymentInformationBenefitsOfferedTypeBridges.Add(iojb);
                            }
                        }
                    }

                    //}
                }
                //else
                //{
                //    employmentInfo.EmploymentInformationBenefitsOfferedTypeBridges.ForEach(bo => bo.IsDeleted = true);
                //}

                // Other job information
                otherJobInformation = Repo.OtherJobInformationById(employmentInfo.OtherJobInformationId) ?? Repo.NewOtherJobInformation(user);
                //  Job information is displayed and required for In-Program Jobs handling this on the front end...

                var jobFoundMethodId = Repo.JobFoundMethodByName(workerAssisted);
                otherJobInformation.JobFoundMethodId      = contract.JobFoundMethodId;
                otherJobInformation.WorkerId              = contract.JobFoundMethodId == jobFoundMethodId.Id ? contract.WorkerId : null;
                otherJobInformation.JobFoundMethodDetails = contract.JobFoundMethodDetails;
                otherJobInformation.WorkProgramId         = contract.WorkProgramId;

                // Expected Schedule is only displayed for current
                if (isCurrentJob || contract.ExpectedScheduleDetails != null)
                {
                    otherJobInformation.ExpectedScheduleDetails = contract.ExpectedScheduleDetails.SafeTrim() != "" ? contract.ExpectedScheduleDetails : null;
                }
                else
                {
                    otherJobInformation.ExpectedScheduleDetails = null;
                }

                IJobType jt = null;
                jt = Repo.JobTypeById(employmentInfo.JobTypeId);

                if (jt != null)
                {
                    if (jt.IsRequired.HasValue && jt.IsRequired.Value)
                    {
                        otherJobInformation.JobSectorId = contract.JobSectorId;
                    }
                    else
                    {
                        otherJobInformation.JobSectorId = null;
                    }
                }

                employmentInfo.OtherJobInformation = otherJobInformation;

                // Leaving reason is displayed only when Past Jobs When an End Date is added for a Current Job
                // When an End Date is added for an In-Program Job

                if (isPastJob)
                {
                    employmentInfo.LeavingReasonId      = contract.LeavingReasonId;
                    employmentInfo.LeavingReasonDetails = contract.LeavingReasonDetails;
                }
                else
                {
                    employmentInfo.LeavingReasonId      = null;
                    employmentInfo.LeavingReasonDetails = null;
                }

                // Wage Hour
                // If Participant is in current job Wage History list is saved.
                IWageHour wageHour = null;
                wageHour              = Repo.WageHourById(employmentInfo.WageHoursId) ?? Repo.NewWageHour(user);
                wageHour.ModifiedDate = modifiedDate;
                wageHour.WageHourWageTypeBridges.ForEach(whtb => whtb.IsDeleted = true);

                if (contract.WageHour?.WageHourAction?.JobActionTypes != null)
                {
                    var jobActions = contract.WageHour?.WageHourAction?.JobActionTypes;

                    if (jobActions.Count == 0)
                    {
                        wageHour.AllWageHourWageTypeBridges.ForEach(i => i.IsDeleted = true);
                    }
                    else
                    {
                        foreach (var x in jobActions)
                        {
                            // Fix ID type.
                            var restore = wageHour.AllWageHourWageTypeBridges?.FirstOrDefault(z => z.WageTypeId == x);

                            if (restore != null)
                            {
                                restore.ModifiedDate = modifiedDate;
                                restore.ModifiedBy   = user;
                                restore.IsDeleted    = false;
                            }
                            else
                            {
                                IWageHourWageTypeBridge iwjb = null;
                                iwjb              = Repo.NewWageHourWageTypeBridge(wageHour, user);
                                iwjb.WageTypeId   = x;
                                iwjb.ModifiedBy   = user;
                                iwjb.ModifiedDate = modifiedDate;
                                wageHour.WageHourWageTypeBridges.Add(iwjb);
                            }
                        }
                    }
                }

                //  edit wagehour information about the participant's Current or In-Program work history.
                if (isCurrentJob)
                {
                    wageHour.CurrentEffectiveDate         = contract.WageHour?.CurrentEffectiveDate.ToDateTimeMonthDayYear();
                    wageHour.CurrentPayTypeDetails        = contract.WageHour?.CurrentPayTypeDetails;
                    wageHour.CurrentAverageWeeklyHours    = contract.WageHour?.CurrentAverageWeeklyHours.ToDecimal();
                    wageHour.ComputedCurrentWageRateUnit  = contract.WageHour?.ComputedCurrentWageRateUnit;
                    wageHour.ComputedPastEndWageRateUnit  = contract.WageHour?.ComputedPastEndWageRateUnit;
                    wageHour.ComputedCurrentWageRateValue = contract.WageHour?.ComputedCurrentWageRateValue.ToDecimal();
                    wageHour.ComputedPastEndWageRateValue = contract.WageHour?.ComputedPastEndWageRateValue.ToDecimal();

                    if (jobType.Name == JobType.Volunteer)
                    {
                        wageHour.CurrentPayRate           = null;
                        wageHour.CurrentHourlySubsidyRate = null;
                        wageHour.WorkSiteContribution     = null;
                        wageHour.CurrentPayRateIntervalId = null;
                    }
                    else
                    {
                        wageHour.CurrentPayRate           = contract.WageHour?.CurrentPayRate.ToDecimal();
                        wageHour.CurrentHourlySubsidyRate = contract.WageHour?.CurrentHourlySubsidyRate.ToDecimal();
                        wageHour.CurrentPayRateIntervalId = contract.WageHour?.CurrentPayRateIntervalId;
                        wageHour.WorkSiteContribution     = contract.WageHour?.WorkSiteContribution.ToDecimal();
                    }

                    // No pay is selected thus clear pay rate info.
                    if (contract.WageHour.WageHourAction.JobActionTypes.IndexOf(WageTypeLookup.NoPay) > -1)
                    {
                        wageHour.CurrentPayRate           = null;
                        wageHour.CurrentHourlySubsidyRate = null;
                        wageHour.WorkSiteContribution     = null;
                        wageHour.CurrentPayRateIntervalId = null;
                    }

                    // clearing the values if the pastJob is converted to current Job
                    wageHour.PastBeginPayRate                = null;
                    wageHour.PastBeginPayRateIntervalId      = null;
                    wageHour.PastEndPayRate                  = null;
                    wageHour.PastEndPayRateIntervalId        = null;
                    wageHour.IsUnchangedPastPayRateIndicator = null;
                    // Here we soft delete histories 

                    var wageHourhistoriesByIdModel = (from y in contract.WageHour?.WageHourHistories select y.Id).ToList();
                    var wageHourhistoriesByIdDb    = (from y in wageHour.WageHourHistories select y.Id).ToList();
                    DeleteFromList(wageHour.WageHourHistories, wageHourhistoriesByIdModel, wageHourhistoriesByIdDb);

                    #region Restoration wageHourHistorical list

                    // list of histories from incoming contract.
                    var historiesModel = contract.WageHour?.WageHourHistories;

                    // List of all histories from database   
                    var wageHourHistory            = wageHour.AllWageHourHistories.Select(i => i);
                    var wageHourHistoryRestoreList = new List<WageHourHistoryContract>();
                    var sortOrder                  = 0;

                    foreach (var x in contract.WageHour?.WageHourHistories)
                    {
                        sortOrder++;
                        x.SortOrder = sortOrder;
                    }

                    foreach (var wageHourHistoryrecord in wageHourHistory)
                    {
                        foreach (var m in historiesModel)
                        {
                            //Get list of WageType Ids from Model and Database
                            //compare to check of they are the same
                            var wageHourHistoryWageTypeByIdModel = m.HistoryPayType.JobActionTypes.ToList();
                            var wageHourHistoryWageTypeByIdDb    = (from y in wageHourHistoryrecord.WageHourHistoryWageTypeBridges where !y.IsDeleted select y.Id).ToList();
                            var modelVsDb = !wageHourHistoryWageTypeByIdModel.Except(wageHourHistoryWageTypeByIdDb).ToList().Any() &&
                                            !wageHourHistoryWageTypeByIdDb.Except(wageHourHistoryWageTypeByIdModel).ToList().Any();

                            if (wageHourHistoryrecord.EffectiveDate        == m.EffectiveDate?.ToDateTimeMonthDayYear() &&
                                wageHourHistoryrecord.PayRate              == m.PayRate.ToDecimal()                     &&
                                wageHourHistoryrecord.HourlySubsidyRate    == m.HourlySubsidyRate.ToDecimal()           &&
                                wageHourHistoryrecord.WorkSiteContribution == m.WorkSiteContribution.ToDecimal()        &&
                                wageHourHistoryrecord.AverageWeeklyHours   == m.AverageWeeklyHours.ToDecimal()          &&
                                wageHourHistoryrecord.PayTypeDetails       == m.PayTypeDetails                          &&
                                wageHourHistoryrecord.PayRateIntervalId    == m.PayRateIntervalId                       &&
                                modelVsDb)
                            {
                                wageHourHistoryrecord.SortOrder = m.SortOrder;
                                wageHourHistoryrecord.IsDeleted = false;
                                wageHourHistoryRestoreList.Add(m);
                            }
                        }
                    }

                    wageHourHistoryRestoreList.ForEach(r => contract.WageHour?.WageHourHistories.Remove(r));

                    #endregion

                    foreach (var history in contract.WageHour?.WageHourHistories)
                    {
                        IWageHourHistory whh = null;
                        whh = wageHour.WageHourHistories.SingleOrDefault(x => x.Id == history.Id && history.Id != 0);

                        if (whh == null || whh.Id == 0)
                        {
                            whh = Repo.NewWageHourHistory(wageHour, user);
                        }

                        wageHour = ProcessWageHourHistory(wageHour, whh, history, jobType, user, modifiedDate);
                    }

                    //When WageHour is moved WageHourHistory and deleted before save
                    deletedWhh?.ForEach(i =>
                                        {
                                            IWageHourHistory whh = null;
                                            whh           = Repo.NewWageHourHistory(wageHour, user);
                                            whh.IsDeleted = true;

                                            wageHour = ProcessWageHourHistory(wageHour, whh, i, jobType, user, modifiedDate);
                                        });
                }
                else
                {
                    if (isPastJob && jobCategory != null)
                    {
                        wageHour.ComputedCurrentWageRateUnit  = contract.WageHour?.ComputedCurrentWageRateUnit;
                        wageHour.ComputedPastEndWageRateUnit  = contract.WageHour?.ComputedPastEndWageRateUnit;
                        wageHour.ComputedCurrentWageRateValue = contract.WageHour?.ComputedCurrentWageRateValue.ToDecimal();
                        wageHour.ComputedPastEndWageRateValue = contract.WageHour?.ComputedPastEndWageRateValue.ToDecimal();


                        //wageHour.WageHourHistories.ForEach(i =>
                        //                                   {
                        //                                       i.IsDeleted = true;
                        //                                       i.WageHourHistoryWageTypeBridges.ForEach(j => j.IsDeleted = true) ;
                        //                                   });

                        wageHour.IsUnchangedPastPayRateIndicator = contract.WageHour?.IsUnchangedPastPayRateIndicator;

                        if (wageHour.IsUnchangedPastPayRateIndicator.HasValue && wageHour.IsUnchangedPastPayRateIndicator.Value)
                        {
                            if (jobType?.Name == JobType.Volunteer)
                            {
                                wageHour.PastBeginPayRate           = null;
                                wageHour.PastBeginPayRateIntervalId = null;
                            }
                            else
                            {
                                wageHour.PastBeginPayRate           = contract.WageHour?.PastBeginPayRate.ToDecimal();
                                wageHour.PastBeginPayRateIntervalId = contract.WageHour?.PastBeginPayRateIntervalId;
                            }

                            wageHour.PastEndPayRate           = null;
                            wageHour.PastEndPayRateIntervalId = null;
                        }
                        else
                        {
                            if (jobType != null && jobType.Name == JobType.Volunteer)
                            {
                                wageHour.PastBeginPayRate           = null;
                                wageHour.PastBeginPayRateIntervalId = null;
                                wageHour.PastEndPayRate             = null;
                                wageHour.PastEndPayRateIntervalId   = null;
                            }
                            else
                            {
                                wageHour.PastBeginPayRate           = contract.WageHour?.PastBeginPayRate.ToDecimal();
                                wageHour.PastBeginPayRateIntervalId = contract.WageHour?.PastBeginPayRateIntervalId;
                                wageHour.PastEndPayRate             = contract.WageHour?.PastEndPayRate.ToDecimal();
                                wageHour.PastEndPayRateIntervalId   = contract.WageHour?.PastEndPayRateIntervalId;
                            }
                        }

                        wageHour.CurrentEffectiveDate      = contract.WageHour?.CurrentEffectiveDate != null ? contract.WageHour?.CurrentEffectiveDate.ToDateTimeMonthDayYear() : null;
                        wageHour.CurrentPayTypeDetails     = contract.WageHour?.CurrentPayTypeDetails;
                        wageHour.CurrentAverageWeeklyHours = contract.WageHour?.CurrentAverageWeeklyHours.ToDecimal();
                        wageHour.CurrentPayRate            = contract.WageHour?.CurrentPayRate           != null ? contract.WageHour?.CurrentPayRate.ToDecimal() : null;
                        wageHour.CurrentPayRateIntervalId  = contract.WageHour?.CurrentPayRateIntervalId != null ? contract.WageHour?.CurrentPayRateIntervalId : null;
                        wageHour.CurrentHourlySubsidyRate  = contract.WageHour?.CurrentHourlySubsidyRate != null ? contract.WageHour?.CurrentHourlySubsidyRate.ToDecimal() : null ;
                        wageHour.WorkSiteContribution      = contract.WageHour?.WorkSiteContribution     != null ? contract.WageHour?.WorkSiteContribution.ToDecimal() : null;
                    }
                }

                // Leaves of Absence is displayed for Current Jobs or In-Program Jobs only.
                if (isCurrentJob || contract.Absences.Count != 0)
                {
                    // leaveabsence
                    // Save list of  leave absences for particular employment 
                    // Here we soft delete Leave absence .
                    var absenceLeavesByIdModel = (from y in contract.Absences select y.Id).ToList();
                    var absenceLeavesByIdDb    = (from y in employmentInfo.Absences select y.Id).ToList();
                    DeleteFromList(employmentInfo.Absences, absenceLeavesByIdModel, absenceLeavesByIdDb);

                    #region Restoration leave absences list

                    // list of histories from model.
                    var absencesModel = contract.Absences;

                    // List of all histories from Database.   
                    var absences = employmentInfo.AllAbsences.Select(i => i).ToList();

                    var restoreList1 = new List<AbsenceContract>();
                    var sortOrder1   = 0;

                    foreach (var x in contract.Absences)
                    {
                        sortOrder1++;
                        x.SortOrder = sortOrder1;
                    }

                    foreach (var w in absences)
                    {
                        foreach (var m in contract.Absences)
                            if (w.BeginDate       == m.BeginDate?.ToDateTimeMonthYear() &&
                                w.EndDate         == m.EndDate?.ToDateTimeMonthYear()   &&
                                w.AbsenceReasonId == m.AbsenceReasonId                  &&
                                w.Details         == m.Details
                            )
                            {
                                w.SortOrder = m.SortOrder;
                                w.IsDeleted = false;
                                restoreList1.Add(m);
                            }
                    }

                    restoreList1.ForEach(r => contract.Absences.Remove(r));

                    #endregion

                    foreach (var absence in contract.Absences)
                    {
                        IAbsence ab = null;
                        ab = employmentInfo.Absences.SingleOrDefault(x => x.Id == absence.Id && absence.Id != 0);

                        if (ab == null || ab.Id == 0)
                        {
                            ab = Repo.NewAbsence(employmentInfo, user);
                        }

                        //sortOrder++;
                        ab.BeginDate       = absence.BeginDate.ToDateTimeMonthDayYear();
                        ab.EndDate         = absence.EndDate.ToDateTimeMonthDayYear();
                        ab.AbsenceReasonId = absence.AbsenceReasonId;
                        ab.Details         = absence.Details;
                        ab.ModifiedDate    = modifiedDate;
                        ab.ModifiedBy      = user;
                        // TODO: Do not need this statement because we pass parent object when newing wageHour history.
                        employmentInfo.Absences.Add(ab);
                    }
                }
                else
                {
                    employmentInfo.Absences.ForEach(i => i.IsDeleted = true);
                }

                employmentInfo.WageHour = wageHour;
                employmentInfo.Notes    = contract.Notes;
                response.UpdatedModel   = employmentInfo;

                if (employmentInfo.EmployerOfRecordInformations != null)
                {
                    var eorTypeId = (int) contract.EmployerOfRecordId.GetValueOrDefault();
                    var jobTypeId = (int) contract.JobTypeId.GetValueOrDefault();
                    employmentInfo.EmployerOfRecordTypeId = eorTypeId;

                    // this is somewhat convoluted, but it works for now (until phase II)

                    if (eorTypeId == EmployerOfRecordType.Other)
                    {
                        var jobTypeIds = Repo.GetJobTypes(i => (bool) i.IsUsedForEmploymentOfRecord && !i.IsDeleted)
                                             .Select(i => i.Id)
                                             .ToList();

                        if (jobTypeIds.Contains(jobTypeId))
                        {
                            IEmployerOfRecordInformation eor;

                            if (employmentInfo.EmployerOfRecordInformations != null && employmentInfo.EmployerOfRecordInformations.Any())
                            {
                                eor = employmentInfo.EmployerOfRecordInformations.FirstOrDefault(); // there's only one, it's in a collection because that's how the keys are in SqlServer
                            }
                            else
                            {
                                eor = new EmployerOfRecordInformation();
                                var employerOfRecordInformations = employmentInfo.EmployerOfRecordInformations;
                                employerOfRecordInformations.Add(eor);
                                employmentInfo.EmployerOfRecordInformations = employerOfRecordInformations;
                            }

                            city = Repo.GetOrCreateCity(contract.EmployerOfRecordDetails?.Location, _googleApi.GetPlaceDetails, _googleApi.GetLatLong, user);

                            eor.CompanyName   = contract.EmployerOfRecordDetails?.CompanyName;
                            eor.Fein          = contract.EmployerOfRecordDetails?.Fein;
                            eor.StreetAddress = contract.EmployerOfRecordDetails?.Location?.FullAddress;
                            eor.City          = city;
                            eor.ZipAddress    = contract.EmployerOfRecordDetails?.Location?.ZipAddress;
                            eor.JobSectorId   = contract.EmployerOfRecordDetails?.JobSectorId;
                            eor.ContactId     = contract.EmployerOfRecordDetails?.ContactId;
                            eor.ModifiedBy    = user;
                            eor.ModifiedDate  = modifiedDate;
                            //eor.IsDeleted = false;
                        }
                        else
                        {
                            // find the record and blow it away if it exists...
                            if (employmentInfo.EmployerOfRecordInformations != null && employmentInfo.EmployerOfRecordInformations.Any())
                            {
                                employmentInfo.EmployerOfRecordTypeId = null;
                                IEmployerOfRecordInformation eor;
                                eor = employmentInfo.EmployerOfRecordInformations.FirstOrDefault(); // there's only one, it's in a collection because that's how the keys are in SqlServer

                                if (eor != null)
                                {
                                    //if (!isRecursiveCall)
                                    //{
                                    Repo.DeleteEoRInfo(employmentInfo.Id);
                                    //}
                                    //else
                                    //{
                                    //    eor.IsDeleted = true;
                                    //}
                                }
                            }
                        }
                    }
                    else
                    {
                        // find the record and blow it away if it exists...
                        if (employmentInfo.EmployerOfRecordInformations != null && employmentInfo.EmployerOfRecordInformations.Any())
                        {
                            IEmployerOfRecordInformation eor;
                            eor = employmentInfo.EmployerOfRecordInformations.FirstOrDefault(); // there's only one, it's in a collection because that's how the keys are in SqlServer

                            if (eor != null)
                            {
                                //if (!isRecursiveCall)
                                //{
                                Repo.DeleteEoRInfo(employmentInfo.Id);
                                //}
                                //else
                                //{
                                //    eor.IsDeleted = true;
                                //}
                            }
                        }
                        else
                        {
                            employmentInfo.EmployerOfRecordTypeId = contract.EmployerOfRecordId;
                        }
                    }
                }

                if (isCurrentJob)
                {
                    employmentInfo.IsCurrentJobAtCreation = true;
                }
                else
                {
                    employmentInfo.IsCurrentJobAtCreation = contract.IsCurrentJobAtCreation.HasValue == true ? contract.IsCurrentJobAtCreation : false;
                }

                if (employmentInfo.Id != 0 && employmentInfo.WeeklyHoursWorkedEntries?.Count > 0 && (jobType.Name == JobType.TMJSubsidized || jobType.Name == JobType.TJSubsidized))
                {
                    employmentInfo.WeeklyHoursWorkedEntries = _weeklyHoursWorkedDomain.UpdateTotalSubsidyAmount(modifiedDate, employmentInfoInterface: employmentInfo);
                }

                // Do a concurrency check.
                response.HasConcurrencyError = !Repo.IsRowVersionStillCurrent(employmentInfo, rowVersion);

                if (!response.HasConcurrencyError)
                {
                    var today                   = DateTime.Today;
                    var canAddTransactionByDate = DatetimeExtensions.IsSameOrBefore(Repo.GetFeatureValue(TransactionTypes.Transactions).First().ParameterValue.ToDateMonthDayYear(),   today);
                    var canAddWorkerTaskByDate  = DatetimeExtensions.IsSameOrBefore(Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear(), today);

                    if (canAddTransactionByDate || canAddWorkerTaskByDate)
                    {
                        var pep = ParticipantHelper.GetMostRecentEnrolledProgram(Participant, AuthUser, true, true);

                        #region Transaction

                        var jobTypeNameList = new List<string> { JobType.UnSubsidized, JobType.SelfEmployed, JobType.StaffingAgency, JobType.TempCustodialParentUnsubsidized, JobType.TempNonCustodialParentUnsubsidized };

                        if (contract.Id == 0 && employmentInfo.EmploymentProgramtypeId == inProgram && jobTypeNameList.Contains(jobType.Name) && pep.IsW2)
                        {
                            var transactionContract = new TransactionContract
                                                      {
                                                          ParticipantId       = Participant.Id,
                                                          WorkerId            = Repo.WorkerByWIUID(AuthUser.WIUID).Id,
                                                          OfficeId            = pep.Office.Id,
                                                          EffectiveDate       = employmentInfo.JobBeginDate.GetValueOrDefault(),
                                                          CreatedDate         = modifiedDate,
                                                          TransactionTypeCode = TransactionTypes.WorkHistory,
                                                          ModifiedBy          = AuthUser.WIUID
                                                      };

                            var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

                            if (transaction != null)
                                Repo.NewTransaction(transaction as ITransaction);
                        }

                        #endregion

                        #region WorkerTask

                        var epStatusList              = new List<string> { EmployabilityPlanStatus.InProgress, EmployabilityPlanStatus.Ended };
                        var epStatuses                = new List<string> { EmployabilityPlanStatus.SystemDeleted, EmployabilityPlanStatus.WorkerDeleted, EmployabilityPlanStatus.WorkerVoided };
                        var workerTaskJobTypeNameList = new List<string> { JobType.UnSubsidized, JobType.StaffingAgency, JobType.TempCustodialParentUnsubsidized, JobType.TempNonCustodialParentUnsubsidized };
                        var eps                       = pep.EmployabilityPlans.OrderByDescending(i => i.BeginDate).ThenByDescending(i => i.Id).Where(i => !epStatuses.Contains(i.EmployabilityPlanStatusType.Name)).ToList();
                        var hasSubmittedEP            = eps?.SelectMany(i => i.EPEIBridges).Where(i => i.EmploymentInformationId == employmentInfo?.Id)?.Any(i => i.EmployabilityPlan.EmployabilityPlanStatusType.Name == EmployabilityPlanStatus.Submitted);

                        if (canAddWorkerTaskByDate && (pep.IsW2 || pep.IsLF) && workerTaskJobTypeNameList.Contains(jobType.Name))
                        {
                            var workerTaskListContract = new WorkerTaskList
                                                         {
                                                             WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Open).Id,
                                                             WorkerId           = Repo.WorkerByWIUID(AuthUser.WIUID).Id,
                                                             ParticipantId      = pep.ParticipantId,
                                                             TaskDate           = modifiedDate,
                                                             StatusDate         = AuthUser.CDODate ?? modifiedDate,
                                                             IsSystemGenerated  = true,
                                                             ModifiedBy         = AuthUser.WIUID,
                                                             ModifiedDate       = modifiedDate
                                                         };
                            if (contract.Id == 0)
                            {
                                var category = Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.EmploymentAddedCode);

                                workerTaskListContract.TaskDetails = category.Description;
                                workerTaskListContract.CategoryId  = category.Id;

                                Repo.NewWorkerTask(workerTaskListContract);
                            }
                            else
                                if (hasSubmittedEP == true && !epStatusList.Contains(eps?.FirstOrDefault(i => !epStatuses.Contains(i.EmployabilityPlanStatusType.Name))?.EmployabilityPlanStatusType.Name))
                                {
                                    var category = Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.EmploymentEditedCode);

                                    workerTaskListContract.TaskDetails = category.Description;
                                    workerTaskListContract.CategoryId  = category.Id;

                                    Repo.NewWorkerTask(workerTaskListContract);
                                }
                        }

                        #endregion
                    }

                    response.HasConcurrencyError = Repo.EmploymentInfoTransactionalSave(employmentInfo, user, AuthUser.MainFrameId,
                                                                                        contract.WageHour.ComputedDB2WageRateUnit,
                                                                                        contract.WageHour.ComputedDB2WageRateValue);
                }
            }

            return response;
        }

        public PreDeleteWHContract PreDeleteCheck(string pin, int id)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var employmentInfo = Participant.EmploymentInformations.FirstOrDefault(i => i.Id == id);

            if (employmentInfo == null)
            {
                return null;
            }

            var popCheck = Repo.PreCheckPop(pin, employmentInfo.EmploymentSequenceNumber);

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category       == RuleReason.WorkHistory
                                                                                                 && i.SubCategory == RuleReason.PreCheckError
                                                                                                 && (i.Code == RuleReason.POP || i.Code == RuleReason.TJTMJWH))
                                                                       .ToList()
                                         };

            var repository = new RuleRepository();

            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("WHDelete")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(employmentInfo);
            session.Insert(popCheck);
            session.Fire();

            var preDeleteWHContract = new PreDeleteWHContract { PopClaim = popCheck?.PopClaim };

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        preDeleteWHContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        preDeleteWHContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow delete if there are any errors.
            preDeleteWHContract.CanDelete = preDeleteWHContract.Errors?.Count == 0;

            return preDeleteWHContract;
        }

        public bool DeleteEmployment(int id, int deleteReasonId, string user, bool isRecursiveCall = false)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var modifiedDate   = DateTime.Now;
            var employmentInfo = (from x in Participant.EmploymentInformations where x.Id == id select x).SingleOrDefault();
            var epStatuses     = new List<string> { EmployabilityPlanStatus.SystemDeleted, EmployabilityPlanStatus.WorkerDeleted, EmployabilityPlanStatus.WorkerVoided };
            var pep            = ParticipantHelper.GetMostRecentEnrolledProgram(Participant, AuthUser, true, true);
            var eps            = pep.EmployabilityPlans.OrderByDescending(i => i.BeginDate).ThenByDescending(i => i.Id).ToList();
            var ep             = eps.SelectMany(i => i.EPEIBridges).Select(i => i.EmployabilityPlan).ToList();
            var epei           = eps.FirstOrDefault(i => i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)?.EPEIBridges.FirstOrDefault(i => i.EmploymentInformationId == id);

            if (employmentInfo != null && employmentInfo.IsDeleted == false)
            {
                employmentInfo.DeleteReasonId = deleteReasonId;

                if (employmentInfo.OtherJobInformation != null)
                {
                    employmentInfo.OtherJobInformation.IsDeleted    = true;
                    employmentInfo.OtherJobInformation.ModifiedBy   = user;
                    employmentInfo.OtherJobInformation.ModifiedDate = modifiedDate;
                }

                //update modifiedBy and modifiedDate for History Mechanism to recognize the change at the time of deletion 
                if (employmentInfo.EmploymentInformationBenefitsOfferedTypeBridges != null)
                {
                    foreach (var eibo in employmentInfo.EmploymentInformationBenefitsOfferedTypeBridges)
                    {
                        eibo.ModifiedBy   = user;
                        eibo.ModifiedDate = modifiedDate;
                    }
                }

                if (employmentInfo.WageHour != null)
                {
                    employmentInfo.WageHour.IsDeleted    = true;
                    employmentInfo.WageHour.ModifiedBy   = user;
                    employmentInfo.WageHour.ModifiedDate = modifiedDate;

                    foreach (var wjab in employmentInfo.WageHour.WageHourWageTypeBridges)
                    {
                        wjab.IsDeleted    = true;
                        wjab.ModifiedBy   = user;
                        wjab.ModifiedDate = modifiedDate;
                    }

                    foreach (var x in employmentInfo.WageHour.WageHourHistories)
                    {
                        x.IsDeleted    = true;
                        x.ModifiedBy   = user;
                        x.ModifiedDate = modifiedDate;

                        foreach (var y in x.WageHourHistoryWageTypeBridges)
                        {
                            y.IsDeleted    = true;
                            y.ModifiedBy   = user;
                            y.ModifiedDate = modifiedDate;
                        }
                    }
                }

                if (employmentInfo.Absences != null)
                {
                    foreach (var x in employmentInfo.Absences)
                    {
                        x.IsDeleted    = true;
                        x.ModifiedBy   = user;
                        x.ModifiedDate = modifiedDate;
                    }
                }

                employmentInfo.ModifiedBy   = user;
                employmentInfo.ModifiedDate = modifiedDate;

                #region WorkerTask

                var jobTypeNameList      = new List<string> { JobType.UnSubsidized, JobType.StaffingAgency, JobType.TempCustodialParentUnsubsidized, JobType.TempNonCustodialParentUnsubsidized };
                var employmentStatusList = new List<string> { EmployabilityPlanStatus.InProgress, EmployabilityPlanStatus.Ended };

                if (DatetimeExtensions.IsSameOrBefore(Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear(), DateTime.Today)
                    && (pep.IsW2 || pep.IsLF) && ep.Any(i => i.EmployabilityPlanStatusType.Name == EmployabilityPlanStatus.Submitted)
                    && !employmentStatusList.Contains(eps.FirstOrDefault(i => !epStatuses.Contains(i.EmployabilityPlanStatusType.Name))?.EmployabilityPlanStatusType.Name)
                    && jobTypeNameList.Contains(Repo.JobTypeById(employmentInfo.JobTypeId.GetValueOrDefault()).Name))
                {
                    var category = Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.EmploymentDeletedCode);
                    var workerTaskListContract = new WorkerTaskList
                                                 {
                                                     TaskDetails        = category.Description,
                                                     CategoryId         = category.Id,
                                                     WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Open).Id,
                                                     WorkerId           = Repo.WorkerByWIUID(AuthUser.WIUID).Id,
                                                     ParticipantId      = pep.ParticipantId,
                                                     TaskDate           = modifiedDate,
                                                     StatusDate         = AuthUser.CDODate ?? modifiedDate,
                                                     IsSystemGenerated  = true,
                                                     ModifiedBy         = AuthUser.WIUID,
                                                     ModifiedDate       = modifiedDate
                                                 };
                    Repo.NewWorkerTask(workerTaskListContract);
                }

                #endregion

                Repo.EmploymentInfoTransactionalDelete(employmentInfo, AuthUser.MainFrameId, epei);

                return true;
            }

            return false;
        }

        private static bool IsInProgramjob(List<IParticipantEnrolledProgram> peps, DateTime? beginDate)
        {
            var isInProgram = peps.Any(i => beginDate > i.EnrollmentDate && beginDate < (i.DisenrollmentDate ?? DateTime.MaxValue));

            return isInProgram;
        }

        private static bool IsCurrentjob(bool? isCurrentlyEmployed)
        {
            return (isCurrentlyEmployed == true);
        }

        private static bool IsPastjob(DateTime? endDate, bool? isCurrentlyEmployed)
        {
            if (endDate != null && (isCurrentlyEmployed == false || isCurrentlyEmployed == null))
            {
                return true;
            }

            return false;
        }

        private static string WhichJobCategory(DateTime? endDate, bool? isCurrentlyEmployed)
        {
            if (IsCurrentjob(isCurrentlyEmployed))
            {
                return "currentJob";
            }
            else
                if (IsPastjob(endDate, isCurrentlyEmployed))
                {
                    return "pastJob";
                }
                else
                {
                    return null;
                }
        }

        private static string GetEmploymentProgramTypeName(IEmploymentInformation empInfo, IParticipant participant)
        {
            var employmentProgType = string.Empty;

            var peps = participant.ParticipantEnrolledPrograms.ToList();

            if (IsInProgramjob(peps, empInfo.JobBeginDate))
            {
                employmentProgType = "In-Program";
            }
            else
            {
                if (WhichJobCategory(empInfo.JobEndDate, empInfo.IsCurrentlyEmployed) == "currentJob")
                {
                    employmentProgType = "Out-of-Program";
                }
                else
                {
                    if (WhichJobCategory(empInfo.JobEndDate, empInfo.IsCurrentlyEmployed) == "pastJob")
                    {
                        employmentProgType = "Out-of-Program";
                    }
                }
            }

            return employmentProgType;
        }

        private static WageHourContract GetWageHourInfo(IEmploymentInformation empInfo)
        {
            var wageHourHistories = new List<WageHourHistoryContract>();
            var jobActionTypes    = new List<int>();
            var jobActionnames    = new List<string>();
            var wageHourAction    = new JobActionTypeContract();

            var wagehour = new WageHourContract
                           {
                               CurrentAverageWeeklyHours       = empInfo.WageHour?.CurrentAverageWeeklyHours.ToString(),
                               CurrentPayRate                  = empInfo.WageHour?.CurrentPayRate?.ToString("N2"),
                               CurrentPayRateIntervalId        = empInfo.WageHour?.CurrentPayRateIntervalId,
                               CurrentPayRateIntervalName      = empInfo.WageHour?.CurrentPayIntervalType?.Name,
                               CurrentEffectiveDate            = empInfo.WageHour?.CurrentEffectiveDate?.ToString("MM/dd/yyyy"),
                               CurrentPayTypeDetails           = empInfo.WageHour?.CurrentPayTypeDetails,
                               CurrentHourlySubsidyRate        = empInfo.WageHour?.CurrentHourlySubsidyRate?.ToString(),
                               WorkSiteContribution            = empInfo.WageHour?.WorkSiteContribution?.ToString(),
                               PastBeginPayRate                = empInfo.WageHour?.PastBeginPayRate?.ToString("N2"),
                               PastEndPayRate                  = empInfo.WageHour?.PastEndPayRate?.ToString("N2"),
                               IsUnchangedPastPayRateIndicator = empInfo.WageHour?.IsUnchangedPastPayRateIndicator,
                               PastBeginPayRateIntervalId      = empInfo.WageHour?.PastBeginPayRateIntervalId,
                               PastBeginPayRateIntervalName    = empInfo.WageHour?.BeginRateIntervalType?.Name,
                               PastEndPayRateIntervalId        = empInfo.WageHour?.PastEndPayRateIntervalId,
                               PastEndRateIntervalName         = empInfo.WageHour?.EndRateIntervalType?.Name,
                               ComputedCurrentWageRateUnit     = empInfo.WageHour?.ComputedCurrentWageRateUnit,
                               ComputedCurrentWageRateValue    = empInfo.WageHour?.ComputedCurrentWageRateValue?.ToString("N2"),
                               ComputedPastEndWageRateUnit     = empInfo.WageHour?.ComputedPastEndWageRateUnit,
                               ComputedPastEndWageRateValue    = empInfo.WageHour?.ComputedPastEndWageRateValue?.ToString("N2"),
                               ModifiedBy                      = empInfo.WageHour?.ModifiedBy,
                               ModifiedDate                    = empInfo.WageHour?.ModifiedDate
                           };

            wageHourAction.JobActionTypes = jobActionTypes;
            wageHourAction.JobActionNames = jobActionnames;
            wagehour.WageHourAction       = wageHourAction;
            wagehour.WageHourHistories = !empInfo.IsDeleted
                                             ? empInfo.WageHour?.WageHourHistories != null
                                                   ? GetWageHourHistoriesInfo(empInfo.WageHour?.WageHourHistories, wagehour, false)
                                                   : wageHourHistories
                                             : empInfo.WageHour?.AllWageHourHistories != null
                                                 ? GetWageHourHistoriesInfo(empInfo.WageHour?.AllWageHourHistories, wagehour, true)
                                                 : wageHourHistories;

            return wagehour;
        }

        private static List<JobDutyContract> GetJobDutiesInfo(IEmploymentInformation empInfo)
        {
            var jobDuties = empInfo?.EmploymentInformationJobDutiesDetailsBridges
                                   .Select(i => new JobDutyContract
                                                {
                                                    Id      = i.JobDutiesId,
                                                    Details = i.JobDutiesDetail?.Details
                                                }).ToList();

            jobDuties = jobDuties ?? new List<JobDutyContract>();

            return (jobDuties);
        }

        private static JobActionTypeContract GetJobActionTypeInfo(IEmploymentInformation empInfo)
        {
            var jobaction = new JobActionTypeContract
                            {
                                JobActionTypes = new List<int>(),
                                JobActionNames = new List<string>()
                            };

            // We need to look at the ActionBridge table to get the list of action
            // needed ID's that the user has previously chosen.
            if (empInfo?.EmploymentInformationBenefitsOfferedTypeBridges == null)
            {
                return jobaction;
            }

            foreach (var jab in empInfo.EmploymentInformationBenefitsOfferedTypeBridges)
            {
                if (jab.BenefitsOfferedTypeId.HasValue)
                {
                    jobaction.JobActionTypes.Add(jab.BenefitsOfferedTypeId.Value);
                }

                jobaction.JobActionNames.Add(jab.BenefitsOfferedType.Name);
            }

            return jobaction;
        }

        private static JobActionTypeContract GetCurrentPayTypeInfo(IEmploymentInformation empInfo)
        {
            // We need to look at the ActionBridge table to get the list of action
            // needed ID's that the user has previously chosen.
            var currentPayType = new JobActionTypeContract
                                 {
                                     JobActionTypes = new List<int>(),
                                     JobActionNames = new List<string>()
                                 };

            if (empInfo.WageHour?.WageHourWageTypeBridges == null)
            {
                return currentPayType;
            }

            foreach (var jab in empInfo.WageHour?.WageHourWageTypeBridges)
            {
                if (jab.WageTypeId.HasValue)
                {
                    currentPayType.JobActionTypes.Add(jab.WageTypeId.Value);
                }

                currentPayType.JobActionNames.Add(jab.WageType?.Name);
            }

            return currentPayType;
        }

        private static void GetOtherJobInformationInfo(IEmploymentInformation empInfo, EmploymentInfoContract empContract)
        {
            if (empInfo.OtherJobInformationId != null)
            {
                empContract.JobSectorId             = empInfo.OtherJobInformation?.JobSectorId;
                empContract.JobSectorName           = empInfo.OtherJobInformation?.JobSector?.Name;
                empContract.JobFoundMethodId        = empInfo.OtherJobInformation?.JobFoundMethodId;
                empContract.JobFoundMethodName      = empInfo.OtherJobInformation?.JobFoundMethod?.Name;
                empContract.WorkerId                = empInfo.OtherJobInformation?.WorkerId;
                empContract.JobFoundMethodDetails   = empInfo.OtherJobInformation?.JobFoundMethodDetails;
                empContract.ExpectedScheduleDetails = empInfo.OtherJobInformation?.ExpectedScheduleDetails;
                empContract.WorkProgramId           = empInfo.OtherJobInformation?.WorkProgramId;
            }
            else
            {
                empContract.JobSectorId             = null;
                empContract.JobFoundMethodId        = null;
                empContract.WorkerId                = null;
                empContract.JobFoundMethodDetails   = null;
                empContract.ExpectedScheduleDetails = null;
            }
        }

        private static List<WageHourHistoryContract> GetWageHourHistoriesInfo(ICollection<IWageHourHistory> wageHourList, WageHourContract wageHour, bool eiIsDeleted)
        {
            var wageHourHistories = new List<WageHourHistoryContract>();

            foreach (var whs in wageHourList.OrderByDescending(x => x.EffectiveDate))
            {
                var wh = new WageHourHistoryContract
                         {
                             Id                    = whs.Id,
                             EffectiveDate         = whs.EffectiveDate?.ToString("MM/dd/yyyy"),
                             PayRate               = whs.PayRate.ToString(),
                             PayRateIntervalId     = whs.PayRateIntervalId,
                             PayRateIntervalName   = whs.IntervalType?.Name,
                             HourlySubsidyRate     = whs.HourlySubsidyRate.ToString(),
                             WorkSiteContribution  = whs.WorkSiteContribution.ToString(),
                             AverageWeeklyHours    = whs.AverageWeeklyHours.ToString(),
                             PayTypeDetails        = whs.PayTypeDetails,
                             ComputedWageRateUnit  = whs.ComputedWageRateUnit,
                             ComputedWageRateValue = whs.ComputedWageRateValue.ToString(),
                             ModifiedDate          = whs.ModifiedDate,
                             ModifiedBy            = whs.ModifiedBy
                         };

                // We need to look at the ActionBridge table to get the list of action
                // needed ID's that the user has previously chosen.
                var pastPayType = new JobActionTypeContract();
                pastPayType.JobActionTypes = new List<int>();
                pastPayType.JobActionNames = new List<string>();

                //TODO: Fix Sprint 15
                if (!eiIsDeleted && whs.WageHourHistoryWageTypeBridges != null)
                {
                    foreach (var jab in whs.WageHourHistoryWageTypeBridges)
                    {
                        if (jab.WageTypeId.HasValue)
                        {
                            pastPayType.JobActionTypes.Add(jab.WageTypeId.Value);
                        }

                        pastPayType.JobActionNames.Add(jab.WageType?.Name);
                    }
                }
                else
                    if (eiIsDeleted && whs.AllWageHourHistoryWageTypeBridges != null)
                    {
                        var maxModifiedDate                         = whs.AllWageHourHistoryWageTypeBridges.Max(i => i.ModifiedDate);
                        var recentAllWageHourHistoryWageTypeBridges = whs.AllWageHourHistoryWageTypeBridges.Where(i => i.ModifiedDate == maxModifiedDate).ToList();

                        foreach (var jab in recentAllWageHourHistoryWageTypeBridges)
                        {
                            if (jab.WageTypeId.HasValue)
                            {
                                pastPayType.JobActionTypes.Add(jab.WageTypeId.Value);
                            }

                            pastPayType.JobActionNames.Add(jab.WageType?.Name);
                        }
                    }
                    else
                    {
                        var pastPayType1 = new JobActionTypeContract();
                        pastPayType1.JobActionTypes = new List<int>();
                        wageHour.WageHourAction     = pastPayType1;
                    }

                wh.HistoryPayType = pastPayType;
                wageHourHistories.Add(wh);
            }

            return wageHourHistories;
        }

        private static List<AbsenceContract> GetAbsencesInfo(IEmploymentInformation empInfo)
        {
            var absences = new List<AbsenceContract>();

            if (empInfo.AllAbsences == null)
            {
                return absences;
            }

            var abs = empInfo.AllAbsences.AsQueryable();

            if (!empInfo.IsDeleted)
            {
                abs = abs.Where(i => !i.IsDeleted);
            }

            var absencesList = abs.ToList();

            foreach (var abd in absencesList)
            {
                var ab = new AbsenceContract
                         {
                             Id              = abd.Id,
                             BeginDate       = abd.BeginDate?.ToString("MM/dd/yyyy"),
                             EndDate         = abd.EndDate?.ToString("MM/dd/yyyy"),
                             AbsenceReasonId = abd.AbsenceReasonId,
                             Details         = abd.Details,
                             ModifiedDate    = abd.ModifiedDate,
                             ModifiedBy      = abd.ModifiedBy
                         };

                absences.Add(ab);
            }

            return absences;
        }

        private static bool DeleteFromList(ICollection<IWageHourHistory> list, List<int> modelIds, List<int> dbIds)
        {
            var deletedList = dbIds.Except(modelIds).ToArray();

            foreach (var n in deletedList)
            {
                var c = list.First(x => x.Id == n);
                c.IsDeleted = true;
            }

            return true;
        }

        private static bool DeleteFromList(ICollection<IAbsence> list, List<int> modelIds, List<int> dbIds)
        {
            var deletedList = dbIds.Except(modelIds).ToArray();

            foreach (var n in deletedList)
            {
                var c = list.First(x => x.Id == n);
                c.IsDeleted = true;
            }

            return true;
        }

        private bool HasEmploymentOnEp(IEmploymentInformation ei)
        {
            var epeiBridges = Participant.EmployabilityPlans
                                         .Where(i => i.EmployabilityPlanStatusTypeId    != EmployabilityPlanStatus.SystemDeletedId
                                                     && i.EmployabilityPlanStatusTypeId != EmployabilityPlanStatus.WorkerDeletedId
                                                     && i.EmployabilityPlanStatusTypeId != EmployabilityPlanStatus.WorkerVoidedId
                                                     && i.EmployabilityPlanStatusTypeId != EmployabilityPlanStatus.EndedId)
                                         .OrderByDescending(i => i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                         .ThenByDescending(i => i.BeginDate)
                                         .Select(i => i.EPEIBridges)
                                         .FirstOrDefault();
            var ep                = epeiBridges?.Where(i => i.EmploymentInformationId == ei.Id).Select(i => i.EmployabilityPlan);
            var hasEmploymentOnEp = ep?.FirstOrDefault()?.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId;

            return hasEmploymentOnEp;
        }

        private IWageHour ProcessWageHourHistory(IWageHour wageHour, IWageHourHistory whh, WageHourHistoryContract history, IJobType jobType, string user, DateTime modifiedDate)
        {
            whh.ModifiedDate = modifiedDate;
            //sortOrder++;
            // Here we soft delete all the Paytype actions associated with workhistories.
            whh.WageHourHistoryWageTypeBridges.ForEach(i => i.IsDeleted = true);

            if (history?.HistoryPayType?.JobActionTypes != null)
            {
                var jobActions = history?.HistoryPayType?.JobActionTypes;

                if (jobActions.Count == 0)
                {
                    whh.AllWageHourHistoryWageTypeBridges.ForEach(i => i.IsDeleted = true);
                }
                else
                {
                    foreach (var x in jobActions)
                    {
                        // TODO: Fix ID type.
                        var restore = whh.AllWageHourHistoryWageTypeBridges?.FirstOrDefault(z => z.WageTypeId == x);

                        if (restore != null)
                        {
                            restore.ModifiedDate = modifiedDate;
                            restore.ModifiedBy   = user;
                            restore.IsDeleted    = false;
                        }
                        else
                        {
                            IWageHourHistoryWageTypeBridge whhjb = null;
                            whhjb              = Repo.NewWageHourHistoryWageTypeBridge(whh, user);
                            whhjb.WageTypeId   = x;
                            whhjb.ModifiedDate = modifiedDate;
                            whh.WageHourHistoryWageTypeBridges.Add(whhjb);
                        }
                    }
                }
            }

            whh.EffectiveDate = history.EffectiveDate?.ToDateTimeMonthDayYear();

            if (jobType.Name == JobType.Volunteer)
            {
                whh.PayRate           = null;
                whh.PayRateIntervalId = null;
            }
            else
            {
                whh.PayRate               = history.PayRate.ToDecimal();
                whh.PayRateIntervalId     = history.PayRateIntervalId;
                whh.ComputedWageRateUnit  = history.ComputedWageRateUnit;
                whh.ComputedWageRateValue = history.ComputedWageRateValue.ToDecimal();
            }

            whh.HourlySubsidyRate    = history.HourlySubsidyRate.ToDecimal();
            whh.WorkSiteContribution = history.WorkSiteContribution.ToDecimal();
            whh.AverageWeeklyHours   = history.AverageWeeklyHours.ToDecimal();
            whh.PayTypeDetails       = history.PayTypeDetails;
            whh.ModifiedDate         = modifiedDate;
            whh.ModifiedBy           = user;
            // TODO: Do not need this statement because we pass parent object when newing  wageHour history.
            wageHour.WageHourHistories.Add(whh);

            return wageHour;
        }

        #endregion
    }
}
