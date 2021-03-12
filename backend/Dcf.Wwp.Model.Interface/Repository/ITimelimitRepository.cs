using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ITimelimitRepository
    {
        IEnumerable<ITimeLimit> TimeLimitsByPin(String pin);
        ITimeLimit TimeLimitById(Int32 id);
        IEnumerable<ITimeLimit> TimeLimitsByIds(IEnumerable<Int32> ids);
        ITimeLimit TimeLimitByDate(String pin, DateTime date, Boolean includedDeleted);

        IEnumerable<ITimeLimit> TimeLimitsHistory(Int32 id);
        IEnumerable<ITimeLimit> TimeLimitsByDates(String pin, List<DateTime> dates);
        ITimeLimit NewTimeLimit();

        IEnumerable<ITimeLimitState> TimeLimitStates(Boolean excludeWisconsin = true);

        IEnumerable<IChangeReason> ChangeReasons();

        ITimeLimitSummary NewTimeLimitSummary();

    }
}