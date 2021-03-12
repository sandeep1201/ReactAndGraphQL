using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GoogleStreetAddress : IGoogleStreetAddress
    {
        public const string StreetNumberType = "street_number";
        public const string StreetNameType   = "route";
        public const string CityType         = "locality";
        public const string StateType        = "administrative_area_level_1";
        public const string CountryType      = "country";
        public const string PostalCodeType   = "postal_code";

        public string   FormattedAddress { get; set; }
        public string   PlaceId          { get; set; }
        public string   MainAddress      { get; set; }
        public string   StreetNumber     { get; set; }
        public string   StreetName       { get; set; }
        public string   City             { get; set; }
        public string   State            { get; set; }
        public string   StateCode        { get; set; }
        public string   Country          { get; set; }
        public string   CountryCode      { get; set; }
        public string   PostalCode       { get; set; }
        public decimal? Latitude         { get; set; }
        public decimal? Longitude        { get; set; }
    }
}
