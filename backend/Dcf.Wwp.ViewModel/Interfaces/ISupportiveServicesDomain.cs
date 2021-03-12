using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ISupportiveServicesDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<SupportiveServiceContract> GetSupportiveServicesForEP(int epId);
        List<SupportiveServiceContract> Upsert(List<SupportiveServiceContract> supportiveServiceContract, int epId);

        #endregion
    }
}
