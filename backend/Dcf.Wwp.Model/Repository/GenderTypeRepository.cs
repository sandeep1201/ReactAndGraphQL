using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IGenderTypeRepository
    {
        public IEnumerable<IGenderType> GenderTypes()
        {
            return _db.GenderTypes.OrderBy(x => x.SortOrder);
        }
    }
}
