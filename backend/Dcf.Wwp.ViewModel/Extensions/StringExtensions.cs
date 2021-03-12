using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class StringExtensions
    {
        public static string JsonEscape(this string s)
        {
            if (s == null)
                return null;

            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char   c = '\0';
            int    i;
            int    len = s.Length;
            var    sb  = new StringBuilder(len + 4);
            string t;

            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                switch (c)
                {
                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '/':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            t = "000" + string.Format("X", c);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        public static int? GetMonthInt(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            try
            {
                var mm  = input.Split('/')[0];
                int mmp = Int32.Parse(mm);

                return mmp;
            }
            catch
            {
                return null;
            }
        }

        public static decimal? ToDecimal(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            var deci = System.Convert.ToDecimal(input);
            return deci;
        }

        public static int ToInt(this string input, int defaultVal = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
                return defaultVal;

            int val;

            if (!int.TryParse(input, out val))
                val = defaultVal;

            return val;
        }

        public static short ToShort(this string input, short defaultVal = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
                return defaultVal;

            short val;

            if (!short.TryParse(input, out val))
                val = defaultVal;

            return val;
        }

        public static string SafeTrim(this string s)
        {
            return s?.Trim();
        }

        public static string AddDollarSign(this string s)
        {
            if (s == null)
                return null;

            var price = "$" + s;

            return price;
        }

        public static string NullStringToBlank(this string s)
        {
            return s ?? string.Empty;
        }

        public static string ToTitleCase(this string s)
        {
            var r = string.Empty;

            if (!string.IsNullOrWhiteSpace(s))
            {
                var ci = Thread.CurrentThread.CurrentCulture;
                var t  = ci.TextInfo;
                r = t.ToTitleCase(s.Trim().ToLower());
            }

            return (r);
        }

        public static string UnKabob(this string s)
        {
            return s.IsNullOrWhiteSpace() ? string.Empty : s.Replace("-", " ");
        }

        public static string TrimAndUpper(this string s)
        {
            return s?.Trim().ToUpper();
        }

        public static string TrimAndLower(this string s)
        {
            return s?.Trim().ToLower();
        }

        public static List<string> SplitStringToDate(this string s, short year)
        {
            var splitPeriod = s.Split('-');
            var beginDate   = DateTime.Parse($"{splitPeriod[0].Trim()}, {year}");
            var endDate     = DateTime.Parse($"{splitPeriod[1].Trim()}, {year}");

            beginDate = beginDate.Month == 12 ? beginDate.AddYears(-1) : beginDate;

            var dates = new List<string> { beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd") };

            return dates;

        }

        public static string Replace(this string s, List<string> replaceStrings, string newString)
        {
            replaceStrings.ForEach(i => s = s.Replace(i, newString));

            return s;
        }
    }
}
