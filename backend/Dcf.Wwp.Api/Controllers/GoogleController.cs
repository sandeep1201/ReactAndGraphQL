using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/gapi")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class GoogleController : Controller
    {
        #region Properties

        private readonly IGoogleApi _googleApi;

        #endregion

        #region Methods

        public GoogleController(IGoogleApi googleApi)
        {
            _googleApi = googleApi;
        }

        [HttpGet]
        [Route("schools/{placeId}/{name}")]
        public IActionResult GetSchools(string name, string placeId)
        {
            var data = _googleApi.GetPredictedSchools(name, placeId);

            return Ok(data);
        }

        [HttpGet]
        [Route("address/{placeId}")]
        public IActionResult GetAddress(string placeId)
        {
            var data = _googleApi.GetPlaceDetails(placeId);

            return Ok(data);
        }

        [HttpGet]
        [Route("addresses/{placeId}/{name}")]
        public IActionResult GetStreetAddresses(string name, string placeId)
        {
            var data = _googleApi.GetPredictedStreetAddresses(name, placeId);

            return Ok(data);
        }

        [HttpGet]
        [Route("us-addresses/{placeId}/{name}")]
        public IActionResult GetUSStreetAddresses(string name, string placeId)
        {
            var data = _googleApi.GetPredictedUSStreetAddresses(name, placeId);

            return Ok(data);
        }

        [HttpGet]
        [Route("colleges/{placeId}/{name}")]
        public IActionResult GetUniversities(string name, string placeId)
        {
            // For now, Google doesn't support a College specific
            // search. The schools type does the same thing.
            var data = _googleApi.GetPredictedSchools(name, placeId);

            return Ok(data);
        }

        [HttpGet]
        [Route("zipCode/{placeId}")]
        public IActionResult GetZipCode(string placeId)
        {
            var data = _googleApi.GetPlaceDetails(placeId);

            return Ok(data);
        }

        [HttpGet]
        [Route("cities/{city}")]
        public IActionResult GetCities(string city)
        {
            var data = _googleApi.GetPredictedCities(city);

            return Ok(data);
        }

        [HttpGet]
        [Route("uscities/{city}")]
        public IActionResult GetUSCities(string city)
        {
            var data = _googleApi.GetPredictedUSCities(city, false);

            return Ok(data);
        }

        [HttpGet]
        [Route("wicities/{city}")]
        public IActionResult GetWICities(string city)
        {
            var data = _googleApi.GetPredictedUSCities(city, true);

            return Ok(data);
        }

        #endregion
    }
}
