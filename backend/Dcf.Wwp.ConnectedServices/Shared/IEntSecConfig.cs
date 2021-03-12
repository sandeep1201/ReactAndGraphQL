using System.Collections.Generic;
using Dcf.Wwp.ConnectedServices.Shared;

namespace Dcf.Wwp.Api
{
    public interface IEntSecConfig
    {
        #region Properties

        string Env                   { get; set; }
        string Endpoint              { get; set; }
        string InteropApplicationKey { get; set; }
        string ApplicationKey        { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
