namespace Dcf.Wwp.ConnectedServices.Interfaces
{
    public interface IGoogleStreetAddress
    {
        #region Properties

        string   FormattedAddress { get; set; }
        string   PlaceId          { get; set; }
        string   MainAddress      { get; set; }
        string   StreetNumber     { get; set; }
        string   StreetName       { get; set; }
        string   City             { get; set; }
        string   State            { get; set; }
        string   StateCode        { get; set; }
        string   Country          { get; set; }
        string   CountryCode      { get; set; }
        string   PostalCode       { get; set; }
        decimal? Latitude         { get; set; }
        decimal? Longitude        { get; set; }

        #endregion

        #region Methods
        
        #endregion
    }
}
