using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class USP_RecentlyAccessed_ProgramStatus_Result
    {
        public int?      ParticipantId          { get; set; }
        public string    ProgramName            { get; set; }
        public string    RecentStatus           { get; set; }
        public DateTime? RecentStatusDate       { get; set; }
        public string    ParticipantFirstName   { get; set; }
        public string    ParticipantMiddleName  { get; set; }
        public string    ParticipantLastName    { get; set; }
        public string    ParticipantSuffixName  { get; set; }
        public decimal?  PinNumber              { get; set; }
        public DateTime? ParticipantDateOfBirth { get; set; }
        public string    WorkerFirstName        { get; set; }
        public string    WorkerMiddleInitial    { get; set; }
        public string    WorkerLastName         { get; set; }
        public string    MFUserId               { get; set; }
        public string    EntsecAgencyCode       { get; set; }
        public string    CountyName             { get; set; }
        public short?    OfficeNumber           { get; set; }
        public string    AssignedWorker         { get; set; }
        public bool?     IsTransfer             { get; set; }
        public string    WorkerAgencyName       { get; set; }
        public string    ProgramCode            { get; set; }
        public bool?     WorkerActiveStatusCode { get; set; }
        public int?      WorkerId               { get; set; }
        public int?      PEPId                  { get; set; }
        public int?      EnrolledProgramId      { get; set; }
        public bool?     IsConfidential         { get; set; }
        public string    GenderIndicator        { get; set; }
    }
}
