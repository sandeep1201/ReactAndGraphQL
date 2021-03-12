using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Auxiliary : BaseEntity
    {
        #region Properties

        public int      ParticipantId                  { get; set; }
        public int      OrganizationId                 { get; set; }
        public int?     OfficeId                       { get; set; }
        public int?     CountyId                       { get; set; }
        public int      AuxiliaryReasonId              { get; set; }
        public int      ParticipationPeriodId          { get; set; }
        public decimal? CaseNumber                     { get; set; }
        public short    ParticipationPeriodYear        { get; set; }
        public decimal  OriginalPayment                { get; set; }
        public decimal  RequestedAmount                { get; set; }
        public bool     IsSystemRequested              { get; set; }
        public bool     IsDeleted                      { get; set; }
        public string   ModifiedBy                     { get; set; }
        public DateTime ModifiedDate                   { get; set; }
        public int      EnrolledProgramId              { get; set; }
        public short?   AGSequenceNumber               { get; set; }
        public string   RequestedUserForApprovalAndDB2 { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                  Participant         { get; set; }
        public virtual Organization                 Organization        { get; set; }
        public virtual Office                       Office              { get; set; }
        public virtual CountyAndTribe               CountyAndTribe      { get; set; }
        public virtual AuxiliaryReason              AuxiliaryReason     { get; set; }
        public virtual ParticipationPeriodLookUp    ParticipationPeriod { get; set; }
        public virtual EnrolledProgram              EnrolledProgram     { get; set; }
        public virtual ICollection<AuxiliaryStatus> AuxiliaryStatuses   { get; set; } = new List<AuxiliaryStatus>();

        #endregion

        #region Clone

        #endregion
    }
}
