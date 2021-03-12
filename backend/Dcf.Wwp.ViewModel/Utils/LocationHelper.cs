using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Helpers;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Utils
{
    public static class LocationHelper
    {
        public static LocationContract GetLocationInfo(object obj, ICity city)
        {
            var location = new LocationContract();
            var sourceType = obj.GetType();
            var properties = sourceType.GetProperties();


            var streetAddress = (from s in properties
                                 where s.Name.Equals("StreetAddress")
                                 select s.GetValue(obj, null)).FirstOrDefault();

            var streetAddress2 = properties.Where(i => i.Name.Equals("StreetAddress"))
                                           .Select(i => i.GetValue(obj, null))
                                           .FirstOrDefault();


            var zipAddress = (from s in properties
                              where s.Name.Equals("ZipAddress")
                              select s.GetValue(obj, null)).FirstOrDefault();

            if (city != null)
            {
                if (city.State != null)
                {
                    location.Description = PrettyLocation.FromCity(city);
                    location.City = city?.Name;
                    location.State = city?.State?.Code;
                    location.Country = city?.State?.Country?.Name;
                    location.GooglePlaceId = city?.GooglePlaceId;
                    location.FullAddress = streetAddress?.ToString();
                    location.ZipAddress = zipAddress?.ToString();
                }
                else
                {
                    location.Description = PrettyLocation.FromCity(city);
                    location.City = city?.Name;
                    location.Country = city?.Country?.Name;
                    location.GooglePlaceId = city?.GooglePlaceId;
                    location.FullAddress = streetAddress?.ToString().SafeTrim();
                    location.ZipAddress = zipAddress?.ToString();
                }
            }
            else
            {
                location.FullAddress = streetAddress?.ToString().SafeTrim();
                location.ZipAddress = zipAddress?.ToString();
            }

            return location;
        }

        private static bool CheckType<T>(object obj)
        {
            return obj is T;
        }
    }
}
