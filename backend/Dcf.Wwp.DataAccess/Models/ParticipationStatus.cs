using System;
using Dcf.Wwp.DataAccess.Base;
using Newtonsoft.Json;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipationStatus : BaseEntity
    {
        #region Properties

        public int?      ParticipantId     { get; set; }
        public int?      StatusId          { get; set; }
        public DateTime? BeginDate         { get; set; }
        public DateTime? EndDate           { get; set; }
        public bool?     isCurrent         { get; set; }
        public int?      EnrolledProgramId { get; set; }
        public string    Details           { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime  ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual Participant Participant { get; set; }

        public virtual ParticipationStatusType Status          { get; set; }
        public virtual EnrolledProgram         EnrolledProgram { get; set; }

        #endregion
    }
}
