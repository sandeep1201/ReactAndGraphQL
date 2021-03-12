using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IT0459_IN_W2_LIMITSRepository
    {
        IT0459_IN_W2_LIMITS NewT0459_IN_W2_LIMITS(bool isTracked);
        List<IT0459_IN_W2_LIMITS> GetLatestW2LimitsMonthsForEachClockType(Decimal pinNum);

        IT0459_IN_W2_LIMITS GetLatestW2LimitsByClockType(Decimal pinNum, ClockTypes clockType);

        IT0459_IN_W2_LIMITS GetW2LimitByMonth(Decimal effectiveMonth, Decimal pinNum);

        List<IT0459_IN_W2_LIMITS> GetW2LimitsByPin(Decimal pinNum);
        List<IT0459_IN_W2_LIMITS> GetSubsequentW2Limits(Decimal pinNum, DateTime timelineMonthDate);
        void DB2_T0459_Update(IT0459_IN_W2_LIMITS db2Record);
    }
}
