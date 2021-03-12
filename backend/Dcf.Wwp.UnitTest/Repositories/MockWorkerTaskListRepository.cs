using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Infrastructure;
using WorkerTaskStatus = Dcf.Wwp.DataAccess.Models.WorkerTaskStatus;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockWorkerTaskListRepository : MockRepositoryBase<WorkerTaskList>, IWorkerTaskListRepository
    {
        #region Properties

        public readonly List<WorkerTaskList> AddedWorkerTaskLists   = new List<WorkerTaskList>();
        public          bool                 IsCareerAssessmentTask = false;
        public          bool                 IsJobReadinessTask     = false;
        public          bool                 IsTestScoresTask       = false;
        public          bool                 HasAddBeenCalled       = false;

        #endregion

        #region Methods

        public new void Add(WorkerTaskList entity)
        {
            HasAddBeenCalled = true;
            AddedWorkerTaskLists.Add(entity);
        }

        public new IEnumerable<WorkerTaskList> GetMany(Expression<Func<WorkerTaskList, bool>> clause)
        {
            var workerTaskList = new List<WorkerTaskList>();

            if (IsCareerAssessmentTask)
                workerTaskList.Add(
                                   new WorkerTaskList
                                   {
                                       CategoryId         = MockWorkerTaskCategoryRepository.CareerAssessmentWorkerTask.Id,
                                       WorkerTaskStatusId = MockWorkerTaskStatusRepository.WorkerTaskStatus.Id
                                   });

            if (IsJobReadinessTask)
                workerTaskList.Add(
                                   new WorkerTaskList
                                   {
                                       CategoryId         = MockWorkerTaskCategoryRepository.JobReadinessWorkerTask.Id,
                                       WorkerTaskStatusId = MockWorkerTaskStatusRepository.WorkerTaskStatus.Id
                                   });

            if (IsTestScoresTask)
                workerTaskList.Add(
                                   new WorkerTaskList
                                   {
                                       CategoryId         = MockWorkerTaskCategoryRepository.TestScoresWorkerTask.Id,
                                       WorkerTaskStatusId = MockWorkerTaskStatusRepository.WorkerTaskStatus.Id
                                   });

            return workerTaskList;
        }

        #endregion
    }

    public class MockWorkerTaskStatusRepository : MockRepositoryBase<WorkerTaskStatus>, IWorkerTaskStatusRepository
    {
        public static readonly WorkerTaskStatus WorkerTaskStatus = new WorkerTaskStatus
                                                                   {
                                                                       Id   = 2,
                                                                       Code = Model.Interface.Constants.WorkerTaskStatus.Open
                                                                   };

        public new WorkerTaskStatus Get(Expression<Func<WorkerTaskStatus, bool>> clause)
        {
            return WorkerTaskStatus;
        }
    }


    public class MockWorkerTaskCategoryRepository : MockRepositoryBase<WorkerTaskCategory>, IWorkerTaskCategoryRepository
    {
        public bool HasGetBeenCalled;

        public static readonly WorkerTaskCategory CareerAssessmentWorkerTask = new WorkerTaskCategory
                                                                               {
                                                                                   Id          = 1,
                                                                                   Code        = WorkerTaskCategoryCodes.CareerAssessmentCode,
                                                                                   Description = "CategoryWorkerTaskDescription"
                                                                               };

        public static readonly WorkerTaskCategory JobReadinessWorkerTask = new WorkerTaskCategory
                                                                           {
                                                                               Id          = 2,
                                                                               Code        = WorkerTaskCategoryCodes.JobReadinessCode,
                                                                               Description = "JobReadinessWorkerTaskDescription"
                                                                           };

        public static readonly WorkerTaskCategory TestScoresWorkerTask = new WorkerTaskCategory
                                                                         {
                                                                             Id          = 3,
                                                                             Code        = WorkerTaskCategoryCodes.TestScoresCode,
                                                                             Description = "TestScoresWorkerTaskDescription"
                                                                         };

        public static readonly WorkerTaskCategory JobAttainmentInitiationTask = new WorkerTaskCategory
                                                                                {
                                                                                    Id          = 4,
                                                                                    Code        = WorkerTaskCategoryCodes.JAInitiatedCode,
                                                                                    Description = "Job Attainment POP Claim Initiation Task Description"
                                                                                };

        public static readonly WorkerTaskCategory LTJAInitiationTask = new WorkerTaskCategory
                                                                       {
                                                                           Id          = 5,
                                                                           Code        = WorkerTaskCategoryCodes.LTJAInitiatedCode,
                                                                           Description = "LTJA POP Claim Initiation Task Description"
                                                                       };

        public static readonly WorkerTaskCategory JAHWInitiationTask = new WorkerTaskCategory
                                                                       {
                                                                           Id          = 6,
                                                                           Code        = WorkerTaskCategoryCodes.JAHWInitiatedCode,
                                                                           Description = "HWJA POP Claim Initiation Task Description"
                                                                       };

        public static readonly WorkerTaskCategory JRInitiationTask = new WorkerTaskCategory
                                                                     {
                                                                         Id          = 7,
                                                                         Code        = WorkerTaskCategoryCodes.JRInitiatedCode,
                                                                         Description = "JR POP Claim Initiation Task Description"
                                                                     };

        public static readonly WorkerTaskCategory EAInitiationTask = new WorkerTaskCategory
                                                                     {
                                                                         Id          = 8,
                                                                         Code        = WorkerTaskCategoryCodes.EAInitiatedCode,
                                                                         Description = "EA POP Claim Initiation Task Description"
                                                                     };

        public static readonly WorkerTaskCategory VTInitiationTask = new WorkerTaskCategory
                                                                     {
                                                                         Id          = 9,
                                                                         Code        = WorkerTaskCategoryCodes.VTInitiatedCode,
                                                                         Description = "VT POP Claim Initiation Task Description"
                                                                     };

        public static readonly WorkerTaskCategory EAReturnedTask = new WorkerTaskCategory
                                                                   {
                                                                       Id          = 10,
                                                                       Code        = WorkerTaskCategoryCodes.EAReturnedCode,
                                                                       Description = "EA POP Claim Initiation Task Description"
                                                                   };

        public static readonly WorkerTaskCategory VTReturnedTask = new WorkerTaskCategory
                                                                   {
                                                                       Id          = 11,
                                                                       Code        = WorkerTaskCategoryCodes.VTReturnedCode,
                                                                       Description = "VT POP Claim Initiation Task Description"
                                                                   };

        public static readonly WorkerTaskCategory EADeniedTask = new WorkerTaskCategory
                                                                 {
                                                                     Id          = 12,
                                                                     Code        = WorkerTaskCategoryCodes.EADeniedCode,
                                                                     Description = "EA POP Claim Initiation Task Description"
                                                                 };

        public static readonly WorkerTaskCategory VTDeniedTask = new WorkerTaskCategory
                                                                 {
                                                                     Id          = 12,
                                                                     Code        = WorkerTaskCategoryCodes.VTDeniedCode,
                                                                     Description = "VT POP Claim Initiation Task Description"
                                                                 };

        public static readonly WorkerTaskCategory PCIATask = new WorkerTaskCategory
                                                             {
                                                                 Id   = 13,
                                                                 Code = WorkerTaskCategoryCodes.PlacementChangeIACode
                                                             };

        public static readonly WorkerTaskCategory JobAttainmentDeniedTask = new WorkerTaskCategory
                                                                            {
                                                                                Id          = 14,
                                                                                Code        = WorkerTaskCategoryCodes.JADeniedCode,
                                                                                Description = "Job Attainment Claim has been denied by adjudicator"
                                                                            };

        public static readonly WorkerTaskCategory JobAttainmentReturnedTask = new WorkerTaskCategory
                                                                              {
                                                                                  Id          = 15,
                                                                                  Code        = WorkerTaskCategoryCodes.JAReturnedCode,
                                                                                  Description = "Job Attainment Claim has been returned."
                                                                              };

        public static readonly WorkerTaskCategory LTJADeniedTask = new WorkerTaskCategory
                                                                   {
                                                                       Id          = 16,
                                                                       Code        = WorkerTaskCategoryCodes.LTJADeniedCode,
                                                                       Description = "Long Term Participant Job Attainment Claim has been denied by adjudicator"
                                                                   };

        public static readonly WorkerTaskCategory LTJAReturnedTask = new WorkerTaskCategory
                                                                     {
                                                                         Id          = 17,
                                                                         Code        = WorkerTaskCategoryCodes.LTJAReturnedCode,
                                                                         Description = "Long Term Participant Job Attainment Claim has been returned"
                                                                     };

        public static readonly WorkerTaskCategory JAHWDeniedTask = new WorkerTaskCategory
                                                                   {
                                                                       Id          = 18,
                                                                       Code        = WorkerTaskCategoryCodes.JAHWDeniedCode,
                                                                       Description = "High Wage Job Attainment Claim has been withdrawn by system"
                                                                   };

        public static readonly WorkerTaskCategory JAHWReturnedTask = new WorkerTaskCategory
                                                                     {
                                                                         Id          = 19,
                                                                         Code        = WorkerTaskCategoryCodes.JAHWReturnedCode,
                                                                         Description = "High Wage Job Attainment Claim has been returned"
                                                                     };

        public static readonly WorkerTaskCategory JRDeniedTask = new WorkerTaskCategory
                                                                 {
                                                                     Id          = 20,
                                                                     Code        = WorkerTaskCategoryCodes.JRDeniedCode,
                                                                     Description = "JR POP Claim Initiation Task Description"
                                                                 };

        public static readonly WorkerTaskCategory JRReturnedTask = new WorkerTaskCategory
                                                                   {
                                                                       Id          = 21,
                                                                       Code        = WorkerTaskCategoryCodes.JRReturnedCode,
                                                                       Description = "JR POP Claim Initiation Task Description"
                                                                   };

        public new IEnumerable<WorkerTaskCategory> GetAll()
        {
            return new List<WorkerTaskCategory> { CareerAssessmentWorkerTask, JobReadinessWorkerTask, TestScoresWorkerTask, LTJAInitiationTask, VTDeniedTask, EADeniedTask, VTReturnedTask, EAReturnedTask, VTInitiationTask, EAInitiationTask, JRInitiationTask, JAHWInitiationTask, JobAttainmentInitiationTask, JobAttainmentDeniedTask, JobAttainmentReturnedTask, JRDeniedTask,JRReturnedTask,LTJADeniedTask,LTJAReturnedTask,JAHWDeniedTask,JAHWReturnedTask };
        }

        public new WorkerTaskCategory Get(Expression<Func<WorkerTaskCategory, bool>> clause)
        {
            var workerTaskCategories = new List<WorkerTaskCategory> { CareerAssessmentWorkerTask, JobReadinessWorkerTask, TestScoresWorkerTask, LTJAInitiationTask, VTDeniedTask, EADeniedTask, VTReturnedTask, EAReturnedTask, VTInitiationTask, EAInitiationTask, JRInitiationTask, JAHWInitiationTask, PCIATask, JobAttainmentInitiationTask, JobAttainmentDeniedTask, JobAttainmentReturnedTask, JRDeniedTask, JRReturnedTask, LTJADeniedTask, LTJAReturnedTask, JAHWDeniedTask, JAHWReturnedTask };
            HasGetBeenCalled         = true;
            return workerTaskCategories.FirstOrDefault(clause.Compile());
        }
    }
}
