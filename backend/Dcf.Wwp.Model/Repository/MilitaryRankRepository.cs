using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IMilitaryRankRepository
    {
        public IMilitaryRank MilitaryRankById(int id)
        {
            return _db.MilitaryRanks.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<IMilitaryRank> MilitaryRanks()
        {
            return _db.MilitaryRanks.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<IMilitaryRank> AllMilitaryRanks()
        {
            return _db.MilitaryRanks.OrderBy(x => x.SortOrder);
        }
    }
}
