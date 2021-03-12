using System.Collections.Generic;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    public interface IGoogleConfig
    {
        #region Properties

        string               ApiKey    { get; set; }
        List<GoogleEndpoint> Endpoints { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
