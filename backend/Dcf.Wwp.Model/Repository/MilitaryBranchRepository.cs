using Dcf.Wwp.Model.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IMilitaryBranch MilitaryBranchById(int? id)
        {
            var militaryBranch = (from mb in _db.MilitaryBranches where mb.Id == id select mb).SingleOrDefault();
            return militaryBranch;
        }

        public IEnumerable<IMilitaryBranch> MilitaryBranches()
        {
            return _db.MilitaryBranches.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<IMilitaryBranch> AllMilitaryBranches()
        {
            return _db.MilitaryBranches.OrderBy(x => x.SortOrder);
        }
    }
}
