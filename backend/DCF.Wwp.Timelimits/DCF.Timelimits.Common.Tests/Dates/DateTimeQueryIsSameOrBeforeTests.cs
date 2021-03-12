using System;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCF.Timelimits.Common.Tests
{
    [TestClass]
    public class DateTimeQueryIsSameOrBeforeTests
    {
        #region IsSameOrBefore
        [TestMethod]
        [Description("DateTime IsSameOrBefore without units")]
        public void datetime_is_same_or_before_without_unit()
        {
            var dateTime1 = new DateTime(2017, 3, 2, 3, 4, 5, 10);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2018, 3, 2, 3, 5, 5, 10)), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2016, 3, 2, 3, 3, 5, 10)), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 4, 2, 3, 4, 5, 10)), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 2, 2, 3, 4, 5, 10)), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 3, 3, 4, 5, 10)), true, "day is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 1, 3, 4, 5, 10)), false, "day is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 4, 4, 5, 10)), true, "hour is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 2, 4, 5, 10)), false, "hour is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 5, 5, 10)), true, "minute is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 3, 5, 10)), false, "minute is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 4, 6, 10)), true, "second is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 4, 4, 11)), false, "second is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 4, 5, 10)), true, "millisecond match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 4, 5, 11)), true, "millisecond is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 3, 2, 3, 4, 5, 9)), false, "millisecond is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1), true, "DateTimes are same or before themselves");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore second should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore before year")]
        public void datetime_is_same_or_before_year()
        {
            var dateTime1 = new DateTime(2017, 1, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 5, 6, 7, 8, 9, 10), DateTimeUnit.Year), true, "year match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2019, 5, 6, 7, 8, 9, 10), DateTimeUnit.Year), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2016, 5, 6, 7, 8, 9, 10), DateTimeUnit.Year), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 1, 1, 1, 0, 0, 0).StartOf(DateTimeUnit.Year), DateTimeUnit.Year), true, "exact start of year");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2017, 11, 30, 23, 59, 59, 999).EndOf(DateTimeUnit.Year), DateTimeUnit.Year), true, "exact end of year");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2018, 1, 1, 1, 0, 0, 0).StartOf(DateTimeUnit.Year), DateTimeUnit.Year), true, "start of next year");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2016, 11, 30, 23, 59, 59, 999).EndOf(DateTimeUnit.Year), DateTimeUnit.Year), false, "end of previous year");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(1980, 11, 30, 23, 59, 59, 999).EndOf(DateTimeUnit.Year), DateTimeUnit.Year), false, "end of year far before");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Year), true, "same datetimes are the same year");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore year should not change moment");

        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore before month")]
        public void datetime_is_same_or_before_month()
        {
            var dateTime1 = new DateTime(2011, 2, 3, 4, 5, 6, 7);

            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 6, 7, 8, 9, 10), DateTimeUnit.Month), true, "month match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2012, 2, 6, 7, 8, 9, 10), DateTimeUnit.Month), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 2, 6, 7, 8, 9, 10), DateTimeUnit.Month), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 5, 6, 7, 8, 9, 10), DateTimeUnit.Month), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 1, 6, 7, 8, 9, 10), DateTimeUnit.Month), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 1, 0, 0, 0, 0), DateTimeUnit.Month), true, "exact start of month");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 28, 23, 59, 59, 999), DateTimeUnit.Month), true, "exact end of month");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 0, 0, 0, 0), DateTimeUnit.Month), true, "start of next month");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 1, 27, 23, 59, 59, 999), DateTimeUnit.Month), false, "end of previous month");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 12, 31, 23, 59, 59, 999), DateTimeUnit.Month), false, "later month but earlier year");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Month), true, "same datetimes are the same month");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore month should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore before day")]
        public void datetime_is_same_or_before_day()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 7, 8, 9, 10), DateTimeUnit.Day), true, "day match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2012, 3, 2, 7, 8, 9, 10), DateTimeUnit.Day), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 3, 2, 7, 8, 9, 10), DateTimeUnit.Day), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 4, 2, 7, 8, 9, 10), DateTimeUnit.Day), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 2, 7, 8, 9, 10), DateTimeUnit.Day), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 3, 7, 8, 9, 10), DateTimeUnit.Day), true, "day is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 7, 8, 9, 10), DateTimeUnit.Day), false, "day is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 0, 0, 0, 0), DateTimeUnit.Day), true, "exact start of day");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 23, 59, 59, 999), DateTimeUnit.Day), true, "exact end of day");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 3, 0, 0, 0, 0), DateTimeUnit.Day), true, "start of next day");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 23, 59, 59, 999), DateTimeUnit.Day), false, "end of previous day");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 3, 10, 0, 0, 0, 0), DateTimeUnit.Day), false, "later day but earlier year");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Day), true, "same datetimes are the same day");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore day should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore before year")]
        public void datetime_is_same_or_before_hour()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), true, "hour match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2012, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 4, 2, 3, 8, 9, 10), DateTimeUnit.Hour), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 1, 2, 3, 8, 9, 10), DateTimeUnit.Hour), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 3, 3, 8, 9, 10), DateTimeUnit.Hour), true, "day is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 3, 8, 9, 10), DateTimeUnit.Hour), false, "day is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 4, 8, 9, 10), DateTimeUnit.Hour), true, "hour is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 2, 8, 9, 10), DateTimeUnit.Hour), false, "hour is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 0, 0, 0), DateTimeUnit.Hour), true, "exact start of hour");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 59, 59, 999), DateTimeUnit.Hour), true, "exact end of hour");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 4, 0, 0, 0), DateTimeUnit.Hour), true, "start of next hour");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 2, 59, 59, 999), DateTimeUnit.Hour), false, "end of previous hour");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Hour), true, "same datetimes are the same hour");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore hour should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore before year")]
        public void datetime_is_same_or_before_minute()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 9, 10), DateTimeUnit.Minute), true, "minute match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2012, 3, 2, 3, 4, 9, 10), DateTimeUnit.Minute), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 3, 2, 3, 4, 9, 10), DateTimeUnit.Minute), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 4, 2, 3, 4, 9, 10), DateTimeUnit.Minute), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 2, 3, 4, 9, 10), DateTimeUnit.Minute), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 3, 3, 4, 9, 10), DateTimeUnit.Minute), true, "day is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 3, 4, 9, 10), DateTimeUnit.Minute), false, "day is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 4, 4, 9, 10), DateTimeUnit.Minute), true, "hour is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 2, 4, 9, 10), DateTimeUnit.Minute), false, "hour is earler");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 5, 9, 10), DateTimeUnit.Minute), true, "minute is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 3, 9, 10), DateTimeUnit.Minute), false, "minute is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 0, 0), DateTimeUnit.Minute), true, "exact start of minute");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 59, 999), DateTimeUnit.Minute), true, "exact end of minute");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 5, 0, 0), DateTimeUnit.Minute), true, "start of next minute");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 3, 59, 999), DateTimeUnit.Minute), false, "end of previous minute");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Minute), true, "same datetimes are the same minute");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore minute should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore is before second")]
        public void datetime_is_same_or_before_second()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 10);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 5, 10), DateTimeUnit.Second), true, "second match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2012, 3, 2, 3, 4, 5, 10), DateTimeUnit.Second), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 3, 2, 3, 4, 5, 10), DateTimeUnit.Second), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 4, 2, 3, 4, 5, 10), DateTimeUnit.Second), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 2, 3, 4, 5, 10), DateTimeUnit.Second), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 3, 3, 4, 5, 10), DateTimeUnit.Second), true, "day is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 1, 4, 5, 10), DateTimeUnit.Second), false, "day is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 4, 4, 5, 10), DateTimeUnit.Second), true, "hour is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 4, 1, 5, 10), DateTimeUnit.Second), false, "hour is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 5, 5, 10), DateTimeUnit.Second), true, "minute is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 3, 5, 10), DateTimeUnit.Second), false, "minute is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 6, 10), DateTimeUnit.Second), true, "second is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 4, 5), DateTimeUnit.Second), false, "second is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 5, 0), DateTimeUnit.Second), true, "exact start of second");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 5, 999), DateTimeUnit.Second), true, "exact end of second");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 6, 0), DateTimeUnit.Second), true, "start of next second");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 4, 999), DateTimeUnit.Second), false, "end of previous second");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Second), true, "same datetimes are the same second");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore second should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsSameOrBefore before millisecond")]
        public void datetime_is_same_or_before_millisecond()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 10);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), true, "millisecond match");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2012, 3, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), true, "year is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2010, 3, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), false, "year is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 4, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), true, "month is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 2, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), false, "month is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 3, 3, 4, 5, 10), DateTimeUnit.Millisecond), true, "day is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 1, 4, 5, 10), DateTimeUnit.Millisecond), false, "day is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 4, 4, 5, 10), DateTimeUnit.Millisecond), true, "hour is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 1, 4, 1, 5, 10), DateTimeUnit.Millisecond), false, "hour is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 5, 5, 10), DateTimeUnit.Millisecond), true, "minute is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 3, 5, 10), DateTimeUnit.Millisecond), false, "minute is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 6, 10), DateTimeUnit.Millisecond), true, "second is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 4, 5), DateTimeUnit.Millisecond), false, "second is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 6, 11), DateTimeUnit.Millisecond), true, "millisecond is later");
            Assert.AreEqual(dateTime1.IsSameOrBefore(new DateTime(2011, 3, 2, 3, 4, 4, 9), DateTimeUnit.Millisecond), false, "millisecond is earlier");
            Assert.AreEqual(dateTime1.IsSameOrBefore(dateTime1, DateTimeUnit.Millisecond), true, "same datetimes are the same millisecond");
            Assert.AreEqual(dateTime1, dateTime2, "IsSameOrBefore millisecond should not change moment");
        }

        #endregion
    }
}