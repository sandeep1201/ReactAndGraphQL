using System;
using System.Collections.Generic;
using System.Data;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public static class Extensions
    {
        #region Properties

        #endregion

        #region Methods

        public static IEnumerable<DataRow> AsEnumerable(this DataTable table)
        {
            for (var i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this DateTime _value, long unixTime) => (UnixEpoch + TimeSpan.FromSeconds(unixTime));

        public static long ToUnixTime(this DateTime _value)
        {
            var ts = (_value.ToUniversalTime() - UnixEpoch);
            return (Convert.ToInt64(ts.TotalSeconds));
        }

        public static long ToUnixTime(this DateTime value, DateTime dateTime)
        {
            var ts = (dateTime.ToUniversalTime() - UnixEpoch);
            return (Convert.ToInt64(ts.TotalSeconds));
        }

        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long) (datetime - sTime).TotalSeconds;
        }


        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }

        #endregion
    }
}
