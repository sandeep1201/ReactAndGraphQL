namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWpGeoAreaRepository
    {
        GeoArea WpGeoAreaByOfficeNumber(short officeNumber, string programCode = "WW");
        GeoArea WpGeoAreaByPin(decimal pin);
    }
}