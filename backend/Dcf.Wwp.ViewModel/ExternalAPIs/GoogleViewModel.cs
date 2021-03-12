using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Api.Library.ExternalAPIs
{
    /// <summary>
    /// PARAMETERS city name 
    /// RETURNS a list of cities with name matching "query". 
    /// </summary>
    /// 
    public static class GoogleViewModel
    {
        private const string API_KEY = "AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE";

        public static decimal[] GetLatLong(string placeId)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/details/json?key={API_KEY}&placeid={placeId}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                throw new Exception("HttpWebRequest could not be created in Google Place API.");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    throw new Exception("HttpWebResponse was empty.");

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                var stream = response.GetResponseStream();
                if (stream == null)
                    throw new Exception("Response Stream is null.");

                var jsonSerializer = new DataContractJsonSerializer(typeof(GooglePlaceDetails));
                var jsonResponse   = jsonSerializer.ReadObject(stream) as GooglePlaceDetails;

                if (jsonResponse == null || jsonResponse.status != "OK")
                    throw new Exception("Response Stream from Google is null.");

                var lat = jsonResponse.result.geometry.location.lat;
                var lng = jsonResponse.result.geometry.location.lng;

                return new[] { lat, lng };
            }
        }

        public static GoogleData GetPlaceDetails(string placeId)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/details/json?key={API_KEY}&placeid={placeId}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                throw new Exception("HttpWebRequest could not be created in Google Place Details API.");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    throw new Exception("HttpWebResponse was empty in Google Place Details API.");

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Google Place Details API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                var stream = response.GetResponseStream();
                if (stream == null)
                    throw new Exception("Google Place Details API Response Stream is null.");

                var jsonSerializer = new DataContractJsonSerializer(typeof(GooglePlaceDetails));
                var gResponse      = jsonSerializer.ReadObject(stream) as GooglePlaceDetails;

                if (gResponse == null)
                    throw new Exception("Google Place Details API Response Stream from Google is null.");

                if (gResponse.status != "OK")
                    throw new Exception($"Unexpected status from Google API: '{gResponse.status}'");

                var gData = new GoogleData
                            {
                                Address = new GoogleStreetAddress
                                          {
                                              PlaceId          = placeId,
                                              FormattedAddress = gResponse.result.formatted_address,
                                              StreetNumber     = gResponse.GetAddressTypeDescription(GoogleStreetAddress.StreetNumberType),
                                              StreetName       = gResponse.GetAddressTypeDescription(GoogleStreetAddress.StreetNameType),
                                              City             = gResponse.GetAddressTypeDescription(GoogleStreetAddress.CityType),
                                              State            = gResponse.GetAddressTypeDescription(GoogleStreetAddress.StateType),
                                              StateCode        = gResponse.GetAddressTypeShortDescription(GoogleStreetAddress.StateType),
                                              Country          = gResponse.GetAddressTypeDescription(GoogleStreetAddress.CountryType),
                                              CountryCode      = gResponse.GetAddressTypeShortDescription(GoogleStreetAddress.CountryType),
                                              PostalCode       = gResponse.GetAddressTypeDescription(GoogleStreetAddress.PostalCodeType),
                                              Latitude         = gResponse.result.geometry.location.lat,
                                              Longitude        = gResponse.result.geometry.location.lat
                                          }
                            };

                return gData;
            }
        }

        public static GoogleData GetPredictedCities(string cityName)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=(cities)&language=en&key={API_KEY}&input={cityName}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    throw new Exception("HttpWebResponse was empty in Google Autocomplete API.");

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                var stream = response.GetResponseStream();
                if (stream == null)
                    throw new Exception("Google Autocomplete API Response Stream is null.");

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");

                if (gResponse.status != "OK")
                    throw new Exception($"Unexpected status from Google API: '{gResponse.status}'");

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
                        if (x.terms?.Length == 3)
                        {
                            data.City    = x.terms[0].value;
                            data.State   = x.terms[1].value;
                            data.Country = x.terms[2].value;
                        }

                    cities.Add(data);
                }

                gData.Cities = cities;

                return gData;
            }
        }

        public static GoogleData GetPredictedSchools(string schoolName, string placeId)
        {
            var latLong = GetLatLong(placeId);
            //Console.WriteLine($"Lat: {latLong[0]}  Long:{latLong[1]}");

            // A radius of 20,000m is ~ 12 miles.
            // 
            // https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670522,151.1957362&radius=2000&type=restaurant&keyword=cruise&key=YOUR_API_KEY
            // https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.09721740000001,-89.5042876&radius=20000&type=school&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&keyword=West
            var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLong[0]},{latLong[1]}&radius=20000&type=school&language=en&key={API_KEY}&keyword={schoolName}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    throw new Exception("HttpWebResponse was empty in Autocomplete Details API.");

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                var stream = response.GetResponseStream();
                if (stream == null)
                    throw new Exception("Google Autocomplete API Response Stream is null.");

                var jsonSerializer = new DataContractJsonSerializer(typeof(GooglePlaceResult));
                var gResponse      = jsonSerializer.ReadObject(stream) as GooglePlaceResult;

                if (gResponse == null)
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");

                var gData  = new GoogleData();
                var places = new List<IGooglePlace>();

                if (gResponse.status == "OK")
                {
                    foreach (var x in gResponse.results)
                    {
                        places.Add(new GooglePlace()
                                   {
                                       Name    = x.name,
                                       PlaceId = x.place_id
                                   });
                    }
                }
                else
                {
                    // TODO: Log a warning.
                    // NOTE: We will often see a "ZERO_RESULTS" message
                }


                gData.Schools = places;

                return gData;
            }
        }

        public static GoogleData GetPredictedStreetAddresses(string streetAddress, string placeId)
        {
            var latLong = GetLatLong(placeId);

            // A radius of 20,000m is ~ 12 miles.
            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=address&location={latLong[0]},{latLong[1]}&radius=20000&language=en&key={API_KEY}&input={streetAddress}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    throw new Exception("HttpWebResponse was empty in Autocomplete Details API.");

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Google Autocomplete API Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                var stream = response.GetResponseStream();
                if (stream == null)
                    throw new Exception("Google Autocomplete API Response Stream is null.");

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");

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
                else
                {
                    // TODO: Log a warning.
                    // NOTE: We will often see a "ZERO_RESULTS" message
                }

                gData.StreetAddresses = streetAddresses;

                return gData;
            }
        }

        public static GoogleData GetPredictedUsCities(string cityName)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?types=(cities)&components=country:us&language=en&key={API_KEY}&input={cityName}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                throw new Exception("HttpWebRequest could not be created in Google Autocomplete API.");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    throw new Exception("HttpWebResponse was empty in Google Autocomplete API.");

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                var stream = response.GetResponseStream();
                if (stream == null)
                    throw new Exception("Google Autocomplete API Response Stream is null.");

                var jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                var gResponse      = jsonSerializer.ReadObject(stream) as CityFromGoogle;

                if (gResponse == null)
                    throw new Exception("Google Autocomplete API Response Stream from Google is null.");

                if (gResponse.status != "OK")
                    throw new Exception($"Unexpected status from Google API: '{gResponse.status}'");

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
                        if (x.terms?.Length == 3)
                        {
                            data.City    = x.terms[0].value;
                            data.State   = x.terms[1].value;
                            data.Country = x.terms[2].value;
                        }

                    cities.Add(data);
                }

                gData.Cities = cities;

                return gData;
            }
        }

        /*
        public static GoogleData GetUsPredictedCities(string cityName)
        {
            string BaseURL =
                "https://maps.googleapis.com/maps/api/place/autocomplete/json?types=(cities)&components=country:us&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&input=";
            try
            {
                HttpWebRequest request = WebRequest.Create(BaseURL + cityName) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    CityFromGoogle jsonResponse
                        = objResponse as CityFromGoogle;

                    var gData = new GoogleData();

                    var cities = new List<GoogleCity>();


                    foreach (var x in jsonResponse.predictions)
                    {
                        var data = new GoogleCity();
                        data.CityStateCountry = x.description;
                        data.PlaceId = x.place_id;
                        var location = new List<string>();
                        foreach (var y in x.terms)
                        {
                            location.Add(y.value);
                        }
                        data.City = location[0];
                        data.State = location[1];
                        data.Country = location[2];
                        cities.Add(data);
                    }

                    gData.UsCities = cities;

                    return gData;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }


        }

        public static GoogleData GetPredictedCities(string cityName)
        {
            var BaseURL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?types=(cities)&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&input=";
            try
            {
                var request = WebRequest.Create(BaseURL + cityName) as HttpWebRequest;
                using (var response = request?.GetResponse() as HttpWebResponse)
                {
                    if (response?.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response?.StatusCode, response?.StatusDescription));

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                    object objResponse = jsonSerializer.ReadObject(response?.GetResponseStream());
                    CityFromGoogle jsonResponse = objResponse as CityFromGoogle;

                    var gData = new GoogleData();
                    var cities = new List<GoogleCity>();

                    if (jsonResponse == null)
                        return gData;

                    foreach (var x in jsonResponse.predictions)
                    {
                        var data = new GoogleCity();
                        data.CityStateCountry = x.description;
                        data.PlaceId = x.place_id;

                        if (x.terms?.Length == 2)
                        {
                            data.City = x.terms[0].value;
                            data.Country = x.terms[1].value;
                        }
                        else if (x.terms?.Length == 3)
                        {
                            data.City = x?.terms[0].value;
                            data.State = x?.terms[1].value;
                            data.Country = x?.terms[2].value;
                        }

                        cities.Add(data);
                    }

                    gData.Cities = cities;

                    return gData;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }

        /// <summary>
        /// PARAMETERS place Id and school name. 
        /// Generates latitude and longitude from place of ID of city. Searches radius for school.
        /// RETURNS a list of 20 schools with name matching "query". Each school object will have a latitude and longitude.
        /// </summary>

        public static GoogleData GetPredictedEstablishments(string name, string placeId)
        {

            string baseUrlSchool = "https://maps.googleapis.com/maps/api/place/search/json?&radius=50000&types=point_of_interest&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE";

            string baseUrlPlaceDetails = "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&placeid=";

            baseUrlPlaceDetails += placeId;

            HttpWebRequest request = WebRequest.Create(baseUrlPlaceDetails) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityDetailsFromGoogle));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                CityDetailsFromGoogle jsonResponse
                    = objResponse as CityDetailsFromGoogle;

                if (jsonResponse == null && jsonResponse.status != "OK")
                {
                    // throw ex.
                }

                var lat = jsonResponse.result.geometry.location.lat.ToString();
                var lng = jsonResponse.result.geometry.location.lng.ToString();

                baseUrlSchool += "&location=" + lat + "," + lng + "&name=" + name;

                HttpWebRequest requestSchools = WebRequest.Create(baseUrlSchool) as HttpWebRequest;

                if (requestSchools == null)
                {
                    //Throw
                }

                using (HttpWebResponse responseSchools = requestSchools.GetResponse() as HttpWebResponse)
                {
                    if (responseSchools.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            responseSchools.StatusCode,
                            responseSchools.StatusDescription));
                    DataContractJsonSerializer jsonSerializerSchools =
                        new DataContractJsonSerializer(typeof(EstablishmentsFromGoogle));
                    object objResponseSchools = jsonSerializerSchools.ReadObject(responseSchools.GetResponseStream());
                    EstablishmentsFromGoogle jsonResponseSchools
                        = objResponseSchools as EstablishmentsFromGoogle;

                    if (jsonResponseSchools == null)
                    {
                        // throw ex.
                    }

                    var gData = new GoogleData();

                    var establishments = new List<GoogleEstablishment>();

                    foreach (var x in jsonResponseSchools.results)
                    {
                        var data = new GoogleEstablishment();
                        data.Name = x.name;
                        data.StreetAddress = x.vicinity;
                        data.PlaceId = x.place_id;
                        establishments.Add(data);
                    }

                    gData.Establishments = establishments;
                    return gData;

                }
            }
        }

        public static GoogleData GetPredictedSchool(string name, string placeId)
        {

            string baseUrlSchool =
                "https://maps.googleapis.com/maps/api/place/search/json?&radius=50000&types=school&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE";

            string baseUrlPlaceDetails =
                "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&placeid=";

            baseUrlPlaceDetails += placeId;

            HttpWebRequest request = WebRequest.Create(baseUrlPlaceDetails) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityDetailsFromGoogle));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                CityDetailsFromGoogle jsonResponse
                    = objResponse as CityDetailsFromGoogle;

                if (jsonResponse == null && jsonResponse.status != "OK")
                {
                    // throw ex.
                }

                var lat = jsonResponse.result.geometry.location.lat.ToString();
                var lng = jsonResponse.result.geometry.location.lng.ToString();

                baseUrlSchool += "&location=" + lat + "," + lng + "&name=" + name;

                HttpWebRequest requestSchools = WebRequest.Create(baseUrlSchool) as HttpWebRequest;

                if (requestSchools == null)
                {
                    //Throw
                }

                using (HttpWebResponse responseSchools = requestSchools.GetResponse() as HttpWebResponse)
                {
                    if (responseSchools.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            responseSchools.StatusCode,
                            responseSchools.StatusDescription));
                    DataContractJsonSerializer jsonSerializerSchools =
                        new DataContractJsonSerializer(typeof(SchoolsFromGoogle));
                    object objResponseSchools = jsonSerializerSchools.ReadObject(responseSchools.GetResponseStream());
                    SchoolsFromGoogle jsonResponseSchools
                        = objResponseSchools as SchoolsFromGoogle;

                    if (jsonResponseSchools == null)
                    {
                        // throw ex.
                    }

                    var gData = new GoogleData();

                    var schools = new List<GoogleSchool>();

                    foreach (var x in jsonResponseSchools.results)
                    {
                        var data = new GoogleSchool();
                        data.Name = x.name;
                        data.StreetAddress = x.vicinity;
                        schools.Add(data);
                    }

                    gData.Schools = schools;

                    return gData;

                }
            }
        }


        public static decimal[] GetLatLong(string placeId)
        {

            string baseUrlPlaceDetails =
                "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&placeid=";

            baseUrlPlaceDetails += placeId;

            HttpWebRequest request = WebRequest.Create(baseUrlPlaceDetails) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityDetailsFromGoogle));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                CityDetailsFromGoogle jsonResponse
                    = objResponse as CityDetailsFromGoogle;

                if (jsonResponse == null && jsonResponse.status != "OK")
                {
                    // throw ex.
                }

                var lat = jsonResponse.result.geometry.location.lat;
                var lng = jsonResponse.result.geometry.location.lng;
                //var streetaddress = jsonResponse.result.formatted_address;
                //var zipCode = jsonResponse.result.address_components[7].long_name;


                var coors = new decimal[] { lat, lng };

                return coors;
            }
        }

        public static GoogleData GetPredictedCollege(string name, string placeId, IRepository repo)
        {

            string baseUrlSchool =
                "https://maps.googleapis.com/maps/api/place/search/json?&radius=25000&types=university&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE";

            string baseUrlPlaceDetails =
                "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&placeid=";

            // Lets check the placeID.
            var cityDb = repo.CityByGooglePlaceId(placeId);

            if (cityDb != null && cityDb.LongitudeNumber != null & cityDb.LatitudeNumber != null)
            {
                baseUrlSchool += "&location=" + cityDb.LatitudeNumber + "," + cityDb.LongitudeNumber + "&name=" + name;
            }
            else
            {
                baseUrlPlaceDetails += placeId;

                HttpWebRequest request = WebRequest.Create(baseUrlPlaceDetails) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer =
                        new DataContractJsonSerializer(typeof(CityDetailsFromGoogle));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    CityDetailsFromGoogle jsonResponse
                        = objResponse as CityDetailsFromGoogle;

                    if (jsonResponse == null && jsonResponse.status != "OK")
                    {
                        // throw ex.
                    }

                    var lat = jsonResponse.result.geometry.location.lat.ToString();
                    var lng = jsonResponse.result.geometry.location.lng.ToString();

                    baseUrlSchool += "&location=" + lat + "," + lng + "&name=" + name;
                }
            }

            HttpWebRequest requestSchools = WebRequest.Create(baseUrlSchool) as HttpWebRequest;

            if (requestSchools == null)
            {
                //Throw
            }

            using (HttpWebResponse responseSchools = requestSchools.GetResponse() as HttpWebResponse)
            {
                if (responseSchools.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        responseSchools.StatusCode,
                        responseSchools.StatusDescription));
                DataContractJsonSerializer jsonSerializerSchools =
                    new DataContractJsonSerializer(typeof(SchoolsFromGoogle));
                object objResponseSchools = jsonSerializerSchools.ReadObject(responseSchools.GetResponseStream());
                SchoolsFromGoogle jsonResponseSchools
                    = objResponseSchools as SchoolsFromGoogle;

                if (jsonResponseSchools == null)
                {
                    // throw ex.
                }

                var gData = new GoogleData();

                var colleges = new List<GoogleCollege>();

                foreach (var x in jsonResponseSchools.results)
                {
                    var data = new GoogleCollege();
                    data.Name = x.name;
                    data.StreetAddress = x.vicinity;
                    colleges.Add(data);
                }

                gData.Colleges = colleges;

                return gData;

            }

        }

        public static GoogleCity GetZipAddress(string placeId)
        {
            //var latLong = GetLatLong(placeId);
            // var lat = latLong[0];
            // var lng = latLong[1];
            // var baseUrlForCityAddress = "https://maps.googleapis.com/maps/api/geocode/json?latlng=";
            // baseUrlForCityAddress +=  lat + "," + lng + "&key=" + "AIzaSyB7YYhcaUXisgNKBw1rd4anGlkMbrX3urY";

            string baseUrlPlaceDetails =
                "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&placeid=";

            baseUrlPlaceDetails += placeId;

            HttpWebRequest request = WebRequest.Create(baseUrlPlaceDetails) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityDetailsFromGoogle));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                CityDetailsFromGoogle jsonResponse
                    = objResponse as CityDetailsFromGoogle;

                if (jsonResponse == null && jsonResponse.status != "OK")
                {
                    // throw ex.
                }


                var city = new GoogleCity();

                //var zipCode = jsonResponse.result.address_components.Select(x => x.types.Equals("postal_code"));
                foreach (var js in jsonResponse.result.address_components)
                {
                    if (js.types[0] == "postal_code")
                    {
                        var zipCode = js.short_name;
                        city.ZipCode = zipCode;
                    }

                }

                return city;
            }
        }

        public static GoogleData GetPredictedStreetAddresses(string streetAddress)
        {
            string BaseURL =
                "https://maps.googleapis.com/maps/api/place/autocomplete/json?options=(address)&components=country:us&language=en&key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&input=";
            string BasePostalAddressUrl =
                "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyDFF7EyTAIRuKNNN6c4oFbbQHwCJeFxJCE&placeid=";
            try
            {
                HttpWebRequest request = WebRequest.Create(BaseURL + streetAddress) as HttpWebRequest;
                using (HttpWebResponse response = request?.GetResponse() as HttpWebResponse)
                {
                    if (response?.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response?.StatusCode,
                            response?.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CityFromGoogle));
                    //DataContractJsonSerializer jsonSerializer1 = new DataContractJsonSerializer(typeof(Predictions));
                    object objResponse = jsonSerializer.ReadObject(response?.GetResponseStream());
                    // object objResponse1 = jsonSerializer1.ReadObject(response.GetResponseStream());
                    CityFromGoogle jsonResponse
                        = objResponse as CityFromGoogle;
                    //Predictions jsonPredictions 
                    //    = objResponse1 as Predictions;
                    var gData = new GoogleData();
                    var streetAddresses = new List<GoogleStreetAddress>();

                    foreach (var x in jsonResponse.predictions)
                    {
                        var data = new GoogleStreetAddress();
                        data.StreetAddress = x.description;
                        data.PlaceId = x.place_id;
                        data.MainAddress = x.structured_formatting.main_text;
                        streetAddresses.Add(data);
                    }

                    gData.GoogleStreetAddresses = streetAddresses;

                    return gData;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }


        }
        */
    }
}
