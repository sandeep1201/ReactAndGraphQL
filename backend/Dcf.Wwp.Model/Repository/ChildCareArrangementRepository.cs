using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IChildCareArrangement ChildCareArrangementById(int? id)
        {
            var childCareArrangement = (from cca in _db.ChildCareArrangements where cca.Id == id select cca).SingleOrDefault();
            return childCareArrangement;
        }

        public IEnumerable<IChildCareArrangement> ChildCareArrangements()
        {
            return from x in _db.ChildCareArrangements where !x.IsDeleted orderby x.SortOrder select x;
        }

        public IEnumerable<IChildCareArrangement> AllChildCareArrangements()
        {
            return from x in _db.ChildCareArrangements orderby x.SortOrder select x;
        }
    }

}
