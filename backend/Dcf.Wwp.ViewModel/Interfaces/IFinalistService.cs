using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.ConnectedServices.Finalist;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IFinalistService
    {
        #region Properties

        #endregion

        #region Methods

        FinalistAddressContract GetAddress(FinalistAddressContract        contract);
        FinalistAddressContract GetAnalyzeAddress(FinalistAddressContract contract);

        #endregion
    }
}
