using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipantEnrolledProgram : BaseEntity
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
        public int?      OfficeId                    { get; set; }
        public int?      RequestForAssistanceId      { get; set; }
        public bool      IsDeleted                   { get; set; }
        public string    ModifiedBy                  { get; set; }
        public DateTime? ModifiedDate                { get; set; }
        public int?      LFFEPId                     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram           EnrolledProgram           { get; set; }
        public virtual Participant               Participant               { get; set; }
        public virtual Worker                    LFFEP                     { get; set; }
        public virtual Worker                    Worker                    { get; set; }
        public virtual Office                    Office                    { get; set; }
        public virtual EnrolledProgramStatusCode EnrolledProgramStatusCode { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfficeTransfer> OfficeTransfers { get; set; }

        public virtual CompletionReason CompletionReason { get; set; }
        public virtual RequestForAssistance RequestForAssistance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEPOtherInformation> PEPOtherInformations { get; set; }*/

        #endregion

        #region NotMappedProp

        [NotMapped]
        public bool IsDisenrolled => EnrolledProgramStatusCodeId == Model.Interface.Constants.EnrolledProgramStatusCode.DisenrolledId;

        [NotMapped]
        public bool IsEnrolled => EnrolledProgramStatusCodeId == Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;

        [NotMapped]
        public bool IsReferred => EnrolledProgramStatusCodeId == Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId;

        [NotMapped]
        public bool IsW2 => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsTmj => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Model.Interface.Constants.EnrolledProgram.TmjProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsTJ => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Model.Interface.Constants.EnrolledProgram.TjProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsCF => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Model.Interface.Constants.EnrolledProgram.CFProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsFCDP => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Model.Interface.Constants.EnrolledProgram.FCDPProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsLF => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Model.Interface.Constants.EnrolledProgram.LFProgramCode.Trim().ToLower();

        #endregion
    }
}
