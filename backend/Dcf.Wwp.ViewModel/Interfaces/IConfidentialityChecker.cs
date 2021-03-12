using System.Collections.Generic;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IConfidentialityChecker
    {
        #region Properties

        int Cached  { get; set; }
        int Web     { get; set; }
        int Retries { get; set; }
        int Total   { get; }

        #endregion

        #region Methods

        GetKeySecurityInfoResponse Check(decimal pin, IEnumerable<IConfidentialPinInformation> confidentialPinInformation = null, string fepMFId = null);

        #endregion
    }
}
