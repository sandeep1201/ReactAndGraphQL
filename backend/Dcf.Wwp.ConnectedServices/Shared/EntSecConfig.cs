using System.Collections.Generic;
using Dcf.Wwp.ConnectedServices.Shared;

namespace Dcf.Wwp.Api
{
    public class EntSecConfig : IEntSecConfig
    {
        #region Properties

        public string Env                   { get; set; }
        public string Endpoint              { get; set; }
        public string InteropApplicationKey { get; set; }
        public string ApplicationKey        { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
