using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Child
    {
        #region Properties

        public decimal?  PinNumber         { get; set; }
        public string    FirstName         { get; set; }
        public string    MiddleInitialName { get; set; } // why not just call this 'MiddleInitial', it's only one char wide?
        public string    LastName          { get; set; }
        public string    SuffixName        { get; set; }
        public DateTime? DateOfBirth       { get; set; }
        public DateTime? DateOfDeath       { get; set; }
        public int?      GenderTypeId      { get; set; }
        public string    GenderIndicator   { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual GenderType                                      GenderType                          { get; set; }
        public virtual ICollection<ChildYouthSectionChild>             ChildYouthSectionChilds             { get; set; }
        public virtual ICollection<ParticipantChildRelationshipBridge> ParticipantChildRelationshipBridges { get; set; }
        public virtual ICollection<RequestForAssistanceChild>          RequestForAssistanceChildren        { get; set; }

        #endregion
    }
}
