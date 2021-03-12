using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IBarrierDetailRepository
    {
        IBarrierDetail BarrierDetailById(Int32? barrierId);

        IBarrierDetail NewBarrierDetailInfo(IParticipant participant, String user);

        IEnumerable<IBarrierDetail> BarrierDetailsByParticipantId(int participantId);
    }
}
