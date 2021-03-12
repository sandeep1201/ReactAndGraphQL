using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWageTypeRepository
    {
        public IEnumerable<IWageType> WageTypes()
        {
            var wts = from x in _db.WageTypes orderby x.SortOrder select x;
            return wts;
        }
    }
}
