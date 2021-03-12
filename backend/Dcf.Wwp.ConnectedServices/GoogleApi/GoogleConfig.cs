using System.Collections.Generic;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    public class GoogleConfig : IGoogleConfig
    {
        #region Properties

        public string               ApiKey    { get; set; }
        public List<GoogleEndpoint> Endpoints { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public class GoogleEndpoint
    {
        public string Name { get; set; }
        public string Url  { get; set; }
    }
}
