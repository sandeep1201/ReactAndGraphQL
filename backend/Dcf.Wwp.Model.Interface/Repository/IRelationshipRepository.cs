using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRelationshipRepository
    {
        IEnumerable<IRelationship> RelationshipTypes();
    }
}