using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Delegates;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ICityDomain
    {
        #region Properties

        #endregion

        #region Methods

        City GetOrCreateCity(IGoogleLocation googleLocation = null, DetailsProvider getDetails = null, LatLongProvider getLatLong = null, string user = null, IFinalistAddress finalistAddress = null, bool isClientReg = false);

        #endregion
    }
}
