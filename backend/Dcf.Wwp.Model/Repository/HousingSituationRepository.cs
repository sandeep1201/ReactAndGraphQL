using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IHousingSituationRepository
    {
        public IHousingSituation HousingSituationById(int? housingSituation)
        {
            return (from x in _db.HousingSituations where x.Id == housingSituation select x).SingleOrDefault();
        }

        public IHousingSituation OtherHousingSituation(int? housingSituation)
        {
            return (from x in _db.HousingSituations where x.Id == housingSituation && x.Name == "Other" select x).SingleOrDefault();
        }

        public IEnumerable<IHousingSituation> HousingSituations()
        {
            return _db.HousingSituations.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<IHousingSituation> AllHousingSituations()
        {
            return _db.HousingSituations.OrderBy(x => x.SortOrder);
        }
    }
}
