namespace Dcf.Wwp.Model.Interface.Constants
{
    public static class RequestForAssistanceStatus
    {
        // Keep in Sync with wwp.RequestForAssistanceStatus table in database!!!
        public const int InProgress      = 1;
        public const int Referred        = 2;
        public const int Enrolled        = 3;
        public const int Disenrolled     = 4;
        public const int RfaDenied       = 5;
        public const int RfaDeniedSystem = 6;
    }

    public enum RequestForAssistanceStatusEnum
    {
        InProgress = 1,
        Referred,
        Enrolled,
        Disenrolled,
        RfaDenied,
        RfaDeniedSystem,
    }
}
