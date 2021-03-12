using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IReferralContactIntervalRepository
    {
        public IEnumerable<IReferralContactInterval> ReferralContactIntervals()
        {
            return (from x in _db.ReferralContactIntervals orderby x.SortOrder select x);
        }
    }
}
