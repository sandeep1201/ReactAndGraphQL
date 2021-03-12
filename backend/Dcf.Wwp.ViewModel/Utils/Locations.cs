using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Api.Library.Domains
{
    /// <summary>
    /// Injectable port of LocationHelper.cs
    /// The idea is to replace the other with this one in tech debt.
    /// </summary>
    public class Locations : ILocations
    {
        #region Properties

        #endregion

        #region Methods

        public LocationContract GetLocationInfo(object obj, ICity city)
        {
            var location   = new LocationContract();
            var sourceType = obj.GetType();
            var properties = sourceType.GetProperties();

            var street = properties.Where(i => i.Name.Equals("StreetAddress"))
                                   .Select(i => i.GetValue(obj, null))
                                   .FirstOrDefault();

            //var zipCode = properties.Where(i => i.Name == "ZipAddress")
            var zipCode = properties.Where(i => i.Name == "ZipCode")
                                    .Select(i => i.GetValue(obj, null))
                                    .FirstOrDefault();

            if (city != null)
            {
                if (city.State != null)
                {
                    location.Description   = FromCity(city);
                    location.City          = city.Name;
                    location.State         = city.State?.Code;
                    location.Country       = city.State?.Country?.Name;
                    location.GooglePlaceId = city.GooglePlaceId;
                    location.FullAddress   = street?.ToString();
                    location.ZipAddress    = zipCode?.ToString();
                }
                else
                {
                    location.Description   = FromCity(city);
                    location.City          = city.Name;
                    location.Country       = city.Country?.Name;
                    location.GooglePlaceId = city.GooglePlaceId;
                    location.FullAddress   = street?.ToString().SafeTrim();
                    location.ZipAddress    = zipCode?.ToString();
                }
            }
            else
            {
                location.FullAddress = street?.ToString().SafeTrim();
                location.ZipAddress  = zipCode?.ToString();
            }

            return (location);
        }

        public FinalistAddressContract GetFinalistLocationInfo(IParticipantContactInfo info)
        {
            var lookupId = info.AddressVerificationTypeLookupId;
            var location = new FinalistAddressContract
                           {
                               AddressLine1        = info.AddressLine1,
                               City                = info.City?.Name,
                               State               = info.City?.State?.Code,
                               Zip                 = info.ZipCode,
                               UseSuggestedAddress = lookupId == AddressVerificationType.FinalistVerifiedId,
                               UseEnteredAddress   = lookupId == AddressVerificationType.WorkerOverrideId || lookupId == AddressVerificationType.FinalistUnverified
                           };


            return (location);
        }

        public FinalistAddressContract GetFinalistLocationInfo(IAlternateMailingAddress info)
        {
            var lookupId = info.AddressVerificationTypeLookupId;
            var location = new FinalistAddressContract
                           {
                               AddressLine1        = info.AddressLine1,
                               City                = info.City?.Name,
                               State               = info.City?.State?.Code,
                               Zip                 = info.ZipCode,
                               UseSuggestedAddress = lookupId == AddressVerificationType.FinalistVerifiedId,
                               UseEnteredAddress   = lookupId == AddressVerificationType.WorkerOverrideId || lookupId == AddressVerificationType.FinalistUnverified
                           };


            return (location);
        }

        public FinalistAddressContract GetFinalistLocationInfo(OrganizationLocation info)
        {
            var lookupId = info.AddressVerificationTypeLookupId;
            var location = new FinalistAddressContract
                           {
                               AddressLine1        = info.AddressLine1,
                               City                = info.City?.Name,
                               State               = info.City?.State?.Code,
                               Zip                 = info.ZipCode,
                               UseSuggestedAddress = lookupId == AddressVerificationType.FinalistVerifiedId,
                               UseEnteredAddress   = lookupId == AddressVerificationType.WorkerOverrideId || lookupId == AddressVerificationType.FinalistUnverified
                           };


            return (location);
        }

        public string FromCity(ICity city)
        {
            var r = string.Empty;

            if (city != null)
            {
                var countryName = city.Country?.Name.Trim() ?? string.Empty;

                if (!string.IsNullOrEmpty(city.Name))
                {
                    if (!string.IsNullOrEmpty(city.State?.Name))
                    {
                        if (!string.IsNullOrEmpty(countryName) && countryName.ToUpper() != "UNITED STATES" && countryName.ToUpper() != "USA")
                        {
                            r = $"{city.Name}, {city.State.Name}, {countryName}";
                        }
                        else
                        {
                            var stateValue = string.IsNullOrWhiteSpace(city.State.Code) ? city.State.Name : city.State.Code; // otherwise just use the City & State (most likely for US)
                            r              = $"{city.Name}, {stateValue}";
                        }
                    }
                    else
                    {
                        r = !string.IsNullOrEmpty(countryName) ? $"{city.Name}, {countryName}" : city.Name;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(city.State?.Name))
                    {
                        if (!string.IsNullOrEmpty(countryName) && countryName.Trim().ToUpper() != "UNITED STATES" && countryName.ToUpper() != "USA")
                        {
                            r = $"{city.State.Name}, {countryName}";
                        }
                        else
                        {
                            r = $"{city.State.Name}"; // otherwise just use the City & State (most likely for US)
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(countryName))
                        {
                            r = $"{countryName}";
                        }
                    }
                }
            }

            return (r);
        }

        #region Phase II

        public LocationContract GetLocationInfo(object obj, City city)
        {
            var location = new LocationContract();

            var sourceType = obj.GetType();
            var properties = sourceType.GetProperties();

            var street = properties.Where(i => i.Name.Equals("StreetAddress"))
                                   .Select(i => i.GetValue(obj, null))
                                   .FirstOrDefault();

            //var zipCode = properties.Where(i => i.Name == "ZipAddress")
            var zipCode = properties.Where(i => i.Name == "ZipCode" || i.Name == "ZipAddress")
                                    .Select(i => i.GetValue(obj, null))
                                    .FirstOrDefault();

            if (city != null)
            {
                if (city.State != null)
                {
                    location.Description   = FromCity(city);
                    location.City          = city.Name;
                    location.State         = city.State?.Code;
                    location.Country       = city.State?.Country?.Name;
                    location.GooglePlaceId = city.GooglePlaceId;
                    location.FullAddress   = street?.ToString();
                    location.ZipAddress    = zipCode?.ToString();
                }
                else
                {
                    location.Description   = FromCity(city);
                    location.City          = city.Name;
                    location.Country       = city.Country?.Name;
                    location.GooglePlaceId = city.GooglePlaceId;
                    location.FullAddress   = street?.ToString().SafeTrim();
                    location.ZipAddress    = zipCode?.ToString();
                }
            }
            else
            {
                location.FullAddress = street?.ToString().SafeTrim();
                location.ZipAddress  = zipCode?.ToString();
            }

            return (location);
        }

        public string FromCity(City city)
        {
            var r = string.Empty;

            if (city != null)
            {
                var countryName = city.Country?.Name.Trim() ?? string.Empty;

                if (!string.IsNullOrEmpty(city.Name))
                {
                    if (!string.IsNullOrEmpty(city.State?.Name))
                    {
                        if (!string.IsNullOrEmpty(countryName) && countryName.ToUpper() != "UNITED STATES" && countryName.ToUpper() != "USA")
                        {
                            r = $"{city.Name}, {city.State.Name}, {countryName}";
                        }
                        else
                        {
                            var stateValue = string.IsNullOrWhiteSpace(city.State.Code) ? city.State.Name : city.State.Code; // otherwise just use the City & State (most likely for US)
                            r              = $"{city.Name}, {stateValue}";
                        }
                    }
                    else
                    {
                        r = !string.IsNullOrEmpty(countryName) ? $"{city.Name}, {countryName}" : city.Name;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(city.State?.Name))
                    {
                        if (!string.IsNullOrEmpty(countryName) && countryName.Trim().ToUpper() != "UNITED STATES" && countryName.ToUpper() != "USA")
                        {
                            r = $"{city.State.Name}, {countryName}";
                        }
                        else
                        {
                            r = $"{city.State.Name}"; // otherwise just use the City & State (most likely for US)
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(countryName))
                        {
                            r = $"{countryName}";
                        }
                    }
                }
            }

            return (r);
        }

        #endregion

        #endregion
    }
}
