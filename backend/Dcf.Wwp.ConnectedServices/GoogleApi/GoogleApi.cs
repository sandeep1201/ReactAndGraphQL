using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    public class GoogleApi : IGoogleApi
    {
        #region Properties

        private const    string        API_KEY = "AIzaSyCe8b2xEeT9zXtjkpQwBzGme48b_tyVK_c";
        private readonly IGoogleConfig _googleConfig;
        private readonly string        _apiKey;

        #endregion

        #region Methods

        public GoogleApi ()
        {   
            // there is ONE area (Repository.cs) that "needs"
            // this component and can't be injected... ;(
            _apiKey = API_KEY;
        }

        public GoogleApi (IGoogleConfig googleConfig)
        {
            _googleConfig = googleConfig;
            _apiKey       = _googleConfig.ApiKey;
        }

        public decimal[] GetLatLong(string placeId)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/details/json?key={_apiKey}&placeid={placeId}&fields=geometry";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Place API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(GooglePlaceDetails));
                var jsonResponse   = jsonSerializer.ReadObject(stream) as GooglePlaceDetails;

                if (jsonResponse == null || jsonResponse.status != "OK")
                {
                    throw new Exception("Response Stream from Google is null.");
                }

                var lat = jsonResponse.result.geometry.location.lat;
                var lng = jsonResponse.result.geometry.location.lng;

                return new[] { lat, lng };
            }
        }

        public IGoogleData GetPlaceDetails(string placeId)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/details/json?key={_apiKey}&placeid={placeId}&fields=address_component,formatted_address,geometry"; // GoogleAPI cost reduction #2709

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Place Details API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty in Google Place Details API.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Google Place Details API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Google Place Details API Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(GooglePlaceDetails));
                var gResponse      = jsonSerializer.ReadObject(stream) as GooglePlaceDetails;

                if (gResponse == null)
                {
                    throw new Exception("Google Place Details API Response Stream from Google is null.");
                }

                if (gResponse.status != "OK" && gResponse.status != "NOT_FOUND")
                {
                    throw new Exception($"Unexpected status from Google API: '{gResponse.status}'");
                }

                var gData = new GoogleData
                            {
                                Address = new GoogleStreetAddress
                                          {
                                              PlaceId          = gResponse.result != null ? placeId : "",
                                              FormattedAddress = gResponse.result?.formatted_address,
                                              StreetNumber     = gResponse.GetAddressTypeDescription(GoogleStreetAddress.StreetNumberType),
                                              StreetName       = gResponse.GetAddressTypeDescription(GoogleStreetAddress.StreetNameType),
                                              // Use the alternate city type to find the City if the original city type returns null
                                              City        = gResponse.GetAddressTypeDescription(GoogleStreetAddress.CityType) ?? gResponse.GetAddressTypeDescription(GoogleStreetAddress.altCityType),
                                              State       = gResponse.GetAddressTypeDescription(GoogleStreetAddress.StateType),
                                              StateCode   = gResponse.GetAddressTypeShortDescription(GoogleStreetAddress.StateType),
                                              Country     = gResponse.GetAddressTypeDescription(GoogleStreetAddress.CountryType),
                                              CountryCode = gResponse.GetAddressTypeShortDescription(GoogleStreetAddress.CountryType),
                                              PostalCode  = gResponse.GetAddressTypeDescription(GoogleStreetAddress.PostalCodeType),
                                              Latitude    = gResponse.result?.geometry.location.lat,
                                              Longitude   = gResponse.result?.geometry.location.lat
                                          }
                            };

                return gData;
            }
        }

        public IGoogleData GetPredictedUSCities(string cityName, bool wi)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=(cities)&components=country:us&language=en&key={_apiKey}&input={cityName}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty in Google Autocomplete API.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");
                }

                if (gResponse.status != "OK")
                {
                    if (gResponse.status.ToUpper() != "ZERO_RESULTS")
                    {
                        throw new Exception($"Unexpected status from Google API: '{gResponse.status}'");
                    }
                }

                var gData  = new GoogleData();
                var cities = new List<IGoogleCity>();

                foreach (var x in gResponse.predictions)
                {
                    var data = new GoogleCity
                               {
                                   CityStateCountry = x.description,
                                   PlaceId          = x.place_id
                               };

                    if (x.terms?.Length == 2)
                    {
                        data.City    = x.terms[0].value;
                        data.Country = x.terms[1].value;
                    }
                    else
                    {
                        if (x.terms?.Length == 3)
                        {
                            data.City    = x.terms[0].value;
                            data.State   = x.terms[1].value;
                            data.Country = x.terms[2].value;
                        }
                    }

                    cities.Add(data);
                }

                gData.Cities = wi ? cities.Where(i => i.State == "WI").ToList() : cities;

                return gData;
            }
        }

        public IGoogleData GetPredictedCities(string cityName)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=(cities)&language=en&key={_apiKey}&input={cityName}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty in Google Autocomplete API.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");
                }

                if (gResponse.status != "OK")
                {
                    if (gResponse.status.ToUpper() != "ZERO_RESULTS")
                    {
                        throw new Exception($"Unexpected status from Google API: '{gResponse.status}'");
                    }
                }

                var gData  = new GoogleData();
                var cities = new List<IGoogleCity>();

                foreach (var x in gResponse.predictions)
                {
                    var data = new GoogleCity
                               {
                                   CityStateCountry = x.description,
                                   PlaceId          = x.place_id
                               };

                    if (x.terms?.Length == 2)
                    {
                        data.City    = x.terms[0].value;
                        data.Country = x.terms[1].value;
                    }
                    else
                    {
                        if (x.terms?.Length == 3)
                        {
                            data.City    = x.terms[0].value;
                            data.State   = x.terms[1].value;
                            data.Country = x.terms[2].value;
                        }
                    }

                    cities.Add(data);
                }

                gData.Cities = cities;

                return gData;
            }
        }

        public IGoogleData GetPredictedSchools( string schoolName, string placeId)
        {
            var latLong = GetLatLong(placeId);

            // A radius of 20,000m is ~ 12 miles.
            var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLong[0]},{latLong[1]}&radius=20000&type=school&language=en&key={_apiKey}&keyword={schoolName}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty in Autocomplete Details API.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(GooglePlaceResult));
                var gResponse      = jsonSerializer.ReadObject(stream) as GooglePlaceResult;

                if (gResponse == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");
                }

                var gData  = new GoogleData();
                var places = new List<IGooglePlace>();

                if (gResponse.status == "OK")
                {
                    foreach (var x in gResponse.results)
                    {
                        places.Add(new GooglePlace
                                   {
                                       Name    = x.name,
                                       PlaceId = x.place_id
                                   });
                    }
                }

                gData.Schools = places;

                return gData;
            }
        }

        public IGoogleData GetPredictedStreetAddresses( string streetAddress, string placeId)
        {
            var latLong = GetLatLong(placeId);

            // A radius of 20,000m is ~ 12 miles.
            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=address&location={latLong[0]},{latLong[1]}&radius=20000&language=en&key={_apiKey}&input={streetAddress}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty in Autocomplete Details API.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");
                }

                var gData           = new GoogleData();
                var streetAddresses = new List<IGoogleStreetAddress>();

                if (gResponse.status == "OK")
                {
                    foreach (var x in gResponse.predictions)
                    {
                        streetAddresses.Add(new GoogleStreetAddress
                                            {
                                                FormattedAddress = x.description,
                                                PlaceId          = x.place_id,
                                                MainAddress      = x.structured_formatting.main_text
                                            });
                    }
                }

                gData.StreetAddresses = streetAddresses;

                return gData;
            }
        }

        public IGoogleData GetPredictedUSStreetAddresses(string streetAddress, string placeId)
        {
            var latLong = GetLatLong(placeId);

            // A radius of 20,000m is ~ 12 miles.
            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=address&components=country:us&location={latLong[0]},{latLong[1]}&radius=20000&language=en&key={_apiKey}&input={streetAddress}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
            {
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    throw new Exception("HttpWebResponse was empty in Autocomplete Details API.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream is null.");
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                {
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");
                }

                var gData           = new GoogleData();
                var streetAddresses = new List<IGoogleStreetAddress>();

                if (gResponse.status == "OK")
                {
                    foreach (var x in gResponse.predictions)
                    {
                        streetAddresses.Add(new GoogleStreetAddress
                                            {
                                                FormattedAddress = x.description,
                                                PlaceId          = x.place_id,
                                                MainAddress      = x.structured_formatting.main_text
                                            });
                    }
                }

                gData.StreetAddresses = streetAddresses;

                return gData;
            }
        }

        #endregion
    }
}
