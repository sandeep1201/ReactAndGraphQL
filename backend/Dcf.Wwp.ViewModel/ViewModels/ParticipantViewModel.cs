using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using NRules;
using NRules.Fluent;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using DCF.Common.Logging;
using Dcf.Wwp.Api.Library.Model;
using DCF.Timelimts.Service;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Constants;
using DCF.Common.Extensions;
using Activity = Dcf.Wwp.DataAccess.Models.Activity;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;
using EmployabilityPlanStatus = Dcf.Wwp.Model.Interface.Constants.EmployabilityPlanStatus;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using EnrolledProgramStatusCode = Dcf.Wwp.Model.Interface.Constants.EnrolledProgramStatusCode;
using WorkerTaskStatus = Dcf.Wwp.Model.Interface.Constants.WorkerTaskStatus;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ParticipantViewModel : BaseInformalAssessmentViewModel
    {
        private readonly string _iPAddress        = "192.168.1.1";
        private readonly string _externalAgencyId = "TANF";

        private readonly ITimelimitService        _timelimitService;    // not used?
        private readonly IDb2TimelimitService     _db2TimelimitService; // not used ?
        private readonly IConfidentialityChecker  _confidentialityChecker;
        private readonly IAuthAccessChecker       _authAccessChecker;
        private readonly ICwwIndService           _cwwIndSvc;
        private readonly IAuthUser                _authUser;
        private readonly IEmployabilityPlanDomain _epDomain;
        private readonly IUnitOfWork              _unitOfWork;
        private readonly ITransactionDomain       _transactionDomain;

        private ILog Logger { get; set; }

        public ParticipantViewModel(IRepository repository, IAuthUser authUser, IConfidentialityChecker confidentialityChecker, ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService, IReadOnlyCollection<ICountyAndTribe> countyAndTribes, ICwwIndService cwwIndSvc, IAuthAccessChecker authAccessChecker, IEmployabilityPlanDomain epDomain, IUnitOfWork unitOfWork, ITransactionDomain transactionDomain) : base(repository, authUser)
        {
            _confidentialityChecker = confidentialityChecker;
            _authAccessChecker      = authAccessChecker;
            _epDomain               = epDomain;
            _unitOfWork             = unitOfWork;
            _countyAndTribes        = countyAndTribes;
            _timelimitService       = timelimitService;
            _db2TimelimitService    = db2TimelimitService;
            _cwwIndSvc              = cwwIndSvc;
            _authUser               = authUser;
            _transactionDomain      = transactionDomain;
            Logger                  = LogProvider.GetLogger(GetType());
        }

        public ParticipantViewModel(string pin, IRepository repository, IAuthUser authUser, IConfidentialityChecker confidentialityChecker, ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService, IReadOnlyCollection<ICountyAndTribe> countyAndTribes, ICwwIndService cwwIndSvc, IAuthAccessChecker authAccessChecker, IEmployabilityPlanDomain epDomain, IUnitOfWork unitOfWork, ITransactionDomain transactionDomain) : base(repository, authUser)
        {
            InitializeFromPin(pin);
            _confidentialityChecker = confidentialityChecker;
            _authAccessChecker      = authAccessChecker;
            _epDomain               = epDomain;
            _unitOfWork             = unitOfWork;
            _countyAndTribes        = countyAndTribes;
            _timelimitService       = timelimitService;
            _db2TimelimitService    = db2TimelimitService;
            _cwwIndSvc              = cwwIndSvc;
            _authUser               = authUser;
            _transactionDomain      = transactionDomain;
            Logger                  = LogProvider.GetLogger(GetType());
        }

        public ParticipantsContract GetParticipant(string pin, bool doRefresh, bool usePepAgency, bool useWS)
        {
            IParticipant participant;

            if (doRefresh)
            {
                // Call the stored procedure which refreshes
                // the Participant table for the given pin.
                participant = Repo.GetRefreshedParticipant(pin);
            }
            else
            {
                // We'll grab the participant from the table and if that returns null,
                // then call the refresh stored procedure which will load it from DB2.
                participant = Repo.GetParticipant(pin) ?? Repo.GetRefreshedParticipant(pin);
            }

            if (participant == null)
            {
                return null;
            }

            var dt = Repo.GetMostRecentPrograms(decimal.Parse(pin));
            var rs = dt.AsEnumerable()
                       .Select(r => new MostRecentProgram
                                    {
                                        ProgramName      = r.Field<string>("ProgramName"),
                                        RecentStatus     = r.Field<string>("RecentStatus"),
                                        RecentStatusDate = r.Field<DateTime?>("RecentStatusDate"),
                                        AssignedWorker   = r.Field<string>("AssignedWorker"),
                                        WIUID            = r.Field<string>("WIUID")
                                    })
                       .ToList();

            var program            = rs.FirstOrDefault(i => i.ProgramName.Trim() == "WW" || i.ProgramName.Trim() == "LF");
            var workerId           = program?.AssignedWorker;
            var isConfidentialCase = false;
            var hasAccess          = true;

            if (useWS)
            {
                var cwwReply = _confidentialityChecker.Check(Convert.ToDecimal(pin), participant.ConfidentialPinInformations, workerId);
                isConfidentialCase = cwwReply.CaseCofidentailStatus?.ToUpper() == "Y";
                var wiuid = Repo.WorkerByMainframeId(cwwReply.FEPSupervisor)?.WIUID;
                hasAccess = _authAccessChecker.HasAccess(wiuid, rs);
            }

            var result = usePepAgency ? GetParticipantsPepContract(participant, isConfidentialCase, hasAccess) : GetParticipantsContract(participant, isConfidentialCase, hasAccess);

            return (result);
        }

        public PinDetailsContract GetParticipantSummary(string pin, bool fromSummary = false)
        {
            var participantDetails = Repo.GetParticipantDetails(pin);

            if (participantDetails != null && fromSummary)
            {
                //ImportAnyAuxTicks(pin, p);
                Repo.UpsertRecentParticipant(AuthUser.Username, (int) participantDetails.ParticipantId);
                Repo.Save(true);
            }

            var contract = GetPinDetailsContract(participantDetails);

            var participant = Repo.GetParticipantById(participantDetails?.ParticipantId);

            // Also add demographic info.
            if (participant != null)
            {
                //var cwwReply = _confidentialityChecker.Check(Convert.ToDecimal(pin), participant.ConfidentialPinInformations);
                //participant.IsConfidentialCase       = cwwReply.CaseCofidentailStatus?.ToUpper() == "Y";
                contract.OtherDemographicInformation = GetPinOtherDemographicInformation(participant);
            }

            return contract;
        }

        public StatusContract EnrollParticipant(EnrolledProgramContract programInfo)
        {
            var statusContract = new StatusContract();
            var modifiedDate   = _authUser.CDODate ?? DateTime.Now;

            if (programInfo?.Id == null || programInfo.Id == 0 || programInfo.AssignedWorker?.Id == null)
            {
                statusContract.ErrorMessages.Add("Bad Request");
                Logger.Error($"Bad request for enrollment by'{_authUser?.MainFrameId}'");
                return statusContract;
            }

            var pep               = Repo.GetPepById(programInfo.Id.Value);
            var oldAssignedWorker = pep.Worker;
            var oldWorkerMFID     = pep.Worker?.MFUserId;
            var newAssignedWorker = Repo.WorkerById(programInfo.AssignedWorker.Id.Value);
            var pinNumber         = pep.Participant.PinNumber.ToString();

            // Enrolling Peps need to have a worker assigned to them..
            if (newAssignedWorker == null)
            {
                statusContract.ErrorMessages.Add("No worker is Assigned");
                Logger.Error($"No worker is assigned for enrollment by'{_authUser?.MainFrameId}'");
                return statusContract;
            }

            // Grab from db2 for W-2.
            if (oldWorkerMFID == null && pep.IsW2)
            {
                var oldFEPDetailsFromDB2 = Repo.GetMostRecentFepDetails(pinNumber);

                if (oldFEPDetailsFromDB2 != null)
                    oldWorkerMFID = oldFEPDetailsFromDB2.MostRecentMFFepId;
            }


            //TODO:Ask in code Review: Are these supposed to be created even if the call to CWW fails...

            #region Transaction

            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = pep.ParticipantId,
                                          WorkerId            = Repo.WorkerByWIUID(_authUser.WIUID).Id,
                                          OfficeId            = pep.Office.Id,
                                          EffectiveDate       = modifiedDate,
                                          CreatedDate         = modifiedDate,
                                          TransactionTypeCode = TransactionTypes.ParticipantEnrolled,
                                          ModifiedBy          = _authUser.WIUID
                                      };

            var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

            if (transaction != null)
            {
                var transactionInterface = transaction as ITransaction;
                var transactions         = pep.Participant.Transactions;

                if (transactionInterface != null && !transactions.Any(i => !i.IsDeleted && i.TransactionTypeId == transactionInterface.TransactionTypeId
                                                                                        && i.EffectiveDate     == transactionInterface.EffectiveDate.Date))
                    Repo.NewTransaction(transactionInterface);
            }

            #endregion

            #region WorkerTask

            if ((pep.IsCF || pep.IsFCDP || pep.IsW2 || pep.IsLF) && Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear().IsSameOrBefore(DateTime.Today))
            {
                CreateAndAddNewWorkerTask(newAssignedWorker, pep, modifiedDate);
            }

            #endregion

            // NOTE: No need to call Save after this call.  It encapsulates the Save.
            Repo.EnrollPep(programInfo.Id, programInfo.AssignedWorker?.Id, AuthUser.MainFrameId);

            if (pep.IsW2)
            {
                try
                {
                    // Call only updates for W-2 not LF by design.
                    UpdateFepInCww(_authUser.MainFrameId, pep.Participant.PinNumber.ToString(), oldWorkerMFID, newAssignedWorker.MFUserId);
                }
                catch (Exception e)
                {
                    // Reverting to old fep.
                    Repo.EnrollPep(programInfo.Id, oldAssignedWorker?.Id, AuthUser.MainFrameId);
                    Repo.Save();
                    Logger.Error($"Webservice for updating FEP failed: new worker is'{newAssignedWorker?.MFUserId}', old worker is '{oldAssignedWorker?.MFUserId}'");
                    statusContract.ErrorMessages.Add("Failed updating FEP in CWW.");
                    return statusContract;
                }
            }

            return statusContract;
        }

        public DisenrollCheckContract PreEnrollParticipant(int? programId)
        {
            if (programId == 0 || programId == null)
            {
                return null;
            }

            var preCheckContract = new DisenrollCheckContract();

            var pep = Repo.GetParticantEnrollment(programId.Value);

            var peps       = Participant.ParticipantEnrolledPrograms;
            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("PreEnrollmentCheck")).To("PreEnrollmentCheck"));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            session.Insert(pep);
            session.Insert(peps);
            session.Insert(preCheckContract);
            session.Fire();

            // Querying the database once for all the applicable rule reasons.
            var possibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category == "Enrollment" && i.SubCategory == "PreCheck");

            foreach (var err in preCheckContract?.ErrorCodes.AsNotNull())
            {
                preCheckContract?.Errors?.Add(possibleRuleReasons?.SingleOrDefault(x => x.Code == err).Name);
            }

            preCheckContract.CanDisenroll = preCheckContract.Errors != null && preCheckContract.Errors.Count == 0;

            return preCheckContract;
        }

        public DisenrollCheckContract PreDisenrollParticipant(int programId)
        {
            if (programId == 0)
            {
                return null;
            }

            var disenrollPep = Participant.ParticipantEnrolledPrograms.FirstOrDefault(x => x.Id == programId);

            if (disenrollPep == null)
            {
                return null;
            }

            var ep = Participant.EmployabilityPlans
                                .OrderByDescending(i => i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                .ThenByDescending(i => i.BeginDate)
                                .FirstOrDefault(i => (i.IsDeleted == false && i.EnrolledProgram?.ProgramCode?.Trim().ToLower() == disenrollPep.EnrolledProgram?.ProgramCode?.Trim().ToLower())
                                                     && (i.EmployabilityPlanStatusTypeId    == EmployabilityPlanStatus.InProgressId
                                                         || i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId)) ?? new EmployabilityPlan();

            var ps = Participant.ParticipationStatus
                                .ToList();

            // Lets Check DB2 for any open records.
            ISP_PreCheckDisenrollment_Result preCheck = null;
            if (!disenrollPep.IsFCDP)
                preCheck = Repo.PreDisenrollmentErrors(disenrollPep.Participant.PinNumber, disenrollPep.CASENumber, disenrollPep.Id);
            else
                preCheck = new SP_PreCheckDisenrollment_Result { ActivityOpen = false, ActivityEndDate = null, PlacementOpen = false, TransactionExist = false };


            var peps                  = Participant.ParticipantEnrolledPrograms;
            var barrierDetails        = Repo.BarrierDetailsByParticipantId(Participant.Id).ToList();
            var employmentInformation = Participant.EmploymentInformations.ToList();
            var workerTaskDate        = Repo.GetFeatureValue("WorkerTaskList").First().ParameterValue.ToDateMonthDayYear();

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category == RuleReason.DisEnroll && i.SubCategory
                                                                                                 == RuleReason.PreCheckError).ToList()
                                         };

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("Disenrollment")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(preCheck);
            session.Insert(peps);
            session.Insert(ep);
            session.Insert(ps);
            session.Insert(disenrollPep);
            session.Insert(barrierDetails);
            session.Insert(employmentInformation);
            session.Insert(workerTaskDate);
            session.Fire();

            var disenrollCheckContract = new DisenrollCheckContract { ActivityEndDate = preCheck?.ActivityEndDate };

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        disenrollCheckContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        disenrollCheckContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow disenroll if there are any errors.
            disenrollCheckContract.CanDisenroll = disenrollCheckContract.Errors?.Count == 0;

            return disenrollCheckContract;
        }

        public DisenrollCheckContract PreTransferParticipant(EnrolledProgramContract transferContract)
        {
            if (transferContract?.OfficeId == null)
            {
                return null;
            }

            var transferPep = Participant.ParticipantEnrolledPrograms.FirstOrDefault(x => x.Id == transferContract.Id);

            if (transferPep == null)
            {
                return null;
            }

            var transferOffice = Repo.GetOfficeById(transferContract.OfficeId.Value);

            if (transferOffice?.OfficeNumber == null)
            {
                return null;
            }

            var isTransfer = Repo.hasOfficeTransfer(Participant.Id);

            transferContract.ContractorId   = transferOffice.ContractAreaId;
            transferContract.ContractorName = transferOffice.ContractArea.ContractAreaName.SafeTrim();

            var elapsedCount = 0;
            if (transferPep.IsInMilwaukee && !transferPep.IsTmj)
            {
                var mostRecentEp = Participant.EmployabilityPlans
                                              .Where(i => i.EmployabilityPlanStatusTypeId    == EmployabilityPlanStatus.InProgressId
                                                          || i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId)
                                              .OrderByDescending(i => i.BeginDate)
                                              .ThenByDescending(i => i.CreatedDate)
                                              .FirstOrDefault();
                if (mostRecentEp != null)
                {
                    var schedules = mostRecentEp.EmployabilityPlanActivityBridges
                                                .Select(i => i.Activity)
                                                .SelectMany(j => j.ActivitySchedules)
                                                .Where(k => k.EmployabilityPlanId == mostRecentEp.Id && k.ActualEndDate == null)
                                                .ToList();

                    if (schedules.Count > 0)
                    {
                        elapsedCount = (schedules
                                               .Count(i => (i.IsRecurring    == true  && i.PlannedEndDate <= DateTime.Today)
                                                           || (i.IsRecurring == false && i.StartDate      <= DateTime.Today)));
                    }
                }
            }

            var contract = ExecuteTransferRules(transferContract, transferPep, Participant.ParticipantEnrolledPrograms.ToList(), isTransfer, elapsedCount);

            return (contract);
        }

        public PreAddingParticipationStatusContract ValidateAddingParticipationStatus(ParticipantStatusContract participantStatusContract, List<Activity> activities)
        {
            var employments = Participant.EmploymentInformations.ToList();
            var peps        = Participant.ParticipantEnrolledPrograms.ToList();
            var psContract  = participantStatusContract;
            var contract    = ExecutePSAddRules(psContract, peps, employments, activities);

            return (contract);
        }


        public void DisenrollParticipant(EnrolledProgramContract programInfo)
        {
            if (programInfo == null || programInfo.Id == 0)
            {
                return;
            }

            var pep   = Repo.GetParticantEnrollment(programInfo.Id.GetValueOrDefault());
            var today = DateTime.Today;

            // End all the current Participation Statuses for the disenrolled program
            var pss = pep.Participant.ParticipationStatus
                         .Where(i => i.IsCurrent == true && i.EnrolledProgramId == pep.EnrolledProgramId);

            pss.ForEach(ps =>
                        {
                            ps.IsCurrent    = false;
                            ps.EndDate      = _authUser.CDODate ?? DateTime.Today;
                            ps.ModifiedDate = DateTime.Now;
                            ps.ModifiedBy   = _authUser.WIUID;

                            var psTransactionContract = new TransactionContract
                                                        {
                                                            ParticipantId       = ps.ParticipantId.GetValueOrDefault(),
                                                            WorkerId            = Repo.WorkerByWIUID(_authUser.WIUID).Id,
                                                            OfficeId            = (pep.Office?.Id).GetValueOrDefault(),
                                                            EffectiveDate       = programInfo.DisenrollmentDate ?? _authUser.CDODate ?? today,
                                                            CreatedDate         = today,
                                                            TransactionTypeCode = TransactionTypes.ParticipationStatusEnd,
                                                            ModifiedBy          = _authUser.WIUID,
                                                            StatusCode          = ps.ParticipationStatusType.Code
                                                        };
                            var psTransaction = _transactionDomain.InsertTransaction(psTransactionContract, true);

                            if (psTransaction != null)
                                Repo.NewTransaction(psTransaction as ITransaction);
                        });

            #region Transactions

            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = pep.ParticipantId,
                                          WorkerId            = Repo.WorkerByWIUID(_authUser.WIUID).Id,
                                          OfficeId            = (pep.Office?.Id).GetValueOrDefault(),
                                          EffectiveDate       = programInfo.DisenrollmentDate ?? _authUser.CDODate ?? today,
                                          CreatedDate         = today,
                                          TransactionTypeCode = TransactionTypes.ParticipantDisEnrolled,
                                          ModifiedBy          = _authUser.WIUID
                                      };

            var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

            if (transaction != null)
                Repo.NewTransaction(transaction as ITransaction);

            #endregion

            #region WorkerTasks

            var participant = pep.Participant;

            if (Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear().IsSameOrBefore(today) &&
                participant.ParticipantEnrolledPrograms.Count(i => i.EnrolledProgramId != pep.EnrolledProgramId &&
                                                                   (i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.ReferredId ||
                                                                    i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.EnrolledId)) == 0)
            {
                participant.WorkerTaskLists
                           .Where(i => i.WorkerTaskStatus.Code == WorkerTaskStatus.Open)
                           .ForEach(i =>
                                    {
                                        i.WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Closed).Id;
                                        i.ModifiedBy         = _authUser.WIUID;
                                        i.ModifiedDate       = DateTime.Now;
                                    });
            }

            #endregion

            Repo.UpdateDisenrollment(pep, programInfo.AssignedWorker?.Id, AuthUser.MainFrameId, AuthUser.Username, programInfo.CompletionReasonDetails, programInfo.DisenrollmentDate, programInfo.CompletionReasonId);
        }

        public void ReassignParticipantToWorker(EnrolledProgramContract programInfo)
        {
            // We use the id to look up the worker before the reassign and we need an new AssignedWorkerId.
            if (programInfo?.Id == null || programInfo.Id == 0 || programInfo.AssignedWorker?.Id == null)
            {
                return;
            }

            // original code - gets the PEP record twice.
            //var oldWorkerMFID = Repo.GetPepById(programInfo.Id.Value)?.Worker?.MFUserId;
            //var newWorker     = Repo.WorkerById(programInfo.AssignedWorker.Id.Value);
            //var pep           = Repo.GetPepById(programInfo.Id.Value);
            //var participant   = pep.Participant;

            // new code - get the PEP row/data, *once*
            var pep           = Repo.GetPepById(programInfo.Id.Value);
            var oldWorkerMFId = pep?.Worker?.MFUserId;
            var newWorker     = Repo.WorkerById(programInfo.AssignedWorker.Id.Value);
            var participant   = pep?.Participant;

            var pinNumber    = participant?.PinNumber.ToString().PadLeft(10, '0');
            var modifiedDate = _authUser.CDODate ?? DateTime.Now;

            // When oldworker in WWP is null, then lets grab it from CWW W-2.
            // We need the oldworker so we can call the UpdateFepInCww.
            if (oldWorkerMFId == null && pep != null && pep.IsW2)
            {
                var oldFEPDetailsFromDB2 = Repo.GetMostRecentFepDetails(pinNumber);

                if (oldFEPDetailsFromDB2 != null)
                    oldWorkerMFId = oldFEPDetailsFromDB2.MostRecentMFFepId;
            }

            // Sanity Check: The assigned worker or the participant is not found, so dont go on.
            if (newWorker == null || participant == null)
                return;

            // We only need to update the FEP in CWW for W2. Do not proceed if we fail updating in cww. 
            if (pep.IsW2)
            {
                try
                {
                    // Remember this call only updates W-2.
                    UpdateFepInCww(_authUser.MainFrameId, pinNumber, oldWorkerMFId, newWorker.MFUserId);
                    Repo.ReassignW2CaseManagerInDB2(participant.PinNumber, newWorker.MFUserId);
                    CreateAndAddNewWorkerTask(newWorker, pep, modifiedDate);
                }
                catch (UserFriendlyException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw new Exception();
                }
            }
            else
            {
                // SP is only for NON-W2 programs.
                try
                {
                    if (!pep.IsFCDP)
                        Repo.ReassignLFCaseManagerInDB2(participant.PinNumber, newWorker.MFUserId);
                }
                catch
                {
                    Logger.Error($"Webservice for updating FEP failed: new worker is'{newWorker?.MFUserId}', old worker is '{oldWorkerMFId}'");
                }
            }

            // Learnfare we only update the LFFepId.
            if (pep.IsLF)
            {
                pep.LFFEPId = newWorker.Id;
                CreateAndAddNewWorkerTask(newWorker, pep, modifiedDate);
            }
            else
                pep.WorkerId = newWorker.Id;

            try
            {
                Repo.UpdateT0532(participant.PinNumber, newWorker.MFUserId, pep.EnrolledProgram.ProgramCode);

                try
                {
                    pep.ModifiedBy   = _authUser.Username;
                    pep.ModifiedDate = DateTime.Now;

                    #region WorkerTasks

                    if (Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear().IsSameOrBefore(DateTime.Today))
                    {
                        pep.Participant
                           .WorkerTaskLists
                           .Where(i => i.WorkerTaskStatus.Code == WorkerTaskStatus.Open)
                           .ForEach(i =>
                                    {
                                        if (i.IsSystemGenerated == true)
                                            i.Worker = newWorker;
                                        else
                                            i.WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Closed).Id;

                                        i.ModifiedBy   = _authUser.WIUID;
                                        i.ModifiedDate = DateTime.Now;
                                    });
                    }

                    #endregion

                    Repo.Save();
                }
                catch (Exception e)
                {
                    // Need to revert the changes to CWW via the webservice if we tried to save a W2.
                    if (pep.IsW2)
                    {
                        // The previous worker is the one from the call before that was new.
                        UpdateFepInCww(_authUser.MainFrameId, pep.Participant.PinNumber.ToString(), newWorker?.MFUserId, oldWorkerMFId);
                    }
                }
            }
            catch (Exception e)
            {
                // Need to revert the changes to CWW via the webservice if we tried to save a W2.
                if (pep.IsW2)
                {
                    // The previous worker is the one from the call before that was new.
                    UpdateFepInCww(_authUser.MainFrameId, pep.Participant.PinNumber.ToString(), newWorker?.MFUserId, oldWorkerMFId);
                }
            }
        }

        public void CreateAndAddNewWorkerTask(IWorker newWorker, IParticipantEnrolledProgram pep, DateTime modifiedDate)
        {
            var category = pep.IsCF
                               ? Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.ChildrenFirstEnrolledCode)
                               : pep.IsFCDP
                                   ? Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.FCDPEnrolledCode)
                                   : Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.PinReassignCode);
            var workerTaskListContract = new WorkerTaskList
                                         {
                                             TaskDetails        = category.Description,
                                             CategoryId         = category.Id,
                                             WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Open).Id,
                                             WorkerId           = newWorker.Id,
                                             ParticipantId      = pep.ParticipantId,
                                             TaskDate           = modifiedDate,
                                             StatusDate         = _authUser.CDODate ?? modifiedDate,
                                             IsSystemGenerated  = true,
                                             ModifiedBy         = _authUser.WIUID,
                                             ModifiedDate       = modifiedDate
                                         };

            Repo.NewWorkerTask(workerTaskListContract);
        }

        private OtherDemographicInformationContract GetPinOtherDemographicInformation(IParticipant participant)
        {
            // Grab Our Data.
            var demoGraphic = participant.OtherDemographics.LastOrDefault();
            var contactInfo = participant.ParticipantContactInfoes.LastOrDefault();

            var householdAddresss = new PlaceAddress
                                    {
                                        StreetAddress = contactInfo?.AddressLine1,
                                        AptUnit       = contactInfo?.AddressLine2,
                                        City          = contactInfo?.City?.Name,
                                        State         = contactInfo?.City?.State?.Name,
                                        ZipCode       = contactInfo?.ZipCode
                                    };

            var mailingAddress = new PlaceAddress
                                 {
                                     StreetAddress = contactInfo?.AlternateMailingAddress?.AddressLine1,
                                     AptUnit       = contactInfo?.AlternateMailingAddress?.AddressLine2,
                                     City          = contactInfo?.AlternateMailingAddress?.City?.Name,
                                     State         = contactInfo?.AlternateMailingAddress?.City?.State?.Name,
                                     ZipCode       = contactInfo?.AlternateMailingAddress?.ZipCode
                                 };

            // DemoGraphic CountyAndTribe is the tribe.
            var otherDemographicInformationContract = OtherDemographicInformationContract.Create(demoGraphic?.IsInterpreterNeeded, participant.HasAlias, demoGraphic?.Language?.Name, demoGraphic?.IsRefugee,
                                                                                                 demoGraphic?.RefugeeEntryDate, demoGraphic?.Country?.Name, demoGraphic?.TribalIndicator, demoGraphic?.TribalId,
                                                                                                 demoGraphic?.CountyAndTribe?.CountyName, demoGraphic?.TribalDetails, contactInfo?.CountyAndTribe?.CountyName, householdAddresss, mailingAddress,
                                                                                                 contactInfo?.PrimaryPhoneNumber, contactInfo?.SecondaryPhoneNumber, contactInfo?.EmailAddress, contactInfo?.HomelessIndicator
                                                                                                );

            return otherDemographicInformationContract;
        }

        private PinDetailsContract GetPinDetailsContract(ISP_ParticipantDetailsReturnType refreshParticipantDetails)
        {
            PinDetailsContract pinDetailsContract = null;

            if (refreshParticipantDetails != null)
            {
                pinDetailsContract = PinDetailsContract.Create(
                                                               GetParticipantBasicInfo(refreshParticipantDetails),
                                                               GetAddressInfo(refreshParticipantDetails),
                                                               GetOfficeCountyInfo(refreshParticipantDetails),
                                                               GetW2EligibilityInfo(refreshParticipantDetails),
                                                               GetEnrolledProgramInfo(refreshParticipantDetails),
                                                               GetRelatedPersons(refreshParticipantDetails),
                                                               GetCwwTransferDetails(refreshParticipantDetails),
                                                               GetMostRecentFepResultFromDB2(refreshParticipantDetails),
                                                               GetOfficeTransferId(refreshParticipantDetails),
                                                               refreshParticipantDetails.DestinationWPOfficeNumber
                                                              );
            }

            return pinDetailsContract;
        }

        private ParticipantsContract GetParticipantsPepContract(IParticipant participant, bool isConfidentialCase, bool hasAccess)
        {
            var contactInfo = participant.ParticipantContactInfoes.FirstOrDefault();

            var countyOfResidenceId          = contactInfo?.CountyOfResidenceId;
            var enrolledProgs                = ParticipantHelper.GetMostRecentEnrolledPrograms(_authUser, participant, _countyAndTribes, OrgList, OfficeList, Repo);
            var totalLifeTImeSubsidizedHours = participant.EmploymentInformations.Sum(i => i.TotalSubsidizedHours);

            var contract = ParticipantsContract.Create(
                                                       participant.Id,
                                                       participant.FirstName,
                                                       participant.MiddleInitialName,
                                                       participant.LastName,
                                                       participant.SuffixName,
                                                       participant.PinNumber,
                                                       participant.DateOfBirth,
                                                       isConfidentialCase,
                                                       hasAccess,
                                                       participant.ParticipantEnrolledProgramCutOverBridges.FirstOrDefault(i => i.EnrolledProgram?.ProgramCode.SafeTrim() == EnrolledProgram.W2ProgramCode)?.CutOverDate.ToString("MM/dd/yyyy"),
                                                       participant.Is60DaysVerified,
                                                       participant.GenderIndicator,
                                                       enrolledProgs,
                                                       countyOfResidenceId: countyOfResidenceId,
                                                       hasBeenThroughClientReg: participant.HasBeenThroughClientReg,
                                                       mciId: participant.MCI_ID,
                                                       totalLifeTimeSubsidizedHours: totalLifeTImeSubsidizedHours,
                                                       totalLifeTimeHoursDate: participant.TotalLifetimeHoursDate
                                                      );

            return (contract);
        }

        private ParticipantsContract GetParticipantsContract(IParticipant participant, bool isConfidentialCase, bool hasAccess)
        {
            var totalLifeTImeSubsidizedHours = participant.EmploymentInformations.Sum(i => i.TotalSubsidizedHours);
            var contract = ParticipantsContract.Create(participant.Id,
                                                       participant.FirstName,
                                                       participant.MiddleInitialName,
                                                       participant.LastName,
                                                       participant.SuffixName,
                                                       participant.PinNumber,
                                                       participant.DateOfBirth,
                                                       isConfidentialCase,
                                                       hasAccess,
                                                       participant.ParticipantEnrolledProgramCutOverBridges.FirstOrDefault(i => i.EnrolledProgram?.ProgramCode.SafeTrim() == EnrolledProgram.W2ProgramCode)?.CutOverDate.ToString("MM/dd/yyyy"),
                                                       participant.Is60DaysVerified,
                                                       participant.GenderIndicator,
                                                       hasBeenThroughClientReg: participant.HasBeenThroughClientReg,
                                                       mciId: participant.MCI_ID,
                                                       totalLifeTimeSubsidizedHours: totalLifeTImeSubsidizedHours, totalLifeTimeHoursDate: participant.TotalLifetimeHoursDate);

            return (contract);
        }

        private BasicInfoContract GetParticipantBasicInfo(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var participantBasicInfo = new BasicInfoContract
                                       {
                                           FirstName         = participantDetails.FirstName.SafeTrim(),
                                           MiddleInitialName = participantDetails.MiddleInitialName.SafeTrim(),
                                           LastName          = participantDetails.LastName.SafeTrim(),
                                           SuffixName        = participantDetails.SuffixName.SafeTrim(),
                                           DateOfBirth       = participantDetails.DateOfBirth,
                                           Age               = participantDetails.Age,
                                           CaseNumber        = participantDetails.CaseNumber,
                                           PinNumber         = participantDetails.PinNumber,
                                           RefugeeCode       = string.IsNullOrWhiteSpace(participantDetails.RefugeeStatusCode) ? "No" : "Yes",
                                           RefugeeEntryDate  = participantDetails.RefugeeEntryDate,
                                           GenderIndicator   = participantDetails.GenderIndicator?.Trim(),
                                           RaceCode          = GetRaceInfo(participantDetails),
                                           IsHispanic        = participantDetails.Hispanic,
                                           CountryOfOrigin   = participantDetails.CountryOfOrigin,
                                           MFWorkerId        = participantDetails.MFWorkerId
                                       };

            return participantBasicInfo;
        }

        private string GetRaceInfo(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var list = new List<string>();

            if (participantDetails.AmericanIndian == true)
            {
                list.Add("American Indian / Alaskan");
            }

            if (participantDetails.Asian == true)
            {
                list.Add("Asian");
            }

            if (participantDetails.Black == true)
            {
                list.Add("Black / African American");
            }

            if (participantDetails.PacificIslander == true)
            {
                list.Add("Hawaiian / Other Pacific Islander");
            }

            if (participantDetails.White == true)
            {
                list.Add("White");
            }

            return string.Join(", ", list);
        }

        private AddressContract GetAddressInfo(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var addressInfo = new AddressContract
                              {
                                  AddressLine1                = participantDetails.AddressLine1?.Trim(),
                                  AddressLine2                = participantDetails.AddressLine2?.Trim(),
                                  City                        = participantDetails.City?.Trim(),
                                  State                       = participantDetails.State,
                                  ZipCode                     = participantDetails.ZipCode,
                                  AlternateAddress1           = participantDetails.AlternateAddressLine1?.Trim(),
                                  AlternateAddress2           = participantDetails.AlternateAddressLine2?.Trim(),
                                  AlternateCity               = participantDetails.AlternateCity?.Trim(),
                                  AlternateState              = participantDetails.AlternateState?.Trim(),
                                  AlternateZipCode            = participantDetails.AlternateZipCode,
                                  LivingArrangement           = participantDetails.LivingArrangement?.Trim(),
                                  PrimaryPhoneNumber          = participantDetails.PhoneNumber?.Trim(),
                                  AlternatePrimaryPhoneNumber = participantDetails.AlternatePhoneNumber?.Trim(),
                                  EmailAddress                = participantDetails.EmailAddress?.Trim()
                              };

            return addressInfo;
        }

        private OfficeCountyContract GetOfficeCountyInfo(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var geoArea = Repo.WpGeoAreaByOfficeNumber(participantDetails.OfficeNumber.GetValueOrDefault());

            var officeCountyInfo = new OfficeCountyContract
                                   {
                                       WPGeoArea = geoArea?.Name?.SafeTrim()
                                   };

            return officeCountyInfo;
        }

        private W2EligibilityContract GetW2EligibilityInfo(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var w2EligibilityInfo = new W2EligibilityContract
                                    {
                                        AGStatuseCode        = participantDetails.AGStatusCode,
                                        AGSequenceNumber     = participantDetails.AGSequenceNumber,
                                        EligibilityBeginDate = participantDetails.EligibilityBeginDate,
                                        EligibilityEndDate   = participantDetails.EligibilityEndDate,
                                        PaymentBeginDate     = participantDetails.PaymentBeginDate,
                                        PaymentEndDate       = participantDetails.PaymentEndDate,
                                        AGFailureReasonCode1 = participantDetails.AGFailureReasonCode1,
                                        AGFailureReasonCode2 = participantDetails.AGFailureReasonCode2,
                                        AGFailureReasonCode3 = participantDetails.AGFailureReasonCode3,
                                        TwoParentStatus      = participantDetails.TwoParentStatus == true,
                                        LearnFareStatus      = !string.IsNullOrWhiteSpace(participantDetails.LearnFareStatus) && participantDetails.LearnFareStatus != "NA",
                                        EPReviewDueDate      = participantDetails.EPReviewDueDate,
                                        ReviewDueDate        = participantDetails.ReviewDueDate,
                                        FSAgOpen             = participantDetails.FSAgOpen,
                                        MAAgOpen             = participantDetails.MAAgOpen,
                                        FPWAgOpen            = participantDetails.FPWAgOpen,
                                        CCAgOpen             = participantDetails.CCAgOpen,
                                        FsetStatus           = participantDetails.FSETStatus,
                                        ChildSupportStatus   = participantDetails.ChildSupportStatus,
                                        DaysInPlacement      = participantDetails.DaysInPlacement,
                                        PlacementCode        = participantDetails.PlacementCode,
                                        MoreThanSixIndv      = participantDetails.MoreThanSixIndv
                                    };

            return w2EligibilityInfo;
        }

        private OfficeTransferContract GetCwwTransferDetails(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var cwwTransferDetails = new OfficeTransferContract
                                     {
                                         NewFepId     = participantDetails.NewFepId,
                                         FepOutOfSync = participantDetails.FepOutOfSync
                                     };

            return cwwTransferDetails;
        }

        private SP_MostRecentFEPFromDB2_Result GetMostRecentFepResultFromDB2(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var mostRecentFepDetails = Repo.GetMostRecentFepDetails(participantDetails.PinNumber.ToString());

            var mostRecentFEPFromDB2_Result = new SP_MostRecentFEPFromDB2_Result
                                              {
                                                  MostRecentMFFepId = mostRecentFepDetails?.MostRecentMFFepId,
                                                  Id                = mostRecentFepDetails?.Id
                                              };

            return mostRecentFEPFromDB2_Result;
        }

        private EnrolledProgramContract GetEnrolledProgramInfo(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var pepStatusCode = Repo.GetEnrolledProgramStatus(participantDetails.EnrolledProgramStatusCodeId);

            var pepInfo = Repo.GetParticipant(participantDetails.PinNumber.ToString())?
                              .ParticipantEnrolledPrograms
                              ?.Where(x => x.EnrolledProgramId == participantDetails.EnrolledProgramId)
                              .FirstOrDefault();

            var worker = new WorkerContract
                         {
                             WamsId    = pepInfo?.Worker?.WAMSId,
                             WorkerId  = pepInfo?.Worker?.MFUserId,
                             Wiuid     = pepInfo?.Worker?.WIUID,
                             FirstName = pepInfo?.Worker?.FirstName.ToTitleCase(),
                             LastName  = pepInfo?.Worker?.LastName.ToTitleCase()
                         };

            // If there is a LearnFare worker we will always pass it down
            // in the contract... we'll let the front end determine if it
            // should be displayed or not.
            var lfWorker = new WorkerContract
                           {
                               WamsId    = pepInfo?.LFFEP?.WAMSId,
                               WorkerId  = pepInfo?.LFFEP?.MFUserId,
                               Wiuid     = pepInfo?.Worker?.WIUID,
                               FirstName = pepInfo?.LFFEP?.FirstName.ToTitleCase(),
                               LastName  = pepInfo?.LFFEP?.LastName.ToTitleCase()
                           };

            var enrolledProgramContract = new EnrolledProgramContract
                                          {
                                              ProgramCode    = participantDetails.ProgramCode?.Trim(),
                                              SubProgramCode = participantDetails.SubProgramCode?.Trim(),
                                              Status         = pepStatusCode,
                                              StatusDate = participantDetails.EnrolledProgramStatusCodeId == 2 ? pepInfo?.ReferralDate : //TODO: replace with constants
                                                           participantDetails.EnrolledProgramStatusCodeId == 3 ? pepInfo?.EnrollmentDate :
                                                           participantDetails.EnrolledProgramStatusCodeId == 4 ? pepInfo?.DisenrollmentDate : null,
                                              AssignedWorker = worker,
                                              LearnFareFEP   = lfWorker
                                          };

            return enrolledProgramContract;
        }

        private int? GetOfficeTransferId(ISP_ParticipantDetailsReturnType participantDetails)
        {
            if (participantDetails.OfficeOutOfSyncIndicator.HasValue &&
                participantDetails.OfficeOutOfSyncIndicator.Value    &&
                participantDetails.DestinationWPOfficeNumber.HasValue)
            {
                // Look up the destiantion Office ID.
                if (participantDetails.EnrolledProgramId == null)
                {
                    // TODO: Fix as transfer user stories
                    //throw new DCFApplicationException("participantDetails.EnrolledProgramId is null");
                    return null;
                }

                var enrolledProgramId = (int) participantDetails.EnrolledProgramId;

                if (enrolledProgramId == 1)
                {
                    enrolledProgramId = 11;
                }

                var destOffice = Repo.GetOfficeByNumberAndProgram(participantDetails.DestinationWPOfficeNumber.Value.ToString(), enrolledProgramId);
                return destOffice?.Id;
            }

            // If in sync, return null;
            return null;
        }

        private List<RelatedPersonContract> GetRelatedPersons(ISP_ParticipantDetailsReturnType participantDetails)
        {
            var list = new List<RelatedPersonContract>();

            if (participantDetails.OtherPersonPinNumber1.HasValue                    ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonFirstName1) ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonLastName1))

            {
                var person = new RelatedPersonContract
                             {
                                 Pin          = participantDetails.OtherPersonPinNumber1,
                                 FirstName    = participantDetails.OtherPersonFirstName1.ToTitleCase(),
                                 LastName     = participantDetails.OtherPersonLastName1.ToTitleCase(),
                                 DateOfBirth  = participantDetails.OtherPersonDOB1,
                                 Age          = participantDetails.OtherPersonAge1,
                                 Relationship = participantDetails.Relationship1
                             };

                list.Add(person);
            }

            if (participantDetails.OtherPersonPinNumber2.HasValue                    ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonFirstName2) ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonLastName2))
            {
                var person = new RelatedPersonContract
                             {
                                 Pin          = participantDetails.OtherPersonPinNumber2,
                                 FirstName    = participantDetails.OtherPersonFirstName2.ToTitleCase(),
                                 LastName     = participantDetails.OtherPersonLastName2.ToTitleCase(),
                                 DateOfBirth  = participantDetails.OtherPersonDOB2,
                                 Age          = participantDetails.OtherPersonAge2,
                                 Relationship = participantDetails.Relationship2
                             };

                list.Add(person);
            }

            if (participantDetails.OtherPersonPinNumber3.HasValue                    ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonFirstName3) ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonLastName3))
            {
                var person = new RelatedPersonContract
                             {
                                 Pin          = participantDetails.OtherPersonPinNumber3,
                                 FirstName    = participantDetails.OtherPersonFirstName3.ToTitleCase(),
                                 LastName     = participantDetails.OtherPersonLastName3.ToTitleCase(),
                                 DateOfBirth  = participantDetails.OtherPersonDOB3,
                                 Age          = participantDetails.OtherPersonAge3,
                                 Relationship = participantDetails.Relationship3
                             };

                list.Add(person);
            }

            if (participantDetails.OtherPersonPinNumber4.HasValue                    ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonFirstName4) ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonLastName4))
            {
                var person = new RelatedPersonContract
                             {
                                 Pin          = participantDetails.OtherPersonPinNumber4,
                                 FirstName    = participantDetails.OtherPersonFirstName4.ToTitleCase(),
                                 LastName     = participantDetails.OtherPersonLastName4.ToTitleCase(),
                                 DateOfBirth  = participantDetails.OtherPersonDOB4,
                                 Age          = participantDetails.OtherPersonAge4,
                                 Relationship = participantDetails.Relationship4
                             };

                list.Add(person);
            }

            if (participantDetails.OtherPersonPinNumber5.HasValue                    ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonFirstName5) ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonLastName5))
            {
                var person = new RelatedPersonContract
                             {
                                 Pin          = participantDetails.OtherPersonPinNumber5,
                                 FirstName    = participantDetails.OtherPersonFirstName5.ToTitleCase(),
                                 LastName     = participantDetails.OtherPersonLastName5.ToTitleCase(),
                                 DateOfBirth  = participantDetails.OtherPersonDOB5,
                                 Age          = participantDetails.OtherPersonAge5,
                                 Relationship = participantDetails.Relationship5
                             };

                list.Add(person);
            }

            if (participantDetails.OtherPersonPinNumber6.HasValue                    ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonFirstName6) ||
                !string.IsNullOrWhiteSpace(participantDetails.OtherPersonLastName6))
            {
                var person = new RelatedPersonContract
                             {
                                 Pin          = participantDetails.OtherPersonPinNumber6,
                                 FirstName    = participantDetails.OtherPersonFirstName6.ToTitleCase(),
                                 LastName     = participantDetails.OtherPersonLastName6.ToTitleCase(),
                                 DateOfBirth  = participantDetails.OtherPersonDOB6,
                                 Age          = participantDetails.OtherPersonAge6,
                                 Relationship = participantDetails.Relationship6
                             };

                list.Add(person);
            }

            return list;
        }

        public void TransferParticipant(EnrolledProgramContract enrolledProgramContract, string pin)
        {
            if (!enrolledProgramContract.Id.HasValue || !enrolledProgramContract.OfficeId.HasValue)
            {
                throw new Exception("Invalid transfer Contract");
            }

            var pep                        = Repo.GetParticantEnrollment(enrolledProgramContract.Id.Value);
            var mostRecentFEPResultFromDB2 = Repo.GetMostRecentFepDetails(pin);
            var participantDetails         = Repo.GetParticipantDetails(pin);

            if (pep == null)
            {
                throw new Exception("PEP record is null.");
            }

            var modifiedDate = DateTime.Now;
            var sourceOffice = pep.Office;
            var destOffice   = Repo.GetOfficeById(enrolledProgramContract.OfficeId.Value);

            if (destOffice.ContractArea.EnrolledProgram.ProgramCode.TrimAndLower() != enrolledProgramContract.ProgramCd.TrimAndLower())
            {
                throw new Exception("Office doesnt support program type");
            }

            var isTransfer = Repo.hasOfficeTransfer(Participant.Id);

            // Before we can transfer, lets make sure we still pass our precheck.
            enrolledProgramContract.ContractorId   = destOffice.ContractAreaId;
            enrolledProgramContract.ContractorName = destOffice.ContractArea.ContractAreaName.SafeTrim();

            var precheck = ExecuteTransferRules(enrolledProgramContract, pep, Participant.ParticipantEnrolledPrograms.ToList(), isTransfer, 0);

            if (precheck.CanDisenroll != true)
            {
                return;
            }

            IWorker destWorker = null;

            if (enrolledProgramContract?.AssignedWorker?.Id != null)
            {
                destWorker = Repo.WorkerById(enrolledProgramContract.AssignedWorker.Id.Value);
            }

            var cfMilPep = Participant.ParticipantEnrolledPrograms.FirstOrDefault(x => x.IsCF && x.IsInMilwaukee && (x.IsEnrolled || x.IsReferred));

            // Special case for W-2 co-enrolled with CF in Milwaukee.
            if (pep.IsW2 && pep.IsInMilwaukee && cfMilPep != null)
            {
                // We do not need to add a new transfer record or write back to Db2 for this case.
                cfMilPep.Office = destOffice;
                cfMilPep.Worker = destWorker;
            }

            #region Transaction

            if (pep.IsW2 || pep.IsLF)
            {
                var transactionContract = new TransactionContract
                                          {
                                              ParticipantId       = pep.ParticipantId,
                                              WorkerId            = Repo.WorkerByWIUID(_authUser.WIUID).Id,
                                              OfficeId            = sourceOffice.Id,
                                              EffectiveDate       = modifiedDate,
                                              CreatedDate         = modifiedDate,
                                              TransactionTypeCode = TransactionTypes.TransferOut,
                                              ModifiedBy          = _authUser.WIUID
                                          };

                var transferOutTransaction = _transactionDomain.InsertTransaction(transactionContract, true);

                if (transferOutTransaction != null)
                    Repo.NewTransaction(transferOutTransaction as ITransaction);

                transactionContract.OfficeId            = destOffice.Id;
                transactionContract.TransactionTypeCode = TransactionTypes.TransferIn;

                var transferInTransaction = _transactionDomain.InsertTransaction(transactionContract, true);

                if (transferInTransaction != null)
                    Repo.NewTransaction(transferInTransaction as ITransaction);
            }

            #endregion

            #region WorkerTasks

            if (Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear().IsSameOrBefore(DateTime.Today))
            {
                var destinationWorker = destWorker;

                pep.Participant
                   .WorkerTaskLists
                   .Where(i => i.WorkerTaskStatus.Code == WorkerTaskStatus.Open)
                   .ForEach(i =>
                            {
                                if ((((pep.IsW2 || pep.IsLF) && pep.IsInBalanceOfState) || pep.IsTJ) && destinationWorker != null)
                                {
                                    if (i.IsSystemGenerated == true)
                                        i.Worker = destinationWorker;
                                    else
                                        i.WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Closed).Id;

                                    i.ModifiedBy   = _authUser.WIUID;
                                    i.ModifiedDate = modifiedDate;
                                }
                                else
                                    if ((pep.IsW2 || pep.IsLF) && pep.IsInMilwaukee)
                                    {
                                        i.WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Closed).Id;
                                        i.ModifiedBy         = _authUser.WIUID;
                                        i.ModifiedDate       = modifiedDate;
                                    }
                            });


                if ((pep.IsW2 || pep.IsLF) && destinationWorker != null)
                {
                    var category = Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.PinTransferCode);
                    var workerTaskListContract = new WorkerTaskList
                                                 {
                                                     TaskDetails        = category.Description,
                                                     CategoryId         = category.Id,
                                                     WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Open).Id,
                                                     WorkerId           = destWorker.Id,
                                                     ParticipantId      = pep.ParticipantId,
                                                     TaskDate           = modifiedDate,
                                                     StatusDate         = _authUser.CDODate ?? modifiedDate,
                                                     IsSystemGenerated  = true,
                                                     ModifiedBy         = _authUser.WIUID,
                                                     ModifiedDate       = modifiedDate
                                                 };
                    Repo.NewWorkerTask(workerTaskListContract);
                }
            }

            #endregion

            // Lets update the FEP in CWW for W-2.
            if (pep.IsW2)
            {
                if (destWorker?.MFUserId != mostRecentFEPResultFromDB2?.MostRecentMFFepId)
                {
                    try
                    {
                        if (pep.IsInMilwaukee && destWorker?.MFUserId == null)
                        {
                            UpdateFepInCww(_authUser.MainFrameId, pep.Participant.PinNumber.ToString(), mostRecentFEPResultFromDB2?.MostRecentMFFepId, mostRecentFEPResultFromDB2?.MostRecentMFFepId);
                        }
                        else
                        {
                            // If the web service returns an error, leave FEP blank in WWP.
                            UpdateFepInCww(_authUser.MainFrameId, pep.Participant.PinNumber.ToString(), mostRecentFEPResultFromDB2?.MostRecentMFFepId, destWorker?.MFUserId);
                        }
                    }
                    catch (Exception e)
                    {
                        destWorker = null;
                    }
                }
            }

            // Lets add a Transfer record, for historical reasons.
            Repo.NewOfficeTransfer(pep, sourceOffice, destOffice, destWorker?.Id, AuthUser.Username);

            Repo.TransferPariticipant(pep, sourceOffice, destOffice, pep.Worker, destWorker, AuthUser.MainFrameId, mostRecentFEPResultFromDB2?.MostRecentMFFepId);

            pep.ModifiedBy   = _authUser.Username;
            pep.ModifiedDate = modifiedDate;

            try
            {
                if (pep.IsInMilwaukee)
                {
                    _epDomain.EPTransfer(pep, modifiedDate);
                    _unitOfWork.Commit();
                }

                Repo.Save();
            }
            catch (Exception ex)
            {
                throw new DCFApplicationException("Transfer failed.  Please try again.", ex);
            }
        }

        private DisenrollCheckContract ExecuteTransferRules(EnrolledProgramContract transferContract, IParticipantEnrolledProgram transferPep, List<IParticipantEnrolledProgram> peps, bool isTransfer, int elapsedCount)
        {
            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category == RuleReason.Transfer && i.SubCategory
                                                                                                 == RuleReason.PreCheckError).ToList()
                                         };

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("PreTransferCheck")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(transferContract);
            session.Insert(transferPep);
            session.Insert(peps);
            session.Insert(isTransfer);
            session.Insert(elapsedCount);

            session.Fire();

            var disenrollCheckContract = new DisenrollCheckContract();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        disenrollCheckContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        disenrollCheckContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            disenrollCheckContract.CanDisenroll = disenrollCheckContract.Errors?.Count == 0;

            return disenrollCheckContract;
        }

        /// <summary>
        /// Calls a Webservice to update FEP in CWW. This call only updates W-2 records inside of DB2. (Not LF)
        /// </summary>
        /// <param name="mainFrameId"></param>
        /// <param name="pinNumber"></param>
        /// <param name="previousFepMainFrameId"></param>
        /// <param name="newFepMainFrameId"></param>
        private void UpdateFepInCww(string mainFrameId, string pinNumber, string previousFepMainFrameId, string newFepMainFrameId)
        {
            var updateFepInformationRequest = new UpdateFEPInformationRequest();
            updateFepInformationRequest.WorkerId  = mainFrameId;
            updateFepInformationRequest.PINNumber = pinNumber.PadLeft(10, '0');
            // Lets send an empty string when pep has no previous worker.
            updateFepInformationRequest.PreviousFEPId    = previousFepMainFrameId ?? "";
            updateFepInformationRequest.CurrentFEPId     = newFepMainFrameId;
            updateFepInformationRequest.ExternalAgencyId = _externalAgencyId;
            updateFepInformationRequest.IPAddress        = _iPAddress;
            var response = _cwwIndSvc.UpdateFEPInformation(updateFepInformationRequest);

            // Web Service sent back errors.
            if (response.Errors != null && response.Errors.Length > 0)
                throw new UserFriendlyException("Error assigning FEP in CWW", response.Errors[0].ErrorMessage);

            UpdateFepForInWwp(newFepMainFrameId, response.UpdatedPINs);
        }

        /// <summary>
        /// Updates Fep(s) in most recent W-2 or LearnFare episode in WWP for given pins.
        /// </summary>       
        /// <param name="pinNumbers"></param>      
        /// <param name="newFepMainFrameId"></param>
        private void UpdateFepForInWwp(string newFepMainFrameId, string[] pinNumbers)
        {
            if (pinNumbers == null || pinNumbers.Length == 0 || newFepMainFrameId.IsNullOrEmpty())
                return;

            var fepReassignVm = new ReassignFepViewModel(Repo);

            fepReassignVm.UpdatePins(newFepMainFrameId, pinNumbers, false);
        }

        public PreAddingParticipationStatusContract ExecutePSAddRules(ParticipantStatusContract participantStatusContract, List<IParticipantEnrolledProgram> peps, List<IEmploymentInformation> employments, List<Activity> activities)
        {
            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category == RuleReason.PSAdd && i.SubCategory
                                                                                                 == RuleReason.PreCheckError).ToList()
                                         };

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("PSAdd")));

            var factory  = repository.Compile();
            var session  = factory.CreateSession();
            var contract = participantStatusContract;


            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(contract);
            session.Insert(employments);
            session.Insert(peps);
            session.Insert(activities);

            session.Fire();

            var preAddingParticipationStatusContract = new PreAddingParticipationStatusContract();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        preAddingParticipationStatusContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        preAddingParticipationStatusContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            preAddingParticipationStatusContract.CanAddParticipationStatus = preAddingParticipationStatusContract.Errors?.Count == 0;

            return preAddingParticipationStatusContract;
        }
    }
}
