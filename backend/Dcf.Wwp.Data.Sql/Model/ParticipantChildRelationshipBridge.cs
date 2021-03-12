using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantChildRelationshipBridge
    {
        #region Properties

        public int?      ParticipantId  { get; set; }
        public int?      ChildId        { get; set; }
        public int?      RelationshipId { get; set; }
        public bool      IsDeleted      { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant  Participant  { get; set; }
        public virtual Child        Child        { get; set; }
        public virtual Relationship Relationship { get; set; }

        #endregion
    }
}
