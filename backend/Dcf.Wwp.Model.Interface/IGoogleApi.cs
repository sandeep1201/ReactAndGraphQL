namespace Dcf.Wwp.Model.Interface
{
    public interface IGoogleApi
    {
        #region Properties

       // IGoogleData DetailsProvider(string placeId);

        #endregion

        #region Methods

        decimal[]   GetLatLong(string placeId);
        IGoogleData GetPlaceDetails(string placeId);
        IGoogleData GetPredictedUSCities(string cityName, bool wi);
        IGoogleData GetPredictedCities(string cityName);
        IGoogleData GetPredictedSchools(string schoolName, string placeId);
        IGoogleData GetPredictedStreetAddresses(string streetAddress, string placeId);
        IGoogleData GetPredictedUSStreetAddresses(string streetAddress, string placeId);

        #endregion
    }
}
