using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Helpers
{
    public static class PrettyLocation
    {
        public static string FromCity(ICity city)
        {
            if (city == null)
                return string.Empty;

            var countryName = city.Country?.Name.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(city.Name))
            {
                if (!string.IsNullOrEmpty(city.State?.Name))
                {
                    if (!string.IsNullOrEmpty(countryName) && countryName.ToUpper() != "UNITED STATES" && countryName.ToUpper() != "USA")
                        return $"{city.Name}, {city.State.Name}, {countryName}";

                    // otherwise just use the City & State (most likely for US)
                    var stateDisplay = string.IsNullOrWhiteSpace(city.State.Code) ? city.State.Name : city.State.Code;
                    return $"{city.Name}, {stateDisplay}";
                }

                if (!string.IsNullOrEmpty(countryName))
                {
                    return $"{city.Name}, {countryName}";
                }

                // As a fallback, just return the city.
                return city.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(city.State?.Name))
                {
                    if (!string.IsNullOrEmpty(countryName) && countryName.Trim().ToUpper() != "UNITED STATES" && countryName.ToUpper() != "USA")
                        return $"{city.State.Name}, {countryName}";

                    // otherwise just use the City & State (most likely for US)
                    return $"{city.State.Name}";
                }

                if (!string.IsNullOrEmpty(countryName))
                {
                    return $"{countryName}";
                }
            }

            return string.Empty;
        }
    }
}
