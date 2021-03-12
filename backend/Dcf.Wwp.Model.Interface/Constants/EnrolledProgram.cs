namespace Dcf.Wwp.Model.Interface.Constants
{
    public static class EnrolledProgram
    {
        // Keep in Sync with EnrolledProgram table in database!!!
        public const int W2 = 1;

        public const int WWC                      = 1;
        public const int WWJ                      = 2;
        public const int WWL                      = 3;
        public const int WWM                      = 4;
        public const int WWN                      = 5;
        public const int WWP                      = 6;
        public const int WWX                      = 7;
        public const int WWZ                      = 8;
        public const int TransformMilwaukeeJobsId = 9;
        public const int ChildrenFirstId          = 10;
        public const int WW                       = 11;
        public const int TransitionalJobsId       = 12;
        public const int LearnFareId              = 13;
        public const int FCDPId                   = 14;

        public const string W2ProgramCode   = "WW";
        public const string TmjProgramCode  = "TMJ";
        public const string TjProgramCode   = "TJ";
        public const string CFProgramCode   = "CF";
        public const string LFProgramCode   = "LF";
        public const string FCDPProgramCode = "FCD";

        //Not a program but avoiding hardcoded EA Code everywhere
        public const string EACode = "EA";
    }
}
