using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IUSP_ProgramStatus_Recent_Result
    {
        #region Properties

        string    ProgramName                 { get; set; }
        string    RecentStatus                { get; set; }
        DateTime? RecentStatusDate            { get; set; }
        int?      Id                          { get; set; }
        int?      ParticipantId               { get; set; }
        int?      EnrolledProgramId           { get; set; }
        int?      EnrolledProgramStatusCodeId { get; set; }
        DateTime? ReferralDate                { get; set; }
        DateTime? EnrollmentDate              { get; set; }
        DateTime? DisenrollmentDate           { get; set; }
        decimal?  CASENumber                  { get; set; }
        string    ReferralRegistrationCode    { get; set; }
        string    CurrentRegCode              { get; set; }
        short?    AGSequenceNumber            { get; set; }
        string    CaseManagerId               { get; set; }
        int?      WorkerId                    { get; set; }
        int?      CompletionReasonId          { get; set; }
        int?      RequestForAssistanceId      { get; set; }
        int?      OfficeId                    { get; set; }
        bool?     IsDeleted                   { get; set; }
        DateTime? ModifiedDate                { get; set; }
        string    ModifiedBy                  { get; set; }
        byte[]    RowVersion                  { get; set; }
        int?      LFFEPId                     { get; set; }
        bool?     IsConfidential              { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
