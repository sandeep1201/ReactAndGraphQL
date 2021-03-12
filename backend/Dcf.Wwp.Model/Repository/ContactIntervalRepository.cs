using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IContactIntervalRepository
    {
        public IEnumerable<IContactInterval> ContactIntervals()
        {
            return (from x in _db.ContactIntervals orderby x.SortOrder select x);
        }
    }
}
