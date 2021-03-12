using System.Collections.Generic;
using Dcf.Wwp.ConnectedServices.Shared;

namespace Dcf.Wwp.Api
{
    public class WcfSoapConfig : IWcfSoapConfig
    {
        #region Properties

        public string           Env      { get; set; }
        public string           Endpoint { get; set; }
        public List<WcfService> Services { get; set; }
        public bool             UseWS    { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public class WcfService
    {
        #region Properties

        public string Name { get; set; }
        public string Uid  { get; set; }
        public string Pwd  { get; set; }
        public string To   { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
