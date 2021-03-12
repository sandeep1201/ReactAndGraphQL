using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IPendingChargeRepository
    {
        IPendingCharge NewPendingCharge(ILegalIssuesSection parentSection, String user);
        void DeletePendingCharge(IPendingCharge PendingCharge);
    }
}
