using System.Collections.Generic;
using Dcf.Wwp.Model.Interface.Delegates;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ICityRepository
    {
        #region Properties

        #endregion

        #region Methods

        IEnumerable<ICity> GetCities();
        ICity              CityByName(string                   name);
        ICity              CityByGooglePlaceId(string          placeId);
        IEnumerable<ICity> GetCitiesByIds(IEnumerable<int>     cityIds);
        ICity              NewCity(IEmploymentInformation      employment,            string          user);
        ICity              NewCity(ISchoolCollegeEstablishment parentObject,          string          user);
        ICity              GetOrCreateCity(IGoogleLocation     googleLocation = null, DetailsProvider getDetails = null, LatLongProvider getLatLong = null, string user = null, IFinalistAddress finalistAddress = null, bool isClientReg = false);
        string             CityDisplayName(ICity               city);

        #endregion
    }
}
