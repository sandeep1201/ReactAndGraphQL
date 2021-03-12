using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IReferralContactIntervalRepository
    {
        IEnumerable<IReferralContactInterval> ReferralContactIntervals();
    }
}