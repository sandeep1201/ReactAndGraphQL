using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialParentRelationshipRepository
    {
        public IEnumerable<INonCustodialParentRelationship> NonCustodialParentRelationships()
        {
            return (from x in _db.NonCustodialParentRelationships where !x.IsDeleted select x);
        }
    }
}
