using System;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using EnumsNET;

namespace DCF.Timelimits.Rules.Domain
{
    public class TimelineMonth
    {
        public ClockTypes ClockTypes { get; set; }
        public DateTime Date { get; private set; }


        public TimelineMonth(DateTime effectiveMonth, ClockTypes timelimitType = Domain.ClockTypes.None, Boolean isFederal = false, Boolean isState = false, Boolean isTwentyFour = true)
        {
            this.Date = effectiveMonth;
            this.ClockTypes = TimelineMonth.GetClockTypes(timelimitType, isFederal, isState, isTwentyFour);
        }

        public static ClockTypes GetClockTypes(ClockTypes timelimitType, Boolean isFederal, Boolean isState, Boolean isTwentyFour)
        {
            timelimitType = isFederal ? timelimitType.CombineFlags(ClockTypes.Federal): timelimitType.RemoveFlags(ClockTypes.Federal);
            timelimitType = isState ? timelimitType.CombineFlags(ClockTypes.State): timelimitType.RemoveFlags(ClockTypes.State);
            timelimitType = timelimitType.CombineFlags(!isTwentyFour && timelimitType.HasAnyFlags(ClockTypes.PlacementLimit) ? ClockTypes.NoPlacementLimit : ClockTypes.None);
            return timelimitType;
        }

        public Boolean IsCurrentMonth => this.Date.IsSameOrAfter(ApplicationContext.Current.Date, DateTimeUnit.Month);

        public Boolean IsFuture => this.Date.IsSameOrAfter(ApplicationContext.Current.Date, DateTimeUnit.Month);

    }
}