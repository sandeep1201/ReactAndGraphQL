using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.ViewModel.Extensions
{
    public static class Strings
    {
		public static String JsonEscape(this String s)
		{
			if (s == null)
				return null;

			if (String.IsNullOrEmpty(s))
				return String.Empty;

			char c = '\0';
			int i;
			int len = s.Length;
			var sb = new StringBuilder(len + 4);
			String t;

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
							t = "000" + String.Format("X", c);
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

		public static String SafeTrim(this String s)
		{
			if (s == null)
				return null;

			return s.Trim();
		}

		public static String AddDollarSign(this String s)
		{
			if (s == null)
				return null;
			var price = "$" + s;

			return price;
		}

		public static String NullStringToBlank(this String s)
		{
			if (s == null)
				return "";

			return s;
		}
	}
}
