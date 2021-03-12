using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IHousingSituationRepository
    {
        IHousingSituation HousingSituationById(int? housingSituation);
        IHousingSituation OtherHousingSituation(int? housingSituation);
        IEnumerable<IHousingSituation> HousingSituations();
        IEnumerable<IHousingSituation> AllHousingSituations();
    }
}
