using System;
using System.Globalization;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Delegates;


namespace Dcf.Wwp.Api.Library.Domains
{
    public class CityDomain : ICityDomain
    {
        #region Properties

        private readonly ICityRepository    _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository   _stateRepository;

        #endregion

        #region Methods

        public CityDomain(ICityRepository cityRepository, ICountryRepository countryRepository, IStateRepository stateRepository)
        {
            _cityRepository    = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository   = stateRepository;
        }

        public City GetOrCreateCity(IGoogleLocation googleLocation = null, DetailsProvider getDetails = null, LatLongProvider getLatLong = null, string user = null, IFinalistAddress finalistAddress = null, bool isClientReg = false)
        {
            var  modifiedDate = DateTime.Now;
            City city         = null;

            if ((!isClientReg && googleLocation == null) || (isClientReg && finalistAddress == null))
            {
                throw new InvalidOperationException("Location must be set.");
            }

            if (!string.IsNullOrWhiteSpace(googleLocation?.GooglePlaceId) || !string.IsNullOrWhiteSpace(finalistAddress?.City))
            {
                if (googleLocation != null && !string.IsNullOrWhiteSpace(googleLocation.GooglePlaceId))
                    city = _cityRepository.Get(i => (i.GooglePlaceId == googleLocation.GooglePlaceId) || (i.Name == googleLocation.City && i.State.Name == googleLocation.State));
                else
                    if (finalistAddress != null && !string.IsNullOrWhiteSpace(finalistAddress.City))
                        city = _cityRepository.Get(i => i.Name == finalistAddress.City && i.State.Code == finalistAddress.State);

                if (city != null)
                {
                    if (googleLocation != null)
                        city.GooglePlaceId = city.GooglePlaceId ?? googleLocation.GooglePlaceId;
                    return city;
                }

                IGoogleData details = null;

                if (googleLocation != null && getDetails != null)
                    details = getDetails(googleLocation.GooglePlaceId);

                if (details != null)
                {
                    googleLocation.City    = details.Address.City;
                    googleLocation.Country = details.Address.Country;
                    googleLocation.State   = details.Address.State;
                    var stateAbbr = !string.IsNullOrEmpty(details.Address.StateCode) ? details.Address.StateCode : details.Address.State;

                    city = _cityRepository.New();

                    city.GooglePlaceId = googleLocation.GooglePlaceId;
                    city.Name          = googleLocation.City;
                    city.ModifiedBy    = user;
                    city.ModifiedDate  = modifiedDate;
                    city.IsDeleted     = false;

                    _cityRepository.Add(city);

                    city.Country = CountryByName(googleLocation.Country);

                    if (city.Country == null)
                    {
                        city.Country      = NewCountry(city, user, modifiedDate);
                        city.Country.Name = googleLocation.Country;
                    }

                    if (!string.IsNullOrWhiteSpace(googleLocation.State))
                    {
                        var state = StateByCodeAndCountryId(stateAbbr, city.Country?.Id) ?? NewState(city, user, modifiedDate);

                        if (state.Id == 0)
                        {
                            state.Name    = googleLocation.State;
                            state.Code    = stateAbbr;
                            state.Country = city.Country;
                        }

                        city.State = state;
                    }

                    city.Latitude  = details.Address.Latitude;
                    city.Longitude = details.Address.Longitude;
                }
                else
                    if (finalistAddress != null)
                    {
                        city = _cityRepository.New();

                        city.Name         = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(finalistAddress.City.ToLower());
                        city.ModifiedBy   = user;
                        city.ModifiedDate = modifiedDate;
                        city.IsDeleted    = false;

                        _cityRepository.Add(city);

                        city.Country = CountryByName("United States");

                        var state = StateByCodeAndCountryId(finalistAddress.State, city.Country?.Id);
                        city.State = state;
                    }
            }
            else
                if (!string.IsNullOrWhiteSpace(googleLocation?.City))
                {
                    city = CityByName(googleLocation.City);

                    if (city != null)
                    {
                        return city;
                    }

                    city = _cityRepository.New();

                    city.Name = googleLocation.City;

                    if (!string.IsNullOrWhiteSpace(googleLocation.State))
                    {
                        var state = StateByCode(googleLocation.State) ?? NewState(city, user, modifiedDate);
                        state.Code = googleLocation.State;
                        city.State = state;
                    }

                    city.Country = CountryByName(googleLocation.Country);

                    if (city.Country != null || city.State == null) return city;
                    city.Country      = NewCountry(city.State, user, modifiedDate);
                    city.Country.Name = googleLocation.Country;
                }
                else
                    if (!string.IsNullOrWhiteSpace(finalistAddress?.City))
                    {
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

        private Country CountryByName(string countryName) => _countryRepository.Get(i => i.Name.ToLower() == countryName.ToLower());

        private Country NewCountry(City city, string user, DateTime modifiedDate)
        {
            var country = _countryRepository.New();

            country.IsDeleted     = false;
            country.IsNonStandard = true;
            country.ModifiedBy    = user;
            country.ModifiedDate  = modifiedDate;

            _countryRepository.Add(country);
            city.Country = country;

            return country;
        }

        private State StateByCodeAndCountryId(string stateCode, int? countryId) => _stateRepository.Get(i => (i.Code.ToLower() == stateCode.ToLower()
                                                                                                              || i.Name.ToLower() == stateCode.ToLower())
                                                                                                             && i.CountryId == countryId);

        private State NewState(City city, string user, DateTime modifiedDate)
        {
            var state = _stateRepository.New();

            state.ModifiedBy    = user;
            state.ModifiedDate  = modifiedDate;
            state.IsNonStandard = true;

            _stateRepository.Add(state);
            city.State = state;

            return state;
        }

        private City CityByName(string name) => _cityRepository.Get(i => i.Name == name);

        private State StateByCode(string stateCode) => _stateRepository.Get(i => i.Code == stateCode);

        private Country NewCountry(State state, string user, DateTime modifiedDate)
        {
            var country = _countryRepository.New();

            country.IsDeleted     = false;
            country.IsNonStandard = true;
            country.ModifiedBy    = user;
            country.ModifiedDate  = modifiedDate;

            _countryRepository.Add(country);
            state.Country = country;

            return country;
        }

        #endregion
    }
}
