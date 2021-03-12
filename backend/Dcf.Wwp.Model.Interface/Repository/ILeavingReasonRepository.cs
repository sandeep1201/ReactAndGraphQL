using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ILeavingReasonRepository
    {
        IEnumerable<IJobTypeLeavingReasonBridge> LeavingReasons();
    }
}
