using System;
using System.Linq;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class WorkerTaskListDomain : IWorkerTaskListDomain
    {
        #region Properties

        private readonly IWorkerTaskListRepository   _workerTaskListRepository;
        private readonly IWorkerRepository           _workerRepository;
        private readonly IWorkerTaskStatusRepository _workerTaskStatusRepository;
        private readonly IAuthUser                   _authUser;
        private readonly IUnitOfWork                 _unitOfWork;
        private readonly Func<string, string>        _convertWIUIdToName;

        #endregion

        #region Methods

        public WorkerTaskListDomain
        (
            IWorkerTaskListRepository   workerTaskListRepository,
            IWorkerRepository           workerRepository,
            IWorkerTaskStatusRepository workerTaskStatusRepository,
            IAuthUser                   authUser,
            IUnitOfWork                 unitOfWork
        )
        {
            _workerTaskListRepository   = workerTaskListRepository;
            _workerRepository           = workerRepository;
            _workerTaskStatusRepository = workerTaskStatusRepository;
            _authUser                   = authUser;
            _unitOfWork                 = unitOfWork;
            _convertWIUIdToName = (wiuId) =>
                                  {
                                      string wn;
                                      switch (wiuId)
                                      {
                                          case "WWP Conversion":
                                              wn = wiuId;
                                              break;
                                          case "WWP Batch":
                                              wn = wiuId;
                                              break;
                                          case "WWP":
                                              wn = wiuId;
                                              break;
                                          case "CWW":
                                              wn = wiuId;
                                              break;
                                          default:
                                          {
                                              var wo = workerRepository.GetAsQueryable()
                                                                       .Where(i => i.WIUId == wiuId)
                                                                       .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                                       .FirstOrDefault();

                                              wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");
                                              break;
                                          }
                                      }

                                      return (wn);
                                  };
        }

        public List<WorkerTaskListContract> GetWorkerTaskLists(string wiuid)
        {
            var workerTaskListContract = new List<WorkerTaskListContract>();
            var worker                 = _workerRepository.Get(i => i.WIUId          == (wiuid ?? _authUser.WIUID));
            var openWorkerStatusId     = _workerTaskStatusRepository.Get(j => j.Code == "OP").Id;
            var workerTaskList         = _workerTaskListRepository.GetMany(i => i.WorkerId == worker.Id && !i.IsDeleted && i.WorkerTaskStatusId == openWorkerStatusId);

            if (workerTaskList == null) return workerTaskListContract;
            workerTaskList.ForEach(i => workerTaskListContract.Add(
                                                                   new WorkerTaskListContract
                                                                   {
                                                                       Id                       = i.Id,
                                                                       ParticipantId            = i.ParticipantId,
                                                                       CategoryId               = i.CategoryId,
                                                                       WorkerId                 = i.WorkerId,
                                                                       StatusDate               = i.StatusDate?.ToString("MM/dd/yyyy"),
                                                                       TaskDate                 = i.TaskDate.ToString("MM/dd/yyyy"),
                                                                       TaskDetails              = i.TaskDetails,
                                                                       IsDeleted                = i.IsDeleted,
                                                                       ParticipantMiddleInitial = i.Participant.MiddleInitial,
                                                                       ParticipantFirstName     = i.Participant.FirstName,
                                                                       ParticipantLastName      = i.Participant.LastName,
                                                                       ParticipantSuffixName    = i.Participant.SuffixName,
                                                                       Pin                      = i.Participant.PinNumber,
                                                                       CategoryCode             = i.WorkerTaskCategory.Code,
                                                                       CategoryName             = i.WorkerTaskCategory.Name,
                                                                       ActionPriorityId         = i.ActionPriorityId,
                                                                       ActionPriorityName       = i.ActionPriority?.Name,
                                                                       WorkerTaskStatusId       = i.WorkerTaskStatusId,
                                                                       WorkerTaskStatusCode     = i.WorkerTaskStatus?.Code,
                                                                       WorkerTaskStatusName     = i.WorkerTaskStatus?.Name,
                                                                       DueDate                  = i.DueDate,
                                                                       IsSystemGenerated        = i.IsSystemGenerated,
                                                                       ModifiedBy               = _convertWIUIdToName(i.ModifiedBy),
                                                                       ModifiedDate             = i.ModifiedDate
                                                                   }));
            return workerTaskListContract;
        }

        public WorkerTaskListContract GetWorkerTaskList(int id)
        {
            WorkerTaskListContract contract       = null;
            var                    workerTaskList = _workerTaskListRepository.Get(i => i.Id == id);

            if (workerTaskList != null)
            {
                contract = new WorkerTaskListContract
                           {
                               Id                       = workerTaskList.Id,
                               ParticipantId            = workerTaskList.ParticipantId,
                               CategoryId               = workerTaskList.CategoryId,
                               WorkerId                 = workerTaskList.WorkerId,
                               StatusDate               = workerTaskList.StatusDate?.ToString("MM/dd/yyyy") ?? "",
                               TaskDate                 = workerTaskList.TaskDate.ToString("MM/dd/yyyy"),
                               TaskDetails              = workerTaskList.TaskDetails,
                               IsDeleted                = workerTaskList.IsDeleted,
                               ParticipantMiddleInitial = workerTaskList.Participant.MiddleInitial,
                               ParticipantFirstName     = workerTaskList.Participant.FirstName,
                               ParticipantLastName      = workerTaskList.Participant.LastName,
                               ParticipantSuffixName    = workerTaskList.Participant.SuffixName,
                               Pin                      = workerTaskList.Participant.PinNumber,
                               CategoryCode             = workerTaskList.WorkerTaskCategory.Code,
                               CategoryName             = workerTaskList.WorkerTaskCategory.Name,
                               ActionPriorityId         = workerTaskList.ActionPriorityId,
                               ActionPriorityName       = workerTaskList.ActionPriority?.Name,
                               WorkerTaskStatusId       = workerTaskList.WorkerTaskStatusId,
                               WorkerTaskStatusCode     = workerTaskList.WorkerTaskStatus?.Code,
                               WorkerTaskStatusName     = workerTaskList.WorkerTaskStatus?.Name,
                               DueDate                  = workerTaskList.DueDate,
                               IsSystemGenerated        = workerTaskList.IsSystemGenerated,
                               ModifiedBy               = _convertWIUIdToName(workerTaskList.ModifiedBy),
                               ModifiedDate             = workerTaskList.ModifiedDate
                           };
            }

            return contract;
        }

        public WorkerTaskListContract UpsertWorkerTaskList(WorkerTaskListContract workerTaskListContract, bool isSystemGenerated = false, bool canCommit = true)
        {
            if (workerTaskListContract == null)
            {
                throw new ArgumentNullException(nameof(workerTaskListContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var workerTaskList = workerTaskListContract.Id != 0
                                     ? _workerTaskListRepository.Get(i => i.Id == workerTaskListContract.Id && !i.IsDeleted)
                                     : _workerTaskListRepository.New();
            workerTaskList.IsSystemGenerated = isSystemGenerated;
            workerTaskList.CategoryId        = workerTaskListContract.CategoryId;
            workerTaskList.TaskDetails       = workerTaskListContract.TaskDetails;
            workerTaskList.ActionPriorityId  = workerTaskListContract.ActionPriorityId;
            workerTaskList.DueDate           = workerTaskListContract.DueDate;
            workerTaskList.StatusDate        = _authUser.CDODate ?? modifiedDate;
            workerTaskList.ModifiedBy        = modifiedBy;
            workerTaskList.ModifiedDate      = modifiedDate;

            if (workerTaskListContract.Id != 0)
            {
                workerTaskListContract.StatusDate           = workerTaskList.StatusDate?.ToString("MM/dd/yyyy");
                workerTaskListContract.WorkerTaskStatusCode = _workerTaskStatusRepository.Get(i => i.Id == workerTaskListContract.WorkerTaskStatusId).Code;
                workerTaskList.WorkerTaskStatusId           = workerTaskListContract.WorkerTaskStatusId;

                _workerTaskListRepository.Update(workerTaskList);
            }
            else
            {
                workerTaskList.WorkerTaskStatusId = _workerTaskStatusRepository.Get(i => i.Code == "OP").Id;
                workerTaskList.ParticipantId      = workerTaskListContract.ParticipantId;
                workerTaskList.TaskDate           = workerTaskListContract.TaskDate != null ? Convert.ToDateTime(workerTaskListContract.TaskDate) : modifiedDate;
                workerTaskList.WorkerId           = workerTaskListContract.WorkerId == 0 ? _workerRepository.Get(i => i.WIUId == _authUser.WIUID).Id : workerTaskListContract.WorkerId;

                _workerTaskListRepository.Add(workerTaskList);
            }

            if (canCommit) _unitOfWork.Commit();

            workerTaskListContract.Id = workerTaskList.Id;

            return workerTaskListContract;
        }

        public void ReassignWorker(int id, int workerId)
        {
            var workerTaskList = _workerTaskListRepository.Get(i => i.Id == id && !i.IsDeleted);

            workerTaskList.WorkerId     = workerId;
            workerTaskList.ModifiedBy   = _authUser.WIUID;
            workerTaskList.ModifiedDate = DateTime.Now;

            _unitOfWork.Commit();
        }

        #endregion
    }
}
