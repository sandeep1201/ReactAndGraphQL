using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IMilitaryRankRepository
    {
        IMilitaryRank MilitaryRankById(int id);

        IEnumerable<IMilitaryRank> MilitaryRanks();

        IEnumerable<IMilitaryRank> AllMilitaryRanks();
    }
}
