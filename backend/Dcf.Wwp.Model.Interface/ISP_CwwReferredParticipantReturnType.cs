using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISP_CwwReferredParticipantReturnType
    {
        long      ID                   { get; set; }
        decimal   PinNumber            { get; set; }
        string    FirstName            { get; set; }
        string    MiddleInitialName    { get; set; }
        string    LastName             { get; set; }
        DateTime? DOBDate              { get; set; }
        string    SuffixName           { get; set; }
        short?    CountyNumber         { get; set; }
        short?    OfficeNumber         { get; set; }
        string    ReferralStatus       { get; set; }
        DateTime? WPReferralDate       { get; set; }
        string    ProgramCode          { get; set; }
        decimal?  CaseNumber           { get; set; }
        string    MFWorkerId           { get; set; }
        bool?     ConfidentialSwitch   { get; set; }
        string    MFConfidentialWorker { get; set; }
        string    WorkerFirstName      { get; set; }
        string    WorkerMiddleInitial  { get; set; }
        string    WorkerLastName       { get; set; }
    }
}
