using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Relationship
    {
        #region Properties

        public string    RelationName { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ICollection<ParticipantChildRelationshipBridge> ParticipantChildRelationshipBridges { get; set; }
        public virtual ICollection<FamilyMember>                       FamilyMembers                       { get; set; }

        #endregion
    }
}
