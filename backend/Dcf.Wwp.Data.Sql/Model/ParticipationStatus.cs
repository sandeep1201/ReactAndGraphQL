using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipationStatu
    {
        #region Properties

        public int?      ParticipantId     { get; set; }
        public int?      StatusId          { get; set; }
        public DateTime? BeginDate         { get; set; }
        public DateTime? EndDate           { get; set; }
        public string    Details           { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime  ModifiedDate      { get; set; }
        public bool?     IsCurrent         { get; set; }
        public int?      EnrolledProgramId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant             Participant             { get; set; }
        public virtual ParticipationStatusType ParticipationStatusType { get; set; }
        public virtual EnrolledProgram         EnrolledProgram         { get; set; }

        #endregion
    }
}
