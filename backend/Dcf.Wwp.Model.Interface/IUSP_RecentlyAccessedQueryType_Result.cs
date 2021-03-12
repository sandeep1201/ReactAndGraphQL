using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IUSP_RecentlyAccessed_ProgramStatus_Result
    {
        int?      ParticipantId          { get; set; }
        int?      PEPId                  { get; set; }
        int?      EnrolledProgramId      { get; set; }
        string    ProgramName            { get; set; }
        string    ProgramCode            { get; set; }
        string    RecentStatus           { get; set; }
        DateTime? RecentStatusDate       { get; set; }
        string    ParticipantFirstName   { get; set; }
        string    ParticipantMiddleName  { get; set; }
        string    ParticipantLastName    { get; set; }
        string    ParticipantSuffixName  { get; set; }
        decimal?  PinNumber              { get; set; }
        DateTime? ParticipantDateOfBirth { get; set; }
        int?      WorkerId               { get; set; }
        string    WorkerFirstName        { get; set; }
        string    WorkerMiddleInitial    { get; set; }
        string    WorkerLastName         { get; set; }
        string    MFUserId               { get; set; }
        bool?     WorkerActiveStatusCode { get; set; }
        string    EntsecAgencyCode       { get; set; }
        string    CountyName             { get; set; }
        short?    OfficeNumber           { get; set; }
        string    AssignedWorker         { get; set; }
        string    WorkerAgencyName       { get; set; }
        bool?     IsTransfer             { get; set; }
        bool?     IsConfidential         { get; set; }
        string    GenderIndicator        { get; set; }
    }
}
