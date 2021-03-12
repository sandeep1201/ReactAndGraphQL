using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ISupportiveServiceTypeRepository
    {
        public IEnumerable<ISupportiveServiceType> SupportiveServiceTypes()
        {
            return _db.SupportiveServiceTypes
                      .OrderBy(i => i.SortOrder)
                      .Select(i => i);
        }
    }
}
