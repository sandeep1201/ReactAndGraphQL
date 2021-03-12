namespace Dcf.Wwp.Model.Interface
{
    public interface IGoogleCity
    {
        #region Properties

        string CityStateCountry { get; set; }
        string City             { get; set; }
        string State            { get; set; }
        string Country          { get; set; }
        string PlaceId          { get; set; }
        
        #endregion

        #region Methods
        
        #endregion
    }
}