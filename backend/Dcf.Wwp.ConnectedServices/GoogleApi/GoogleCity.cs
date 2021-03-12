using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    public class GoogleCity : IGoogleCity
    {
        #region Properties

        public string CityStateCountry { get; set; }
        public string City             { get; set; }
        public string State            { get; set; }
        public string Country          { get; set; }
        public string PlaceId          { get; set; }

        #endregion
    }
}
