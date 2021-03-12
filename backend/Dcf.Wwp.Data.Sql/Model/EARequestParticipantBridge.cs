using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EARequestParticipantBridge
    {
        #region Properties

        public int       ParticipantId      { get; set; }
        public int       EARequestId        { get; set; }
        public int?      EAIndividualTypeId { get; set; }
        public int?      EARelationTypeId   { get; set; }
        public bool?     IsIncluded         { get; set; }
        public DateTime? SSNAppliedDate     { get; set; }
        public int?      SSNExemptTypeId    { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime  ModifiedDate       { get; set; }

        #endregion

        #region Nav Properties

        public virtual Participant Participant { get; set; }

        #endregion
    }
}
