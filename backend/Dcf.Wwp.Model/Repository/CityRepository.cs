using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Delegates;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ICityRepository
    {
        #region Properties

        #endregion

        #region Methods

        public IEnumerable<ICity> GetCities() => _db.Cities;

        public ICity CityByName(string name) => _db.Cities.FirstOrDefault(i =>  i.Name == name);

        public ICity CityByGooglePlaceId(string placeId) => _db.Cities.FirstOrDefault(i => i.GooglePlaceId == placeId);

        public IEnumerable<ICity> GetCitiesByIds(IEnumerable<int> cityIds)
        {
            var r = _db.Cities.Where(city => cityIds.Contains(city.Id));

            return (r);
        }

        public ICity NewCity(IEmploymentInformation employment, string user)
        {
            var city = new City { ModifiedBy = user, ModifiedDate = DateTime.Now, IsDeleted = false };

            city.EmploymentInformations.Add((EmploymentInformation) employment);

            employment.City = city;

            _db.Cities.Add(city);

            return (city);
        }

        public ICity NewCity(ISchoolCollegeEstablishment schoolCollegeEstablishment, string user)
        {
            var city = new City
                       {
                           ModifiedBy   = user,
                           ModifiedDate = DateTime.Now,
                           IsDeleted    = false
                       };

            _db.Cities.Add(city);

            schoolCollegeEstablishment.City = city;

            return (city);
        }

        public ICity GetOrCreateCity(IGoogleLocation googleLocation = null, DetailsProvider getDetails = null, LatLongProvider getLatLong = null, string user = null, IFinalistAddress finalistAddress = null, bool isClientReg = false)
        {
            ICity city = null;

            if ((!isClientReg && googleLocation == null) || (isClientReg && finalistAddress == null))
            {
                throw new InvalidOperationException("Location must be set."); // this should be a 'ArgumentNullException'
            }

            if (!string.IsNullOrWhiteSpace(googleLocation?.GooglePlaceId) || !string.IsNullOrWhiteSpace(finalistAddress?.City))
            {
                if (googleLocation != null && !string.IsNullOrWhiteSpace(googleLocation.GooglePlaceId))
                    city = _db.Cities.FirstOrDefault(i => (i.GooglePlaceId == googleLocation.GooglePlaceId) || (i.Name == googleLocation.City && i.State.Name == googleLocation.State));
                else
                    if (finalistAddress != null && !string.IsNullOrWhiteSpace(finalistAddress.City))
                        city = _db.Cities.FirstOrDefault(i => i.Name == finalistAddress.City && i.State.Code == finalistAddress.State);

                // If we have a City already, we will just use that.
                if (city != null)
                {
                    if (googleLocation != null)
                        city.GooglePlaceId = city.GooglePlaceId ?? googleLocation.GooglePlaceId;
                    return city;
                }

                // Lookup using the details provider
                IGoogleData details = null;

                if (googleLocation != null && getDetails != null)
                    details = getDetails(googleLocation.GooglePlaceId);

                if (details != null)
                {
                    googleLocation.City    = details.Address.City;
                    googleLocation.Country = details.Address.Country;
                    googleLocation.State   = details.Address.State;
                    var stateAbbr = !string.IsNullOrEmpty(details.Address.StateCode) ? details.Address.StateCode : details.Address.State;

                    city = new City
                           {
                               GooglePlaceId = googleLocation.GooglePlaceId,
                               Name          = googleLocation.City,
                               ModifiedBy    = user,
                               ModifiedDate  = DateTime.Now,
                               IsDeleted     = false
                           };

                    _db.Cities.Add((City) city);

                    // Figure out the country
                    city.Country = CountryByName(googleLocation.Country);

                    if (city.Country == null)
                    {
                        city.Country      = NewCountry(city, user);
                        city.Country.Name = googleLocation.Country;
                    }

                    if (!string.IsNullOrWhiteSpace(googleLocation.State))
                    {
                        var state = StateByCodeAndCountryId(stateAbbr, city.Country?.Id) ?? NewState(city, user);

                        if (state.Id == 0)
                        {
                            state.Name    = googleLocation.State;
                            state.Code    = stateAbbr;
                            state.Country = city.Country;
                        }

                        city.State = state;
                    }

                    city.LatitudeNumber  = details.Address.Latitude;
                    city.LongitudeNumber = details.Address.Longitude;
                }
                else
                    if (finalistAddress != null)
                    {
                        city = new City
                               {
                                   Name         = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(finalistAddress.City.ToLower()),
                                   ModifiedBy   = user,
                                   ModifiedDate = DateTime.Now,
                                   IsDeleted    = false
                               };
                        _db.Cities.Add((City) city);

                        city.Country = CountryByName("United States");

                        var state = StateByCodeAndCountryId(finalistAddress.State, city.Country?.Id);
                        city.State = state;
                    }
            }
            else
                if (!string.IsNullOrWhiteSpace(googleLocation?.City))
                {
                    // TODO: The front end does not usually allow this condition through its validation,
                    city = CityByName(googleLocation.City);

                    if (city != null)
                    {
                        return city;
                    }

                    city = new City { Name = googleLocation.City };

                    if (!string.IsNullOrWhiteSpace(googleLocation.State))
                    {
                        var state = StateByCode(googleLocation.State) ?? NewState(city, user);
                        state.Code = googleLocation.State;
                        city.State = state;
                    }

                    city.Country = CountryByName(googleLocation.Country);

                    if (city.Country == null && city.State != null)
                    {
                        city.Country      = NewCountry(city.State, user);
                        city.Country.Name = googleLocation.Country;
                    }
                }
                else
                    if (!string.IsNullOrWhiteSpace(finalistAddress?.City))
                    {
                        // TODO: The front end does not usually allow this condition through its validation,
                        city = CityByName(finalistAddress.City);

                        if (city != null)
                        {
                            return city;
                        }

                        city = new City { Name = finalistAddress.City };

                        if (!string.IsNullOrWhiteSpace(finalistAddress.State))
                        {
                            var state = StateByCode(finalistAddress.State);
                            city.State = state;
                        }

                        city.Country = CountryByName("United States");
                    }

            return city;
        }

        public string CityDisplayName(ICity city)
        {
            var r = string.Empty;

            if (city != null)
            {
                if (city.State != null)
                {
                    r = city.Country != null ? $"{city.Name}, {city.State.Name}, {city.Country.Name}" : $"{city.Name}, {city.State.Name}";
                }
                else
                {
                    r = city.Country != null ? $"{city.Name}, {city.Country.Name}" : $"{city.Name}";
                }
            }

            return (r);
        }

        #endregion
    }
}
