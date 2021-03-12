using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class USP_ProgramStatus_Recent_Result
    {
        public string    ProgramName                 { get; set; }
        public string    RecentStatus                { get; set; }
        public DateTime? RecentStatusDate            { get; set; }
        public int?      Id                          { get; set; }
        public int?      ParticipantId               { get; set; }
        public int?      EnrolledProgramId           { get; set; }
        public int?      EnrolledProgramStatusCodeId { get; set; }
        public DateTime? ReferralDate                { get; set; }
        public DateTime? EnrollmentDate              { get; set; }
        public DateTime? DisenrollmentDate           { get; set; }
        public decimal?  CASENumber                  { get; set; }
        public string    ReferralRegistrationCode    { get; set; }
        public string    CurrentRegCode              { get; set; }
        public short?    AGSequenceNumber            { get; set; }
        public string    CaseManagerId               { get; set; }
        public int?      WorkerId                    { get; set; }
        public int?      CompletionReasonId          { get; set; }
        public int?      RequestForAssistanceId      { get; set; }
        public int?      OfficeId                    { get; set; }
        public bool?     IsDeleted                   { get; set; }
        public DateTime? ModifiedDate                { get; set; }
        public string    ModifiedBy                  { get; set; }
        public byte[]    RowVersion                  { get; set; }
        public int?      LFFEPId                     { get; set; }
        public bool?     IsConfidential              { get; set; }
    }
}
