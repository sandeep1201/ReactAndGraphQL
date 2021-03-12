using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Extensions
{
	public static class BoolExtensions
	{
		public static string ToYesNo(this bool? input)
		{
			switch (input)	
			{
				case null:
					return null;
				case true:
					return "Yes";
				case false:
					return "No";
				default:
					return String.Empty;
			}
		}

        /// <summary>
        ///  Returns a boolean as "Y", "N" or empty string. Example True returns "Y"
        /// </summary>
        /// <param name="input"></param>
        /// <returns>"Y","N" or empty string</returns>
        public static string ToYn(this bool? input)
        {
            switch (input)
            {
                case null:
                    return null;
                case true:
                    return "Y";
                case false:
                    return "N";
                default:
                    return String.Empty;
            }
        }

        public static string ToYesNoNonNull(this bool input)
		{
			switch (input)
			{
				case true:
					return "Yes";
				case false:
					return "No";
				default:
					return String.Empty;
			}
		}

		public static bool? ToBool(this string val)
        {
            if (String.IsNullOrWhiteSpace(val))
                return null;

            if (val.ToLower().StartsWith("y"))
                return true;

            return false;
        }

	    public static bool? ToBoolFromCheckBox(this int?[] val)
	    {
		    if (val == null)
			    return null;

			if (val.Length > 0)
	            return Convert.ToBoolean(val[0]);

            return false;
	    }

	}
}
