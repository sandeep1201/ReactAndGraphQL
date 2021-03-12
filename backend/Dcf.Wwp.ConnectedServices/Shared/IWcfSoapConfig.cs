using System.Collections.Generic;
using Dcf.Wwp.Api;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public interface IWcfSoapConfig
    {
        #region Properties

        string           Env      { get; set; }
        string           Endpoint { get; set; }
        List<WcfService> Services { get; set; }
        bool             UseWS    { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public interface IWcfSoapService
    {
        #region Properties

        string Name { get; set; }
        string Uid  { get; set; }
        string Pwd  { get; set; }
        string To   { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
