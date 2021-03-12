using Dcf.Wwp.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IMilitaryDischargeTypeRepository
    {
        public IMilitaryDischargeType DischargeTypeById(int id)
        {
            return _db.MilitaryDischargeTypes.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<IMilitaryDischargeType> DischargeTypes()
        {
            return _db.MilitaryDischargeTypes.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<IMilitaryDischargeType> AllDischargeTypes()
        {
            return _db.MilitaryDischargeTypes.OrderBy(x => x.SortOrder);
        }
    }
}
