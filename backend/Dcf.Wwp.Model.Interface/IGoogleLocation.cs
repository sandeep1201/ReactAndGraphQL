namespace Dcf.Wwp.Model.Interface
{
    public interface IGoogleLocation
    {
        #region Properties

        string Description   { get; set; }
        string Longitude     { get; set; }
        string Latitude      { get; set; }
        string City          { get; set; }
        string State         { get; set; }
        string Country       { get; set; }
        string FullAddress   { get; set; }
        string ZipAddress    { get; set; }
        string GooglePlaceId { get; set; }

        #endregion

        #region Methods
        
        #endregion
    }
}
