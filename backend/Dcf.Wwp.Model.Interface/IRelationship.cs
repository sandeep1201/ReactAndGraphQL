using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IRelationship : ICommonDelModel, ICloneable
    {
        String RelationName { get; set; }
        ICollection<IFamilyMember> FamilyMembers { get; set; }
    }
}
