using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class LocationExtensions
    {
        public static string ToCity(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            var city = input.Split(',')[0];

	        city = city.SafeTrim();

			return city;
        }

        public static string ToState(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            var state = input.Split(',')[1];
			state = state.SafeTrim();
	        return state;
        }

        public static string ToCountry(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            var country = input.Split(',')[2];
			country = country.SafeTrim();
	        return country;
        }

		public static string RemoveStateIfNone(this string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return null;

			var state = input.Replace("None", "");
			return state;
		}

		public static string ToCountryShort(this string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return null;

			var country = input.Split(',')[1];
			country = country.SafeTrim();
			return country;
		}

		public static string ToVerboseLocation(string city, string state, string country)
        {

            var x = city + ", " + state + ", " +  country;

            return x;
        }
    }
}
