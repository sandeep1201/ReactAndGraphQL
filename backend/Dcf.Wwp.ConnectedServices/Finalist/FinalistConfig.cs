using System.Collections.Generic;

namespace Dcf.Wwp.ConnectedServices.Finalist
{
    public class FinalistConfig : IFinalistConfig
    {
        #region Properties

        public List<FinalistEndpoint> EndPoints { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public class FinalistEndpoint
    {
        public string Env      { get; set; }
        public string EndPoint { get; set; }
    }
}
