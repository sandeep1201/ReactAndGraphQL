using System;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class DateTimeExtensionsClass
    {
        #region General Date/Times Extensions

        //public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static DateTime ConvertFromUnixTimestamp(this DateTime instance, double timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(this DateTime instance)
        {
            var diff = instance - UnixEpoch;
            return Math.Floor(diff.TotalSeconds);
        }

        public static double ConvertToUnixTimestamp(this DateTime instance, DateTime dateTime)
        {
            var diff = dateTime - UnixEpoch;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime FromUnixTime(this DateTime instance, long unixTime)
        {
            return (UnixEpoch + TimeSpan.FromSeconds(unixTime));
        }

        public static long ToUnixTime(this DateTime instance)
        {
            var ts = (instance - UnixEpoch);
            return (Convert.ToInt64(ts.TotalSeconds));
        }

        public static long ToUnixTime(this DateTime instance, DateTime value)
        {
            var ts = (value - UnixEpoch);
            return (Convert.ToInt64(ts.TotalSeconds));
        }

        public static bool IsBetween(this DateTime instance, DateTime startDate, DateTime endDate)
        {
            return ((startDate <= instance) && (instance <= endDate));
        }

        public static DateTime Previous(this DateTime instance, DayOfWeek dayOfWeek)
        {
            var diff = instance.DayOfWeek - dayOfWeek;
            if (diff > 0)
                return (instance.Date.AddDays(-diff));
            else
                return (instance.Date.AddDays(-7 - diff));
        }

        public static DateTime Next(this DateTime value, DayOfWeek dayOfWeek)
        {
            var diff = dayOfWeek - value.DayOfWeek;
            if (diff <= 0)
                return (value.Date.AddDays(7 + diff));
            else
                return (value.Date.AddDays(diff));
        }

        public static DateTime FirstOfMonth(this DateTime value)
        {
            return (new DateTime(value.Year, value.Month, 1));
        }

        public static DateTime FirstOfMonth(this DateTime value, DayOfWeek dayOfWeek)
        {
            var result = value.FirstOfMonth();
            if (result.DayOfWeek == dayOfWeek)
                return (result);
            else
                return (result.Next(dayOfWeek));
        }

        public static DateTime LastOfMonth(this DateTime value)
        {
            return (new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month)));
        }

        public static DateTime LastOfMonth(this DateTime value, DayOfWeek dayOfWeek)
        {
            DateTime dateTime = value.LastOfMonth();

            if (dateTime.DayOfWeek == dayOfWeek)
                return (dateTime);
            else
                return (dateTime.Previous(dayOfWeek));
            // add fix to return date as 23:59:59... currently returns 12:00:00am which is wrong
        }

        public static int Quarter(this DateTime value)
        {
            int qtr;

            switch (value.Month)
            {
                case 1:
                    qtr = 1;
                    break;
                case 2:
                    qtr = 1;
                    break;
                case 3:
                    qtr = 1;
                    break;
                case 4:
                    qtr = 2;
                    break;
                case 5:
                    qtr = 2;
                    break;
                case 6:
                    qtr = 2;
                    break;
                case 7:
                    qtr = 3;
                    break;
                case 8:
                    qtr = 3;
                    break;
                case 9:
                    qtr = 3;
                    break;
                case 10:
                    qtr = 4;
                    break;
                case 11:
                    qtr = 4;
                    break;
                case 12:
                    qtr = 4;
                    break;
                default:
                    qtr = -1;
                    break;
            }
            return (qtr);
        }

        public static DateTime ToDeloitteDate(this DateTime instance)
        {
            // this is a custom function to accomodate deloitte's required format for web services.
            var deloitteDate = new DateTime(instance.Year, instance.Month, instance.Day);

            return (deloitteDate);
        }

        #endregion

        #region TimeFrames

        /*
         * Some of these are obvious and redundant (like CurrentMonthStartDate() bcs)
         * you could have easily figured it out in your code, but they are included
         * for consistency across time frames Hourly, Daily, Weekly, Monthly, Quarterly, Yearly...
         */

        #region Hourly

        public static DateTime PreviousHourStart(this DateTime value)
        {
            var dt = value.AddHours(-1);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
            return (dt);
        }

        public static DateTime PreviousHourEnd(this DateTime value)
        {
            var dt = value.AddHours(-1);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 59, 59);
            return (dt);
        }

        public static DateTime PreviousHourStartWithDelay(this DateTime value, int delay)
        {
            // get the current top of the hour, then subtract delay mins
            var dtCurrentHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var dtFrom = dtCurrentHour.AddMinutes(-delay);

            // can also acccomplish it this way...
            //var tsDelay       = new TimeSpan(0,delay,0);
            //var dtFrom2       = dtCurrentHour.Subtract(tsDelay);

            return (dtFrom);
        }

        public static DateTime PreviousHourEndWithDelay(this DateTime value, int delay)
        {
            var dtCurrentHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var dtTo = dtCurrentHour.AddMinutes(-delay);

            return (dtTo);
        }

        public static DateTime CurrentHourStart(this DateTime value)
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

            return (dt);
        }

        public static DateTime CurrentHourEnd(this DateTime value)
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 59, 59);

            return (dt);
        }

        public static DateTime HourStart(this DateTime value)
        {
            var dt = new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0);

            return (dt);
        }

        public static DateTime HourEnd(this DateTime value)
        {
            var dt = new DateTime(value.Year, value.Month, value.Day, value.Hour, 59, 59);

            return (dt);
        }

        #endregion

        #region Daily

        public static DateTime PreviousDayStart(this DateTime value)
        {
            var dt = new DateTime(value.Year, value.Month, value.Day - 1, 0, 0, 0);
            return (dt);
        }

        public static DateTime PreviousDayStart(this DateTime value, string dateValue)
        {
            DateTime dtTemp;
            DateTime.TryParse(dateValue, out dtTemp);
            var dt = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day - 1, 0, 0, 0);
            return (dt);
        }

        public static DateTime PreviousDayEnd(this DateTime value)
        {
            var dt = value.AddDays(-1);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return (dt);
        }

        public static DateTime PreviousDayEnd(this DateTime value, string dateValue)
        {
            DateTime dtTemp;
            DateTime.TryParse(dateValue, out dtTemp);
            var dt = dtTemp.AddDays(-1);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return (dt);
        }

        public static DateTime CurrentDayStart(this DateTime value)
        {
            return (DateTime.Today);
        }

        public static DateTime CurrentDayEnd(this DateTime value)
        {
            var dt = DateTime.Now;
            return (new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59));
        }

        #endregion

        #region Weekly

        /// <summary>
        /// Returns the Start Date of the Previous Week RELATIVE to populated date
        /// </summary>
        /// <param name="value">none</param>
        /// <returns>DateTime</returns>
        public static DateTime PreviousWeekStart(this DateTime value)
        {
            var dtTemp = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);

            if (dtTemp.DayOfWeek != DayOfWeek.Sunday)
            {
                dtTemp = value.Previous(DayOfWeek.Sunday);
            }

            dtTemp = dtTemp.Previous(DayOfWeek.Sunday);

            return (dtTemp);
        }

        public static DateTime PreviousWeekEnd(this DateTime value)
        {
            var dtTemp = value.Previous(DayOfWeek.Saturday);
            dtTemp = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day, 23, 59, 59);
            return (dtTemp);
        }

        public static DateTime CurrentWeekStart(this DateTime value)
        {
            var dtTemp = DateTime.Now;
            if (dtTemp.DayOfWeek == DayOfWeek.Sunday)
            {
                dtTemp = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day);
            }
            else
            {
                dtTemp = DateTime.Now.Previous(DayOfWeek.Sunday);
                dtTemp = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day);
            }
            return (dtTemp);
        }

        public static DateTime CurrentWeekEnd(this DateTime value)
        {
            var dt = DateTime.Now;
            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            }
            else
            {
                dt = DateTime.Now.Next(DayOfWeek.Saturday);
                dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            }
            return (dt);
        }

        #endregion

        #region Monthly
