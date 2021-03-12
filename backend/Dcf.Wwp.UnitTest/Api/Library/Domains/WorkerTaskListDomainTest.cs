using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class WorkerTaskListDomainTest
    {
        #region Properties

        private                 WorkerTaskListDomain         _workerTaskListDomain;
        private                 MockWorkerTaskListRepository _workerTaskListRepository;
        private const           int                          WorkerId           = 100;
        private const           int                          TaskId             = 150;
        private readonly        IAuthUser                    _authUser          = new AuthUser { WIUID      = "1111" };
        private static readonly WorkerTaskStatus             TaskStatusOpen     = new WorkerTaskStatus { Id = 1, Code = "OP" };
        private static readonly WorkerTaskStatus             TaskStatusComplete = new WorkerTaskStatus { Id = 2, Code = "CP" };
        private static readonly WorkerTaskStatus             TaskStatusClosed   = new WorkerTaskStatus { Id = 3, Code = "CL" };

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _workerTaskListRepository = new MockWorkerTaskListRepository();
            _workerTaskListDomain     = new WorkerTaskListDomain(_workerTaskListRepository, null, new MockWorkerStatusRepository(), _authUser, new MockUnitOfWork());
        }

        [TestMethod]
        public void UpsertWorkerTaskListCannotBeNull()
        {
            var workerTaskListDomain = new WorkerTaskListDomain(new MockWorkerTaskListRepository(), null, null, null, null);

            Assert.ThrowsException<ArgumentNullException>(() => { workerTaskListDomain.UpsertWorkerTaskList(null); });
        }

        [TestMethod]
        public void UpsertWorkerTaskList_WhenNewWorkerTask_CallsAddMethod()
        {
            _workerTaskListDomain.UpsertWorkerTaskList(new WorkerTaskListContract { WorkerId = WorkerId });
            Assert.IsTrue(_workerTaskListRepository.HasAddBeenCalled);
        }

        [TestMethod]
        [DynamicData(nameof(GetContract), DynamicDataSourceType.Method)]
        public void UpsertWorkerTaskList_WhenWorkerTaskIsEditedOrMarkedComplete_CallsUpdateMethod(WorkerTaskListContract contract)
        {
            try
            {
                _workerTaskListDomain.UpsertWorkerTaskList(contract);
                Assert.IsTrue(_workerTaskListRepository.HasUpdateBeenCalled);
            }
            catch (Exception ex)
            {
                Assert.Fail(JsonConvert.SerializeObject(new [] { new { Data = contract, Error = ex.InnerException?.Message ?? ex.Message } }));
            }
        }

        [TestMethod]
        public void UpsertWorkerTaskList_WhenMarkAsCompleteWorkerTask_UpdatesWorkerTaskListItem()
        {
            var workerTaskListContract = new WorkerTaskListContract { Id = TaskId, WorkerTaskStatusId = TaskStatusComplete.Id };

            _workerTaskListDomain.UpsertWorkerTaskList(workerTaskListContract);
            Assert.AreEqual(workerTaskListContract.WorkerTaskStatusCode,                        TaskStatusComplete.Code);
            Assert.AreEqual(workerTaskListContract.WorkerTaskStatusId,                          TaskStatusComplete.Id);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.WorkerTaskStatusId, TaskStatusComplete.Id);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.ModifiedBy,         _authUser.WIUID);
            Assert.IsTrue(_workerTaskListRepository.UpdatedWorkerTaskList.ModifiedDate >= DateTime.Now.AddMinutes(-5));
        }

        [TestMethod]
        public void UpsertWorkerTaskList_WhenWorkerTaskIsEdited_UpdatesWorkerTaskListItem()
        {
            var workerTaskListContract = new WorkerTaskListContract { Id = TaskId, CategoryId = 1, TaskDetails = "Test", DueDate = DateTime.Now, ActionPriorityId = 2, WorkerTaskStatusId = 3 };

            _workerTaskListDomain.UpsertWorkerTaskList(workerTaskListContract);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.CategoryId,         workerTaskListContract.CategoryId);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.TaskDetails,        workerTaskListContract.TaskDetails);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.DueDate,            workerTaskListContract.DueDate);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.ActionPriorityId,   workerTaskListContract.ActionPriorityId);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.WorkerTaskStatusId, workerTaskListContract.WorkerTaskStatusId);
            Assert.AreEqual(_workerTaskListRepository.UpdatedWorkerTaskList.ModifiedBy,         _authUser.WIUID);
            Assert.IsTrue(_workerTaskListRepository.UpdatedWorkerTaskList.ModifiedDate >= DateTime.Now.AddMinutes(-5));
        }

        [TestMethod]
        public void UpsertWorkerTaskList_WhenWorkerTaskIsNew_AddsCorrectValues()
        {
            var workerTaskListContract = new WorkerTaskListContract { Id = 0, CategoryId = 1, ParticipantId = 12345, TaskDetails = "Test", DueDate = DateTime.Now, ActionPriorityId = 2, WorkerTaskStatusId = 3, WorkerId = WorkerId };

            _workerTaskListDomain.UpsertWorkerTaskList(workerTaskListContract);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.CategoryId,         workerTaskListContract.CategoryId);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.TaskDetails,        workerTaskListContract.TaskDetails);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.DueDate,            workerTaskListContract.DueDate);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.ActionPriorityId,   workerTaskListContract.ActionPriorityId);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.WorkerTaskStatusId, TaskStatusOpen.Id);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.ModifiedBy,         _authUser.WIUID);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.WorkerId,           workerTaskListContract.WorkerId);
            Assert.AreEqual(_workerTaskListRepository.NewWorkerTaskList.ParticipantId,      workerTaskListContract.ParticipantId);
            Assert.IsTrue(_workerTaskListRepository.NewWorkerTaskList.ModifiedDate >= DateTime.Now.AddMinutes(-5));
        }

        private static IEnumerable<WorkerTaskListContract[]> GetContract()
        {
            return new []
                   {
                       new WorkerTaskListContract { Id = TaskId, WorkerTaskStatusId = TaskStatusComplete.Id },
                       new WorkerTaskListContract { Id = TaskId, CategoryId         = 1, TaskDetails = "Test", DueDate = DateTime.Now, ActionPriorityId = 2, WorkerTaskStatusId = 3 }
                   }.Select(i => new[] { i });
        }

        #endregion

        #region Repositories

        private class MockWorkerStatusRepository : MockRepositoryBase<WorkerTaskStatus>, IWorkerTaskStatusRepository
        {
            public new WorkerTaskStatus Get(Expression<Func<WorkerTaskStatus, bool>> clause)
            {
                return new List<WorkerTaskStatus> { TaskStatusComplete, TaskStatusClosed, TaskStatusOpen }.FirstOrDefault(clause.Compile());
            }
        }

        private class MockWorkerTaskListRepository : MockRepositoryBase<WorkerTaskList>, IWorkerTaskListRepository
        {
            public bool           HasAddBeenCalled;
            public bool           HasUpdateBeenCalled;
            public WorkerTaskList UpdatedWorkerTaskList;
            public WorkerTaskList NewWorkerTaskList;

            public new void Add(WorkerTaskList entity)
            {
                NewWorkerTaskList = entity;
                HasAddBeenCalled  = true;
            }

            public new void Update(WorkerTaskList entity)
            {
                UpdatedWorkerTaskList = entity;
                HasUpdateBeenCalled   = true;
            }
        }

        #endregion
    }
}
