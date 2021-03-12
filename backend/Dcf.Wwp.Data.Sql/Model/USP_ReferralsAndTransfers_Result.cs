using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class USP_ReferralsAndTransfers_Result
    {
        public string    ParticipantFirstname     { get; set; }
        public string    ParticipantMiddleInitial { get; set; }
        public string    ParticipantLastName      { get; set; }
        public string    ParticipantSuffix        { get; set; }
        public decimal?  PinNumber                { get; set; }
        public DateTime? ParticipantDOB           { get; set; }
        public string    EnrolledProgram          { get; set; }
        public string    Status                   { get; set; }
        public DateTime? ReferralDate             { get; set; }
        public string    MFUserId                 { get; set; }
        public string    WorkerFirstName          { get; set; }
        public string    WorkerMiddleInitial      { get; set; }
        public string    WorkerLastName           { get; set; }
        public string    WorkerSuffix             { get; set; }
        public string    AgencyName               { get; set; }
        public string    CountyName               { get; set; }
        public short?    OfficeNumber             { get; set; }
        public bool?     IsTransfer               { get; set; }
        public int?      GroupOrder               { get; set; }
        public long?     Rn                       { get; set; }
        public bool?     IsConfidential           { get; set; }
    }
}
