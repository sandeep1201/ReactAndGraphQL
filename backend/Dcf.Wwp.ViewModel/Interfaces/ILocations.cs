using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ILocations
    {
        #region Properties

        #endregion

        #region Methods

        #region Phase I legacy usage

        LocationContract        GetLocationInfo(object                           obj, ICity city); // Phase I old stuff
        FinalistAddressContract GetFinalistLocationInfo(IParticipantContactInfo  info);
        FinalistAddressContract GetFinalistLocationInfo(IAlternateMailingAddress info);
        string                  FromCity(ICity                                   city);

        #endregion

        #region Phase II Domains and DataAccess usage

        LocationContract GetLocationInfo(object obj, City city);
        string           FromCity(City          city);

        #endregion

        #endregion
    }
}
