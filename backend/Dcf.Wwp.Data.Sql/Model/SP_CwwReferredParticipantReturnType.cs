using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SP_CwwReferredParticipantReturnType
    {
        public long      ID                   { get; set; }
        public decimal   PinNumber            { get; set; }
        public string    FirstName            { get; set; }
        public string    MiddleInitialName    { get; set; }
        public string    LastName             { get; set; }
        public DateTime? DOBDate              { get; set; }
        public string    SuffixName           { get; set; }
        public short?    CountyNumber         { get; set; }
        public short?    OfficeNumber         { get; set; }
        public string    ReferralStatus       { get; set; }
        public DateTime? WPReferralDate       { get; set; }
        public string    ProgramCode          { get; set; }
        public decimal?  CaseNumber           { get; set; }
        public string    MFWorkerId           { get; set; }
        public bool?     ConfidentialSwitch   { get; set; }
        public string    MFConfidentialWorker { get; set; }
        public string    WorkerFirstName      { get; set; }
        public string    WorkerMiddleInitial  { get; set; }
        public string    WorkerLastName       { get; set; }
    }
}
