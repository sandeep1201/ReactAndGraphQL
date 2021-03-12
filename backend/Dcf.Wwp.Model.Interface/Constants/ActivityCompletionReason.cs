namespace Dcf.Wwp.Model.Interface.Constants
{
    public class ActivityCompletionReason
    {
        // These strings are used for lookups in ActivityCompletionReason and match the Code
        // values in the ActivityCompletionReason table.
        public const string A = @"Successfully completed";
        public const string B = @"Incomplete/interrupted";
        public const string D = @"Disenrollment";
        public const string K = @"Failed to participate - not good cause";
        public const string L = @"Inappropriate assignment";
        public const string N = @"Activity ended due to CMF placement/employment";
        public const string O = @"System completed";
        public const string P = @"Completed appropriate formal assessment within prior 12 mo.";
        public const string S = @"Participant receiving SSI";
        public const string T = @"Transferred case";
        public const string V = @"Educational attainment and vocational training completion";

        // Keep in Sync with ActivityCompletionReason table in database!!!
        public const int AId = 1;
        public const int BId = 2;
        public const int DId = 3;
        public const int KId = 5;
        public const int LId = 6;
        public const int NId = 8;
        public const int OId = 9;
        public const int PId = 10;
        public const int SId = 11;
        public const int TId = 12;
        public const int VId = 13;
    }
}
