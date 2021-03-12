using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockWorkerStatusRepository : MockRepositoryBase<WorkerTaskStatus>, IWorkerTaskStatusRepository
    {
        private static readonly WorkerTaskStatus TaskStatusOpen     = new WorkerTaskStatus { Id = 1, Code = "OP" };
        private static readonly WorkerTaskStatus TaskStatusComplete = new WorkerTaskStatus { Id = 2, Code = "CP" };
        private static readonly WorkerTaskStatus TaskStatusClosed   = new WorkerTaskStatus { Id = 3, Code = "CL" };

        public new WorkerTaskStatus Get(Expression<Func<WorkerTaskStatus, bool>> clause)
        {
            return new List<WorkerTaskStatus> { TaskStatusComplete, TaskStatusClosed, TaskStatusOpen }.FirstOrDefault(clause.Compile());
        }
    }
}
