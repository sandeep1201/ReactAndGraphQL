using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IT0460_IN_W2_EXTRepository : ICommonRepo
    {
        IT0460_IN_W2_EXT NewT0460InW2Ext(bool isTracked);

        IT0460_IN_W2_EXT GetW2ExtensionByClockType(Decimal pinNum, ClockTypes timelimitType, Int32 sequenceNumber);

        IEnumerable<IT0460_IN_W2_EXT> GetW2Extensions(Decimal pinNum);

        int? GetCurrentExtensionSequenceNumber(int participantId, int timelimitTypeId);

    }
}
