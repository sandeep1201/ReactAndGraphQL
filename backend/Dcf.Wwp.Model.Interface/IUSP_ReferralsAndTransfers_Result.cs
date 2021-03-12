using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IUSP_ReferralsAndTransfers_Result
    {
        string    ParticipantFirstname     { get; set; }
        string    ParticipantMiddleInitial { get; set; }
        string    ParticipantLastName      { get; set; }
        string    ParticipantSuffix        { get; set; }
        decimal?  PinNumber                { get; set; }
        DateTime? ParticipantDOB           { get; set; }
        string    EnrolledProgram          { get; set; }
        string    Status                   { get; set; }
        DateTime? ReferralDate             { get; set; }
        string    MFUserId                 { get; set; }
        string    WorkerFirstName          { get; set; }
        string    WorkerMiddleInitial      { get; set; }
        string    WorkerLastName           { get; set; }
        string    WorkerSuffix             { get; set; }
        string    AgencyName               { get; set; }
        string    CountyName               { get; set; }
        short?    OfficeNumber             { get; set; }
        bool?     IsTransfer               { get; set; }
        bool?     IsConfidential           { get; set; }
        int?      GroupOrder               { get; set; }
        long?     Rn                       { get; set; }
    }
}
