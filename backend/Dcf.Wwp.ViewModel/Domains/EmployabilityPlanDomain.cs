using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using NRules;
using NRules.Fluent;
using Activity = Dcf.Wwp.DataAccess.Models.Activity;
using IActivityRepository = Dcf.Wwp.DataAccess.Interfaces.IActivityRepository;
using IEmployabilityPlanRepository = Dcf.Wwp.DataAccess.Interfaces.IEmployabilityPlanRepository;
using IRuleReason = Dcf.Wwp.Model.Interface.IRuleReason;
using Dcf.Wwp.Model.Interface.Constants;
using ActivityCompletionReason = Dcf.Wwp.Model.Interface.Constants.ActivityCompletionReason;
using EmployabilityPlan = Dcf.Wwp.DataAccess.Models.EmployabilityPlan;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using IGoalRepository = Dcf.Wwp.DataAccess.Interfaces.IGoalRepository;
using IOfficeRepository = Dcf.Wwp.DataAccess.Interfaces.IOfficeRepository;
using IParticipantRepository = Dcf.Wwp.DataAccess.Interfaces.IParticipantRepository;
using IRuleReasonRepository = Dcf.Wwp.DataAccess.Interfaces.IRuleReasonRepository;
using IWorkerRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerRepository;
using IWorkerTaskListRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerTaskListRepository;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using RuleReason = Dcf.Wwp.Data.Sql.Model.RuleReason;
using WorkerTaskStatus = Dcf.Wwp.Model.Interface.Constants.WorkerTaskStatus;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class EmployabilityPlanDomain : IEmployabilityPlanDomain
    {
        #region Properties

        private readonly IEmployabilityPlanRepository                       _epRepo;
        private readonly IActivityRepository                                _activityRepo;
        private readonly IGoalRepository                                    _goalRepo;
        private readonly IDocumentRepository                                _docRepo;
        private readonly IParticipantRepository                             _participantRepo;
        private readonly ISupportiveServiceRepository                       _supportiveServicesRepo;
        private readonly IWorkerRepository                                  _workerRepo;
        private readonly IUnitOfWork                                        _unitOfWork;
        private readonly IAuthUser                                          _authUser;
        private readonly IEmployabilityPlanGoalBridgeRepository             _epGoalBridgeRepo;
        private readonly IEmployabilityPlanActivityBridgeRepository         _epActivityBridgeRepo;
        private readonly IActivityScheduleRepository                        _activityScheduleRepository;
        private readonly IEmployabilityPlanStatusTypeRepository             _employabilityPlanStatusTypeRepo;
        private readonly IRuleReasonRepository                              _ruleReasonRepository;
        private readonly IActivityScheduleFrequencyBridgeRepository         _activityScheduleFrequencyBridgeRepository;
        private readonly IEmployabilityPlanEmploymentInfoBridgeRepository   _epeiRepo;
        private readonly IOfficeRepository                                  _officeRepo;
        private readonly IActivityContactBridgeRepository                   _activityContactBridgeRepository;
        private readonly INonSelfDirectedActivityRepository                 _nonSelfDirActivityRepo;
        private readonly IActivityDomain                                    _activityDomain;
        private readonly IGoalDomain                                        _goalDomain;
        private readonly ITransactionDomain                                 _transactionDomain;
        private readonly IPOPClaimDomain                                    _popClaimDomain;
        private readonly IParticipationEntryRepository                      _participationEntryRepo;
        private readonly IParticipantEnrolledProgramCutOverBridgeRepository _participantEnrolledProgramCutOverBridgeRepository;
        private readonly IWorkerTaskStatusRepository                        _workerTaskStatusRepository;
        private readonly IWorkerTaskCategoryRepository                      _workerTaskCategoryRepository;
        private readonly IWorkerTaskListRepository                          _workerTaskListRepository;
        private readonly IFileUploadDomain                                  _fileUploadDomain;

        private readonly Func<string, string> _convertWIUIdToName; // this is sneaky, don't do this at home.

        #endregion

        #region Methods

        public EmployabilityPlanDomain(
            IActivityRepository                                activityRepo,
            IGoalRepository                                    goalRepo,
            IDocumentRepository                                docRepo,
            IEmployabilityPlanRepository                       epRepo,
            IParticipantRepository                             participantRepo,
            ISupportiveServiceRepository                       supportiveServicesRepo,
            IWorkerRepository                                  workerRepo,
            IUnitOfWork                                        unitOfWork,
            IAuthUser                                          authUser,
            IEmployabilityPlanGoalBridgeRepository             epGoalBridgeRepo,
            IEmployabilityPlanStatusTypeRepository             employabilityPlanStatusTypeRepo,
            IEmployabilityPlanActivityBridgeRepository         epActivityBridgeRepo,
            IActivityScheduleRepository                        activityScheduleRepository,
            IRuleReasonRepository                              ruleReasonRepository,
            IActivityScheduleFrequencyBridgeRepository         activityScheduleFrequencyBridgeRepository,
            IEmployabilityPlanEmploymentInfoBridgeRepository   epeiRepo,
            IOfficeRepository                                  officeRepo,
            IActivityContactBridgeRepository                   activityContactBridgeRepository,
            INonSelfDirectedActivityRepository                 nonSelfDirActivityRepo,
            IActivityDomain                                    activityDomain,
            IGoalDomain                                        goalDomain,
            ITransactionDomain                                 transactionDomain,
            IPOPClaimDomain                                    popClaimDomain,
            IParticipationEntryRepository                      participationEntryRepo,
            IParticipantEnrolledProgramCutOverBridgeRepository participantEnrolledProgramCutOverBridgeRepository,
            IWorkerTaskStatusRepository                        workerTaskStatusRepository,
            IWorkerTaskCategoryRepository                      workerTaskCategoryRepository,
            IWorkerTaskListRepository                          workerTaskListRepository,
            IFileUploadDomain                                  fileUploadDomain)
        {
            _activityRepo                                      = activityRepo;
            _epRepo                                            = epRepo;
            _docRepo                                           = docRepo;
            _supportiveServicesRepo                            = supportiveServicesRepo;
            _workerRepo                                        = workerRepo;
            _goalRepo                                          = goalRepo;
            _participantRepo                                   = participantRepo;
            _unitOfWork                                        = unitOfWork;
            _authUser                                          = authUser;
            _epGoalBridgeRepo                                  = epGoalBridgeRepo;
            _employabilityPlanStatusTypeRepo                   = employabilityPlanStatusTypeRepo;
            _epActivityBridgeRepo                              = epActivityBridgeRepo;
            _activityScheduleRepository                        = activityScheduleRepository;
            _ruleReasonRepository                              = ruleReasonRepository;
            _activityScheduleFrequencyBridgeRepository         = activityScheduleFrequencyBridgeRepository;
            _epeiRepo                                          = epeiRepo;
            _officeRepo                                        = officeRepo;
            _activityContactBridgeRepository                   = activityContactBridgeRepository;
            _nonSelfDirActivityRepo                            = nonSelfDirActivityRepo;
            _activityDomain                                    = activityDomain;
            _goalDomain                                        = goalDomain;
            _transactionDomain                                 = transactionDomain;
            _popClaimDomain                                    = popClaimDomain;
            _participationEntryRepo                            = participationEntryRepo;
            _participantEnrolledProgramCutOverBridgeRepository = participantEnrolledProgramCutOverBridgeRepository;
            _workerTaskStatusRepository                        = workerTaskStatusRepository;
            _workerTaskCategoryRepository                      = workerTaskCategoryRepository;
            _workerTaskListRepository                          = workerTaskListRepository;
            _fileUploadDomain                                  = fileUploadDomain;

            _convertWIUIdToName = (wiuId) =>
                                  {
                                      var wo = workerRepo.GetAsQueryable()
                                                         .Where(i => i.WIUId == wiuId)
                                                         .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                         .FirstOrDefault();

                                      var wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");

                                      return (wn);
                                  };
        }

        public List<EmployabilityPlanContract> GetEmployabilityPlans(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var pinNo = decimal.Parse(pin);

            var results = _epRepo.GetMany(ep => ep.Participant.PinNumber                 == pinNo)
                                 .OrderByDescending(i => i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                 .ThenByDescending(i => i.BeginDate)
                                 .ThenBy(i => i.EmployabilityPlanStatusType.SortOrder)
                                 .ThenBy(i => i.EnrolledProgram.SortOrder)
                                 .Select(ep => new EmployabilityPlanContract
                                               {
                                                   Id                              = ep.Id,
                                                   EnrolledProgramId               = ep.EnrolledProgramId,
                                                   EnrolledProgramName             = ep.EnrolledProgram?.Name,
                                                   EnrolledProgramCd               = ep.EnrolledProgram?.ProgramCode,
                                                   EmployabilityPlanStatusTypeId   = ep.EmployabilityPlanStatusTypeId,
                                                   EmployabilityPlanStatusTypeName = ep.EmployabilityPlanStatusType?.Name,
                                                   IsDeleted                       = ep.IsDeleted,
                                                   BeginDate                       = ep.BeginDate,
                                                   EndDate                         = ep.EndDate,
                                                   Notes                           = ep.Notes,
                                                   CanSaveWithoutActivity          = ep.CanSaveWithoutActivity,
                                                   CanSaveWithoutActivityDetails   = ep.CanSaveWithoutActivityDetails,
                                                   ModifiedBy                      = ep.ModifiedBy,
                                                   ModifiedDate                    = ep.ModifiedDate,
                                                   CreatedDate                     = ep.CreatedDate,
                                                   OrganizationId                  = ep.OrganizationId,
                                                   PEPId                           = ep.ParticipantEnrolledProgramId,
                                                   SubmitDate                      = ep.SubmitDate
                                               }).ToList();

            return (results);
        }

        public EmployabilityPlanContract GetPlanById(int epId)
        {
            // grab the EP
            var ep = _epRepo.Get(i => i.Id == epId && i.IsDeleted == false);

            // package it up
            var result = new EmployabilityPlanContract
                         {
                             Id                              = ep.Id,
                             EnrolledProgramId               = ep.EnrolledProgramId,
                             EnrolledProgramName             = ep.EnrolledProgram?.Name,
                             EnrolledProgramCd               = ep.EnrolledProgram?.ProgramCode,
                             EmployabilityPlanStatusTypeId   = ep.EmployabilityPlanStatusTypeId,
                             EmployabilityPlanStatusTypeName = ep.EmployabilityPlanStatusType?.Name,
                             IsDeleted                       = ep.IsDeleted,
                             BeginDate                       = ep.BeginDate,
                             EndDate                         = ep.EndDate,
                             Notes                           = ep.Notes,
                             CanSaveWithoutActivity          = ep.CanSaveWithoutActivity,
                             CanSaveWithoutActivityDetails   = ep.CanSaveWithoutActivityDetails,
                             ModifiedBy                      = _convertWIUIdToName(ep.ModifiedBy),
                             ModifiedDate                    = ep.ModifiedDate,
                             CreatedDate                     = ep.CreatedDate,
                             OrganizationId                  = ep.OrganizationId,
                             PEPId                           = ep.ParticipantEnrolledProgramId,
                             SubmitDate                      = ep.SubmitDate
                         };

            // send it off ;)
            return (result);
        }

        public DocumentResponse GetDocumentsForPin(string pin)
        {
            var docResponse = new DocumentResponse();
            var pinNo       = decimal.Parse(pin);
            var epBeginDate = DateTime.Now.AddYears(-7);

            var docs = _docRepo.GetMany(i => i.EmployabilityPlan.Participant.PinNumber == pinNo
                                             && i.EmployabilityPlan.BeginDate          >= epBeginDate
                                             && (i.EmployabilityPlan.EmployabilityPlanStatusType.Name    == EmployabilityPlanStatus.Submitted
                                                 || i.EmployabilityPlan.EmployabilityPlanStatusType.Name == EmployabilityPlanStatus.Ended)
                                             && !i.EmployabilityPlan.IsDeleted)
                               .OrderByDescending(i => i.ModifiedDate)
                               .ThenByDescending(i => i.Id)
                               .GroupBy(i => i.EmployabilityPlanId)
                               .Select(i => i.First())
                               .Select(j => new DocumentRequest
                                            {
                                                EpId         = j.EmployabilityPlanId,
                                                DocumentId   = $"W{j.Id}",
                                                IsDeleted    = j.IsDeleted,
                                                IsScanned    = j.IsScanned,
                                                IsSigned     = j.EmployabilityPlan.DateSigned.HasValue,
                                                UploadedDate = j.UploadedDate,
                                                ModifiedBy   = j.ModifiedBy,
                                                ModifiedDate = j.ModifiedDate
                                            }).ToList();

            //TODO: Once We have info on the fetch criteria from CM-- pass that info to RetrieveAllDocument method
            //var resp = _fileUploadDomain.RetrieveAllDocument(pin);

            //if (resp != null)
            //{
            //    docs = docs.Union(resp).ToList();
            //}

            docs = docs.Select(doc => doc.DocumentId)
                       .Distinct()
                       .Select(code => docs.First(doc => doc.DocumentId == code))
                       .ToList();

            docResponse.MessageCode = "SUCCESS";
            docResponse.Documents   = docs.ToList();

            return docResponse;
        }


        public bool DeletePlan(string pin, int epId, bool isVoid, bool isAutoDeleted, bool epTransfer)
        {
            var updateTime = DateTime.Now;
            var ep         = _epRepo.Get(i => i.Id == epId);
            var epStatus = isVoid
                               ? EmployabilityPlanStatus.WorkerVoided
                               : isAutoDeleted
                                   ? EmployabilityPlanStatus.SystemDeleted
                                   : EmployabilityPlanStatus.WorkerDeleted;
            var employabilityPlanStatusTypeId = _employabilityPlanStatusTypeRepo.Get(i => i.Name == epStatus).Id;

            ep.EmployabilityPlanStatusTypeId = employabilityPlanStatusTypeId;
            ep.IsDeleted                     = true;
            ep.ModifiedBy                    = _authUser.WIUID;
            ep.ModifiedDate                  = updateTime;

            if (!isVoid)
            {
                var goals = ep.EmployabilityPlanGoalBridges.Select(i => i.Goal).ToList();
                goals.AsNotNull().ForEach(i => _goalDomain.DeleteGoal(i.Id, epId, false));

                var activities = ep.EmploybilityPlanActivityBridges.Select(i => i.Activity).ToList();
                activities.AsNotNull().ForEach(i => _activityDomain.DeleteActivityAndSchedules(epId, i));

                var supportiveServices = ep.SupportiveServices.Select(ss => ss).ToList();
                supportiveServices.AsNotNull().ForEach(i => _supportiveServicesRepo.Delete(i));

                var epEmployments = ep.EmploybilityPlanEmploymentInfoBridges.Select(ei => ei).ToList();
                epEmployments.ForEach(i => _epeiRepo.Delete(i));
            }
            else
            {
                _epGoalBridgeRepo.Delete(i => i.EmployabilityPlanId           == epId);
                _epActivityBridgeRepo.Delete(i => i.EmployabilityPlanId       == epId);
                _epeiRepo.Delete(i => i.EmployabilityPlanId                   == epId);
                _activityScheduleRepository.Delete(i => i.EmployabilityPlanId == epId);
            }

            if (!isVoid && !epTransfer)
                _unitOfWork.Commit();

            return true;
        }

        // TODO: Refactor SubmitPlan method in TDD Style, Method is too Big.
        public EmployabilityPlanContract SubmitPlan(string pin, int epId)
        {
            var modifiedBy                    = _authUser.WIUID;
            var modifiedDate                  = DateTime.Now;
            var decimalPin                    = decimal.Parse(pin);
            var workerId                      = _workerRepo.Get(i => i.WIUId                   == _authUser.WIUID).Id;
            var employabilityPlans            = _epRepo.GetMany(ep => ep.Participant.PinNumber == decimalPin).ToList();
            var employabilityPlan             = employabilityPlans.First(i => i.Id             == epId);
            var enrolledProgramId             = employabilityPlan.EnrolledProgramId;
            var recentSubmittedEP             = _activityDomain.GetRecentSubmittedEP(pin, employabilityPlan.Id, enrolledProgramId);
            var recentSubmittedEPActivities   = recentSubmittedEP?.EmploybilityPlanActivityBridges.Select(i => i.Clone().ActivityId).ToList();
            var employabilityPlanStatusTypeId = _employabilityPlanStatusTypeRepo.Get(i => i.Name == EmployabilityPlanStatus.Submitted).Id;

            if (recentSubmittedEP != null && recentSubmittedEP.BeginDate == employabilityPlan.BeginDate)
                DeletePlan(pin, recentSubmittedEP.Id, true, false, false);

            employabilityPlan.EmployabilityPlanStatusTypeId = employabilityPlanStatusTypeId;
            employabilityPlan.SubmitDate                    = _authUser.CDODate ?? DateTime.Today;
            employabilityPlan.ModifiedBy                    = modifiedBy;
            employabilityPlan.ModifiedDate                  = modifiedDate;

            #region Cut Over

            var epStatuses = new List<string> { EmployabilityPlanStatus.Submitted, EmployabilityPlanStatus.Ended, EmployabilityPlanStatus.WorkerVoided };
            var updateCutOver = employabilityPlans.Count(i => i.EnrolledProgramId == enrolledProgramId &&
                                                              epStatuses.Contains(i.EmployabilityPlanStatusType?.Name)) == 0;

            if (updateCutOver)
            {
                var parms = new Dictionary<string, object>
                            {
                                ["PinNumber"]         = employabilityPlan.Participant.PinNumber.ToString(),
                                ["ParticipantId"]     = employabilityPlan.ParticipantId,
                                ["EnrolledProgramId"] = employabilityPlan.EnrolledProgramId,
                                ["BeginDate"]         = employabilityPlan.BeginDate.ToString("yyyy-MM-dd"),
                                ["MFUserId"]          = _authUser.MainFrameId,
                                ["ModifiedBy"]        = _authUser.WIUID
                            };

                var rs = _epRepo.GetStoredProcReturnValue("USP_EP_CutOver", parms);

                if (rs != 0) throw new DCFApplicationException("Failed due to SProc issue. Please try again.");
            }

            #endregion

            #region Transaction

            var canInsertVocationalPopClaim  = true;
            var canInsertEducationalPopClaim = true;
            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = employabilityPlan.ParticipantId,
                                          WorkerId            = workerId,
                                          OfficeId            = employabilityPlan.ParticipantEnrolledProgram.OfficeId.GetValueOrDefault(),
                                          EffectiveDate       = employabilityPlan.BeginDate,
                                          CreatedDate         = modifiedDate,
                                          TransactionTypeCode = TransactionTypes.NewEmployabilityPlan,
                                          ModifiedBy          = _authUser.WIUID,
                                      };

            _transactionDomain.InsertTransaction(transactionContract);

            employabilityPlan.EmploybilityPlanActivityBridges.ForEach(bridge =>
                                                                      {
                                                                          transactionContract.TransactionTypeCode = TransactionTypes.ActivityBegins;
                                                                          transactionContract.EffectiveDate       = bridge.Activity.StartDate.GetValueOrDefault();
                                                                          transactionContract.StatusCode          = bridge.Activity.ActivityType.Code;

                                                                          if (recentSubmittedEPActivities == null || !recentSubmittedEPActivities.Contains(bridge.ActivityId))
                                                                              _transactionDomain.InsertTransaction(transactionContract);

                                                                          if (bridge.Activity.EndDate != null)
                                                                          {
                                                                              transactionContract.EffectiveDate       = bridge.Activity.EndDate ?? modifiedDate;
                                                                              transactionContract.TransactionTypeCode = TransactionTypes.ActivityEnd;

                                                                              _transactionDomain.InsertTransaction(transactionContract);

                                                                              #region POPClaim

                                                                              if (canInsertVocationalPopClaim)
                                                                                  canInsertVocationalPopClaim = _popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, bridge.Activity.ActivityType.Code, bridge.Activity.ActivityCompletionReason.Name, bridge.Activity.EndDate, bridge.ActivityId, POPClaimType.VocationalTrainingCd);

                                                                              if (canInsertEducationalPopClaim)
                                                                                  canInsertEducationalPopClaim = _popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, bridge.Activity.ActivityType.Code, bridge.Activity.ActivityCompletionReason.Name, bridge.Activity.EndDate, bridge.ActivityId, POPClaimType.EducationalAttainmentCd);

                                                                              #endregion
                                                                          }
                                                                      });

            #endregion

            #region WorkerTask

            if (employabilityPlan.ParticipantEnrolledProgram.IsW2 || employabilityPlan.ParticipantEnrolledProgram.IsLF)
            {
                var openStatusWorkerTaskId = _workerTaskStatusRepository.Get(i => i.Code == WorkerTaskStatus.Open).Id;
                var workerTaskCategories   = _workerTaskCategoryRepository.GetAll().ToList();
                var careerAssessmentWorkerTask = new WorkerTaskList
                                                 {
                                                     WorkerTaskStatusId = _workerTaskStatusRepository.Get(i => i.Code == WorkerTaskStatus.Open).Id,
                                                     WorkerId           = workerId,
                                                     ParticipantId      = employabilityPlan.ParticipantId,
                                                     TaskDate           = modifiedDate,
                                                     StatusDate         = _authUser.CDODate ?? modifiedDate,
                                                     IsSystemGenerated  = true,
                                                     ModifiedBy         = _authUser.WIUID,
                                                     ModifiedDate       = modifiedDate
                                                 };

                var careerAssessmentWorkerTaskCategory = workerTaskCategories.First(j => j.Code == WorkerTaskCategoryCodes.CareerAssessmentCode);
                var openWorkerTasks                    = _workerTaskListRepository.GetMany(i => i.ParticipantId == employabilityPlan.ParticipantId && i.WorkerTaskStatusId == openStatusWorkerTaskId).ToList();

                if (employabilityPlan.Participant.CareerAssessments.Count == 0 && openWorkerTasks.All(i => i.CategoryId != careerAssessmentWorkerTaskCategory.Id))
                {
                    careerAssessmentWorkerTask.TaskDetails = careerAssessmentWorkerTaskCategory.Description;
                    careerAssessmentWorkerTask.CategoryId  = careerAssessmentWorkerTaskCategory.Id;

                    _workerTaskListRepository.Add(careerAssessmentWorkerTask);
                }

                var jobReadinessWorkerTaskCategory = workerTaskCategories.First(j => j.Code == WorkerTaskCategoryCodes.JobReadinessCode);

                if (employabilityPlan.Participant.JobReadinesses.Count == 0 && openWorkerTasks.All(i => i.CategoryId != jobReadinessWorkerTaskCategory.Id))
                {
                    var jobReadinessWorkerTask = careerAssessmentWorkerTask.Clone();

                    jobReadinessWorkerTask.TaskDetails = jobReadinessWorkerTaskCategory.Description;
                    jobReadinessWorkerTask.CategoryId  = jobReadinessWorkerTaskCategory.Id;

                    _workerTaskListRepository.Add(jobReadinessWorkerTask);
                }

                var testScoresWorkerTaskCategory = workerTaskCategories.First(j => j.Code == WorkerTaskCategoryCodes.TestScoresCode);

                if (employabilityPlan.Participant.EducationExams.Count == 0 && openWorkerTasks.All(i => i.CategoryId != testScoresWorkerTaskCategory.Id))
                {
                    var testScoresWorkerTask = careerAssessmentWorkerTask.Clone();

                    testScoresWorkerTask.TaskDetails = testScoresWorkerTaskCategory.Description;
                    testScoresWorkerTask.CategoryId  = testScoresWorkerTaskCategory.Id;

                    _workerTaskListRepository.Add(testScoresWorkerTask);
                }
            }

            #endregion

            if (recentSubmittedEP != null && recentSubmittedEP.BeginDate != employabilityPlan.BeginDate)
            {
                var activityIds = recentSubmittedEP.EmploybilityPlanActivityBridges?.Select(i => i.ActivityId).ToList();
                if (activityIds.Count > 0)
                {
                    var schedules = _activityScheduleRepository.GetMany(i => activityIds.Contains(i.ActivityId) && i.EmployabilityPlanId == recentSubmittedEP.Id).ToList();

                    foreach (var schedule in schedules)
                    {
                        DateTime? endDate = null;
                        if (schedule.IsRecurring == true && schedule.PlannedEndDate != null)
                            endDate = new DateTime(Math.Min(((DateTime) schedule.PlannedEndDate).Ticks, employabilityPlan.BeginDate.AddDays(-1).Ticks));
                        else
                            if (schedule.IsRecurring == false && schedule.StartDate != null)
                                endDate = new DateTime(Math.Min(((DateTime) schedule.StartDate).Ticks, employabilityPlan.BeginDate.AddDays(-1).Ticks));

                        schedule.ActualEndDate = endDate;
                    }
                }

                recentSubmittedEP.EndDate                       = employabilityPlan.BeginDate.AddDays(-1);
                recentSubmittedEP.EmployabilityPlanStatusTypeId = EmployabilityPlanStatus.EndedId;
                recentSubmittedEP.ModifiedBy                    = modifiedBy;
                recentSubmittedEP.ModifiedDate                  = modifiedDate;

                _epRepo.Update(recentSubmittedEP);
            }

            _activityDomain.EndStatus(epId, decimal.Parse(pin), null, modifiedDate);
            _epRepo.Update(employabilityPlan);
            EPTransactionalSave(employabilityPlan, recentSubmittedEP?.Id, employabilityPlan.EnrolledProgram?.ProgramCode.ToLower().SafeTrim(), employabilityPlan.ModifiedDate);

            var epContract = GetPlanById(epId);

            return (epContract);
        }

        public EndEPContract EndEP(EndEPContract contract, string pin, int epId)
        {
            var updateTime  = DateTime.Now;
            var ep          = _epRepo.Get(i => i.Id == epId);
            var goalIds     = contract.Goals.Select(i => i.Id).ToList();
            var activityIds = contract.Activities.Select(i => i.Id).ToList();
            var goals       = _goalRepo.GetMany(i => goalIds.Contains(i.Id)).ToList();
            var activities  = _activityRepo.GetMany(i => activityIds.Contains(i.Id)).ToList();
            var xml = new XElement("Activities", contract.Activities.Select(a => new XElement("Activity", new XElement("EPId", epId),
                                                                                              new XElement("ActivityId",       a.Id),
                                                                                              new XElement("EndDate",          a.EndDate)))).ToString();

            contract.Goals.ForEach(goal =>
                                   {
                                       var g = goals.FirstOrDefault(i => i.Id == goal.Id);

                                       if (g == null) return;
                                       g.EndDate          = goal.EndDate.ToDateTimeMonthDayYear();
                                       g.GoalEndReasonId  = goal.EndReasonId;
                                       g.IsGoalEnded      = true;
                                       g.EndReasonDetails = "Ended due to disenrollment";
                                       g.ModifiedBy       = _authUser.WIUID;
                                       g.ModifiedDate     = updateTime;

                                       _goalRepo.Update(g);
                                   });

            var activityContract = new List<ActivityContract>();
            contract.Activities.ForEach(a => activityContract.Add(
                                                                  new ActivityContract
                                                                  {
                                                                      Id                         = a.Id,
                                                                      ActivityCompletionReasonId = a.ActivityCompletionReasonId,
                                                                      EndDate                    = a.EndDate,
                                                                      ActivitySchedules = activities.FirstOrDefault(i => i.Id == a.Id)
                                                                                                    ?.ActivitySchedules.Where(i => i.EmployabilityPlanId == epId && !i.IsDeleted)
                                                                                                    .Select(asc => new ActivityScheduleContract
                                                                                                                   {
                                                                                                                       Id            = asc.Id,
                                                                                                                       ActualEndDate = a.EndDate
                                                                                                                   }).ToList()
                                                                  }));

            if (activityContract.Any())
                _activityDomain.UpsertElapsedActivity(activityContract, pin, epId, true, false);

            ep.EndDate                       = _authUser.CDODate ?? DateTime.Today;
            ep.EmployabilityPlanStatusTypeId = EmployabilityPlanStatus.EndedId;
            ep.ModifiedBy                    = _authUser.WIUID;
            ep.ModifiedDate                  = updateTime;

            _epRepo.Update(ep);
            _activityDomain.EndActivityTransactionalSave(xml, programCd: ep.EnrolledProgram.ProgramCode.ToLower().SafeTrim());

            return contract;
        }

        public PreCheckEPContract PreSaveCheck(int partId, bool submittingEP, EmployabilityPlanContract contract)
        {
            var activities = contract.Id == 0 ? new List<DataAccess.Models.EmployabilityPlanActivityBridge>() : _epActivityBridgeRepo.GetMany(i => i.EmployabilityPlanId == contract.Id).ToList();
            var pts = _participationEntryRepo.GetMany(i => i.ParticipantId == partId && i.ParticipationDate
                                                           >= contract.BeginDate     && i.ParticipatedHours != null && !i.IsDeleted);
            var ep         = _epRepo.GetMany(i => i.Id                         == contract.Id                   && !i.IsDeleted);
            var hasContact = ep.Any(e => e.EnrolledProgram?.ProgramCode.Trim() != EnrolledProgram.W2ProgramCode && e.ParticipantEnrolledProgram?.Worker?.WorkerContactInfos?.Count > 0);
            var possibleRuleReasons = _ruleReasonRepository.GetMany(i => (i.Category == Wwp.Model.Interface.Constants.RuleReason.EPSave || i.Category == Wwp.Model.Interface.Constants.RuleReason.EPSubmit)
                                                                         && i.SubCategory == Wwp.Model.Interface.Constants.RuleReason.PreCheckError).ToList();
            //ToDo: Better way to change RuleReason to IRuleReason
            var ruleReasons = new List<IRuleReason>();

            foreach (var possibleRuleReason in possibleRuleReasons)
            {
                var ruleReason = new RuleReason
                                 {
                                     Category    = possibleRuleReason.Category,
                                     SubCategory = possibleRuleReason.SubCategory,
                                     Name        = possibleRuleReason.Name,
                                     Code        = possibleRuleReason.Code
                                 };

                ruleReasons.Add(ruleReason);
            }

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = ruleReasons
                                         };

            var repository = new RuleRepository();

            if (submittingEP)
                repository.Load(i => i.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("EP") || rule.IsTagged("EPSubmit")));
            else
                repository.Load(i => i.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("EP")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(activities);
            session.Insert(contract);
            session.Insert(pts);
            session.Insert(hasContact);
            session.Fire();

            var preSavingEPContract = new PreCheckEPContract();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        preSavingEPContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        preSavingEPContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow delete if there are any errors.
            preSavingEPContract.CanSaveEP = preSavingEPContract.Errors?.Count == 0;

            return preSavingEPContract;
        }


        public EmployabilityPlanContract UpsertPlan(EmployabilityPlanContract employabilityPlanContract, string pin, int subsequentEPId)
        {
            var updateTime = DateTime.Now;
            var pinNo      = 0.0m;
            decimal.TryParse(pin, out pinNo);
            var participant = _participantRepo.Get(p => p.PinNumber                            == pinNo);
            var pep         = participant.ParticipantEnrolledPrograms.FirstOrDefault(i => i.Id == employabilityPlanContract.PEPId);

            var employabilityPlan = employabilityPlanContract.Id != 0 ? _epRepo.Get(g => g.Id == employabilityPlanContract.Id && g.IsDeleted == false) : _epRepo.New();

            employabilityPlan.Participant                   = participant;
            employabilityPlan.EnrolledProgramId             = employabilityPlanContract.EnrolledProgramId;
            employabilityPlan.BeginDate                     = employabilityPlanContract.BeginDate;
            employabilityPlan.EndDate                       = employabilityPlanContract.EndDate;
            employabilityPlan.EmployabilityPlanStatusTypeId = employabilityPlanContract.EmployabilityPlanStatusTypeId;
            employabilityPlan.Notes                         = employabilityPlanContract.Notes;
            employabilityPlan.ParticipantEnrolledProgramId  = employabilityPlanContract.PEPId;
            employabilityPlan.CanSaveWithoutActivity        = employabilityPlanContract.CanSaveWithoutActivity;
            employabilityPlan.CanSaveWithoutActivityDetails = employabilityPlanContract.CanSaveWithoutActivityDetails;
            employabilityPlan.OrganizationId                = GetOrganizationByOfficeId(pep?.OfficeId);
            employabilityPlan.ModifiedBy                    = _authUser.WIUID;
            employabilityPlan.ModifiedDate                  = updateTime;

            if (employabilityPlan.Id == 0)
            {
                _epRepo.Add(employabilityPlan);

                if (subsequentEPId != 0)
                {
                    var hasActivities = CarryOverActivities(employabilityPlan, subsequentEPId, updateTime);

                    if (employabilityPlanContract.CanSaveWithoutActivity == true && hasActivities)
                        return new EmployabilityPlanContract
                               {
                                   ErrorMessage = _ruleReasonRepository.Get(i => i.Category    == Wwp.Model.Interface.Constants.RuleReason.EPSave        &&
                                                                                 i.SubCategory == Wwp.Model.Interface.Constants.RuleReason.PreCheckError &&
                                                                                 i.Code        == Wwp.Model.Interface.Constants.RuleReason.EP).Name
                               };

                    CarryOverGoals(employabilityPlan, subsequentEPId, updateTime);
                }
            }

            _unitOfWork.Commit();

            var ep = new EmployabilityPlanContract(
                                                   employabilityPlan.Id,
                                                   employabilityPlan.EnrolledProgramId,
                                                   employabilityPlan.EnrolledProgram?.Name,
                                                   employabilityPlan.EnrolledProgram?.ProgramCode,
                                                   employabilityPlan.BeginDate,
                                                   employabilityPlan.EndDate,
                                                   employabilityPlan.EmployabilityPlanStatusTypeId,
                                                   employabilityPlan.EmployabilityPlanStatusType?.Name,
                                                   employabilityPlan.Notes,
                                                   _convertWIUIdToName(employabilityPlan.ModifiedBy),
                                                   employabilityPlan.ModifiedDate,
                                                   employabilityPlan.CreatedDate,
                                                   employabilityPlan.CanSaveWithoutActivity,
                                                   employabilityPlan.CanSaveWithoutActivityDetails,
                                                   employabilityPlan.OrganizationId,
                                                   employabilityPlan.ParticipantEnrolledProgramId,
                                                   employabilityPlan.SubmitDate);

            return (ep);
        }

        private int? GetOrganizationByOfficeId(int? officeId)
        {
            if (officeId == null) return null;

            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var org = _officeRepo.GetMany(i => (i.Id == officeId)
                                               && ((i.InActivatedDate                           == null || i.InActivatedDate                           >= currentDate) && i.ActiviatedDate                          <= currentDate)
                                               && ((i.ContractArea.InActivatedDate              == null || i.ContractArea.InActivatedDate              >= currentDate) && i.ContractArea.ActivatedDate              <= currentDate)
                                               && ((i.ContractArea.Organization.InActivatedDate == null || i.ContractArea.Organization.InActivatedDate >= currentDate) && i.ContractArea.Organization.ActivatedDate <= currentDate))
                                 .Select(i => i.ContractArea.Organization)
                                 .FirstOrDefault(i => (i.InActivatedDate == null || i.InActivatedDate >= currentDate && i.ActivatedDate <= currentDate));

            return org?.Id;
        }

        private void CarryOverGoals(EmployabilityPlan employabilityPlan, int subsequentEPId, DateTime updateTime)
        {
            var epGoalBridges = _epRepo.GetMany(i => i.Id == subsequentEPId)
                                       .SelectMany(i => i.EmployabilityPlanGoalBridges)
                                       .Where(i => i.Goal.IsGoalEnded == false)
                                       .ToList();

            epGoalBridges.ForEach(epGoal =>
                                  {
                                      var epGoalBridge = _epGoalBridgeRepo.New();

                                      epGoalBridge.EmployabilityPlan = employabilityPlan;
                                      epGoalBridge.GoalId            = epGoal.GoalId;
                                      epGoalBridge.ModifiedBy        = _authUser.WIUID;
                                      epGoalBridge.ModifiedDate      = updateTime;
                                      _epGoalBridgeRepo.Add(epGoalBridge);
                                  });
        }

        public bool CarryOverActivities(EmployabilityPlan employabilityPlan, int subsequentEPId, DateTime updateTime, List<Activity> epActivities = null, bool isTransfer = false)
        {
            var tableActivities = _epActivityBridgeRepo.GetMany(i => i.EmployabilityPlanId == subsequentEPId)
                                                       .Select(i => i.Activity)
                                                       .Where(i => i.ActivityCompletionReasonId == null)
                                                       .ToList();
            var activities = isTransfer ? epActivities : tableActivities;

            activities.AsNotNull().ForEach(activity =>
                                           {
                                               var epActivityBridge = _epActivityBridgeRepo.New();

                                               epActivityBridge.EmployabilityPlan = employabilityPlan;
                                               epActivityBridge.Activity          = activity;
                                               epActivityBridge.ModifiedBy        = _authUser.WIUID;
                                               epActivityBridge.ModifiedDate      = updateTime;

                                               _epActivityBridgeRepo.Add(epActivityBridge);

                                               var schedules = isTransfer
                                                                   ? activity.ActivitySchedules
                                                                             .Where(i => i.EmployabilityPlanId == subsequentEPId
                                                                                         && i.IsRecurring      == true
                                                                                             ? i.PlannedEndDate >= employabilityPlan.BeginDate
                                                                                             : i.StartDate      >= employabilityPlan.BeginDate)
                                                                             .ToList()
                                                                   : activity.ActivitySchedules
                                                                             .Where(i => i.EmployabilityPlanId == subsequentEPId)
                                                                             .ToList();

                                               var allElapsed = schedules.All(i => i.IsRecurring == true ? i.PlannedEndDate < employabilityPlan.BeginDate : i.StartDate < employabilityPlan.BeginDate);

                                               if (!allElapsed)
                                               {
                                                   schedules.Where(i => i.IsRecurring == true ? i.PlannedEndDate >= employabilityPlan.BeginDate : i.StartDate >= employabilityPlan.BeginDate)
                                                            .ForEach(schedule =>
                                                                     {
                                                                         var       newSchedule      = _activityScheduleRepository.New();
                                                                         var       frequencies      = schedule.ActivityScheduleFrequencyBridges?.ToList();
                                                                         var       isBiWeekly       = schedule.FrequencyType?.Name == "Biweekly";
                                                                         var       isFutureSchedule = schedule.StartDate           > employabilityPlan.BeginDate;
                                                                         var       ssd              = isFutureSchedule ? schedule.StartDate : employabilityPlan.BeginDate;
                                                                         DateTime? startDate        = null;

                                                                         if (isBiWeekly)
                                                                         {
                                                                             var dayWk  = DayOfWeek.Sunday;
                                                                             var ssDay  = ssd?.DayOfWeek.GetHashCode() + 1;
                                                                             var oldSsd = schedule.StartDate;
                                                                             var oSsDay = oldSsd?.DayOfWeek.GetHashCode() + 1;
                                                                             var nWkDay = schedule.StartDate?.StartOfWeek(week: "next");
                                                                             var ssWk   = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear((DateTime) oldSsd, CalendarWeekRule.FirstDay, dayWk);
                                                                             var neSsWk = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear((DateTime) nWkDay, CalendarWeekRule.FirstDay, dayWk);
                                                                             var nSsWk  = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear((DateTime) ssd,    CalendarWeekRule.FirstDay, dayWk);
                                                                             var days   = frequencies?.Select(i => i.WKFrequency).OrderBy(i => i.SortOrder).Select(i => i.SortOrder).ToList();
                                                                             var maxDay = days?.Max();

                                                                             if (!isFutureSchedule)
                                                                             {
                                                                                 startDate = (oSsDay <= maxDay ? ssWk : neSsWk) % 2 == nSsWk % 2
                                                                                                 ? ssDay <= maxDay
                                                                                                       ? ssd?.AddDays((int) (days?.Find(i => i >= ssDay) - ssDay))
                                                                                                       : ssd?.StartOfWeek(week: "next-two").AddDays(days[0] - 1)
                                                                                                 : ssd?.StartOfWeek(week: "next").AddDays(days[0] - 1);
                                                                             }
                                                                         }

                                                                         startDate = startDate ?? ssd;

                                                                         newSchedule.Activity          = activity;
                                                                         newSchedule.EmployabilityPlan = employabilityPlan;
                                                                         newSchedule.StartDate         = startDate;
                                                                         newSchedule.IsRecurring       = schedule.IsRecurring;
                                                                         newSchedule.FrequencyTypeId   = schedule.FrequencyTypeId;
                                                                         newSchedule.PlannedEndDate    = schedule.PlannedEndDate;
                                                                         newSchedule.HoursPerDay       = schedule.HoursPerDay;
                                                                         newSchedule.BeginTime         = schedule.BeginTime;
                                                                         newSchedule.EndTime           = schedule.EndTime;
                                                                         newSchedule.ModifiedDate      = updateTime;
                                                                         newSchedule.ModifiedBy        = _authUser.WIUID;

                                                                         _activityScheduleRepository.Add(newSchedule);

                                                                         if (newSchedule.IsRecurring != true || (newSchedule.FrequencyType?.Name != "Weekly"   &&
                                                                                                                 newSchedule.FrequencyType?.Name != "Biweekly" &&
                                                                                                                 newSchedule.FrequencyType?.Name != "Monthly")) return;

                                                                         frequencies?.ForEach(frequency =>
                                                                                              {
                                                                                                  var newFrequency = _activityScheduleFrequencyBridgeRepository.New();

                                                                                                  newFrequency.ActivitySchedule = newSchedule;
                                                                                                  newFrequency.WKFrequencyId    = frequency.WKFrequencyId;
                                                                                                  newFrequency.MRFrequencyId    = frequency.MRFrequencyId;
                                                                                                  newFrequency.ModifiedBy       = _authUser.WIUID;
                                                                                                  newFrequency.ModifiedDate     = updateTime;

                                                                                                  _activityScheduleFrequencyBridgeRepository.Add(newFrequency);
                                                                                              });
                                                                     });
                                               }
                                               else
                                               {
                                                   schedules.ForEach(schedule =>
                                                                     {
                                                                         var newSchedule = _activityScheduleRepository.New();

                                                                         newSchedule.Activity          = activity;
                                                                         newSchedule.EmployabilityPlan = employabilityPlan;
                                                                         newSchedule.StartDate         = schedule.StartDate;
                                                                         newSchedule.IsRecurring       = schedule.IsRecurring;
                                                                         newSchedule.FrequencyTypeId   = schedule.FrequencyTypeId;
                                                                         newSchedule.PlannedEndDate    = schedule.PlannedEndDate;
                                                                         newSchedule.HoursPerDay       = schedule.HoursPerDay;
                                                                         newSchedule.BeginTime         = schedule.BeginTime;
                                                                         newSchedule.EndTime           = schedule.EndTime;
                                                                         newSchedule.ModifiedDate      = updateTime;
                                                                         newSchedule.ModifiedBy        = _authUser.WIUID;

                                                                         _activityScheduleRepository.Add(newSchedule);

                                                                         if (newSchedule.IsRecurring != true || (newSchedule.FrequencyType?.Name != "Weekly"   &&
                                                                                                                 newSchedule.FrequencyType?.Name != "Biweekly" &&
                                                                                                                 newSchedule.FrequencyType?.Name != "Monthly")) return;

                                                                         var frequencies = schedule.ActivityScheduleFrequencyBridges?.ToList();

                                                                         frequencies?.ForEach(frequency =>
                                                                                              {
                                                                                                  var newFrequency = _activityScheduleFrequencyBridgeRepository.New();

                                                                                                  newFrequency.ActivitySchedule = newSchedule;
                                                                                                  newFrequency.WKFrequencyId    = frequency.WKFrequencyId;
                                                                                                  newFrequency.MRFrequencyId    = frequency.MRFrequencyId;
                                                                                                  newFrequency.ModifiedBy       = _authUser.WIUID;
                                                                                                  newFrequency.ModifiedDate     = updateTime;

                                                                                                  _activityScheduleFrequencyBridgeRepository.Add(newFrequency);
                                                                                              });
                                                                     });
                                               }
                                           });

            return activities.Any();
        }

        public void EPTransfer(IParticipantEnrolledProgram pep, DateTime modifiedDate)
        {
            var transferDate = _authUser.CDODate ?? DateTime.Today;
            var endDate      = transferDate.AddDays(-1);
            var oldEPs = _epRepo.GetMany(ep => ep.Participant.PinNumber           == pep.Participant.PinNumber
                                               && ep.ParticipantEnrolledProgramId == pep.Id
                                               && (ep.EmployabilityPlanStatusTypeId    == EmployabilityPlanStatus.InProgressId
                                                   || ep.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId
                                                   || ep.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.EndedId))
                                .OrderByDescending(i => i.BeginDate)
                                .ThenByDescending(i => i.ModifiedDate)
                                .Take(2).ToList();
            var inProgressEP = oldEPs.FirstOrDefault(i => i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId);
            var oldEP        = oldEPs.FirstOrDefault(i => i.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId);

            if (inProgressEP != null)
            {
                DeletePlan(inProgressEP.Participant.PinNumber.ToString(), inProgressEP.Id, false, true, true);
            }

            if (oldEP == null) return;
            {
                var oldEndDate = oldEP.EndDate;

                oldEP.EndDate                       = endDate;
                oldEP.EmployabilityPlanStatusTypeId = EmployabilityPlanStatus.EndedId;
                oldEP.ModifiedDate                  = modifiedDate;
                oldEP.ModifiedBy                    = _authUser.WIUID;
                _epRepo.Update(oldEP);

                var newEP = _epRepo.New();

                newEP.ParticipantId                 = oldEP.ParticipantId;
                newEP.EnrolledProgramId             = oldEP.EnrolledProgramId;
                newEP.BeginDate                     = transferDate;
                newEP.EndDate                       = oldEndDate;
                newEP.Notes                         = oldEP.Notes;
                newEP.EmployabilityPlanStatusTypeId = EmployabilityPlanStatus.SubmittedId;
                newEP.ParticipantEnrolledProgramId  = oldEP.ParticipantEnrolledProgramId;
                newEP.CanSaveWithoutActivity        = oldEP.CanSaveWithoutActivity;
                newEP.CanSaveWithoutActivityDetails = oldEP.CanSaveWithoutActivityDetails;
                newEP.OrganizationId                = GetOrganizationByOfficeId(pep?.Office.Id);
                newEP.SubmitDate                    = transferDate;
                newEP.ModifiedBy                    = _authUser.WIUID;
                newEP.ModifiedDate                  = modifiedDate;

                var activities       = oldEP.EmploybilityPlanActivityBridges?.Select(i => i.Activity).Where(i => i.ActivityCompletionReasonId == null).ToList();
                var clonedActivities = new List<Activity>();
                activities?.ForEach(a =>
                                    {
                                        var aClone = a.Clone();
                                        aClone.ActivitySchedules = a.ActivitySchedules
                                                                    .Where(i => i.EmployabilityPlanId == oldEP.Id
                                                                                && (i.IsRecurring == true && i.PlannedEndDate >= newEP.BeginDate)
                                                                                || (i.IsRecurring == false && i.StartDate >= newEP.BeginDate))
                                                                    .ToList();
                                        if (aClone.ActivitySchedules.Count > 0)
                                            clonedActivities.Add(aClone);
                                    });

                var newActivities = new List<Activity>();

                clonedActivities.ForEach(a =>
                                         {
                                             var activity = _activityRepo.New();
                                             activity.ActivityTypeId             = a.ActivityTypeId;
                                             activity.Description                = a.Description;
                                             activity.ActivityLocationId         = a.ActivityLocationId;
                                             activity.Details                    = a.Details;
                                             activity.IsDeleted                  = a.IsDeleted;
                                             activity.ModifiedBy                 = a.ModifiedBy;
                                             activity.ModifiedDate               = a.ModifiedDate;
                                             activity.ActivityCompletionReasonId = a.ActivityCompletionReasonId;
                                             activity.EndDate                    = a.EndDate;
                                             activity.StartDate                  = a.StartDate;
                                             a.ActivityContactBridges.ForEach(acb =>
                                                                              {
                                                                                  var activityContactBridge = _activityContactBridgeRepository.New();
                                                                                  activityContactBridge.Activity     = activity;
                                                                                  activityContactBridge.ContactId    = acb.ContactId;
                                                                                  activityContactBridge.ModifiedBy   = _authUser.WIUID;
                                                                                  activityContactBridge.ModifiedDate = modifiedDate;
                                                                                  activity.ActivityContactBridges.Add(activityContactBridge);
                                                                              });
                                             a.NonSelfDirectedActivities.ForEach(nsa =>
                                                                                 {
                                                                                     var nonSelfDirectedActivity = _nonSelfDirActivityRepo.New();
                                                                                     nonSelfDirectedActivity.Activity      = activity;
                                                                                     nonSelfDirectedActivity.BusinessName  = nsa.BusinessName;
                                                                                     nonSelfDirectedActivity.CityId        = nsa.CityId;
                                                                                     nonSelfDirectedActivity.PhoneNumber   = nsa.PhoneNumber;
                                                                                     nonSelfDirectedActivity.StreetAddress = nsa.StreetAddress;
                                                                                     nonSelfDirectedActivity.ZipAddress    = nsa.ZipAddress;
                                                                                     nonSelfDirectedActivity.ModifiedBy    = _authUser.WIUID;
                                                                                     nonSelfDirectedActivity.ModifiedDate  = modifiedDate;
                                                                                     activity.NonSelfDirectedActivities.Add(nonSelfDirectedActivity);
                                                                                 });
                                             activity.ActivitySchedules = a.ActivitySchedules.Select(asc => asc.Clone()).ToList();

                                             newActivities.Add(activity);
                                         });

                var activityIds = activities?.Select(i => i.Id).ToList();

                if (activityIds != null && activityIds.Count > 0)
                {
                    activities.ForEach(i =>
                                       {
                                           i.ActivityCompletionReasonId = ActivityCompletionReason.TId;
                                           i.EndDate                    = endDate;
                                           _activityRepo.Update(i);
                                       });

                    var schedules = activities.SelectMany(i => i.ActivitySchedules).Where(i => i.EmployabilityPlanId == oldEP.Id).ToList();
                    schedules.ForEach(i =>
                                      {
                                          i.ActualEndDate = endDate;
                                          _activityScheduleRepository.Update(i);
                                      });
                }

                CarryOverGoals(newEP, oldEP.Id, modifiedDate);
                CarryOverActivities(newEP, oldEP.Id, modifiedDate, newActivities, true);
                newActivities.ForEach(activity =>
                                      {
                                          activity.StartDate = newEP.BeginDate;
                                          _activityRepo.Add(activity);
                                      });
                _epRepo.Add(newEP);

                var epei = _epeiRepo.GetMany(i => i.EmployabilityPlanId == oldEP.Id).ToList();
                epei.ForEach(i =>
                             {
                                 var newEpei = _epeiRepo.New();

                                 newEpei.EmployabilityPlan       = newEP;
                                 newEpei.EmploymentInformationId = i.EmploymentInformationId;
                                 newEpei.ModifiedBy              = _authUser.WIUID;
                                 newEpei.ModifiedDate            = modifiedDate;
                                 _epeiRepo.Add(newEpei);
                             });

                EPTransactionalSave(newEP, oldEP.Id, newEP.EnrolledProgram.ProgramCode.ToLower().SafeTrim(), newEP.ModifiedDate);
            }
        }

        private void EPTransactionalSave(EmployabilityPlan newEp, int? oldEpId, string programCd, DateTime modifiedDate)
        {
            if (programCd == "ww" || programCd == "cf")
            {
                using (var tx = _epRepo.GetDataBase().BeginTransaction())
                {
                    try
                    {
                        _unitOfWork.Commit();

                        var parms = new Dictionary<string, object>
                                    {
                                        ["EPId"]           = newEp.Id,
                                        ["SubsequentEPId"] = oldEpId ?? (object) DBNull.Value,
                                        ["ProgramCd"]      = programCd,
                                        ["FromBatch"]      = false,
                                        ["ModifiedBy"]     = _authUser.WIUID,
                                        ["ModifiedDate"]   = modifiedDate,
                                        ["CurrentDate"]    = _authUser.CDODate ?? (object) DBNull.Value
                                    };

                        var rs = _epRepo.GetStoredProcReturnValue("USP_Create_Participation_Entries", parms);

                        if (rs == 0)
                            tx.Commit();
                        else
                            throw new DCFApplicationException("Failed due to SProc issue. Please try again.");
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw new DCFApplicationException("Failed due to SProc issue. Please try again.", ex);
                    }

                    tx.Dispose();
                }
            }
            else
            {
                _unitOfWork.Commit();
            }
        }

        #region ChildCareAuthorizations

        public string GetChildCareAuthorizationsByPin(string pin)
        {
            return "{\"effectivePeriod\":\"12/01/2020 - 06/30/2021\",\"periods\":[{\"month\":\"December\",\"year\":2020,\"children\":[{\"name\":\"David Warner\",\"hours\":120}]},{\"month\":\"January\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120},{\"name\":\"William Smith\",\"hours\":120}]},{\"month\":\"February\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120},{\"name\":\"William Smith\",\"hours\":120},{\"name\":\"Chris Gayle\",\"hours\":120}]},{\"month\":\"March\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120},{\"name\":\"Sachin Tendlukar\",\"hours\":120}]},{\"month\":\"April\",\"year\":2021,\"children\":[{\"name\":\"David Warner\",\"hours\":120}]}]}";
        }

        #endregion
    }

    #endregion
}
