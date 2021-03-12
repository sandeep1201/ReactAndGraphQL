using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRelationshipRepository
    {
        public IEnumerable<IRelationship> RelationshipTypes()
        {
            return (from x in _db.Relationships where !x.IsDeleted select x);
        }
    }
}