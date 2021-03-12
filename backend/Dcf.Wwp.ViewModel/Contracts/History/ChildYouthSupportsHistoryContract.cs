using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class ChildYouthSupportsHistoryContract
    {
        public List<HistoryValueContract> HasChildren { get; set; }
        public List<ChildHistoryContract> Children { get; set; }
        public List<HistoryValueContract> HasTeensWithDisabilityInNeedOfChildCare { get; set; }
        public List<ChildHistoryContract> Teens { get; set; }
        public List<HistoryValueContract> HasWicBenefits { get; set; }
        public List<HistoryValueContract> IsInHeadStart { get; set; }
        public List<HistoryValueContract> IsInAfterSchoolOrSummerProgram { get; set; }
        public List<HistoryValueContract> AfterSchoolOrSummerProgramNotes { get; set; }
        public List<HistoryValueContract> IsInMentoringProgram { get; set; }
        public List<HistoryValueContract> MentoringProgramNotes { get; set; }
        public List<HistoryValueContract> HasChildWelfareWorker { get; set; }
        public List<HistoryValueContract> ChildWelfareWorkerPlan { get; set; }
        public List<HistoryValueContract> ChildWelfareWorkerChildren { get; set; }
        public List<HistoryValueContract> ChildWelfareWorkerContactId { get; set; }
        public List<HistoryValueContract> DidOrWillAgeOutOfFosterCare { get; set; }
        public List<HistoryValueContract> FosterCareNotes { get; set; }
        public List<HistoryValueContract> HasFutureChildCareChanges { get; set; }
        public List<HistoryValueContract> FutureChildCareChangesNotes { get; set; }
        public List<HistoryValueContract> Notes { get; set; }

        public ChildYouthSupportsHistoryContract()
        {
            Children = new List<ChildHistoryContract>();
            Teens = new List<ChildHistoryContract>();
        }
    }

    public class ChildHistoryContract
    {
        public List<HistoryValueContract> CareArrangementId { get; set; }
        public List<HistoryValueContract> AgeCategoryId { get; set; }
        public List<HistoryValueContract> IsSpecialNeeds { get; set; }
        public List<HistoryValueContract> Details { get; set; }
        public List<HistoryValueContract> FirstName { get; set; }
        public List<HistoryValueContract> LastName { get; set; }
        public List<HistoryValueContract> DateOfBirth { get; set; }

        public ChildHistoryContract()
        {
            CareArrangementId = new List<HistoryValueContract>();
            AgeCategoryId = new List<HistoryValueContract>();
            IsSpecialNeeds = new List<HistoryValueContract>();
            Details = new List<HistoryValueContract>();
            FirstName = new List<HistoryValueContract>();
            LastName = new List<HistoryValueContract>();
            DateOfBirth = new List<HistoryValueContract>();
        }
    }

}