/*
        public static DateTime PreviousMonthStart(this DateTime value)
        {
            // if (value) //TODO: fix to initialize that if value is 1/1/1 then initalize to DateTime.Now
            var month = (Month)value.Month;
            var year = value.Year;

            var prevMonth = --month;
            if (prevMonth < Month.January)
            {
                month = Month.December;
                year = --year;
            }

            var dt = new DateTime(year, (int)month, 1);
            return (dt);
        }

        public static DateTime PreviousMonthStart(this DateTime value, string dateValue)
        {
            DateTime dtTemp;
            DateTime.TryParse(dateValue, out dtTemp);

            var month = (Month)dtTemp.Month;
            var year = dtTemp.Year;

            var prevMonth = --month;
            if (prevMonth < Month.January)
            {
                month = Month.December;
                year = --year;
            }

            var dt = new DateTime(year, (int)month, 1);
            return (dt);
        }

        public static DateTime PreviousMonthEnd(this DateTime value)
        {
            var month = (Month)value.Month;
            var year = value.Year;

            var prevMonth = --month;
            if (prevMonth < Month.January)
            {
                month = Month.December;
                year = --year;
            }

            var dtTemp = new DateTime(year, (int)month, 1).LastOfMonth();
            var dt = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day, 23, 59, 59);
            return (dt);
        }

        public static DateTime PreviousMonthEnd(this DateTime value, string dateValue)
        {
            DateTime dtTemp1;
            DateTime.TryParse(dateValue, out dtTemp1);

            var month = (Month)dtTemp1.Month;
            var year = dtTemp1.Year;

            var prevMonth = --month;
            if (prevMonth < Month.January)
            {
                month = Month.December;
                year = --year;
            }
            var dtTemp = new DateTime(year, (int)month, 1).LastOfMonth();
            var dt = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day, 23, 59, 59);
            return (dt);
        }

        public static DateTime CurrentMonthStart(this DateTime value)
        {
            var dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return (dt);
        }

        public static DateTime CurrentMonthEnd(this DateTime value)
        {
            //var ddt = DateTime.Now.LastOfMonth();
            var ddt = value.LastOfMonth();
            var dt = new DateTime(ddt.Year, ddt.Month, ddt.Day, 23, 59, 59);
            return (dt);
        }

        public static DateTime NextMonthStart(this DateTime value)
        {
            // if (value) //TODO: fix to initialize that if value is 1/1/1 then initalize to DateTime.Now
            var month = (Month)value.Month;
            var year = value.Year;

            var nextMonth = ++month;
            if (nextMonth > Month.December)
            {
                month = Month.January;
                year = ++year;
            }

            var dt = new DateTime(year, (int)month, 1);
            return (dt);
        }
*/
        #endregion

        #region Quarterly

        public static DateTime PreviousQuarterStart(this DateTime value)
        {
            DateTime dt;
            var year = value.Year;
            var qtr = value.Quarter() - 1;

            if (qtr < 1)
            {
                // we're in the first quater, then the previous one was 4th of last year, duh
                qtr = 4;
                year--;
            }

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 1, 1);
                    break;
                case 2:
                    dt = new DateTime(year, 4, 1);
                    break;
                case 3:
                    dt = new DateTime(year, 7, 1);
                    break;
                case 4:
                    dt = new DateTime(year, 10, 1);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime PreviousQuarterStart(this DateTime value, string dateValue)
        {
            DateTime dt;
            DateTime.TryParse(dateValue, out dt);
            var year = dt.Year;
            var qtr = dt.Quarter() - 1;

            if (qtr < 1)
            {
                qtr = 4;
                year--;
            }

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 1, 1);
                    break;
                case 2:
                    dt = new DateTime(year, 4, 1);
                    break;
                case 3:
                    dt = new DateTime(year, 7, 1);
                    break;
                case 4:
                    dt = new DateTime(year, 10, 1);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime PreviousQuarterEnd(this DateTime value)
        {
            DateTime dt;
            var year = value.Year;
            var qtr = value.Quarter() - 1;

            if (qtr < 1)
            {
                // we're in the first quater, then the previous one was 4th of last year, duh
                qtr = 4;
                year--;
            }

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 3, 31, 23, 59, 59);
                    break;
                case 2:
                    dt = new DateTime(year, 6, 30, 23, 59, 59);
                    break;
                case 3:
                    dt = new DateTime(year, 9, 30, 23, 59, 59);
                    break;
                case 4:
                    dt = new DateTime(year, 12, 31, 23, 59, 59);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime PreviousQuarterEnd(this DateTime value, string dateValue)
        {
            DateTime dt;
            DateTime.TryParse(dateValue, out dt);
            var year = dt.Year;
            var qtr = dt.Quarter() - 1;

            if (qtr < 1)
            {
                qtr = 4;
                year--;
            }

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 3, 31, 23, 59, 59);
                    break;
                case 2:
                    dt = new DateTime(year, 6, 30, 23, 59, 59);
                    break;
                case 3:
                    dt = new DateTime(year, 9, 30, 23, 59, 59);
                    break;
                case 4:
                    dt = new DateTime(year, 12, 31, 23, 59, 59);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime CurrentQuarterStart(this DateTime value)
        {
            DateTime dt;
            var year = value.Year;
            var qtr = value.Quarter();

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 1, 1);
                    break;
                case 2:
                    dt = new DateTime(year, 4, 1);
                    break;
                case 3:
                    dt = new DateTime(year, 7, 1);
                    break;
                case 4:
                    dt = new DateTime(year, 10, 1);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime CurrentQuarterStart(this DateTime value, string dateValue)
        {
            DateTime dt;
            DateTime.TryParse(dateValue, out dt);
            var year = dt.Year;
            var qtr = dt.Quarter();

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 1, 1);
                    break;
                case 2:
                    dt = new DateTime(year, 4, 1);
                    break;
                case 3:
                    dt = new DateTime(year, 7, 1);
                    break;
                case 4:
                    dt = new DateTime(year, 10, 1);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime CurrentQuarterEnd(this DateTime value)
        {
            DateTime dt;
            var year = value.Year;
            var qtr = value.Quarter();

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 3, 31, 23, 59, 59);
                    break;
                case 2:
                    dt = new DateTime(year, 6, 30, 23, 59, 59);
                    break;
                case 3:
                    dt = new DateTime(year, 9, 30, 23, 59, 59);
                    break;
                case 4:
                    dt = new DateTime(year, 12, 31, 23, 59, 59);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        public static DateTime CurrentQuarterEnd(this DateTime value, string dateValue)
        {
            DateTime dt;
            DateTime.TryParse(dateValue, out dt);
            var year = value.Year;
            var qtr = value.Quarter();

            switch (qtr)
            {
                case 1:
                    dt = new DateTime(year, 3, 31, 23, 59, 59);
                    break;
                case 2:
                    dt = new DateTime(year, 6, 30, 23, 59, 59);
                    break;
                case 3:
                    dt = new DateTime(year, 9, 30, 23, 59, 59);
                    break;
                case 4:
                    dt = new DateTime(year, 12, 31, 23, 59, 59);
                    break;
                default:
                    dt = new DateTime();
                    break;
            }
            return (dt);
        }

        #endregion

        #region Yearly

        public static DateTime PreviousYearStart(this DateTime value)
        {
            return (new DateTime((value.Year - 1), 1, 1));
        }

        public static DateTime PreviousYearStart(this DateTime value, string dateValue)
        {
            DateTime dt;
            DateTime.TryParse(dateValue, out dt);
            return (new DateTime((dt.Year - 1), 1, 1));
        }

        public static DateTime PreviousYearEnd(this DateTime value)
        {
            return (new DateTime((value.Year - 1), 12, 31, 23, 59, 59));
        }

        public static DateTime PreviousYearEnd(this DateTime value, string dateValue)
        {
            DateTime dt;
            DateTime.TryParse(dateValue, out dt);
            return (new DateTime((dt.Year - 1), 12, 31, 23, 59, 59));
        }

        public static DateTime CurrentYearStart(this DateTime value)
        {
            return (new DateTime((DateTime.Now.Year), 1, 1));
        }

        public static DateTime CurrentYearEnd(this DateTime value)
        {
            return (new DateTime((value.Year), 12, 31, 23, 59, 59));
        }

        #endregion

        #region Day-based computed dates

        #region 7 days-based

        public static DateTime PreviousSevenDaysStart(this DateTime value)
        {
            var dt = DateTime.Now.AddDays(-6);
            dt = new DateTime(dt.Year, dt.Month, dt.Day);

            return (dt);
        }

        public static DateTime PreviouSevensDaysEnd(this DateTime value)
        {
            var dt = DateTime.Now;
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);

            return (dt);
        }

        #endregion

        #region 30 days-based

        public static DateTime PreviousThirtyDaysStart(this DateTime value)
        {
            var dt = DateTime.Now.AddDays(-30);
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            return (dt);
        }

        public static DateTime PreviouThirtysDaysEnd(this DateTime value)
        {
            var dt = DateTime.Now;
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return (dt);
        }

        #endregion

        #endregion

        #endregion
    }

}
