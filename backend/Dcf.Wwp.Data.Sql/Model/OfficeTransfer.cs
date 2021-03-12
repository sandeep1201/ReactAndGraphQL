using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class OfficeTransfer
    {
        #region Properties

        public int       ParticipantId                { get; set; }
        public int       ParticipantEnrolledProgramId { get; set; }
        public int?      SourceOfficeId               { get; set; }
        public int?      SourceAssignedWorkerId       { get; set; }
        public int?      DestinationOfficeId          { get; set; }
        public int?      DestinationAssignedWorkerId  { get; set; }
        public DateTime  TransferDate                 { get; set; }
        public bool      IsDeleted                    { get; set; }
        public string    ModifiedBy                   { get; set; }
        public DateTime? ModifiedDate                 { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                Participant                { get; set; }
        public virtual ParticipantEnrolledProgram ParticipantEnrolledProgram { get; set; }
        public virtual Office                     SourceOffice               { get; set; }
        public virtual Worker                     SourceAssignedWorker       { get; set; }
        public virtual Office                     DestinationOffice          { get; set; }
        public virtual Worker                     DestinationAssignedWorker  { get; set; }

        #endregion
    }
}
