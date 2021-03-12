using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantEnrolledProgram
    {
        #region Properties

        public int       ParticipantId               { get; set; }
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
        public bool      IsDeleted                   { get; set; }
        public string    ModifiedBy                  { get; set; }
        public DateTime? ModifiedDate                { get; set; }
        public int?      LFFEPId                     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                      Participant               { get; set; }
        public virtual EnrolledProgram                  EnrolledProgram           { get; set; }
        public virtual EnrolledProgramStatusCode        EnrolledProgramStatusCode { get; set; }
        public virtual CompletionReason                 CompletionReason          { get; set; }
        public virtual RequestForAssistance             RequestForAssistance      { get; set; }
        public virtual Worker                           Worker                    { get; set; }
        public virtual Worker                           LFFEP                     { get; set; }
        public virtual Office                           Office                    { get; set; }
        public virtual ICollection<PEPOtherInformation> PEPOtherInformations       { get; set; } = new List<PEPOtherInformation>();
        public virtual ICollection<EmployabilityPlan>   EmployabilityPlans        { get; set; } = new List<EmployabilityPlan>();
        public virtual ICollection<OfficeTransfer>      OfficeTransfers           { get; set; } = new List<OfficeTransfer>();

        #endregion
    }
}
