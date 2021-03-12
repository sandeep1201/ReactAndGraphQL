using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IChild : ICommonDelModel
    {
        decimal?  PinNumber         { get; set; }
        string    FirstName         { get; set; }
        string    MiddleInitialName { get; set; }
        string    LastName          { get; set; }
        string    SuffixName        { get; set; }
        DateTime? DateOfBirth       { get; set; }
        DateTime? DateOfDeath       { get; set; }
        string    GenderIndicator   { get; set; }
        int?      GenderTypeId      { get; set; }

        ICollection<IChildYouthSectionChild>             ChildYouthSectionChilds             { get; set; }
        ICollection<IParticipantChildRelationshipBridge> ParticipantChildRelationshipBridges { get; set; }
        IGenderType                                      GenderType                          { get; set; }
    }
}
