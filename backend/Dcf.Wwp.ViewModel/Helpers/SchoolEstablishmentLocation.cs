using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Extensions;


namespace Dcf.Wwp.Api.Library.Helpers
{
    public static class SchoolEstablishmentLocation
    {

	    public static string OutputFormattedForeignOrDomesticLocation(string cityName, string stateName, string countryName)
	    {
		    string x = null;

		    cityName = cityName.SafeTrim();
		    stateName = stateName.SafeTrim();
		    countryName = countryName.SafeTrim();

			if (cityName != null && stateName != null && countryName != null)
			{
				x = cityName + ", " + stateName + ", " + countryName;

			    if (cityName == String.Empty)
			    {

					x = String.Empty;
				}
		    }
		    else if (cityName != null && countryName != null)
		    {
			    x = cityName + ", " + countryName;
		    }
		
		    return x;
	    }

		public static List<String> SeparateForeignOrDomesticLocation(string location)
		{
			if (location == null)
				return null;
			// School Location string broken down into city state country.
			var commaNumber = new List<int>();

			// Check to see if the country contains a state.
			int i = 0;
			if (location != null)
				while ((i = location.IndexOf(',', i)) != -1)
				{
					commaNumber.Add(1);
					// Increment the index.
					i++;
				}

			string cityName;
			string stateName;
			string countryName;

		

			if (commaNumber.Count == 2)
			{
				cityName = location.ToCity().SafeTrim();
				stateName = location.ToState().SafeTrim();
				countryName = location.ToCountry().SafeTrim();
				var loc = new List<String>() { cityName, stateName, countryName };

				return loc;
			}
			else
			{
				cityName = location.ToCity().SafeTrim();
				countryName = location.ToCountryShort().SafeTrim();
				var loc = new List<String>() { cityName, countryName };

				return loc;
			}

		}
	}
}
