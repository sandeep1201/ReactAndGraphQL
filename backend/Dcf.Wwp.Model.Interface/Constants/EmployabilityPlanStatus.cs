namespace Dcf.Wwp.Model.Interface.Constants
{
    public static class EmployabilityPlanStatus
    {
        // Keep in Sync with EmployabilityPlanStatusType table in database!!!
        public const int InProgressId    = 1;
        public const int SubmittedId     = 3;
        public const int SystemDeletedId = 4;
        public const int WorkerDeletedId = 5;
        public const int WorkerVoidedId  = 6;
        public const int EndedId         = 7;

        public const string InProgress    = "In Progress";
        public const string Submitted     = "Submitted";
        public const string SystemDeleted = "System Deleted";
        public const string WorkerDeleted = "Worker Deleted";
        public const string WorkerVoided  = "Worker Voided";
        public const string Ended         = "Ended";
    }
}
