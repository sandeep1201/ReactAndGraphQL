using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ILeavingReasonRepository
    {
        public IEnumerable<IJobTypeLeavingReasonBridge> LeavingReasons()
        {
            return _db.JobTypeLeavingBridges
                      .OrderBy(i => i.JobType.SortOrder);
        }
    }
}
